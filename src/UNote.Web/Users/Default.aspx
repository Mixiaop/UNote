<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/User.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="UNote.Web.Users.Default" %>

<%@ Register Src="~/Controls/RightColumn.ascx" TagPrefix="uc1" TagName="RightColumn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="userHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="userBody" runat="server">
    <!-- Page Header -->
    <div class="content bg-image" style="background-image: url('/img/various/promo-code.png');">
        <div class="push-50-t push-15 clearfix">
            <div class="push-15-r pull-left animated fadeIn">
                <img class="img-avatar img-avatar-thumb" src="<%= Model.LoginedUser.AvatarUrl.IsNotNullOrEmpty()?Model.LoginedUser.AvatarUrl:"/img/avatars/avatar13.jpg" %>" alt="">
            </div>
            <h1 class="h2 text-white push-5-t animated zoomIn"><%= Model.LoginedUser.FormatNickName %></h1>
            <h2 class="h5 text-white-op animated zoomIn"><%= Model.LoginedUser.Username %></h2>
        </div>
    </div>
    <!-- END Page Header -->
    <!-- Stats -->
    <div class="content bg-white border-b">
        <div class="row items-push text-uppercase">
            <div class="col-xs-3 col-sm-3">
                <div class="font-w700 text-gray-darker animated fadeIn">Notes</div>
                <a class="h2 font-w300 text-primary animated flipInX"><%= Model.NotesCount %></a>
            </div>
            <div class="col-xs-3 col-sm-3">
                <div class="font-w700 text-gray-darker animated fadeIn">FOLLOWERS</div>
                <a class="h2 font-w300 text-primary animated flipInX"><%= Model.FollowersCount %></a>
            </div>
            <div class="col-xs-3 col-sm-3">
                <div class="font-w700 text-gray-darker animated fadeIn">Movies</div>
                <a href="http://www.mbjuan.com" target="_blank" class="h3 font-w300 text-primary animated flipInX">M</a>
            </div>
        </div>
    </div>
    <!-- END Stats -->
    <!-- Page Content -->
    <div class="content content-boxed">

        <div class="row">
            <div class="col-sm-6">
                <div class="block">
                    <div class="block-header">
                        <h3 class="block-title">技术研发部小花园</h3>
                    </div>
                    <div class="block-content block-content-full text-center">
                        <div class="push-30">
                            <i class="fa fa-stack-overflow fa-5x push-30-t"></i>
                        </div>
                        <a class="btn btn-minw btn-rounded btn-noborder btn-success push-5 push-30-t" href="/img/content/技术部大纲 v1.0.pdf" target="_blank">Play It</a>
                        <h3 class="font-w300 text-black push-30-t"><a href="/img/content/技术部大纲 v1.0.pdf" target="_blank">了解技术部规范</a></h3>
                        <div class="mheight-150">
                            <p class="text-gray-dark">Know about the technology department</p>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="block">
                    <div class="block-header">
                        <h3 class="block-title">.Net开发必读</h3>
                    </div>
                    <div class="block-content block-content-full text-center">
                        <div class="push-30">
                            <a href="/img/avatars/avatar18.jpg" target="_blank">
                                <img class="img-avatar img-avatar96 img-avatar-thumb push-30-t" src="/img/avatars/avatar17.jpg" alt=""></a>
                        </div>
                        <a class="btn btn-minw btn-rounded btn-noborder btn-success push-5" href="http://imgnote.youzy.cn/content/media/files/f00004100.pdf" target="_blank">Know It</a>
                        <h3 class="font-w300 text-black push-30-t"><a href="http://imgnote.youzy.cn/content/media/files/f00004100.pdf" target="_blank">了解秘籍</a></h3>
                        <div class="mheight-150">
                            <p class="text-gray-dark">To be the high hand's road.</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row hide">
            <div class="col-sm-12 col-lg-12">
                <!-- Timeline -->
                <div class="block">
                    <div class="block-header bg-gray-lighter">
                        <ul class="block-options">
                        </ul>
                        <h3 class="block-title"><i class="fa fa-pencil"></i>VisitLogs</h3>
                    </div>
                    <div class="block-content">
                        <ul class="list list-timeline pull-t">
                            <% if (Model.VisitLogs != null && Model.VisitLogs.Count > 0)
                                {
                                    foreach (var log in Model.VisitLogs)
                                    {
                                        //if (log.Content != null) { 
                            %>
                            <!-- Node -->
                            <li>
                                <div class="list-timeline-time"><%= log.FormatLastVisitTime %></div>
                                <i class="fa <%= log.TypeId==1?"fa-folder-o":"fa-file-o" %> list-timeline-icon <%= log.TypeId==1?"bg-modern":"bg-success" %>"></i>
                                <div class="list-timeline-content">
                                    <% if (log.TypeId == 1)
                                        { %>
                                    <p class="font-s13">看了目录 <a href="<%= RouteContext.GetRouteUrl("Notes.Contents", log.NodeId) %>" data-toggle="tooltip" title="所属目录" class="node"><%= log.Node.NodeName %></a></p>
                                    <p class="font-w600"></p>
                                    <%}
                                        else
                                        {
                                            if (log.Content != null)
                                            { %>
                                    <p class="font-w600"></p>
                                    <p class="font-s13">看了笔记 <a href="<%= RouteContext.GetRouteUrl("Notes.ContentInfo", log.ContentId) %>" title="<%= log.Content.Title %>" target="_blank"><%= log.Content.Title %></a></p>
                                    <%}
                                        } %>
                                </div>
                            </li>
                            <!-- END Node -->
                            <%//}

                                    }
                                }%>
                            <!-- System Notification -->
                            <li></li>
                            <!-- END System Notification -->
                        </ul>
                    </div>
                </div>
                <!-- END Timeline -->
            </div>
            <div class="col-sm-5 col-lg-4 hidden">
                <!-- Followers -->
                <div class="block">
                    <div class="block-header bg-gray-lighter">
                        <ul class="block-options">
                            <li></li>
                        </ul>
                        <h3 class="block-title"><i class="fa fa-fw fa-share-alt"></i>Followers</h3>
                    </div>
                    <div class="block-content rc-recentnotes">
                        <div class="block-content bg-white">
                            <ul class="list list-simple">

                                <% if (Model.FollowersCount > 0)
                                    {

                                        foreach (var info in Model.Followers)
                                        {
                                            var content = info.Content;
                                %>
                                <li>

                                    <div class="push-5 clearfix">
                                        <span class="font-s13 text-muted push-10-l pull-right">&nbsp; <%= content.FormatCreationTime %></span>
                                        <% if (content.Node != null)
                                            { %>
                                        <a href="<%= RouteContext.GetRouteUrl("Notes.Contents", content.NodeId) %>" data-toggle="tooltip" title="所属目录" class="node"><span class="label label-info push-5-r"><%= content.Team!=null?content.Team.Name+" / ":"" %> <%= content.Node.NodeName %></span></a>
                                        <%} %>
                                        <% if (content.Node.NodeType == UNote.NodeType.Html)
                                            { %>
                                        <a class="title" href="<%= content.NodeHtmlHomePage %>" target="_blank" title="<%= content.Title %>"><%= content.Title %></a>
                                        <%}
                                            else
                                            { %>
                                        <a href="<%= RouteContext.GetRouteUrl("Notes.ContentInfo", info.ContentId) %>" title="<%= content.Title %>" target="_blank" class="title"><%= content.Title %></a>
                                        <%} %>
                                    </div>
                                    <%--<div class="font-s13"><%= content.Summary %></div>--%>
                                                
                                </li>
                                <%} %>
                                <%}
                                    else
                                    { %>
                                <li>no followers</li>
                                <%} %>
                            </ul>
                        </div>
                        <%-- <div class="text-center push">
                                        <small><a href="javascript:void(0)">Load More..</a></small>
                                    </div>--%>
                    </div>
                </div>
                <!-- END Followers -->
            </div>
        </div>
    </div>
    <!-- END Page Content -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="userFoot" runat="server">
</asp:Content>
