<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/User.master" AutoEventWireup="true" CodeBehind="ContentInfo.aspx.cs" Inherits="UNote.Web.Public.ContentInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="userHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="userBody" runat="server">
                <!-- Classic Content -->
                <section class="content">
                    <!-- Section Content -->
                    <div class="">
                        <div class="row">
                            <div class="col-md-8 bg-white">
                                <div class="bg-white">
                    <section class="content content-boxed">
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
                                <% if(Model.LoginedUser.Id==Model.Content.UserId){ %>
                                <div class="push-50-t clearfix">
                                    <div class="btn-toolbar pull-center">
                                        <% if(Model.Content.FileSize>0){ %>
                                        <a class="btn btn-default" data-toggle="tooltip" title="文件下载" href="<%= Model.Content.FileUrl %>"><i class="fa fa-download"></i> <%= Model.Content.FormatFileSize %></a>
                                        <%} %>
                                        <a class="btn btn-default" data-toggle="tooltip" title="编辑" href="EditContent.aspx?contentId=<%= Model.ContentId %>"><i class="fa fa-edit"></i></a>
                                    </div>
                                </div>
                                <%} %>
                                <!-- END Actions -->
                            </div>
                        </div>
                        <!-- END Section Content -->
                    </section>
                </div>
                                
                               
                            </div>
                            <div class="col-md-4">
                                <!-- Search -->
                                <div class="block">
                                    <div class="block-header bg-gray-lighter">
                                        <h3 class="block-title">Search</h3>
                                    </div>
                                    <div class="block-content block-content-full">
                                        <form action="frontend_search.html" method="post">
                                            <div class="input-group input-group-lg">
                                                <input class="form-control" type="text" placeholder="关键字">
                                                <div class="input-group-btn">
                                                    <button class="btn btn-default"><i class="fa fa-search"></i></button>
                                                </div>
                                            </div>
                                        </form>
                                    </div>
                                </div>
                                <!-- END Search -->

                                <!-- About -->
                               <%-- <a class="block block-link-hover3" href="AddContent.aspx?nodeid=<%= Model.NodeId %>" title="写笔记">
                                    <div class="block-content border-t">
                                        <div class="row items-push text-center">
                                            <div class="col-xs-12">
                                                <div class="push-5"><i class="si si-pencil fa-2x"></i></div>
                                                <div class="h5 font-w300 text-muted"><%= Model.Node.ContentTotal %> notes</div>
                                            </div>
                                          
                                        </div>
                                    </div>
                                </a>--%>
                                <!-- END About -->
                            </div>
                        </div>
                    </div>
                    <!-- END Section Content -->
                </section>
                <!-- END Classic Content -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="userFoot" runat="server">
</asp:Content>
