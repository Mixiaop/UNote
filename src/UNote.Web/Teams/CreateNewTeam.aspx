<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/User.master" AutoEventWireup="true" CodeBehind="CreateNewTeam.aspx.cs" Inherits="UNote.Web.Teams.CreateNewTeam" %>
<asp:Content ID="Content1" ContentPlaceHolderID="userHead" runat="server">
    <%--<link href="/lib/plugins/jquery.fineuploader/fineuploader.css" rel="stylesheet" />--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="userBody" runat="server">
    <form runat="server">
        <!-- Page Header -->
        <div class="content bg-gray-lighter">
            <div class="row items-push">
                <div class="col-sm-7">
                    <h1 class="page-heading">新建团队<small> create new team</small>
                    </h1>
                </div>
                <div class="col-sm-5 text-right hidden-xs">
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
                        <div class="col-lg-12">
                            <div class="form-horizontal push-10-t">
                                <div class="form-group">
                                    <div class="col-xs-12">
                                        <div class="form-material">
                                            <asp:TextBox runat="server" ID="tbName" CssClass="form-control"></asp:TextBox>
                                            <label>团队名称</label>
                                        </div>
                                        <div class="help-block">
                                        </div>
                                    </div>
                                </div>
                               <%-- <div class="form-group">
                                    <div class="col-xs-12">
                                        <div class="form-material">
                                            <div id="uploadFile"></div>
                                            <asp:HiddenField runat="server" ID="hfLogoId" />
                                            <asp:HiddenField runat="server" ID="hfLogoUrl" />
                                            <label></label>
                                        </div>
                                        <div class="help-block">
                                            
                                        </div>
                                    </div>
                                </div>--%>
                                <div class="form-group">
                                    <div class="col-xs-12">
                                        <div class="form-material">
                                            <asp:TextBox ID="tbIntroduction" CssClass="form-control" TextMode="MultiLine"  Height="450px" runat="server"></asp:TextBox>
                                            <label>团队简介</label>
                                        </div>
                                        <div class="help-block">
                                            
                                        </div>
                                    </div>
                                </div>
                                 <div class="form-group">
                                    <div class="col-xs-12">
                                        <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="创建" />
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
    <%--<script type="text/javascript">
        require(['jquery', 'utils/notify', 'kindeditor', 'jquery.fineuploader'], function ($, notify) {
            $('#uploadFile').fineUploader({
                request: { endpoint: "/AjaxServices/JQFineUploader_UploadFile.ashx" },
                multiple: false,
                text: {
                    uploadButton: '上传LOGO'
                }
            }).on('complete',
            function (event, id, fileName, responseJSON) {
                if (responseJSON.success) {
                    $("#<%= hf.ClientID %>").val(responseJSON.fileId);
                    $("#<%= hfFileUrl.ClientID %>").val(responseJSON.fileUrl);
                    $("#<%= hfFileSize.ClientID %>").val(responseJSON.fileSize);
                }
            });

        });
    </script>--%>
</asp:Content>
