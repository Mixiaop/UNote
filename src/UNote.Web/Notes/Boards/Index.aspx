﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/User.master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="UNote.Web.Notes.Boards.Index" %>

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
                    <li class="current"><span>/</span> <%= Model.Node.NodeName %>&nbsp;&nbsp;<a class="btn-options"><i class="fa fa-cog"></i></a></li>
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
    
    <script type="text/javascript" src="/js/notes/boards/index.controller.js"></script>
</asp:Content>
