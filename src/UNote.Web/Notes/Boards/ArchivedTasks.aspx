<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/User.master" AutoEventWireup="true" CodeBehind="ArchivedTasks.aspx.cs" Inherits="UNote.Web.Notes.Boards.ArchivedTasks" %>
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
                    <li><span>/</span> <a href="<%= RouteContext.GetRouteUrl("Notes.Boards", Model.Node.Id) %>"><%= Model.Node.NodeName %></a></li>
                    <li class="current"><span>/</span> 归档的任务列表</li>
                </ul>
            </div>
            <div class="col-lg-9 col-xs-4 text-right viewtype-wrapper">
               
            </div>
        </div>
    </div>
    <!-- END Page Header -->
    <!-- Content -->
    <div class="content note-contents">
        <div class="">
            <div class="row">
                <div class="col-md-12">
                    <!-- Normal -->
                    <% if (Model.Result.TotalCount > 0)
                        {
                            foreach (var content in Model.Result.Items)
                            {
                    %>
                    <div class="block">

                        <div class="block-content note-contents-item">
                            <div class="row items-push">
                                <div class="col-md-12">
                                    <!-- options -->
                                    <div class="block-options-simple btn-group btn-group-xs">
                                        <div class="btn-group btn-group-xs">
                                            <button class="btn btn-default btn-more" type="button" data-toggle="dropdown" title="更多操作">...</button>
                                            <%--<ul class="dropdown-menu dropdown-menu-right">
                                                <li>
                                                    <a tabindex="-1" href="<%=RouteContext.GetRouteUrl("Notes.EditContent",content.Id, 0, U.Utilities.Web.WebHelper.GetUrl().EncodeUTF8Base64())%>">编辑</a>
                                                </li>
                                                <li>
                                                    <a tabindex="-1" href="javascript:void(0)" data-id="<%= content.Id %>" name="btnRemove">删除</a>
                                                </li>
                                                <li class="divider"></li>
                                                <li><a tabindex="-1" href="<%=RouteContext.GetRouteUrl("Notes.AddContentItem",content.Id, content.NodeId, content.Node.NodeTypeId, U.Utilities.Web.WebHelper.GetUrl().EncodeUTF8Base64())%>">添加内容项</a></li>
                                            </ul>--%>
                                        </div>
  
                                    </div>
                                    <!-- End options -->
                                    <h4 class="text-uppercase push-10">
                                        <a class="text-primary-dark AddVisitCount" href="<%= RouteContext.GetRouteUrl("Notes.ContentInfo", content.Id) %>" title="<%= content.Title %>" target="_blank"><%= content.Title %></a></h4>
                                    <p class="push-20"></p>
                                    <!-- ContentItems-->
                                    <!-- Tags -->
                                    <%--<p class="tags">
                                        <% if (content.Node != null)
                                           { %>
                                        <a href="<%= RouteContext.GetRouteUrl("Notes.Contents", content.NodeId) %>" data-toggle="tooltip" title="所属目录"><span class="label label-info push-5-r"><%= content.Node.NodeName %></span></a>
                                        <%} %>
                                        <% foreach (var tag in content.FormatTags)
                                           { %>
                                        <a href="<%= RouteContext.GetRouteUrl("Notes.Contents", content.NodeId, Model.GetKeywords, GetTag(tag)) %>"><span class="label label-info push-5-r"><%= tag %></span></a>
                                        <%} %>
                                    </p>--%>
                                    <!-- Followers -->
                                    <p>
                                        <% 
                                            if (content.Followers != null)
                                            {
                                                foreach (var follower in content.Followers)
                                                {
                                                    if (follower != null )
                                                    { %>
                                        <div class="item item-circle bg-info-light text-info unote-avatar unote-avatar">
                                            <%= follower.NickName %>
                                        </div>
                                        <%}
        }
    } %>
                                    </p>
                                </div>
                                <%--<% if (content.VisitCount > 0)
                                   { %>
                                <p class="visitcount">
                                    <%= content.VisitCount %> 次浏览
                                </p>
                                <%} %>--%>
                                <em class="pull-right createtime"><%= content.CreationTime.ToString("yyyy-MM-dd HH:mm") %></em>
                            </div>
                        </div>
                    </div>
                    <%}
                        }
                        else
                        {
                            Response.Write("<div class=\"note-none\">没有数据.</div>");
                        } %>
                    <!-- END Normal -->
                    </div>
                    <!-- Pagination -->
                    <nav>
                        <ul class="pagination">
                            <%= Model.PagingHtml   %>
                        </ul>
                    </nav>
                    <!-- END Pagination -->
                </div>
            </div>
        </div>
    <!-- END  Content -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="userFoot" runat="server">
</asp:Content>
