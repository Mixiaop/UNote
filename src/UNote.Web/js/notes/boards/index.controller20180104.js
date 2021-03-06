﻿require(['jquery', 'underscore', 'handlebars', 'notes/boards/taskInfoDialog', 'notes/boards/tagSettingsDialog', 'bootstrap.colorpicker', 'jquery.ui', 'jquery.slimscroll', 'jquery.confirm'],
    function ($, _, handlebars, taskInfoDialog, tagSettingsDialog, bsColor, jqueryUI, jquerySlimScroll, jqueryConfirm) {
        var vc = {};
        vc.nodeId = parseInt($('#hidNodeId').val());
        vc.teamId = parseInt($('#hidTeamId').val());
        vc.tags = [];     //标签列表
        vc.users = [];    //Team所有用户列表

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
                            var $this = this;

                            //filter create new column
                            if (columnId > 0) {
                                
                                //delete column
                                var $deleteColumn = $(this).find('.board-delete');
                                $deleteColumn.unbind('click');
                                $deleteColumn.bind('click', function () {
                                    $.confirm({
                                        confirmButtonClass: 'btn-danger',
                                        title: '删除列表',
                                        content: '您确定要永远删除这个列表吗?',
                                        confirm: function () {
                                            vc.modules.service.columnDelete(columnId);
                                        }
                                    });
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

                        //任务之间移动
                        $('.board-content-list').sortable({
                            connectWith: '.board-content-list',
                            dropOnEmpty: true,
                            opacity: .75,
                            placeholder: 'draggable-placeholder',
                            tolerance: 'pointer',
                            start: function (e, ui) {
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
                            }
                        });

                        //##event - 任务项点击
                        $('.js-block-task').unbind('click');
                        $('.js-block-task').bind('click', function () {
                            var id = parseInt($(this).data('id'));
                            taskInfoDialog.open({
                                id: id,
                                resTitle: vc.modules.renders.taskTitle,
                                resFinishd: vc.modules.renders.taskFinishd,
                                resDelete: vc.modules.renders.taskRemove,
                                resBody: vc.modules.renders.taskTagBody,
                                resTags: vc.modules.renders.taskTags,
                                resUsers: vc.modules.renders.taskUsers
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
                            var nickName = user.NickName.substring(0, 1);
                            $div.append('<div class="item item-circle bg-info-light text-info js-tooltip" title="' + user.NickName + '">' + nickName + '</div>');
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
                    }
                }
            }
        }();

        //create new column
        //-------------------
        //------------------module new column
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
                        BoardService.CreateColumn(vc.nodeId, $txtColumnTitle.val(), $txtColor.val(), function (res) {
                            $(this).text('提交');
                            committing = false;
                            var json = res.value;
                            if (json != null && res != null && json.Success) {
                                _resetForm();
                                //add new column
                                vc.modules.columnList.addNewColumn(json.Result);
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

        ////-------------------
        ////------------------module column list 
        //vc.modules.columnList = function () {

        //    var _bindItemEvents = function () {
        //        $('.board-content-list').sortable({
        //            connectWith: '.board-content-list',
        //            dropOnEmpty: true,
        //            opacity: .75,
        //            placeholder: 'draggable-placeholder',
        //            tolerance: 'pointer',
        //            start: function (e, ui) {
        //                ui.placeholder.css({
        //                    'height': ui.item.outerHeight(),
        //                    'margin-bottom': ui.item.css('margin-bottom')
        //                });
        //            },
        //            update: function (e, ui) {
        //                var $item = $(ui.item[0]);
        //                var targetColumnId = parseInt($item.parents('.board').data('columnid'));
        //                var itemIds = $(this).sortable("toArray");
        //                var currentId = parseInt($item.data('id'));

        //                //判断目标列执行更新操作
        //                var ids = [];
        //                var exists = false;
        //                if (itemIds.length > 0) {
        //                    for (var i = 0; i < itemIds.length; i++) {
        //                        if (itemIds[i].indexOf('-') != -1) {
        //                            var id = parseInt(itemIds[i].split('-')[2]);
        //                            ids.push(id);
        //                            if (id == currentId) {
        //                                exists = true;
        //                            }
        //                        }
        //                    }
        //                }

        //                if (ids.length > 0 && exists) {
        //                    //update item orders
        //                    BoardService.ResetTaskOrders(targetColumnId, ids, function (res) {
        //                        var json = res.value;
        //                        if (!json.Success) {
        //                            console.log('BoardService.ResetTaskOrders error: ' + JSON.stringify(json));
        //                        }
        //                    });
        //                }


        //            }
        //        });

        //        //##event - 任务项点击
        //        $('.js-block-task').unbind('click');
        //        $('.js-block-task').bind('click', function () {
        //            var id = parseInt($(this).data('id'));
        //            taskInfoDialog.open({
        //                id: id,
        //                resTitle: vc.modules.columnList.renderTitle,
        //                resFinishd: vc.modules.columnList.renderFinishd,
        //                resDelete: vc.modules.columnList.renderDelete,
        //                resBody: vc.modules.columnList.renderTagBody,
        //                resTags: vc.modules.columnList.renderTags,
        //                resUsers: vc.modules.columnList.renderUsers
        //            });
        //            return false;
        //        });

        //        //##event 列表项checkbox点击
        //        $('.board ul li span').unbind('click');
        //        $('.board ul li span').bind('click', function (e) {
        //            var taskId = parseInt($(this).parents('li').data('id'));
        //            if (parseInt($(this).data('check')) == 1) {
        //                _renderCancelTask(taskId);
        //                //post
        //                BoardService.CancelFinishTask(taskId, function (res) { });

        //            } else {
        //                _renderFinishTask(taskId);
        //                //post
        //                BoardService.FinishTask(taskId, function (res) { console.log(res); });
        //            }
        //            return false;
        //        });

        //        vc.modules.$.bindEvents();
        //    }

        //    var _addNewColumn = function (column) {
        //        vc.modules.renders.columnAdd(column);

        //        setTimeout(function () {
        //            vc.modules.renders.resizeColumns();
        //        }, 500);

        //        BoardService.GetAllTasks(column.Id, function (res) {
        //            var json = res.value;
        //            if (json != null && json.Success) {
        //                if (json.Result.length > 0) {
        //                    _.each(json.Result, function (content) {
        //                        _addNewItem(content);
        //                    });
        //                }
        //            } else {
        //                console.log("BoardService.GetAllTasks error: " + JSON.stringify(json));
        //            }
        //            vc.modules.renders.columnLoading(column.Id, false);
        //        });

        //    };

        //    var _addNewItem = function (content) {
        //        var $column = $('#board-column-' + content.ColumnId);
        //        var $html = $('#tempContent').html();
        //        var template = handlebars.compile($html);
        //        var $data = template({ content: content });
        //        $column.find('.board-content-list').prepend($data);

        //        //console.log(content.ColumnTaskFinished);
        //        if (content.ColumnTaskFinished) {
        //            _renderFinishTask(content.Id);
        //        }

        //        if (content.ExistsBody) {
        //            _renderTagBody(content.Id, true);
        //        }

        //        _renderTags(content.Id, content.FormatTags);

        //        _renderUsers(content.Id, content.Followers);

        //        _bindItemEvents();
        //    }

        //    //-------------------
        //    //------------------renders 刷新视图
        //    //更新标题
        //    var _renderTitle = function (taskId, newTitle) {
        //        var $task = $('#board-content-' + taskId);

        //        $task.find('.block-content a').html(newTitle);
        //    }

        //    //完成任务
        //    var _renderFinishTask = function (taskId) {
        //        //update view
        //        var $task = $('#board-content-' + taskId);
        //        $task.find('input[type=checkbox]').prop('checked', true);
        //        $task.find('.block').addClass('over');
        //        $task.find('span').data('check', 1);
        //    }

        //    //取消任务
        //    var _renderCancelTask = function (taskId) {
        //        //update view
        //        var $task = $('#board-content-' + taskId);
        //        $task.find('.block').removeClass('over');
        //        $task.find('input[type=checkbox]').prop('checked', false);
        //        $task.find('span').data('check', 0);
        //    }

        //    //删除后触发
        //    var _renderDelete = function (taskId) {
        //        var $task = $('#board-content-' + taskId);
        //        $task.remove();
        //    }

        //    //根据内容是否显示icon
        //    var _renderTagBody = function (taskId, have) {
        //        var $task = $('#board-content-' + taskId);
        //        if (have)
        //            $task.find('.js-content').removeClass('hidden');
        //        else
        //            $task.find('.js-content').addClass('hidden');
        //    }

        //    //标签
        //    //tagList ['name1','name2']
        //    var _renderTags = function (taskId, tagList) {
        //        var $task = $('#board-content-' + taskId);
        //        var $ul = $task.find('.js-block-tags ul');

        //        //remove
        //        $ul.find('li').each(function () {
        //            if ($(this).data('type') == 'tag') {
        //                $(this).remove();
        //            }
        //        });

        //        if (tagList != undefined && tagList.length > 0) {
        //            _.each(tagList, function (tag) {
        //                var color = '';
        //                _.each(vc.tags, function (obj) {
        //                    if (tag == obj.Name)
        //                        color = obj.StyleColor; //get color
        //                });
        //                if (color == '')
        //                    color = '#999';

        //                $ul.append('<li data-type="tag"><label class="label" style="background:' + color + ';">' + tag + '</label></li>');
        //            });
        //        }

        //    }

        //    var _renderUsers = function (taskId, followers) {
        //        var $task = $('#board-content-' + taskId);
        //        var $div = $task.find('.js-block-users');

        //        //remove
        //        $div.find('div').each(function () {
        //            $(this).remove();
        //        });

        //        if (followers != undefined && followers.length > 0) {
        //            _.each(followers, function (user) {
        //                var nickName = user.NickName.substring(0, 1);
        //                $div.append('<div class="item item-circle bg-info-light text-info js-tooltip" title="' + user.NickName + '">' + nickName + '</div>');
        //            });
        //        }
        //    }

        //    //-------------------
        //    //------------------end renders 

        //    var _init = function () {
        //        setTimeout(function () {
        //            BoardService.GetAllColumns(vc.nodeId, function (res) {
        //                var json = res.value;
        //                if (json != null && json.Success) {
        //                    if (json.Result.length > 0) {
        //                        _.each(json.Result, function (column) {
        //                            _addNewColumn(column);
        //                        });
        //                    }
        //                } else {
        //                    console.log("BoardService.GetAllColumns error: " + JSON.stringify(json));
        //                }


        //                vc.modules.renders.columnsLoading(false);
        //            });
        //        }, 300);
        //    }

        //    return {
        //        init: _init,
        //        renderTitle: _renderTitle,
        //        //渲染并触发事件
        //        renderFinishd: function (taskId, isFinish) {
        //            if (isFinish)
        //                _renderFinishTask(taskId);
        //            else
        //                _renderCancelTask(taskId);
        //        },
        //        renderDelete: _renderDelete,
        //        renderTagBody: _renderTagBody,
        //        renderTags: _renderTags,
        //        renderUsers: _renderUsers,
        //        addNewColumn: _addNewColumn,
        //        addNewItem : _addNewItem
        //    }
        //}();

        vc.modules.service = function () {
            var committing = false; 
            
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
                        }
                    });
                },
                //列表 - 删除提供异步回调
                columnDelete: function (columnId) {
                    BoardService.DeleteColumn(columnId, function (res) {
                        var json = res.value;
                        if (json.Success) {
                            vc.modules.renders.columnRemove(columnId);
                        } else {
                            alert(json.Error.Message);
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
                                vc.modules.columnList.addNewItem(json.Result);
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
                        }
                    });
                },
                //任务 - 完成任务
                taskFinish: function (taskId) {
                    vc.modules.renders.taskFinish(taskId);
                    //post
                    BoardService.FinishTask(taskId, function (res) { console.log(res); });
                },
                //任务 - 取消
                taskCancel: function (taskId) {
                    vc.modules.renders.taskCancel(taskId);
                    //post
                    BoardService.CancelFinishTask(taskId, function (res) { });
                }
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
                    $('[data-toggle="tooltip"], .js-tooltip').tooltip({
                        container: 'body',
                        animation: false
                    });
                }
            }
        }();

        //|--------------------------------------|
        //|---------------end modules------------|

        function _initialize() {
            TagService.GetAllTags(vc.nodeId, function (res) {
                var json = res.value;
                if (json.Success) {
                    if (json.Result.length > 0) {
                        vc.tags = json.Result;
                    }
                    vc.modules.service.initialize();
                    //vc.modules.newColumn.init();
                    //vc.modules.columnList.init();
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