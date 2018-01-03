require(['jquery', 'underscore', 'handlebars', 'notes/boards/taskInfoDialog', 'notes/boards/tagSettingsDialog', 'bootstrap.colorpicker', 'jquery.ui', 'jquery.slimscroll', 'jquery.confirm'],
    function ($, _, handlebars, taskInfoDialog, tagSettingsDialog, bsColor, jqueryUI, jquerySlimScroll, jqueryConfirm) {
        var vc = {};
        vc.nodeId = parseInt($('#hidNodeId').val());
        vc.teamId = parseInt($('#hidTeamId').val());
        vc.tags = [];     //标签列表
        vc.users = [];    //Team所有用户列表
        vc.modules = {};

        function _initialize() {
            TagService.GetAllTags(vc.nodeId, function (res) {
                var json = res.value;
                if (json.Success) {
                    if (json.Result.length > 0) {
                        vc.tags = json.Result;
                    }
                    vc.modules.newColumn.init();
                    vc.modules.columnList.init();
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
        }

        function _bindJQueryEvents() {
            //##events 
            $('[data-toggle="tooltip"], .js-tooltip').tooltip({
                container: 'body',
                animation: false
            });
        }

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

        //columns loading control
        //-------------------
        //------------------module column list 
        vc.modules.columnList = function () {
            var $loadingColumns = $('#loadingColumns');

            var _bindColumnEvents = function () {
                var $boards = $('.board');

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
                            BoardService.ResetColumnOrders(ids, function (res) {
                                var json = res.value;
                                if (!json.Success) {
                                    console.log('BoardService.ResetColumnOrders error: ' + JSON.stringify(json));
                                }
                            });
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
                        var $deleteColumn = $(this).find('.board-delete');
                        var $newItem = $(this).find('.board-new');
                        var $newForm = $(this).find('.board-new-form');
                        var $newFormTitle = $newForm.find('input');
                        var $newFormCommit = $newForm.find('.btn-success');
                        var $newFormCancel = $newForm.find('.btn-default');
                        var committing = false;

                        //delete column 
                        $deleteColumn.unbind('click');
                        $deleteColumn.bind('click', function () {
                            $.confirm({
                                confirmButtonClass: 'btn-danger',
                                title: '删除列表',
                                content: '您确定要永远删除这个列表吗?',
                                confirm: function () {
                                    BoardService.DeleteColumn(columnId, function (res) {
                                        var json = res.value;
                                        if (json.Success) {
                                            $this.remove();
                                        } else {
                                            alert(json.Error.Message);
                                        }
                                    });
                                }
                            });
                        });

                        function _resetForm() {
                            $newFormTitle.val('');
                            $newFormCommit.text('提交');
                            $newFormCommit.attr('disabled', 'disabled');
                            $newForm.addClass('hidden');
                        }

                        //add item click
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
                            _resetForm();
                        });

                        $newFormCommit.unbind('click');
                        $newFormCommit.bind('click', function () {
                            if (!committing) {
                                committing = true;
                                $(this).text('添加中.');
                                BoardService.AddTask(vc.nodeId, columnId, $newFormTitle.val(), function (res) {
                                    $(this).text('提交');
                                    committing = false;
                                    var json = res.value;
                                    if (json != null && res != null && json.Success) {
                                        _resetForm();
                                        //add new column
                                        _addNewItem(json.Result);
                                    } else {
                                        console.log('error: ' + json.Error.Message);
                                    }
                                });
                            }
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
                    }
                });

                //bind contents 
                _bindItemEvents();
            }

            var _bindItemEvents = function () {
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
                            BoardService.ResetTaskOrders(targetColumnId, ids, function (res) {
                                var json = res.value;
                                if (!json.Success) {
                                    console.log('BoardService.ResetTaskOrders error: ' + JSON.stringify(json));
                                }
                            });
                        }


                    }
                });

                //##event - 任务项点击
                $('.js-block-task').unbind('click');
                $('.js-block-task').bind('click', function () {
                    var id = parseInt($(this).data('id'));
                    taskInfoDialog.open({
                        id: id,
                        resTitle: vc.modules.columnList.renderTitle,
                        resFinishd: vc.modules.columnList.renderFinishd,
                        resDelete: vc.modules.columnList.renderDelete,
                        resBody: vc.modules.columnList.renderTagBody,
                        resTags: vc.modules.columnList.renderTags,
                        resUsers: vc.modules.columnList.renderUsers
                    });
                    return false;
                });

                //##event 列表项checkbox点击
                $('.board ul li span').unbind('click');
                $('.board ul li span').bind('click', function (e) {
                    var taskId = parseInt($(this).parents('li').data('id'));
                    if (parseInt($(this).data('check')) == 1) {
                        _renderCancelTask(taskId);
                        //post
                        BoardService.CancelFinishTask(taskId, function (res) { });

                    } else {
                        _renderFinishTask(taskId);
                        //post
                        BoardService.FinishTask(taskId, function (res) { console.log(res); });
                    }
                    return false;
                });

                _bindJQueryEvents();
            }

            var _resizeColumns = function () {
                var $boards = $('.board');
                var height = $(window).height();

                $('.board-list').css('height', (height - 115) + 'px');
                $('.board').css('height', (height - 160) + 'px');
                $('.board-content-list').css('height', (height - 212) + 'px');

                //scroll
                $('.board-content-list').slimScroll({
                    height: (height - 212) + 'px'
                });
            }

            var _addNewColumn = function (column) {
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


                setTimeout(function () {
                    _resizeColumns();
                    _bindColumnEvents();
                }, 500);

                BoardService.GetAllTasks(column.Id, function (res) {
                    var $loading = $('#board-column-' + column.Id).find('.loading');
                    var json = res.value;
                    if (json != null && json.Success) {
                        if (json.Result.length > 0) {
                            _.each(json.Result, function (content) {
                                _addNewItem(content);
                            });
                        }
                    } else {
                        console.log("BoardService.GetAllTasks error: " + JSON.stringify(json));
                    }
                    $loading.addClass('hidden');
                });

            };

            var _addNewItem = function (content) {
                var $column = $('#board-column-' + content.ColumnId);
                var $html = $('#tempContent').html();
                var template = handlebars.compile($html);
                var $data = template({ content: content });
                $column.find('.board-content-list').prepend($data);

                //console.log(content.ColumnTaskFinished);
                if (content.ColumnTaskFinished) {
                    _renderFinishTask(content.Id);
                }

                if (content.ExistsBody) {
                    _renderTagBody(content.Id, true);
                }

                _renderTags(content.Id, content.FormatTags);

                _renderUsers(content.Id, content.Followers);

                _bindItemEvents();
            }

            //-------------------
            //------------------renders 刷新视图
            //更新标题
            var _renderTitle = function (taskId, newTitle) {
                var $task = $('#board-content-' + taskId);

                $task.find('.block-content a').html(newTitle);
            }

            //完成任务
            var _renderFinishTask = function (taskId) {
                //update view
                var $task = $('#board-content-' + taskId);
                $task.find('input[type=checkbox]').prop('checked', true);
                $task.find('.block').addClass('over');
                $task.find('span').data('check', 1);
            }

            //取消任务
            var _renderCancelTask = function (taskId) {
                //update view
                var $task = $('#board-content-' + taskId);
                $task.find('.block').removeClass('over');
                $task.find('input[type=checkbox]').prop('checked', false);
                $task.find('span').data('check', 0);
            }

            //删除后触发
            var _renderDelete = function (taskId) {
                var $task = $('#board-content-' + taskId);
                $task.remove();
            }

            //根据内容是否显示icon
            var _renderTagBody = function (taskId, have) {
                var $task = $('#board-content-' + taskId);
                if (have)
                    $task.find('.js-content').removeClass('hidden');
                else
                    $task.find('.js-content').addClass('hidden');
            }

            //标签
            //tagList ['name1','name2']
            var _renderTags = function (taskId, tagList) {
                var $task = $('#board-content-' + taskId);
                var $ul = $task.find('.js-block-tags ul');

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

            }

            var _renderUsers = function (taskId, followers) {
                var $task = $('#board-content-' + taskId);
                var $div = $task.find('.js-block-users');

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
            }

            //-------------------
            //------------------end renders 

            var _init = function () {
                setTimeout(function () {
                    BoardService.GetAllColumns(vc.nodeId, function (res) {
                        var json = res.value;
                        if (json != null && json.Success) {
                            if (json.Result.length > 0) {
                                _.each(json.Result, function (column) {
                                    _addNewColumn(column);
                                });
                            }
                        } else {
                            console.log("BoardService.GetAllColumns error: " + JSON.stringify(json));
                        }


                        $loadingColumns.addClass('hidden');
                    });
                }, 300);
            }

            return {
                init: _init,
                renderTitle: _renderTitle,
                //渲染并触发事件
                renderFinishd: function (taskId, isFinish) {
                    if (isFinish)
                        _renderFinishTask(taskId);
                    else
                        _renderCancelTask(taskId);
                },
                renderDelete: _renderDelete,
                resizeColumns: _resizeColumns,
                renderTagBody: _renderTagBody,
                renderTags: _renderTags,
                renderUsers: _renderUsers,
                addNewColumn: _addNewColumn
            }
        }();


        //init modules
        $(function () {
            _initialize();

            $('#btn-tagSettings').click(function () {
                tagSettingsDialog.open({ nodeId: vc.nodeId });
            });

            $(window).resize(function () {
                vc.modules.columnList.resizeColumns();
            });
        });

        //tests
        //tagSettingsDialog.open({ nodeId: vc.nodeId }); 
        //taskInfoDialog.open({
        //id: 68,
        //    //resTitle: vc.modules.columnList.renderTitle,
        //    //resFinishd: vc.modules.columnList.renderFinishd,
        //    //resDelete: vc.modules.columnList.renderDelete
        //});
    });