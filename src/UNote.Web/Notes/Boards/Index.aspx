<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/User.master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="UNote.Web.Notes.Boards.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="userHead" runat="server">
    <link rel="stylesheet" href="/lib/plugins/bootstrap-colorpicker/css/bootstrap-colorpicker.min.css">
    <link rel="stylesheet" href="/css/unote-boards.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="userBody" runat="server">
    <!-- Page Header -->
    <div class="page-header">
        <div class="row">
            <div class="col-lg-3 col-xs-8">
                <ul>
                    <li><%= Model.Node.TeamId> 0? Model.Node.Team.Name: "我的笔记" %></li>
                    <% foreach (var node in Model.Parents)
                       { %>
                    <li><span>/</span> <a href="<%= RouteContext.GetRouteUrl("Notes.Contents", node.Id) %>"><%= node.NodeName %></a></li>
                    <%} %>
                    <li class="current"><span>/</span> <%= Model.Node.NodeName %></li>
                </ul>
            </div>
        </div>
    </div>
    <!-- END Page Header -->
    <div class="content">
        <input type="hidden" id="hidNodeId" value="<%= Model.GetNodeId %>" />
        <div class="board-list">
            <img src="/img/loading.gif" alt="loading" title="loading" id="loadingColumns" />
            <!-- board -->
            <div class="board board-new-column">
                <button class="btn btn-default btn-block"><i class="fa fa-plus"></i>&nbsp;新建列表</button>
                <div class="card hidden">
                    <div class="card">
                        <label>
                            列表名
                        </label>
                        <input type="text" name="title" autocomplete="off" class="form-control">
                        <label>
                            颜色
                        </label>
                        <input type="text" name="color" autocomplete="off" class="form-control">
                        <div class="clearfix">
                            <button type="button" disabled="disabled" class="btn btn-success pull-left">
                                提交
                            </button>
                            <button type="button" class="btn btn-default btn-cancel pull-right">
                                取消
                            </button>
                        </div>
                    </div>
                </div>
            </div>
            <!-- end board -->
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="userFoot" runat="server">
    <!-- template -->
    <script id="tempColumn" type="text/x-handlebars-template">
        <!-- board -->
        <div class="board" id="board-column-{{column.Id}}" data-columnid="{{column.Id}}">
            <div class="board-inner ">
                <header class="board-header" >
                    <h3 class="board-title ">
                        <span class="board-title-text color-label label">{{column.Title}}</span>
                        <button type="button" class="board-delete" data-toggle="tooltip" data-original-title="删除列表"><i class="fa fa-trash"></i></button>
                        <div class="board-count-badge clearfix">
                            <button type="button" class="board-count-badge-button no-right-border btn btn-sm btn-default hidden" data-toggle="tooltip" data-original-title="编辑列表"><i class="fa fa-pencil"></i></button>
                            <button type="button" class="board-count-badge-button btn btn-sm btn-default board-new" data-toggle="tooltip" data-original-title="新任务"><i class="fa fa-plus"></i></button>
                        </div>
                    </h3>
                </header>
                <div class="board-list-component">
                    <div class="board-new-form hidden">
                        <div class="card">
                            <label>
                                标题
                            </label>
                            <input type="text" autocomplete="off" class="form-control">
                            <div class="clearfix">
                                <button type="button" disabled="disabled" class="btn btn-success pull-left">
                                    提交
                                </button>
                                <button type="button" class="btn btn-default pull-right">
                                    取消
                                </button>
                            </div>
                        </div>
                    </div>
                    <img src="/img/loading.gif" alt="loading" title="loading" class="loading push-5-l push-5-t" />
                    <ul class="board-content-list">
                        <%--<li>
                            <div class="block">
                                <div class="block-content">
                                    我是任务1Hello world 2222我是任务1Hello world 2222我是任务1Hello world 2222我是任务1Hello world 2222
                                </div>
                            </div>
                        </li>--%>
                    </ul>
                </div>
            </div>
        </div>
        <!-- end board -->
    </script>

    <script id="tempContent" type="text/x-handlebars-template">
        <li id="board-content-{{content.Id}}" data-id="{{content.Id}}" data-columnid="{{content.ColumnId}}" >
                            <div class="block">
                                <div class="block-content">
                                    <label class="css-input css-checkbox css-checkbox-success"><input type="checkbox" ><span ></span></label> <a href="javascript:;">{{content.Title}}</a>
                                </div>
                            </div>
                        </li>
    </script>
    <!-- end template -->
    
    <script>
        require(['jquery', 'underscore', 'handlebars', 'notes/boards/taskInfoDialog', 'bootstrap.colorpicker', 'jquery.ui'], function ($, _, handlebars, taskInfoDialog, bsColor, jqueryUI) {
            var vc = {};
            vc.nodeId = parseInt($('#hidNodeId').val());

            vc.modules = {};

            //create new column
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
                                if (confirm('您确定要永远删除这个列表吗？')) {
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

                            function _resetForm() {
                                $newFormTitle.val('');
                                $newFormCommit.text('提交');
                                $newFormCommit.attr('disabled', 'disabled');
                                $newForm.addClass('hidden');
                            }

                            //add item click
                            $newItem.unbind('click');
                            $newItem.bind('click', function () {
                                $newForm.removeClass('hidden');
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
                                    BoardService.AddItem(vc.nodeId, columnId, $newFormTitle.val(), function (res) {
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
                                BoardService.ResetItemOrders(targetColumnId, ids, function (res) {
                                    var json = res.value;
                                    if (!json.Success) {
                                        console.log('BoardService.ResetItemOrders error: ' + JSON.stringify(json));
                                    }
                                });
                            }
                            

                        }
                    });

                    //列表项点击
                    $('.board ul li').unbind('click');
                    $('.board ul li').bind('click', function () {
                        var id = parseInt($(this).data('id'));
                        taskInfoDialog.open();
                        return false;
                    });

                    //列表项checkbox点击
                    $('.board ul li span').unbind('click');
                    $('.board ul li span').bind('click', function (e) {
                        var taskId = parseInt($(this).parents('li').data('id'));
                        if (parseInt($(this).data('check')) == 1) {
                            _renderCancelTask(taskId);
                            //post
                            BoardService.CancelFinishTask(taskId, function (res) { });
                            
                        } else {
                            _renderfinishTask(taskId);
                            //post
                            BoardService.FinishTask(taskId, function (res) { console.log(res);});
                        }
                        return false;
                    });
                }

                var _resizeColumns = function () {
                    var $boards = $('.board');
                    var height = $(window).height() - 120;
                    $('.board-list').css('height', height + 'px');
                    $('.board').css('height', (height - 36) + 'px');
                    $('.board-content-list').css('height', (height - 36) + 'px');
                }

                var _addNewColumn = function (column) {
                    var $newColumnWrapper = $('.board-new-column');

                    var $html = $('#tempColumn').html();
                    var template = handlebars.compile($html);
                    var $data = template({ column: column });
                    $newColumnWrapper.before($data);
                    //set class
                    if (column.Class != '') {
                        $('#board-column-' + column.Id).find('.board-header').addClass('board-header-color');
                        $('#board-column-' + column.Id).find('.board-header').css('border-color', column.Class);
                        $('#board-column-' + column.Id).find('.board-title-text').css('background', column.Class);
                        $('#board-column-' + column.Id).find('.board-title-text').css('color', '#fff');
                        //console.log($($data).find('.board-header').html());
                    }

                    setTimeout(function () {
                        _resizeColumns();
                        _bindColumnEvents();
                    }, 500);

                    BoardService.GetAllItems(column.Id, function (res) {
                        var $loading = $('#board-column-' + column.Id).find('.loading');
                        var json = res.value;
                        if (json != null && json.Success) {
                            if (json.Result.length > 0) {
                                _.each(json.Result, function (content) {
                                    _addNewItem(content);
                                });
                            }
                        } else {
                            console.log("BoardService.GetAllItems error: " + JSON.stringify(json));
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

                    console.log(content.ColumnTaskFinished);
                    if (content.ColumnTaskFinished) {
                        _renderfinishTask(content.Id);
                    }

                    _bindItemEvents();
                }

                var _renderfinishTask = function (taskId) {
                    //update view
                    var $task = $('#board-content-' + taskId);
                    $task.find('input[type=checkbox]').prop('checked', true);
                    $task.find('.block').addClass('over');
                    $task.find('span').data('check', 1);
                }

                var _renderCancelTask = function (taskId) {
                    //update view
                    var $task = $('#board-content-' + taskId);
                    $task.find('.block').removeClass('over');
                    $task.find('input[type=checkbox]').prop('checked', false);
                    $task.find('span').data('check', 0);
                }

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
                    resizeColumns: _resizeColumns,
                    addNewColumn: _addNewColumn
                }
            }();

            //init modules
            $(function () {
                vc.modules.newColumn.init();
                vc.modules.columnList.init();

                $(window).resize(function () {
                    vc.modules.columnList.resizeColumns();
                });
            });

        });
    </script>
</asp:Content>
