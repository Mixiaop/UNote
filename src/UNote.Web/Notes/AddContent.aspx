<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/User.master" AutoEventWireup="true" CodeBehind="AddContent.aspx.cs" Inherits="UNote.Web.Notes.AddContent" ValidateRequest="false" %>

<%@ Import Namespace="U" %>
<asp:Content ID="Content1" ContentPlaceHolderID="userHead" runat="server">
    <link href="/editors/kindeditor/css/default.css" rel="stylesheet" />
    <link href="/lib/plugins/jquery.fineuploader/fineuploader.css" rel="stylesheet" />
    <link href="/lib/plugins/jquery-tags-input/jquery.tagsinput.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="userBody" runat="server">
    <form runat="server">
        <asp:PlaceHolder runat="server" ID="phCreateNode" Visible="false">
            <div class="alert alert-info">您还未创建目录，马上去 <a href="<%= RouteContext.GetRouteUrl("Notes.Nodes") %>" class="btn btn-primary">创建</a></div>
        </asp:PlaceHolder>
        <!-- Page Header -->
        <div class="page-header">
            <div class="row">
                <div class="col-sm-6">
                    <ul>
                        <li><%= Model.Node.TeamId>0?Model.Node.Team.Name:"我的笔记" %></li>
                        <% foreach (var node in Model.Parents)
                           { %>
                        <li><span>/</span> <a href="<%= RouteContext.GetRouteUrl("Notes.Contents", node.Id) %>"><%= node.NodeName %></a></li>
                        <%} %>
                        <li><span>/</span> <a href="<%= RouteContext.GetRouteUrl("Notes.Contents", Model.Node.Id) %>"><%= Model.Node.NodeName %></a></li>
                        <% if(Model.GetContentId>0){ %>
                        <li><span>/</span> <a href="<%= RouteContext.GetRouteUrl("Notes.ContentInfo", Model.GetContentId) %>" target="_blank"><%= Model.Parent.Title %></a></li>
                        <li class="current"><span>/</span> 添加内容项</li>
                        <%}else{ %>
                        <li class="current"><span>/</span> 写笔记</li>
                        <%} %>
                    </ul>
                </div>
                <div class="col-sm-6"></div>
            </div>
        </div>
        <!-- END Page Header -->
        <!-- Page Content -->
        <div class="content note-form">
            <div class="note-form-action">
                <% if (Model.CurrentNodeType == UNote.NodeType.Normal)
                   { %>
                <a><span class="badge active"><%= UNote.NodeType.Normal.ToAlias() %></span></a>
                <a href="<%= RouteContext.GetRouteUrl("Notes.AddContent", Model.NodeId, (int)UNote.NodeType.Html) %>"><span class="badge"><%= UNote.NodeType.Html.ToAlias() %></span></a>
                <a href="<%= RouteContext.GetRouteUrl("Notes.AddContent", Model.NodeId, (int)UNote.NodeType.Word) %>"><span class="badge"><%= UNote.NodeType.Word.ToAlias() %></span></a>

                <%}
                   else if (Model.CurrentNodeType == UNote.NodeType.Html)
                   { %>
                <a href="<%= RouteContext.GetRouteUrl("Notes.AddContent", Model.NodeId, (int)UNote.NodeType.Normal) %>"><span class="badge"><%= UNote.NodeType.Normal.ToAlias() %></span></a>
                <a><span class="badge active"><%= UNote.NodeType.Html.ToAlias() %></span></a>
                <a href="<%= RouteContext.GetRouteUrl("Notes.AddContent", Model.NodeId, (int)UNote.NodeType.Word) %>"><span class="badge"><%= UNote.NodeType.Word.ToAlias() %></span></a>

                <%}
                   else if (Model.CurrentNodeType == UNote.NodeType.Word)
                   { %>
                <a href="<%= RouteContext.GetRouteUrl("Notes.AddContent", Model.NodeId, (int)UNote.NodeType.Normal) %>"><span class="badge"><%= UNote.NodeType.Normal.ToAlias() %></span></a>
                <a href="<%= RouteContext.GetRouteUrl("Notes.AddContent", Model.NodeId, (int)UNote.NodeType.Html) %>"><span class="badge"><%= UNote.NodeType.Html.ToAlias() %></span></a>
                <a><span class="badge active"><%= UNote.NodeType.Word.ToAlias() %></span></a>
                <%} %>
            </div>
            <!-- Dynamic Table Full -->
            <div class="block">
                <div class="block-content block-content-narrow">
                    <asp:Literal runat="server" ID="ltlMessage"></asp:Literal>
                    <div class="row">
                        <div class="col-lg-9">
                            <div class="form-horizontal push-10-t">
                                <div class="form-group">
                                    <div class="col-xs-12">
                                        <div class="form-material">
                                            <asp:TextBox runat="server" ID="tbTitle" CssClass="form-control"></asp:TextBox>
                                            <label>标题</label>
                                        </div>
                                        <div class="help-block">
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-xs-12">
                                        <div class="form-material">
                                            <div id="uploadPicture"></div>
                                            <div id="thumbPicture"></div>
                                            <asp:HiddenField runat="server" ID="hfPreviewId" />
                                            <asp:HiddenField runat="server" ID="hfPreviewUrl" />
                                            <label></label>
                                        </div>
                                        <div class="help-block">
                                            显示在列表的预览图
                                        </div>
                                    </div>
                                </div>
                                <% if (Model.CurrentNodeType == UNote.NodeType.Normal)
                                   { %>
                                <div class="form-group">
                                    <div class="col-xs-12">
                                        <div class="form-material">
                                            <div id="uploadFile"></div>
                                            <asp:HiddenField runat="server" ID="hfFileId" />
                                            <asp:HiddenField runat="server" ID="hfFileUrl" />
                                            <asp:HiddenField runat="server" ID="hfFileSize" />
                                            <label></label>
                                        </div>
                                        <div class="help-block">
                                            需要保存的文件 （文件太大就别传了，建议小文件“1M - 500M” ）
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-xs-12">
                                        <div class="form-material">
                                            <asp:TextBox ID="tbContent" CssClass="kindeditor editor form-control" TextMode="MultiLine" Height="480px" runat="server"></asp:TextBox>
                                            <label>内容</label>
                                        </div>
                                        <div class="help-block">
                                            笔记的内容，记录你任何想要写的内容“图文”
                                        </div>
                                    </div>
                                </div>
                                <%}
                                   else if (Model.CurrentNodeType == UNote.NodeType.Html)
                                   { %>
                                <div class="form-group">
                                    <div class="col-xs-12">
                                        <div class="form-material">
                                            <asp:TextBox runat="server" ID="tbHtmlBody" TextMode="MultiLine" Height="80px" CssClass="form-control"></asp:TextBox>
                                            <label>描述</label>
                                        </div>
                                        <div class="help-block">
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-xs-12">
                                        <div class="form-material">
                                            <div id="uploadHtml"></div>
                                            <asp:HiddenField runat="server" ID="hfHtmlFileId" />
                                            <asp:HiddenField runat="server" ID="hfHtmlFileUrl" />
                                            <asp:HiddenField runat="server" ID="hfHtmlFileSize" />
                                            <asp:HiddenField runat="server" ID="hfHtmlCode" />
                                            <label></label>
                                        </div>
                                        <div class="help-block">
                                            需要上传的HTML压缩包（必须为 zip、gz 后缀）
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-xs-12">
                                        <div class="form-material">
                                            <asp:TextBox runat="server" ID="tbHtmlHomePage" CssClass="form-control"></asp:TextBox>
                                            <label>应用首页</label>
                                        </div>
                                        <div class="help-block">
                                            上传的Html包的首页名称或路径，如：index.html
                                        </div>
                                    </div>
                                </div>
                                <%}
                                   else if (Model.CurrentNodeType == UNote.NodeType.Word)
                                   { %>
                                <div class="form-group">
                                    <div class="col-xs-12">
                                        <div class="form-material">
                                            <div id="uploadWordFile"></div>
                                            <asp:HiddenField runat="server" ID="hfWordFileId" />
                                            <asp:HiddenField runat="server" ID="hfWordFileUrl" />
                                            <asp:HiddenField runat="server" ID="hfWordFileSize" />
                                            <label></label>
                                        </div>
                                        <div class="help-block">
                                            需要解析的word文件 （.doc、.docx）
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-xs-12">
                                        <div class="form-material">
                                            <asp:TextBox ID="tbWordContent" CssClass="kindeditor editor form-control" TextMode="MultiLine" Height="480px" runat="server"></asp:TextBox>
                                            <label>内容</label>
                                        </div>
                                        <div class="help-block">
                                            笔记的内容，记录你任何想要写的内容“图文”
                                        </div>
                                    </div>
                                </div>
                                <%} %>
                            </div>
                        </div>
                        <div class="col-lg-3">
                            <% if (Model.GetContentId == 0) {  %>
                            <div class="form-horizontal push-10-t">
                                <div class="form-group">
                                    <div class="col-xs-12">
                                        <div class="form-material">
                                            <asp:DropDownList runat="server" ID="ddlIsTop" CssClass="form-control">
                                                <asp:ListItem Value="0" Text="否"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="是"></asp:ListItem>
                                            </asp:DropDownList>
                                            <label>置顶</label>
                                        </div>
                                        <div class="help-block">
                                            将内容放在顶处
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-xs-12">
                                        <div class="form-material">
                                            <asp:TextBox runat="server" ID="tbTags" CssClass="form-control"></asp:TextBox>
                                            <label>标签</label>
                                        </div>
                                        <div class="help-block">
                                            按“回车”添加新标签
                                        </div>
                                        <% if (Model.Tags.Count > 0)
                                           { %>
                                        <div class="tags">
                                            <% foreach (var tag in Model.Tags)
                                               { %>
                                            <a href="javascript:;"><%= tag.Name %></a> <%} %>
                                        </div>
                                        <%} %>
                                    </div>
                                </div>
                                <% if(Model.Node.TeamId>0){ %>
                                <div class="form-group hidden">
                                    <div class="col-xs-12">
                                        <div class="form-material">
                                            <label>
                                                关注的人 <a href="javascript:;" id="btnAddFollowers" title="添加关注" data-toggle="tooltip">
                                                    <div class="item item-circle bg-success-light text-success unote-avatar unote-avatar-add">
                                                        <i class="si si-plus"></i>
                                                    </div>
                                                </a>
                                            </label>
                                        </div>
                                        <div style="padding-top: 25px;" id="followerWrapper">
                                        </div>
                                        <asp:HiddenField runat="server" ID="hfFollowerIds" />
                                    </div>
                                </div>
                                <%} %>
                                <%} %>
                                <%--<div class="form-group">
                                    <div class="col-xs-12">
                                        <div class="form-material">
                                            <asp:DropDownList runat="server" ID="ddlPublic" CssClass="form-control">
                                                <asp:ListItem Text="私有" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="公开" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <label>是否公开</label>
                                        </div>
                                        <div class="help-block">
                                            “公开”在客户端会全部显示，“私有”只有创建者才能看到。
                                        </div>
                                    </div>
                                </div>--%>
                                <div class="form-group">
                                    <div class="col-xs-12">
                                        <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="发布内容" />
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <!-- END Dynamic Table Full -->
        </div>
        <!-- END Page Content -->
    </form>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="userFoot" runat="server">
    <% if (Model.CurrentNodeType == UNote.NodeType.Normal)
       { %>
    <script type="text/javascript">
        require(['jquery', 'utils/notify', 'kindeditor', 'jquery.fineuploader'], function ($, notify) {
            $('#uploadPicture').fineUploader({
                request: { endpoint: "/AjaxServices/JqueryFineUploader/UploadPicture.ashx?size=400" },
                multiple: false,
                text: {
                    uploadButton: '上传预览图'
                }
            }).on('complete',
           function (event, id, fileName, responseJSON) {
               if (responseJSON.success) {
                   var thumbBox = $("#thumbPicture");
                   thumbBox.html('');
                   thumbBox.append('<img src="' + responseJSON.showthumb + '" >');
                   $("#<%= hfPreviewId.ClientID %>").val(responseJSON.pictureId);
                   $("#<%= hfPreviewUrl.ClientID %>").val(responseJSON.thumb);

               } else {
                   alert(responseJSON.error);
               }
           });

            $('#uploadFile').fineUploader({
                request: { endpoint: "/AjaxServices/JQFineUploader_UploadFile.ashx" },
                multiple: false,
                text: {
                    uploadButton: '上传文件'
                }
            }).on('complete',
            function (event, id, fileName, responseJSON) {
                if (responseJSON.success) {
                    $("#<%= hfFileId.ClientID %>").val(responseJSON.fileId);
                    $("#<%= hfFileUrl.ClientID %>").val(responseJSON.fileUrl);
                    $("#<%= hfFileSize.ClientID %>").val(responseJSON.fileSize);
                }
            });

            var arrEditor = new Array();

            $('.kindeditor').each(function () {
                var editor = KindEditor.create('#' + $(this).attr('id'), {
                    width: '100%',
                    items: ['code','source', '|',
                            'plainpaste', 'wordpaste', '|', 'justifyleft', 'justifycenter', 'justifyright',
                            'justifyfull', 'insertorderedlist', 'insertunorderedlist', 'clearhtml', '|', 'table', 'hr', 'fullscreen', '/',
                            'formatblock', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold',
                            'italic', 'underline', 'strikethrough', '|', 'image', 'multiimage',
                           'insertfile', 'link', 'unlink'],
                    uploadJson: '/editors/kindeditor/handler/upload_json.ashx'
                });
                arrEditor[arrEditor.length] = editor;
            });
            //alert(1);
            $('#<%= btnSave.ClientID%>').on('click', function () {
                if (arrEditor.length != 0) {
                    for (var i = 0; i < arrEditor.length; i++) {
                        arrEditor[i].sync();
                    }
                }
                return true;
            });
        });
    </script>
    <%}
       else if (Model.CurrentNodeType == UNote.NodeType.Html)
       { %>
    <script type="text/javascript">
        require(['jquery', 'utils/notify', 'kindeditor', 'jquery.fineuploader'], function ($, notify) {
            $('#uploadPicture').fineUploader({
                request: { endpoint: "/AjaxServices/JqueryFineUploader/UploadPicture.ashx?size=400" },
                multiple: false,
                text: {
                    uploadButton: '上传预览图'
                }
            }).on('complete',
           function (event, id, fileName, responseJSON) {
               if (responseJSON.success) {
                   var thumbBox = $("#thumbPicture");
                   thumbBox.html('');
                   thumbBox.append('<img src="' + responseJSON.showthumb + '" >');
                   $("#<%= hfPreviewId.ClientID %>").val(responseJSON.pictureId);
                   $("#<%= hfPreviewUrl.ClientID %>").val(responseJSON.thumb);

               } else {
                   alert(responseJSON.error);
               }
           });

            $('#uploadHtml').fineUploader({
                request: { endpoint: "/AjaxServices/Notes/UploadHtmlContent.ashx" },
                multiple: false,
                text: {
                    uploadButton: '上传压缩包'
                }
            }).on('complete',
            function (event, id, fileName, responseJSON) {
                if (responseJSON.success) {
                    $("#<%= hfHtmlFileId.ClientID %>").val(responseJSON.fileId);
                    $("#<%= hfHtmlFileUrl.ClientID %>").val(responseJSON.fileUrl);
                    $("#<%= hfHtmlFileSize.ClientID %>").val(responseJSON.fileSize);
                    $("#<%= hfHtmlCode.ClientID %>").val(responseJSON.htmlCode);
                    $("#<%= tbHtmlHomePage.ClientID %>").val(responseJSON.htmlHomePage);
                } else {
                    alert(responseJSON.error);
                }
            });
        });
    </script>
    <%}
       else if (Model.CurrentNodeType == UNote.NodeType.Word)
       { %>
    <script type="text/javascript">
        require(['jquery', 'utils/notify', 'kindeditor', 'jquery.fineuploader'], function ($, notify) {
            $('#uploadPicture').fineUploader({
                request: { endpoint: "/AjaxServices/JqueryFineUploader/UploadPicture.ashx?size=400" },
                multiple: false,
                text: {
                    uploadButton: '上传预览图'
                }
            }).on('complete',
           function (event, id, fileName, responseJSON) {
               if (responseJSON.success) {
                   var thumbBox = $("#thumbPicture");
                   thumbBox.html('');
                   thumbBox.append('<img src="' + responseJSON.showthumb + '" >');
                   $("#<%= hfPreviewId.ClientID %>").val(responseJSON.pictureId);
                   $("#<%= hfPreviewUrl.ClientID %>").val(responseJSON.thumb);

               } else {
                   alert(responseJSON.error);
               }
           });

            var arrEditor = new Array();
            var editor;
            KindEditor.options.filterMode = false;
            $('.kindeditor').each(function () {
                editor = KindEditor.create('#' + $(this).attr('id'), {
                    width: '100%',
                    items: ['source', '|',
                            'plainpaste', 'wordpaste', '|', 'justifyleft', 'justifycenter', 'justifyright',
                            'justifyfull', 'insertorderedlist', 'insertunorderedlist', 'clearhtml', '|', 'table', 'hr', 'fullscreen', '/',
                            'formatblock', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold',
                            'italic', 'underline', 'strikethrough', '|', 'image', 'multiimage',
                           'insertfile', 'link', 'unlink'],
                    uploadJson: '/editors/kindeditor/handler/upload_json.ashx',
                    filterMode: false
                });
                arrEditor[arrEditor.length] = editor;
            });

            $('#<%= btnSave.ClientID%>').on('click', function () {
                if (arrEditor.length != 0) {
                    for (var i = 0; i < arrEditor.length; i++) {
                        arrEditor[i].sync();
                    }
                }
                return true;
            });

            $('#uploadWordFile').fineUploader({
                request: { endpoint: "/AjaxServices/Notes/UploadWordContent.ashx" },
                multiple: false,
                text: {
                    uploadButton: '上传 word'
                }
            }).on('complete',
            function (event, id, fileName, res) {
                if (res.success) {
                    editor.html(res.html);
                } else {
                    alert(res.error);
                }
            });
        });
    </script>
    <%} %>
    <script>
        require(['jquery', 'utils/notify', 'notes/addFollowerDialog', 'jquery.tagsinput'], function ($, notify, addFollowerDialog) {
            var $tags = $('.note-form .tags');
            var $tbTag = $('#<%= tbTags.ClientID %>');
            
            $tbTag.tagsInput({
                height: '36px',
                width: '100%',
                defaultText: '添加标签',
                removeWithBackspace: true,
                delimiter: [',']
            });

            $tags.find('a').on('click', function () {
                var val = $tbTag.val();
                var tag = $(this).text();

                if (val == '') {
                    $tbTag.addTag(tag);
                } else {
                    if (val.indexOf(tag) == -1) {
                        $tbTag.addTag(tag);
                    } else {
                        $tbTag.removeTag(tag);
                    }
                }
            });

            //add followers
            var $followerWrapper = $('#followerWrapper');
            var $hfFollowersIds = $('#<%= hfFollowerIds.ClientID%>');
            var followers = [];
            var teamId = parseInt('<%= Model.Node.TeamId%>');
            var addFollower = function (info) {
                var exists = false;
                for (var i = 0; i < followers.length; i++) {
                    if (followers[i].id == info.id) {
                        exists = true;
                        break;
                    }
                }
                
                if (!exists) {
                    followers.push(info);
                }

                $hfFollowersIds.val('');
                var ids = '';
                for (var i = 0; i < followers.length; i++) {
                    ids += followers[i].id + ',';
                }

                if (ids != '')
                    ids = ids.substring(0, ids.length - 1);

                $hfFollowersIds.val(ids);
            }

            var renderFollowers = function () {
                $followerWrapper.html('');
                for (var i = 0; i < followers.length; i++) {
                    var info = followers[i];
                    $followerWrapper.append('<a>');
                    $followerWrapper.append('<div class="item item-circle bg-info-light text-info unote-avatar">' + info.nickName + '</div>');
                    $followerWrapper.append('</a> ');
                }
            }

            $('#btnAddFollowers').click(function () {
                addFollowerDialog.open({
                    teamId: teamId, callback: function (userIds) {
                        if (userIds != '') {
                            var list = userIds.split(',');
                            for (var i = 0; i < list.length; i++) {
                                var item = list[i].split(':');
                                var info = {};
                                info.id = item[0];
                                info.nickName = item[1];
                                addFollower(info);
                            }

                            renderFollowers();
                        }
                    }
                });
            });
        });
    </script>
</asp:Content>
