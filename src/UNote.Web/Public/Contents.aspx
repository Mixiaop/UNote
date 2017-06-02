<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/User.master" AutoEventWireup="true" CodeBehind="Contents.aspx.cs" Inherits="UNote.Web.Public.Contents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="userHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="userBody" runat="server">
    <div class="content bg-gray-lighter">
                    <div class="row items-push">
                        <div class="col-sm-7">
                            <h1 class="page-heading">
                                我的首页 <small>dribs and drabs，together</small>
                            </h1>
                        </div>
                    </div>
                </div>
                <!-- Classic Content -->
                <section class="content">
                    <!-- Section Content -->
                    <div class="">
                        <div class="row">
                            <div class="col-md-8">
                               
                                <!-- Story -->
                                <% if (Model.ContentList.TotalCount > 0)
                                   {
                                       foreach (var content in Model.ContentList.Items)
                                       { 
                                       %>
                                <div class="block">
                                    <div class="block-content">
                                        <div class="row items-push">
                                            <% if(content.FormatPreviewUrl.IsNotNullOrEmpty()){ %>
                                            <div class="col-md-4">
                                                <a href="ContentInfo.aspx?contentId=<%= content.Id %>">
                                                    <img class="img-responsive" src="<%= content.FormatPreviewUrl %>" alt="<%= content.Title %>">
                                                </a>
                                            </div>
                                            <%} %>
                                            <div class="col-md-<%= content.FormatPreviewUrl.IsNotNullOrEmpty()?"8":"12" %>">
                                                <div class="font-s12 push-10">
                                                    <em class="pull-right"><% if(content.User!=null){ %><a><%= content.User.NickName %></a><%}else{%><a>admin</a><%} %>&nbsp;&nbsp;<%= content.CreationTime.ToString("yyyy-MM-dd HH:mm") %></em>
                                                     <%= content.LastModificationTime.HasValue ?("最后编辑于 "+content.LastModificationTime.Value.ToString("yyyy-MM-dd HH:mm")):"" %> 
                                                </div>
                                                <h4 class="text-uppercase push-10"><a class="text-primary-dark" name="btnView" href="javascript:;" data-id="<%= content.Id %>" title="<%= content.Title %>"><%= content.Title %></a></h4>
                                                <p class="push-20"><%= content.Summary %><%= !content.Summary.IsNullOrEmpty()?"...":"" %></p>
                                               <p class="tags">
                                                    <% if(content.Node!=null){ %>
                                                    <a  title="所属目录 - <%= content.Node.NodeName %>"><span class="label label-info push-5-r"><%= content.Node.NodeName %></span></a>
                                                    <%} %>
                                                </p>
                                                <div class="btn-group btn-group-sm"> 
                                                    <a class="btn btn-default" title="查看" name="btnView" data-id="<%= content.Id %>"><i class="fa fa-search push-5-r"></i></a>
                                                    <%= content.FileUrl.IsNotNullOrEmpty()?"<a class=\"btn btn-default\" href=\""+content.FileUrl+"\" target=\"_blank\" title=\"下载\"><i class=\"fa fa-download push-5-r\"></i> "+content.FormatFileSize+"</a>":"" %>  
                                                    <%--<a class="btn btn-default" href="EditContent.aspx?contentid=<%= content.Id %>" title="编辑"><i class="fa fa-edit push-5-r"></i></a>
                                                    <a class="btn btn-default" href="javascript:void(0)" title="删除"><i class="fa fa-remove push-5-r"></i></a>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                    <%}
                                   } %>
                                <!-- END Story -->
                                <!-- Pagination -->
                                <nav>
                                    <ul class="pagination">
                                        <%= Model.PageHtml %>
                                    </ul>
                                </nav>
                                <!-- END Pagination -->
                            </div>
                            <div class="col-md-4">
                                <!-- About -->
                                <a class="block block-link-hover3" href="AddContent.aspx" title="写笔记">
                                    <div class="block-content border-t">
                                        <div class="row items-push text-center">
                                            <div class="col-xs-12">
                                                <div class="push-5"><i class="si si-pencil fa-2x"></i></div>
                                            </div>
                                          
                                        </div>
                                    </div>
                                </a>
                                <!-- END About -->
                            </div>
                        </div>
                    </div>
                    <!-- END Section Content -->
                </section>
                <!-- END Classic Content -->
                
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="userFoot" runat="server">
    <script>
        require(['jquery', 'public/contentInfoDialog'], function ($, publicContentInfoDialog) {
            var $containerSearch = $('#containerSearch');
            var $keywords = $containerSearch.find('input[type=text]');
            var $commit = $containerSearch.find('button');
            var nodeId = parseInt($containerSearch.find('input[type=hidden]').val());

            var _commit = function () {
                var url = "Contents.aspx?wd=";

                window.location.href = url + $keywords.val();
            }
            $keywords.on('keydown', function (e) {
                if (e.keyCode == 13) {
                    _commit();
                }
            });
            $commit.on('click', _commit);

            //view
            $('a[name=btnView]').on('click', function () {
                var contentId = parseInt($(this).data('id'));
                publicContentInfoDialog.open({ contentId: contentId });
            });
        });
    </script>
</asp:Content>