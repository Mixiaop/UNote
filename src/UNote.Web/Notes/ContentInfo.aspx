<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/User.master" AutoEventWireup="true" CodeBehind="ContentInfo.aspx.cs" Inherits="UNote.Web.Notes.ContentInfo" %>

<%@ Register Src="~/Controls/RightColumn.ascx" TagPrefix="uc1" TagName="RightColumn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="userHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="userBody" runat="server">
     <!-- Page Header -->
                            <div class="page-header">
                                <div class="row">
                                    <div class="col-lg-3 col-xs-8">
                                       <ul>
                                           <li><%= Model.Content.Node.TeamId>0?Model.Content.Node.Team.Name:"我的笔记" %></li>
                                           <% foreach(var node in Model.Parents){ %>
                                           <li><span>/</span> <a href="<%= RouteContext.GetRouteUrl("Notes.Contents", node.Id) %>"><%= node.NodeName %></a></li>
                                           <%} %>
                                           <li><span>/</span> <a href="<%= RouteContext.GetRouteUrl("Notes.Contents", Model.Content.NodeId) %>"><%= Model.Content.Node.NodeName %></a></li>
                                           <li class="current"><span>/</span> <%= Model.Content.Title %></li>
                                       </ul>
                                    </div>
                                    <div class="col-lg-5 col-xs-4 text-right viewtype-wrapper">
                                    </div>
                                </div>
                            </div>
                <!-- END Page Header -->
                <!-- Classic Content -->
                <div class="content note-contentinfo">
                    <!-- Section Content -->
                    <div class="">
                        <div class="row">
                            <div class="col-xs-12 bg-white contentinfo">
                                <div >
                    <div class="content content-boxed">
                        <!-- Section Content -->
                        <h1 class="text-black text-center push-30-t push-10" ><%= Model.Content.Title %></h1>
                        <div class="text-center" >
                            <em><%= Model.Content.CreationTime.ToString("yyyy-MM-dd HH:mm") %></em>
                           <%-- <%= Model.Content.LastModificationTime.HasValue?"最后编辑于 "+Model.Content.LastModificationTime.Value.ToString("yyyy-MM-dd HH:mm"):"" %> --%>

                        </div>
                        <div class="row push-50-t push-50 nice-copy-story">
                            <div class="col-sm-12">
                                <div class="noteContent">
                                    <%= Model.Content.Body %>
                                </div>
                                <!-- Actions -->
                                <%--<% if(Model.LoginedUser.Id==Model.Content.UserId){ %>--%>
                                <div class="push-50-t clearfix">
                                    <div class="btn-toolbar pull-center">
                                        <a class="btn btn-default" data-toggle="tooltip" title="编辑" href="<%=RouteContext.GetRouteUrl("Notes.EditContent",Model.ContentId, 0, U.Utilities.Web.WebHelper.GetUrl().EncodeUTF8Base64())%>"><i class="fa fa-edit"></i></a>
                                        <% if(Model.Content.FileSize>0){ %>
                                        <a class="btn btn-default" data-toggle="tooltip" title="文件下载" href="<%= Model.Content.FileUrl %>"><i class="fa fa-download"></i> <%= Model.Content.FormatFileSize %></a>
                                        <%} %>
                                        <a class="btn btn-default" data-toggle="tooltip" title="下载 pdf" href="/notes/download.aspx?type=pdf&contentid=<%= Model.ContentId %>" target="_blank"><i class="fa fa-download"></i> .pdf</a>
                                        <a class="btn btn-default" data-toggle="tooltip" title="下载 doc" href="/notes/download.aspx?type=doc&contentid=<%= Model.ContentId %>" target="_blank"><i class="fa fa-download"></i> .doc</a>
                                        
                                        
                                    </div>
                                </div>
                                <%--<%} %>--%>
                                <!-- END Actions -->
                            </div>
                        </div>
                        <!-- END Section Content -->
                    </div>
                </div>
                            </div>
                            <%--<div class="col-md-4">
                                <uc1:RightColumn runat="server" ID="RightColumn" />
                            </div>--%>
                        </div>
                    </div>
                    <!-- END Section Content -->
                </div>
                <!-- END Classic Content -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="userFoot" runat="server">
</asp:Content>
