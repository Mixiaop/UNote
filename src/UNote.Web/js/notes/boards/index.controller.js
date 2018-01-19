require(['jquery', 'underscore', 'handlebars', 'notes/boards/taskInfoDialog', 'notes/boards/tagSettingsDialog', 'bootstrap.colorpicker', 'jquery.ui', 'jquery.slimscroll', 'jquery.confirm', 'jquery.signalR', 'signalR.hubs'],
    function ($, _, handlebars, taskInfoDialog, tagSettingsDialog, bsColor, jqueryUI, jquerySlimScroll, jqueryConfirm) {
        var vc = {};
        vc.nodeId = parseInt($('#hidNodeId').val());
        vc.teamId = parseInt($('#hidTeamId').val());
        vc.tags = [];                                //标签列表
        vc.users = [];                               //Team所有用户列表

        //|--------------------------------------|
        //|---------------modules----------------|

        vc.modules = {};

        vc.modules.renders = function () {
            var $task = function (taskId) {
                return $('#board-content-' + taskId);
            }

            var $column = function (columnId) {
                return $('#board-column-' + columnId);
            }

            var events = function () {
                return {
                    //列表 - 所有事件初始化
                    columnsBind: function () {
                        var $boards = $('.board');

                        //列之间移动
                        $('.board-list').sortable({
                            connectWith: '.board',
                            dropOnEmpty: true,
                            opacity: .75,
                            handle: '.board-header',
                            stop: function (event, ui) {
                                var columnIds = $(this).sortable("toArray");
                                var ids = [];
                                if (columnIds.length > 0) {
                                    for (var i = 0; i < columnIds.length; i++) {
                                        if (columnIds[i].indexOf('-') != -1) {
                                            ids.push(columnIds[i].split('-')[2]);
                                        }
                                    }
                                }
                                if (ids.length > 0) {
                                    //update column orders
                                    vc.modules.service.columnResetOrders(ids);
                                }
                                //console.log(ids);
                            }
                        });

                        //bind columns
                        $.each($boards, function () {
                            var columnId = parseInt($(this).data('columnid'));
                            var columnTitle = $(this).data('title');
                            var $this = this;

                            //filter create new column
                            if (columnId > 0) {
                                //archived column
                                var $archived = $(this).find('.board-title .js-archived');
                                $archived.unbind('click');
                                $archived.bind('click', function () {
                                    vc.modules.service.columnArchiveTasks(columnId, columnTitle);
                                });


                                //delete column
                                var $deleteColumn = $(this).find('.board-title .js-deleteColumn');
                                $deleteColumn.unbind('click');
                                $deleteColumn.bind('click', function () {
                                    vc.modules.service.columnDelete(columnId, columnTitle);
                                });

                                //add item click
                                var $newItem = $(this).find('.board-new');
                                var $newForm = $(this).find('.board-new-form');
                                var $newFormTitle = $newForm.find('input');
                                var $newFormCommit = $newForm.find('.btn-success');
                                var $newFormCancel = $newForm.find('.btn-default');

                                $newItem.unbind('click');
                                $newItem.bind('click', function (e) {
                                    $newForm.removeClass('hidden');
                                    $newFormTitle.focus();
                                    $(document).one("click", function () {
                                        $newForm.addClass('hidden');
                                    });
                                    e.stopPropagation();
                                });

                                $newForm.unbind('click');
                                $newForm.bind('click', function (e) {
                                    e.stopPropagation();
                                });

                                $newFormCancel.unbind('click');
                                $newFormCancel.bind('click', function () {
                                    vc.modules.renders.columnNewTaskFormReset(columnId);
                                });

                                var committing = false;
                                $newFormCommit.unbind('click');
                                $newFormCommit.bind('click', function () {
                                    var title = $.trim($newFormTitle.val());
                                    vc.modules.service.taskFormAdd(columnId, title);
                                });

                                $newFormTitle.unbind('keydown');
                                $newFormTitle.keydown(function (e) {
                                    if (e.keyCode == 13) {
                                        $newFormCommit.click();
                                    }
                                });

                                $newFormTitle.unbind('keyup');
                                $newFormTitle.keyup(function () {
                                    if ($newFormTitle.val() != '') {
                                        $newFormCommit.removeAttr('disabled');
                                    } else {
                                        $newFormCommit.attr('disabled', 'disabled');
                                    }
                                });

                                //---------标题点击 
                                var $titleBlock = $(this).find('.board-title-text');
                                var $titleInputBlock = $(this).find('.board-title-text-input');
                                var $titleInput = $(this).find('.board-title-text-input input');

                                $titleInput.val($titleBlock.text());

                                $titleBlock.unbind('click');
                                $titleBlock.bind('click', function () {
                                    $titleBlock.addClass('hidden');
                                    $titleInputBlock.removeClass('hidden');
                                    $titleInput.focus();
                                    return false;
                                });

                                $titleInput.unbind('blur');
                                $titleInput.bind('blur', function () {
                                    $titleBlock.removeClass('hidden');
                                    $titleInputBlock.addClass('hidden');

                                    var value = $.trim($titleInput.val());
                                    if ($titleBlock.text() != value && value != '') {
                                        $titleBlock.html(value);

                                        vc.modules.service.columnUpdateTitle(columnId, value);
                                    } else {
                                        //reset
                                        $titleInput.val($titleBlock.text());
                                    }
                                    return false;
                                });
                                //---------end 标题点击 
                            }
                        });
                    },
                    //任务 - 所有任务项事件初始化
                    tasksBind: function () {
                        var slimScrollStyle = '';
                        var taskMoving = false;
                        var taskMovingStartColumnId = 0;

                        //任务之间移动
                        $('.board-content-list').sortable({
                            connectWith: '.board-content-list',
                            dropOnEmpty: true,
                            opacity: .75,
                            placeholder: 'draggable-placeholder',
                            tolerance: 'pointer',
                            start: function (e, ui) {
                                var $item = $(ui.item[0]);
                                var columnId = parseInt($item.parents('.board').data('columnid'));
                                var $slimDiv = $column(columnId).find('.slimScrollDiv');

                                //fix slimScroll overflow task div
                                slimScrollStyle = $slimDiv.attr('style');
                                $slimDiv.removeAttr('style');
                                taskMoving = true;
                                taskMovingStartColumnId = columnId;

                                ui.placeholder.css({
                                    'height': ui.item.outerHeight(),
                                    'margin-bottom': ui.item.css('margin-bottom')
                                });
                            },
                            update: function (e, ui) {
                                var $item = $(ui.item[0]);
                                var targetColumnId = parseInt($item.parents('.board').data('columnid'));
                                var itemIds = $(this).sortable("toArray");
                                var currentId = parseInt($item.data('id'));

                                //判断目标列执行更新操作
                                var ids = [];
                                var exists = false;
                                if (itemIds.length > 0) {
                                    for (var i = 0; i < itemIds.length; i++) {
                                        if (itemIds[i].indexOf('-') != -1) {
                                            var id = parseInt(itemIds[i].split('-')[2]);
                                            ids.push(id);
                                            if (id == currentId) {
                                                exists = true;
                                            }
                                        }
                                    }
                                }

                                if (ids.length > 0 && exists) {
                                    //update item orders
                                    vc.modules.service.taskResetOrders(targetColumnId, ids);
                                }
                            }, stop: function (e, ui) {
                                if (taskMoving && taskMovingStartColumnId > 0) {
                                    var $div = $column(taskMovingStartColumnId).find('.slimScrollDiv');
                                    $div.attr('style', slimScrollStyle);
                                    taskMoving = false;
                                    taskMovingStartColumnId = 0;
                                }
                            }
                        });

                        //##event - 任务项点击
                        $('.js-block-task').unbind('click');
                        $('.js-block-task').bind('click', function () {
                            var id = parseInt($(this).data('id'));
                            taskInfoDialog.open({
                                id: id,
                                resTitle: vc.modules.broadcast.updateTaskTitle,
                                resFinishd: function (taskId, finished) {
                                    if (finished) {
                                        vc.modules.broadcast.finishTask(taskId);
                                    } else {
                                        vc.modules.broadcast.cancelTask(taskId);
                                    }
                                },
                                resDelete: vc.modules.broadcast.deleteTask,
                                resBody: vc.modules.broadcast.updateTaskBody,
                                resTags: vc.modules.broadcast.updateTaskTags,
                                resUsers: vc.modules.broadcast.updateTaskFollowers,
                                resExpirationDate: vc.modules.broadcast.updateTaskExpirationDate
                            });
                            return false;
                        });

                        //##event 列表项checkbox点击
                        $('.board ul li span').unbind('click');
                        $('.board ul li span').bind('click', function (e) {
                            var taskId = parseInt($(this).parents('li').data('id'));
                            if (parseInt($(this).data('check')) == 1) {
                                vc.modules.service.taskCancel(taskId);

                            } else {
                                vc.modules.service.taskFinish(taskId);
                            }
                            return false;
                        });

                        vc.modules.$.bindEvents();
                    },
                }
            }();

            //all column/task renders
            return {
                //布局发生变化更新列高度
                resizeColumns: function () {
                    var $boards = $('.board');
                    var height = $(window).height();

                    $('.board-list').css('height', (height - 115) + 'px');
                    $('.board').css('height', (height - 160) + 'px');
                    $('.board-content-list').css('height', (height - 212) + 'px');

                    //scroll
                    $('.board-content-list').slimScroll({
                        height: (height - 212) + 'px'
                    });
                },
                //列载入时loading控制
                columnsLoading: function (show) {
                    var $loading = $('#loadingColumns');
                    if (show) {
                        $loading.removeClass('hidden');
                    } else {
                        $loading.addClass('hidden');
                    }
                },
                //-------------------Columns
                //单列首次Loading
                columnLoading: function (columnId, show) {
                    var $loading = $('#board-column-' + columnId).find('.loading');

                    if (show) {
                        $loading.removeClass('hidden');
                    } else {
                        $loading.addClass('hidden');
                    }
                },
                //列表 - 新增列
                columnAdd: function (column) {
                    var $newColumnWrapper = $('.board-new-column');

                    var $html = $('#tempColumn').html();
                    var template = handlebars.compile($html);
                    var $data = template({ column: column });
                    $newColumnWrapper.before($data);

                    var $column = $('#board-column-' + column.Id);
                    //set class
                    if (column.Class != '') {
                        //$('#board-column-' + column.Id).find('.board-header').css('border-color', column.Class);
                        $column.find('.board-inner').css('border-top', '2px solid ' + column.Class);
                        $column.find('.board-title-text').css('background', column.Class);
                        $column.find('.board-title-text').css('color', '#fff');
                        //console.log($($data).find('.board-header').html());
                    }

                    events.columnsBind();
                },
                //列表 - 移除
                columnRemove: function (columnId) {
                    var $column = $('#board-column-' + columnId);
                    if ($column != null)
                        $column.remove();
                },
                //列表 - 新增任务表单重置
                columnNewTaskFormReset: function (columnId) {
                    var $column = $('#board-column-' + columnId);
                    var $newForm = $column.find('.board-new-form');
                    var $newFormTitle = $newForm.find('input');
                    var $newFormCommit = $newForm.find('.btn-success');

                    $newFormTitle.val('');
                    $newFormCommit.text('提交');
                    $newFormCommit.attr('disabled', 'disabled');
                    $newForm.addClass('hidden');
                },
                columnNewTaskFormCommitting: function (columnId, text) {
                    $column(columnId).find('.board-new-form .btn-success').text(text);
                },
                columnResetOrders: function (columnIds) {
                    if (columnIds != undefined && columnIds.length > 0) {
                        _.each(columnIds, function (id) {
                            $column(id).insertBefore($('.board-new-column'));
                        });
                    }
                },
                //-------------------Tasks
                //任务项 - 添加
                taskAdd: function (task) {
                    var $html = $('#tempContent').html();
                    var template = handlebars.compile($html);
                    var $data = template({ content: task });
                    $column(task.ColumnId).find('.board-content-list').prepend($data);

                    if (task.ColumnTaskFinished) {
                        vc.modules.renders.taskFinish(task.Id);
                    }

                    if (task.ExistsBody) {
                        vc.modules.renders.taskTagBody(task.Id, true);
                    }

                    vc.modules.renders.taskTags(task.Id, task.FormatTags);

                    vc.modules.renders.taskUsers(task.Id, task.Followers);

                    vc.modules.renders.taskExpirationDate(task.Id, task.ColumnTaskExpirationDate);
                    events.tasksBind();
                },
                //任务项 - 更新标题
                taskTitle: function (taskId, newTitle) {
                    $task(taskId).find('.block-content a').html(newTitle);
                },
                //任务项 - 完成或取消
                taskFinishd: function (taskId, isFinish) {
                    if (isFinish)
                        vc.modules.renders.taskFinish(taskId);
                    else
                        vc.modules.renders.taskCancel(taskId);
                },
                //任务项 - 完成任务
                taskFinish: function (taskId) {
                    //update view
                    var $item = $task(taskId);
                    $item.find('input[type=checkbox]').prop('checked', true);
                    $item.find('.block').addClass('over');
                    $item.find('span').data('check', 1);
                },
                //任务项 - 取消任务
                taskCancel: function (taskId) {
                    //update view
                    var $item = $task(taskId);
                    $item.find('.block').removeClass('over');
                    $item.find('input[type=checkbox]').prop('checked', false);
                    $item.find('span').data('check', 0);
                },
                //任务项 - 移除任务
                taskRemove: function (taskId) {
                    $task(taskId).remove();
                },
                //任务项 - 是否显示备注Icon
                taskTagBody: function (taskId, have) {
                    var $item = $task(taskId);
                    if (have)
                        $item.find('.js-content').removeClass('hidden');
                    else
                        $item.find('.js-content').addClass('hidden');
                },
                //任务项 - 已选中的标签列表
                taskTags: function (taskId, tagList) {
                    var $ul = $task(taskId).find('.js-block-tags ul');

                    //remove
                    $ul.find('li').each(function () {
                        if ($(this).data('type') == 'tag') {
                            $(this).remove();
                        }
                    });

                    if (tagList != undefined && tagList.length > 0) {
                        _.each(tagList, function (tag) {
                            var color = '';
                            _.each(vc.tags, function (obj) {
                                if (tag == obj.Name)
                                    color = obj.StyleColor; //get color
                            });
                            if (color == '')
                                color = '#999';

                            $ul.append('<li data-type="tag"><label class="label" style="background:' + color + ';">' + tag + '</label></li>');
                        });
                    }
                },
                //任务项 - 参与者
                taskUsers: function (taskId, followers) {
                    var $div = $task(taskId).find('.js-block-users');

                    //remove
                    $div.find('div').each(function () {
                        $(this).remove();
                    });

                    if (followers != undefined && followers.length > 0) {
                        _.each(followers, function (user) {
                            var nickName = user.NickName.charAt(user.NickName.length - 1);
                            var avatarUrl = user.AvatarUrl;
                            if (avatarUrl != null && avatarUrl.length > 10) {
                                var $html = $('#tempAvatarPreview').html();
                                var template = handlebars.compile($html);
                                var $data = template({ user: user });
                                $div.append($data);
                            } else {
                                $div.append('<div class="item item-circle bg-info-light text-info js-tooltip" title="' + user.NickName + '">' + nickName + '</div>');
                            }
                        });
                    }
                },
                //任务项 - 截止日期
                taskExpirationDate: function (taskId, date) {
                    var $html = $task(taskId).find('.js-expirationDate');


                    if (date == '' || date == undefined) {
                        $html.addClass('hidden');
                    } else {
                        $html.html('<i class="fa fa-clock-o"></i> ' + date);
                        $html.removeClass('hidden');
                        var now = new Date();
                        var expirationDate = new Date(date + ' 23:59:59');

                        if (parseInt($task(taskId).find('span').data('check')) != 1 && now > expirationDate) {
                            $html.css({ 'padding-left': '5px', 'padding-right': '5px' });
                            $html.css({ 'background': '#FF4747', 'color': '#fff' });
                            $html.find('i').css({ 'color': '#fff' });
                        }
                    }

                },
                taskResetOrders: function (columnId, taskIds) {
                    if (taskIds != undefined && taskIds.length > 0) {
                        var $ul = $column(columnId).find('.board-content-list');
                        _.each(taskIds, function (id) {
                            $ul.append($task(id));
                        });
                    }
                }
            }
        }();

        vc.modules.service = function () {
            var committing = false;

            var $task = function (taskId) {
                return $('#board-content-' + taskId);
            }

            var $column = function (columnId) {
                return $('#board-column-' + columnId);
            }

            return {
                initialize: function () {
                    setTimeout(function () {
                        BoardService.GetAllColumns(vc.nodeId, function (res) {
                            var json = res.value;
                            if (json != null && json.Success) {
                                if (json.Result.length > 0) {
                                    _.each(json.Result, function (column) {
                                        vc.modules.service.columnItemAdd(column);
                                    });
                                }
                            } else {
                                console.log("BoardService.GetAllColumns error: " + JSON.stringify(json));
                            }


                            vc.modules.renders.columnsLoading(false);
                        });
                    }, 300);
                },
                //列表 - 新增列表项
                columnItemAdd: function (column) {
                    vc.modules.renders.columnAdd(column);

                    setTimeout(function () {
                        vc.modules.renders.resizeColumns();
                    }, 500);

                    BoardService.GetAllTasks(column.Id, function (res) {
                        var json = res.value;
                        if (json != null && json.Success) {
                            if (json.Result.length > 0) {
                                _.each(json.Result, function (content) {
                                    vc.modules.service.taskItemAdd(content);
                                });
                            }
                        } else {
                            console.log("BoardService.GetAllTasks error: " + JSON.stringify(json));
                        }
                        vc.modules.renders.columnLoading(column.Id, false);
                    });
                },
                //列表 - 重置排序
                //columnIds List<int>
                columnResetOrders: function (columnIds) {
                    BoardService.ResetColumnOrders(columnIds, function (res) {
                        var json = res.value;
                        if (!json.Success) {
                            console.log('BoardService.ResetColumnOrders error: ' + JSON.stringify(json));
                        } else {
                            vc.modules.broadcast.resetColumnOrders(columnIds);
                        }
                    });
                },
                //列表 - 删除提供异步回调
                columnDelete: function (columnId, columnTitle) {
                    $.confirm({
                        confirmButtonClass: 'btn-danger',
                        title: '删除列表',
                        content: '您确定要永远删除列表【' + columnTitle + '】吗?',
                        confirm: function () {
                            BoardService.DeleteColumn(columnId, function (res) {
                                var json = res.value;
                                if (json.Success) {
                                    //vc.modules.renders.columnRemove(columnId);
                                    vc.modules.broadcast.deleteColumn(columnId);
                                } else {
                                    $.alert({
                                        title: '删除失败',
                                        content: json.Error.Message
                                    });
                                }
                            });
                        }
                    });
                },
                //列表 - 更新标题
                columnUpdateTitle: function (columnId, title) {
                    BoardService.UpdateColumnTitle(columnId, title, function (res) {
                        if (!res.value.Success) {
                            console.log('error: BoardService.UpdateColumnTitle');
                        }
                    });
                },
                //列表 - 归档列表已完成的任务
                columnArchiveTasks: function (columnId, columnTitle) {
                    $.confirm({
                        confirmButtonClass: 'btn-danger',
                        title: '归档任务',
                        content: '您确定要归档【' + columnTitle + '】下所有已完成的任务吗?',
                        confirm: function () {
                            var $currentColumn = $column(columnId);
                            var taskIds = [];
                            var $li = $currentColumn.find('.board-content-list li');

                            $li.each(function () {
                                var taskId = parseInt($(this).data('id'));
                                if ($(this).find('span').data('check') == 1) {
                                    taskIds.push(taskId);
                                }
                            });

                            if (taskIds.length > 0) {
                                _.each(taskIds, function (taskId) {
                                    vc.modules.renders.taskRemove(taskId);
                                });

                                //post
                                BoardService.ArchiveTasks(taskIds, function (res) {
                                    var json = res.value;
                                    if (!json.Success) {
                                        console.log('BoardService.CancelFinishTask error: ' + JSON.stringify(json));
                                    }
                                });
                            }
                        }
                    });
                },
                //任务 - 新增任务项
                taskItemAdd: function (task) {
                    vc.modules.renders.taskAdd(task);
                },
                //任务 - 表单添加
                taskFormAdd: function (columnId, title) {
                    if (!committing) {
                        committing = true;
                        vc.modules.renders.columnNewTaskFormCommitting(columnId, '添加中.');
                        BoardService.AddTask(vc.nodeId, columnId, title, function (res) {
                            vc.modules.renders.columnNewTaskFormCommitting(columnId, '提交');
                            committing = false;
                            var json = res.value;
                            if (json != null && res != null && json.Success) {
                                vc.modules.renders.columnNewTaskFormReset(columnId);
                                //add new column
                                //vc.modules.service.taskItemAdd(json.Result);
                                vc.modules.broadcast.addTask(json.Result);
                            } else {
                                console.log('error: ' + json.Error.Message);
                            }
                        });
                    }
                },
                //任务 - 重置排序
                taskResetOrders: function (targetColumnId, taskIds) {
                    BoardService.ResetTaskOrders(targetColumnId, taskIds, function (res) {
                        var json = res.value;
                        if (!json.Success) {
                            console.log('BoardService.ResetTaskOrders error: ' + JSON.stringify(json));
                        } else {
                            vc.modules.broadcast.resetTaskOrders(targetColumnId, taskIds);
                        }
                    });
                },
                //任务 - 完成任务
                taskFinish: function (taskId) {
                    vc.modules.renders.taskFinish(taskId);
                    //post
                    BoardService.FinishTask(taskId, function (res) {
                        var json = res.value;
                        if (!json.Success) {
                            console.log('BoardService.FinishTask error: ' + JSON.stringify(json));
                        } else {
                            vc.modules.broadcast.finishTask(taskId);
                        }
                    });
                },
                //任务 - 取消
                taskCancel: function (taskId) {
                    vc.modules.renders.taskCancel(taskId);
                    //post
                    BoardService.CancelFinishTask(taskId, function (res) {
                        var json = res.value;
                        if (!json.Success) {
                            console.log('BoardService.CancelFinishTask error: ' + JSON.stringify(json));
                        } else {
                            vc.modules.broadcast.cancelTask(taskId);
                        }
                    });
                }
            }
        }();

        vc.modules.newColumn = function () {
            var $btnNewColumn = $('.board-new-column>button');
            var $txtColumnTitle = $('.board-new-column input[name=title]');
            var $txtColor = $('.board-new-column input[name=color]');
            var $btnCommit = $('.board-new-column .btn-success');
            var $btnCancel = $('.board-new-column .card .btn-cancel')
            var $form = $('.board-new-column .card');
            var committing = false;

            var _bindEvents = function () {
                $btnNewColumn.on('click', function () {
                    $form.removeClass('hidden');
                    $btnNewColumn.addClass('hidden');

                    $txtColor.colorpicker({ 'fomart': 'hex', 'inline': false });
                });

                $btnCancel.on('click', function () {
                    _resetForm();
                });

                $txtColumnTitle.keydown(function (e) {
                    if (e.keyCode == 13) {
                        $btnCommit.click();
                    }
                });

                $txtColumnTitle.keyup(function () {
                    if ($txtColumnTitle.val() != '') {
                        $btnCommit.removeAttr('disabled');
                    } else {
                        $btnCommit.attr('disabled', 'disabled');
                    }
                });

                $btnCommit.click(function () {
                    if (!committing) {
                        committing = true;
                        $(this).text('创建中.');
                        BoardService.CreateColumn(vc.nodeId, $.trim($txtColumnTitle.val()), $.trim($txtColor.val()), function (res) {
                            $(this).text('提交');
                            committing = false;
                            var json = res.value;
                            if (json != null && res != null && json.Success) {
                                _resetForm();
                                //add new column
                                var column = json.Result;
                                vc.modules.broadcast.createColumn(column);
                            } else {
                                console.log('error: ' + json.Error.Message);
                            }
                        });
                    }
                });
            }

            function _resetForm() {
                $txtColumnTitle.val('');
                $txtColor.val('');
                $btnCommit.text('提交');
                $btnCommit.attr('disabled', 'disabled');
                $form.addClass('hidden');
                $btnNewColumn.removeClass('hidden');
            }

            function _init() {
                _bindEvents();
            };

            return {
                init: _init
            }
        }();

        vc.modules.$ = function () {
            return {
                initialize: function () {
                    $('#btn-tagSettings').click(function () {
                        tagSettingsDialog.open({ nodeId: vc.nodeId });
                    });

                    $(window).resize(function () {
                        vc.modules.renders.resizeColumns();
                    });
                },
                bindEvents: function () {
                    $('[data-toggle="popover"], .js-popover').popover({
                        container: 'body',
                        trigger: 'hover',
                        html: true,
                        placement: 'right',
                        animation: true
                    });
                    $('[data-toggle="tooltip"], .js-tooltip').tooltip({
                        container: 'body',
                        animation: true
                    });
                }
            }
        }();

        vc.modules.broadcast = function () {
            var notifier = $.connection.TaskBoardNotifier;

            function _initialize() {
                //clients 
                $.extend(notifier.client, {
                    //columns
                    createColumn: function (column) {
                        vc.modules.service.columnItemAdd(column);
                    },
                    resetColumnOrders: function (columnIds) {
                        vc.modules.renders.columnResetOrders(columnIds);
                    },
                    deleteColumn: function (columnId) {
                        vc.modules.renders.columnRemove(columnId);
                    },
                    //tasks
                    addTask: function (task) {
                        vc.modules.service.taskItemAdd(task);
                    },
                    updateTaskTitle: function (taskId, newTitle) {
                        vc.modules.renders.taskTitle(taskId, newTitle);
                    },
                    updateTaskBody: function (taskId, haveBody) {
                        vc.modules.renders.taskTagBody(taskId, haveBody != '');
                    },
                    updateTaskExpirationDate: function (taskId, date) {
                        vc.modules.renders.taskExpirationDate(taskId, date);
                    },
                    finishTask: function (taskId) {
                        vc.modules.renders.taskFinish(taskId);
                    },
                    cancelTask: function (taskId) {
                        vc.modules.renders.taskCancel(taskId);
                    },
                    updateTaskTags: function (taskId, tagList) {
                        vc.modules.renders.taskTags(taskId, tagList);
                    },
                    updateTaskFollowers: function (taskId, users) {
                        vc.modules.renders.taskUsers(taskId, users);
                    },
                    resetTaskOrders: function (columnId, taskIds) {
                        vc.modules.renders.taskResetOrders(columnId, taskIds);
                    },
                    deleteTask: function (taskId) {
                        vc.modules.renders.taskRemove(taskId);
                    }
                });

                $.connection.hub.start()
                                .done(function () {
                                    notifier.server.joinBoardRoom(vc.nodeId);
                                    console.log('connect successfully');
                                });
            }

            return {
                initialize: _initialize,
                //columns
                createColumn: function (column) {
                    notifier.server.createColumn(vc.nodeId, column);
                },
                resetColumnOrders: function (columnIds) {
                    notifier.server.resetColumnOrders(vc.nodeId, columnIds);
                },
                deleteColumn: function (columnId) {
                    notifier.server.deleteColumn(vc.nodeId, columnId);
                },
                //tasks
                addTask: function (task) {
                    notifier.server.addTask(vc.nodeId, task);
                },
                updateTaskTitle: function (taskId, newTitle) {
                    notifier.server.updateTaskTitle(vc.nodeId, taskId, newTitle);
                },
                updateTaskBody: function (taskId, haveBody) {
                    notifier.server.updateTaskBody(vc.nodeId, taskId, haveBody);
                },
                updateTaskExpirationDate: function (taskId, date) {
                    notifier.server.updateTaskExpirationDate(vc.nodeId, taskId, date);
                },
                finishTask: function (taskId) {
                    notifier.server.finishTask(vc.nodeId, taskId);
                },
                cancelTask: function (taskId) {
                    notifier.server.cancelTask(vc.nodeId, taskId);
                },
                updateTaskTags: function (taskId, tagList) {
                    notifier.server.updateTaskTags(vc.nodeId, taskId, tagList);
                },
                updateTaskFollowers: function (taskId, users) {
                    notifier.server.updateTaskFollowers(vc.nodeId, taskId, users);
                },
                resetTaskOrders: function (columnId, taskIds) {
                    notifier.server.resetTaskOrders(vc.nodeId, columnId, taskIds);
                },
                deleteTask: function (taskId) {
                    notifier.server.deleteTask(vc.nodeId, taskId);
                }
            }
        }();


        //|--------------------------------------|
        //|---------------end modules------------|

        function _initialize(callback) {

            $avatarModal = $('#modalAvatarPreview');

            TagService.GetAllTags(vc.nodeId, function (res) {
                var json = res.value;
                if (json.Success) {
                    if (json.Result.length > 0) {
                        vc.tags = json.Result;
                    }
                    vc.modules.service.initialize();
                    vc.modules.newColumn.init();
                }
            });

            TeamService.GetAllMembers(vc.teamId, function (res) {
                var json = res.value;
                if (json.Success) {
                    if (json.Result.length > 0) {
                        vc.users = json.Result;
                    }
                }
            });

            vc.modules.$.initialize();
            vc.modules.broadcast.initialize();

        }

        _initialize();

        //tests
        //tagSettingsDialog.open({ nodeId: vc.nodeId }); 
        //taskInfoDialog.open({
        //id: 68,
        //    //resTitle: vc.modules.columnList.renderTitle,
        //    //resFinishd: vc.modules.columnList.renderFinishd,
        //    //resDelete: vc.modules.columnList.renderDelete
        //});

        //setTimeout(function () {
        //vc.modules.renders.taskExpirationDate(68, '2017-08-09');
        //}, 3000);
    });