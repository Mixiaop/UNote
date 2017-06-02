<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/UZero.Master" AutoEventWireup="true" CodeBehind="Content_Add.aspx.cs" Inherits="UNote.Console.Notes.Content_Add" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
    <link href="/editors/kindeditor/css/default.css" rel="stylesheet" />
    <link href="/assets/js/plugins/jquery.fineuploader/fineuploader.css" rel="stylesheet" />

    <script src="/editors/kindeditor/kindeditor.js" type="text/javascript"></script>
    <script src="/editors/kindeditor/lang/zh_CN.js" type="text/javascript"></script>
    <script src="../assets/js/plugins/jquery.fineuploader/jquery.fineuploader-3.4.1.min.js"></script>
    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <form runat="server">
        <!-- Page Header -->
        <div class="content bg-gray-lighter">
            <div class="row items-push">
                <div class="col-sm-7">
                    <h1 class="page-heading">写笔记<small></small>
                    </h1>
                </div>
                  <div class="col-sm-5 text-right hidden-xs">
                            <ol class="breadcrumb push-10-t">
                                <li><a class="link-effect" href="Content_Query.aspx">所有笔记</a></li>
                                <li>写笔记</li>
                            </ol>
                        </div>
            </div>
        </div>
        <!-- END Page Header -->
        <!-- Page Content -->
        <div class="content">
            <!-- Dynamic Table Full -->
            <div class="block">
                <div class="block-content block-content-narrow">
                    <asp:Literal runat="server" ID="ltlMessage"></asp:Literal>
                    <div class="row">
                    <div class="col-lg-7">
                    <div class="form-horizontal push-10-t">
                        <div class="form-group">
                            <div class="col-xs-12">
                                <div class="form-material">
                                    <asp:TextBox runat="server" ID="tbTitle" CssClass="form-control"></asp:TextBox>
                                    <label >标题</label>
                                </div>
                                <div class="help-block">
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-xs-12">
                                <div class="form-material">
                                   <div id="uploadFile" ></div>
                                    <asp:HiddenField runat="server" ID="hfFileId" />
                                    <asp:HiddenField runat="server" ID="hfFileUrl" />
                                    <asp:HiddenField runat="server" ID="hfFileSize" />
                                    <label></label>
                                </div>
                                <div class="help-block">
                                    需要保存的文件 （文件太大就别传了，建议小文件“1M - 100M” ）
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-xs-12">
                                <div class="form-material">
                                    <asp:TextBox ID="tbContent" CssClass="kindeditor editor form-control" TextMode="MultiLine"  runat="server"></asp:TextBox>
                                    <label >内容</label>
                                </div>
                                <div class="help-block">
                                    笔记的内容，记录你任何想要写的内容“图文”
                                </div>
                            </div>
                        </div>
                    </div>
                        </div>
                        <div class="col-lg-5">
                            <div class="form-horizontal push-10-t">
                        <div class="form-group">
                            <div class="col-xs-12">
                                <div class="form-material">
                                    <asp:DropDownList ID="ddlNode" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <label >所属分类</label>
                                </div>
                                <div class="help-block">
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-xs-12">
                                <div class="form-material">
                                    <asp:TextBox runat="server" ID="tbTags"  CssClass="form-control"></asp:TextBox>
                                    <label >标签</label>
                                </div>
                                <div class="help-block">
                                    多个标签请用英文逗号（,）分开
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-xs-12">
                                <div class="form-material">
                                    <asp:DropDownList runat="server" ID="ddlPublic" CssClass="form-control">
                                        <asp:ListItem Text="公开" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="私有" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                    <label >是否公开</label>
                                </div>
                                <div class="help-block">
                                    “公开”在客户端会全部显示，“私有”只有创建者才能看到。
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-xs-12">
                                <asp:Button runat="server" ID="btnSave" OnClientClick="validForm()" CssClass="btn btn-primary"  Text="添加内容"   />
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
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
    <script type="text/javascript">
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
        KindEditor.ready(function (K) {
            $('.kindeditor').each(function () {
                var editor = K.create('#' + $(this).attr('id'), {
                    uploadJson: '/editors/kindeditor/handler/upload_json.ashx'
                });
                arrEditor[arrEditor.length] = editor;
            });
        });
        var validForm = function () {
            if (arrEditor.length != 0) {
                for (var i = 0; i < arrEditor.length; i++) {
                    arrEditor[i].sync();
                }
            }
            return true;
        }
    </script>
</asp:Content>
