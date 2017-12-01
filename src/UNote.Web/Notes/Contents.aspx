<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/User.master" AutoEventWireup="true" CodeBehind="Contents.aspx.cs" Inherits="UNote.Web.Notes.Contents" %>

<%@ Register Src="~/Controls/RightColumn.ascx" TagPrefix="uc1" TagName="RightColumn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="userHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="userBody" runat="server">
    <!-- Page Header -->
    <div class="page-header">
        <div class="row">
            <div class="col-lg-3 col-xs-8">
                <ul>
                    <li><%= Model.Node.TeamId>0?Model.Node.Team.Name:"我的笔记" %></li>
                    <% foreach (var node in Model.Parents)
                       { %>
                    <li><span>/</span> <a href="<%= RouteContext.GetRouteUrl("Notes.Contents", node.Id) %>"><%= node.NodeName %></a></li>
                    <%} %>
                    <li class="current"><span>/</span> <%= Model.Node.NodeName %></li>
                </ul>
            </div>
            <div class="col-lg-5 col-xs-4 text-right viewtype-wrapper">
                <%--<% if (Model.Node.ListShowType == UNote.NodeListShowType.List)
                   { %>
                <a class="viewtype active" data-type="1" data-toggle="tooltip" title="列表模式"><i class="si si-list"></i></a><a href="javascript:;" class="viewtype viewtype-onactive" data-type="2" data-toggle="tooltip" title="网格模式"><i class="si si-grid"></i></a>
                <%}
                   else
                   { %>
                <a href="javascript:;" class="viewtype viewtype-onactive" data-type="1" data-toggle="tooltip" title="列表模式"><i class="si si-list"></i></a><a class="viewtype active" data-type="2" data-toggle="tooltip" title="网格模式"><i class="si si-grid"></i></a>
                <%} %>--%>
            </div>
        </div>
    </div>
    <!-- END Page Header -->
    <% if (Model.Tags != null && Model.Tags.Count > 0)
       { %>
    <div class="row filter-tags-wrapper">
        <div class="col-lg-8 col-xs-12">

            <ul class="filter-tags">
                <li><a href="<%= RouteContext.GetRouteUrl("Notes.Contents", Model.GetNodeId, Model.GetKeywords) %>">全部</a></li>
                <% foreach (var tag in Model.Tags)
                   { %>
                <% if (Model.GetTag.Contains(tag.Name))
                   { %>
                <li><a class="active"><%= tag.Name %></a><a href="<%=RouteContext.GetRouteUrl("Notes.Contents", Model.GetNodeId) %>" class="tag-close"><i class="fa fa-close"></i></a></li>
                <%}
                   else
                   { %>
                <li><a href="<%= RouteContext.GetRouteUrl("Notes.Contents", Model.GetNodeId, Model.GetKeywords, tag.Name) %>"><%= tag.Name %></a></li>
                <%} %>
                <%} %>
            </ul>

        </div>
    </div>
    <%} %>
    <!-- Content -->
    <div class="content note-contents">
        <div class="">
            <div class="row">
                <div class="col-md-8">
                    <% if (Model.Node.ListShowType == UNote.NodeListShowType.List)
                       { %>
                    <!-- Normal -->
                    <% if (Model.ContentList.TotalCount > 0)
                       {
                           foreach (var content in Model.ContentList.Items)
                           { 
                    %>
                    <div class="block">

                        <div class="block-content note-contents-item">
                            <div class="row items-push">
                                <% if (content.FormatPreviewUrl.IsNotNullOrEmpty())
                                   { %>
                                <div class="col-md-4">
                                    <% if (content.FormatNodeType == UNote.NodeType.Html)
                                       { %>
                                    <a href="<%= content.NodeHtmlHomePage %>" title="查看" target="_blank" data-id="<%= content.Id %>" class="AddVisitCount">
                                        <img class="img-responsive" src="<%= content.FormatPreviewUrl %>" alt="<%= content.Title %>">
                                    </a>
                                    <%}
                                       else
                                       { %>
                                    <a href="<%= RouteContext.GetRouteUrl("Notes.ContentInfo", content.Id) %>" title="查看" target="_blank" class="AddVisitCount">
                                        <img class="img-responsive" src="<%= content.FormatPreviewUrl %>" alt="<%= content.Title %>">
                                    </a>
                                    <%} %>
                                </div>
                                <%} %>
                                <div class="col-md-<%= content.FormatPreviewUrl.IsNotNullOrEmpty()?"8":"12" %>">
                                    <!-- options -->
                                    <div class="block-options-simple btn-group btn-group-xs">
                                        <div class="btn-group btn-group-xs">
                                            <button class="btn btn-default btn-more" type="button" data-toggle="dropdown" title="更多操作">...</button>
                                            <ul class="dropdown-menu dropdown-menu-right">
                                                <li>
                                                    <a tabindex="-1" href="<%=RouteContext.GetRouteUrl("Notes.EditContent",content.Id, 0, U.Utilities.Web.WebHelper.GetUrl().EncodeUTF8Base64())%>">编辑</a>
                                                </li>
                                                <li>
                                                    <a tabindex="-1" href="javascript:void(0)" data-id="<%= content.Id %>" name="btnRemove">删除</a>
                                                </li>
                                                <li class="divider"></li>
                                                <li><a tabindex="-1" href="<%=RouteContext.GetRouteUrl("Notes.AddContentItem",content.Id, content.NodeId, content.Node.NodeTypeId, U.Utilities.Web.WebHelper.GetUrl().EncodeUTF8Base64())%>">添加内容项</a></li>
                                            </ul>
                                        </div>
                                        <% if (content.FileUrl.IsNotNullOrEmpty())
                                           { %>
                                        <a href="<%= content.FileUrl %>" class="btn btn-default" data-toggle="tooltip" title="下载附件"><i class="fa fa-download"></i><%= content.FormatFileSize %></a>
                                        <%} %>
                                    </div>
                                    <!-- End options -->
                                    <%--<div class="font-s12 push-10">
                                                                <% if (content.TeamId > 0 && content.User != null)
                                                                   { %>
                                                                <%} %> <%= content.LastModificationTime.HasValue ?("最后编辑 "+content.FormatLastModificationTime):"" %>
                                                            </div>--%>
                                    <h4 class="text-uppercase push-10">
                                        <% if (content.FormatNodeType == UNote.NodeType.Html)
                                           { %>
                                        <a class="text-primary-dark AddVisitCount" href="<%= content.NodeHtmlHomePage %>" target="_blank" title="<%= content.Title %>" data-id="<%= content.Id %>"><%= content.Title %></a>
                                        <%}
                                           else
                                           { %>
                                        <a class="text-primary-dark AddVisitCount" href="<%= RouteContext.GetRouteUrl("Notes.ContentInfo", content.Id) %>" title="<%= content.Title %>" target="_blank"><%= content.Title %></a><%} %></h4>
                                    <p class="push-20"><%= content.Summary %><%= !content.Summary.IsNullOrEmpty()?"...":"" %></p>
                                    <!-- ContentItems-->
                                    <% if(content.ContentItemCount > 0){ %>
                                    <ul class="list list-timeline pull-t" style="padding:0;padding:10px 0 10px 0;">
                                        <% foreach(var item in content.ContentItems){ %>
                                        <li>
                                             <div class="list-timeline-time"><%= item.FormatCreationTime %></div>
                                             <i class="fa fa-file-o list-timeline-icon bg-primary"></i>
                                            <div class="list-timeline-content">
                                                <p class="font-s13"><a href="<%= RouteContext.GetRouteUrl("Notes.ContentInfo", item.Id) %>" data-toggle="tooltip" title="<%= item.Title %>" class="node" target="_blank"><%= item.Title %></a> &nbsp;&nbsp;<a href="<%=RouteContext.GetRouteUrl("Notes.EditContent", item.Id, 0, U.Utilities.Web.WebHelper.GetUrl().EncodeUTF8Base64())%>" data-toggle="tooltip" title="编辑" ><i class="fa fa-edit"></i></a></p>
                                            </div>
                                        </li>
                                        <%} %>
                                    </ul>
                                    <%} %>
                                    <!-- Tags -->
                                    <p class="tags">
                                        <% if (content.Node != null)
                                           { %>
                                        <a href="<%= RouteContext.GetRouteUrl("Notes.Contents", content.NodeId, Model.GetKeywords, Model.GetTag) %>" data-toggle="tooltip" title="所属目录"><span class="label label-info push-5-r"><%= content.Node.NodeName %></span></a>
                                        <%} %>
                                        <% if (content.IsTop)
                                           { %>
                                        <a class="istop"><span class="label label-info push-5-r">置顶</span></a>
                                        <%} %>
                                        <% foreach (var tag in content.FormatTags)
                                           { %>
                                        <a href="<%= RouteContext.GetRouteUrl("Notes.Contents", content.NodeId, Model.GetKeywords, GetTag(tag)) %>"><span class="label label-info push-5-r"><%= tag %></span></a>
                                        <%} %>
                                    </p>
                                    <!-- Followers -->
                                    <p>
                                        <% 
                                            if (content.Followers != null)
                                            {
                                                foreach (var follower in content.Followers)
                                                {
                                                    if (follower != null && follower.User != null)
                                                    { %>
                                        <div class="item item-circle bg-info-light text-info unote-avatar unote-avatar">
                                            <%= follower.User.FormatNickName %>
                                        </div>
                                        <%}
        }
    } %>
                                    </p>
                                </div>
                                <% if (content.VisitCount > 0)
                                   { %>
                                <p class="visitcount">
                                    <%--<i class="fa fa-eye" title="共 <%= content.VisitCount %> 次浏览"></i> <%= content.VisitCount %> --%>
                                    <%= content.VisitCount %> 次浏览
                                </p>
                                <%} %>
                                <% if (content.TeamId > 0)
    {
        if (content.User != null)
        {%>
                                <em class="pull-right createtime"><a><%= content.User.FormatNickName %></a> <%= content.FormatCreationTime %></em>
                                <%}
    }
    else
    { %>
                                <em class="pull-right createtime"><%= content.FormatCreationTime %></em>
                                <%} %>
                            </div>
                        </div>
                    </div>
                    <%}
                                   }
                       else
                       {
                           Response.Write("<div class=\"note-none\">没有笔记.</div>");
                       } %>
                    <!-- END Normal -->
                    <%}
                       else
                       { %>
                    <!-- Grid  -->
                    <% if (Model.ContentList.TotalCount > 0)
                       {
                           var index = 1;
                           var nextBreak = 1;
                           foreach (var content in Model.ContentList.Items)
                           { 
                    %>
                    <% if (index == nextBreak)
                       {
                           nextBreak += 3;
                    %>
                    <div class="content content-boxed">
                        <div class="row">
                            <%} %>
                            <!-- Item -->
                            <div class="col-md-4">
                                <div class="block note-contents-grid">
                                    <% if (content.FormatPreviewUrl.IsNotNullOrEmpty())
                                       { %>
                                    <% if (content.FormatNodeType == UNote.NodeType.Html)
                                       { %>
                                    <a href="<%= content.NodeHtmlHomePage %>" title="查看" target="_blank" data-id="<%= content.Id %>" class="AddVisitCount">
                                        <img class="img-responsive" src="<%= content.FormatPreviewUrl %>" alt="<%= content.Title %>">
                                    </a>
                                    <%}
                                       else
                                       { %>
                                    <a href="javascript:;" title="查看" name="btnView" data-id="<%= content.Id %>" class="AddVisitCount">
                                        <img class="img-responsive" src="<%= content.FormatPreviewUrl %>" alt="<%= content.Title %>">
                                    </a>
                                    <%}
                                       }%>
                                    <div class="block-content">
                                        <!-- options -->
                                        <div class="block-options-simple btn-group btn-group-xs">
                                            <div class="btn-group btn-group-xs">
                                                <button class="btn btn-default btn-more" type="button" data-toggle="dropdown" title="更多操作">...<%--<span class="caret"></span>--%></button>
                                                <ul class="dropdown-menu dropdown-menu-right">
                                                    <li>
                                                        <a tabindex="-1" href="<%= RouteContext.GetRouteUrl("Notes.EditContent", content.Id, 0, U.Utilities.Web.WebHelper.GetUrl().EncodeUTF8Base64())%>">编辑</a>
                                                    </li>
                                                    <li>
                                                        <a tabindex="-1" href="javascript:;" data-id="<%= content.Id %>" data-nodeid="<%= content.NodeId %>" name="btnMovie">移动</a>
                                                    </li>
                                                    <li>
                                                        <a tabindex="-1" href="javascript:void(0)" data-id="<%= content.Id %>" name="btnRemove">删除</a>
                                                    </li>
                                                    <%--<li class="divider"></li>
                                                                        <li class="dropdown-header">pics</li>
                                                                         <li>
                                                                            <a tabindex="-1" href="javascript:void(0)" data-id="<%= content.Id %>" name="btnRemove">上传</a>
                                                                             <a tabindex="-1" href="javascript:void(0)" data-id="<%= content.Id %>" name="btnRemove">管理</a>
                                                                        </li>--%>
                                                </ul>
                                            </div>
                                            <% if (content.FileUrl.IsNotNullOrEmpty())
                                               { %>
                                            <a href="<%= content.FileUrl %>" class="btn btn-default" data-toggle="tooltip" title="下载附件"><i class="fa fa-download"></i><%= content.FormatFileSize %></a>
                                            <%} %>
                                        </div>
                                        <!-- End options -->

                                        <h4 class="push-10"><% if (content.FormatNodeType == UNote.NodeType.Html)
                                                               { %>
                                            <a class="text-primary-dark AddVisitCount" href="<%= content.NodeHtmlHomePage %>" target="_blank" title="<%= content.Title %>" data-id="<%= content.Id %>"><%= content.Title %></a>
                                            <%}
                                                               else
                                                               { %>
                                            <a class="text-primary-dark AddVisitCount" href="javascript:;" title="<%= content.Title %>" name="btnView" data-id="<%= content.Id %>"><%= content.Title %></a><%} %></h4>
                                        <p><%= content.Summary %><%= !content.Summary.IsNullOrEmpty()?"...":"" %></p>
                                        <p class="tags">
                                            <% if (content.Node != null)
                                               { %>
                                            <a href="<%= RouteContext.GetRouteUrl("Notes.Contents", content.NodeId, Model.GetKeywords, Model.GetTag)%>" data-toggle="tooltip" title="所属目录"><span class="label label-info push-5-r"><%= content.Node.NodeName %></span></a>
                                            <%} %>
                                            <% if (content.IsTop)
                                               { %>
                                            <a class="istop"><span class="label label-info push-5-r">置顶</span></a>
                                            <%} %>
                                            <% foreach (var tag in content.FormatTags)
                                               { %>
                                            <a href="<%= RouteContext.GetRouteUrl("Notes.Contents", content.NodeId, Model.GetKeywords, tag) %>"><span class="label label-info push-5-r"><%= tag %></span></a>
                                            <%} %>
                                        </p>
                                        <% if (content.VisitCount > 0)
                                           { %>
                                        <p class="visitcount grid-visitcount">
                                            <%= content.VisitCount %> 次浏览
                                        </p>
                                        <%} %>
                                        <div class="font-s12 push">
                                            <em class="pull-right createtime"><a><%= content.User.FormatNickName %></a> <%= content.FormatCreationTime %></em>
                                            <div class="clearfix"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- END Item -->
                            <% index++; %>
                            <% if (index == nextBreak || index == Model.ContentList.Items.Count + 1)
                               {%>
                        </div>

                    </div>
                    <%}%>
                    <%}
                                   }
                       else
                       {
                           Response.Write("<div class=\"note-none\">没有笔记.</div>");
                       } %>
                    <!-- END Grid  -->
                    <%} %>

                    <!-- Pagination -->
                    <nav>
                        <ul class="pagination">
                            <%= Model.PageHtml %>
                        </ul>
                    </nav>
                    <!-- END Pagination -->
                </div>
                <div class="col-md-4">
                    <uc1:RightColumn runat="server" ID="RightColumn" />
                </div>
            </div>
        </div>
    </div>
    <!-- END  Content -->

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="userFoot" runat="server">
    <script>
        require(['jquery', 'notes/contentInfoDialog', 'notes/moveContentDialog', 'utils/notify'], function ($, contentInfoDialog, moveContentDialog, notify) {
            var $btnRemove = $('a[name=btnRemove]');
            var $btnMove = $('a[name=btnMovie]');
            var $viewTypeWrapper = $('.viewtype-wrapper');

            $btnMove.on('click', function () {
                var contentId = parseInt($(this).data('id'));
                var nodeId = parseInt($(this).data('nodeid'));
                moveContentDialog.open();
            });

            $btnRemove.on('click', function () {
                var contentId = parseInt($(this).data('id'));
                if (confirm('你确认删除这篇笔记吗？')) {
                    ContentService.Delete(contentId, function (res) {
                        var json = res.value;
                        if (json.Success) {
                            notify.success('删除成功');
                            setTimeout(function () { window.location.href = window.location.href }, 500);
                        } else {
                            notify.error(json.Error.Message);
                        }
                    });
                }

            });

            //view
            $('a[name=btnView]').on('click', function () {
                var contentId = parseInt($(this).data('id'));
                contentInfoDialog.open({ contentId: contentId });
            });            $viewTypeWrapper.find('.viewtype-onactive').on('click', function () {
                var type = parseInt($(this).data('type'));                NodeService.SetListShowType(parseInt("<%= Model.Node.Id%>"), type, function (res) {
                    var json = res.value;
                    if (json.Success) {
                        window.location.href = window.location.href
                    } else {
                        notify.error(json.Error.Message);
                    }
                });
            });            //addVisitCount            $('.AddVisitCount').on('click', function () {
                var contentId = parseInt($(this).data('id'));                if (contentId > 0) {
                    ContentService.AddVisitCount(contentId, function (res) { });
                }
            });

        });
    </script>
</asp:Content>
