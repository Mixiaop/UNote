<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/User.master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="UNote.Web.Notes.Boards.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="userHead" runat="server">
    <link rel="stylesheet" href="/editors/kindeditor/css/default.css" />
    <link rel="stylesheet" href="/lib/plugins/bootstrap-colorpicker/css/bootstrap-colorpicker.min.css">
    <link rel="stylesheet" href="/lib/plugins/bootstrap-datepicker/bootstrap-datepicker3.css">
    <link rel="stylesheet" href="/css/unote-boards.css" />
    <style>
        .board .dropdown-menu {
            top: 80%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="userBody" runat="server">
    <!-- Page Header -->
    <div class="page-header">
        <div class="row">
            <div class="col-lg-12 col-xs-12">
                <ul>
                    <li><%= Model.Node.TeamId> 0? Model.Node.Team.Name: "我的笔记" %></li>
                    <% foreach (var node in Model.Parents)
                       { %>
                    <li><span>/</span> <a href="<%= RouteContext.GetRouteUrl("Notes.Contents", node.Id) %>"><%= node.NodeName %></a></li>
                    <%} %>
                    <li class="current"><span>/</span> <%= Model.Node.NodeName %>&nbsp;&nbsp; </li>
                </ul>
                <div class="board-menus">
                    <a href="javascript:;" class="btn-options" data-toggle="dropdown"><i class="si si-list"></i> 菜单</a>
                    <ul class="dropdown-menu dropdown-menu-right push-30-r">
                        <li><a tabindex="-1" href="javascript:;" id="btn-tagSettings"><i class="si si-settings pull-right"></i>标签设置</a></li>
                        <li><a tabindex="-1" href="<%= RouteContext.GetRouteUrl("Notes.Boards.ArchivedTasks", Model.GetNodeId) %>"><i class="si si-cloud-download pull-right"></i>查看归档</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <!-- END Page Header -->
    <div class="content">
        <input type="hidden" id="hidNodeId" value="<%= Model.GetNodeId %>" />
        <input type="hidden" id="hidTeamId" value="<%= Model.Node.TeamId %>" />
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
        <div class="board" id="board-column-{{column.Id}}" data-columnid="{{column.Id}}" data-title="{{column.Title}}">
            <div class="board-inner ">
                <header class="board-header">
                    <h3 class="board-title">
                        <span class="board-title-text label">{{column.Title}}</span>
                        <span class="board-title-text-input label hidden" style="background: none;">
                            <input type="text" class="form-control" />
                        </span>
                        <button type="button" class="board-delete js-listset" data-toggle="dropdown" data-original-title="列表设置"><i class="fa fa-angle-down"></i></button>
                        <ul class="dropdown-menu dropdown-menu-right">
                            <li class="dropdown-header">列表菜单</li>
                            <li>
                                <a tabindex="-1" href="javascript:void(0)" class="js-archived">
                                    <i class="si si-cloud-download pull-right"></i>归档已完成任务
                                </a>
                            </li>
                            <li class="divider"></li>
                            <li>
                                <a tabindex="-1" href="javascript:void(0)" class="js-deleteColumn">
                                    <i class="si si-trash pull-right"></i>删除列表
                                </a>
                            </li>
                        </ul>
                        <div class="board-count-badge clearfix">
                            <button type="button" class="board-count-badge-button no-right-border btn btn-sm btn-default hidden" data-toggle="tooltip" data-original-title="编辑列表"><i class="fa fa-pencil"></i></button>
                            <button type="button" class="board-count-badge-button btn btn-sm btn-default board-new" data-toggle="tooltip" data-original-title="新任务"><i class="fa fa-plus"></i></button>
                        </div>
                    </h3>
                </header>
                <div class="board-list-component">
                    <div class="board-new-form hidden" style="border: 0;">
                        <div class="card">
                            <label>
                                标题
                            </label>
                            <input type="text" autocomplete="off" class="form-control">
                            <div class="clearfix">
                                <button type="button" disabled="disabled" data-mode="1" class="btn btn-success pull-left">
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
        <li id="board-content-{{content.Id}}" class="js-block-task" data-id="{{content.Id}}" data-columnid="{{content.ColumnId}}">
            <div class="block">
                <div class="block-content">
                    <label class="css-input css-checkbox css-checkbox-success">
                        <input type="checkbox"><span></span></label>
                    <a href="javascript:;">{{content.Title}}</a>
                </div>
                <div class="block-tags js-block-tags">
                    <ul class="clearfix">
                        <li class="js-content hidden js-tooltip" title="任务备注"><i class="fa fa-list"></i></li>
                        <li class="time js-expirationDate hidden"><i class="fa fa-clock-o"></i>2017-12-25</li>
                        <%--<li><label class="label" style="background:#000;">111</label></li>
                                        <li data-type='tag'><label class="label" style="background:#000;">111</label></li>
                                        <li><label class="label" style="background:#000;">111</label></li>
                                        <li><label class="label" style="background:#000;">111</label></li>
                                        <li><label class="label" style="background:#000;">111</label></li>--%>
                        <%--<li data-type='tag'><label class="label" style="background:#000;">紧急</label></li>
                                        <li data-type='tag'><label class="label" style="background:#000;">紧急中中</label></li>--%>
                    </ul>
                </div>
                <div class="block-users js-block-users clearfix">
                    <%--<div class="item item-circle bg-info-light text-info">施俊彦</div>
                                     <div class="item item-circle bg-info-light text-info">陈三</div>--%>
                </div>
            </div>
        </li>
    </script>

    <script id="tempAvatarPreview" type="text/x-handlebars-template">
        <div class='item item-circle bg-info-light text-info js-popover' data-content='<div class="text-center ribbon ribbon-modern ribbon-success"><div style="position:absolute;left:0px;bottom:0px;background:rgba(0,0,0,.5);height:25px;line-height:25px;color:#fff;width:100%;">{{user.NickName}}</div><p><img src="{{user.AvatarUrl}}" width="150" /></p></div>'>
            <img src="{{user.AvatarUrl}}" width="100%" style="margin-top:-5px;" />
        </div>
    </script>


    <!-- end template -->

    <script type="text/javascript" src="/js/notes/boards/index.controller.js"></script>
</asp:Content>
