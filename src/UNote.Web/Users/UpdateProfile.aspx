<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/User.master" AutoEventWireup="true" CodeBehind="UpdateProfile.aspx.cs" Inherits="UNote.Web.Users.UpdateProfile" %>

<asp:Content ID="Content3" ContentPlaceHolderID="userHead" runat="server">
    <link href="/lib/plugins/jquery.fineuploader/fineuploader.css" rel="stylesheet" />
    <link href="/lib/plugins/cropper/cropper.min.css" rel="stylesheet" />

    <style>
        .qq-upload-list {
            margin-top: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="userBody" runat="server">
    <form runat="server">
        <!-- Page Header -->
        <div class="content bg-gray-lighter">
            <div class="row items-push">
                <div class="col-sm-7">
                    <h1 class="page-heading">编辑个人资料<small> Edit your profile</small>
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
                                            <asp:TextBox runat="server" ID="tbNickName" CssClass="form-control"></asp:TextBox>
                                            <label>昵称</label>
                                        </div>
                                        <div class="help-block">
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-xs-4">
                                        <div class="form-material">
                                            <div id="uploadPicture" style="margin-bottom: 10px;"></div>
                                            <div class="js-img-cropper-preview overflow-hidden" style="height: 200px;"></div>                                            <label>头像</label>
                                        </div>
                                    </div>
                                    <div class="col-xs-8">
                                        <div id="thumbPicture"></div>
                                        <asp:HiddenField runat="server" ID="hfPreviewId" />
                                        <asp:HiddenField runat="server" ID="hfPreviewUrl" />
                                        <asp:HiddenField runat="server" ID="hfPicX" />
                                        <asp:HiddenField runat="server" ID="hfPicY" />
                                        <asp:HiddenField runat="server" ID="hfPicW" />
                                        <asp:HiddenField runat="server" ID="hfPicH" />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-xs-12">
                                        <asp:LinkButton ID="LinkButton1" CssClass="btn btn-sm btn-primary" runat="server" OnClick="BtnSave_Click">保存</asp:LinkButton>
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
<asp:Content ID="Content2" ContentPlaceHolderID="userFoot" runat="server">
    <script>

        require(['jquery', 'cropper', 'jquery.fineuploader'], function ($, cropper) {
            //上传图片
            $('#uploadPicture').fineUploader({
                request: { endpoint: "/AjaxServices/JqueryFineUploader/UploadPicture.ashx" },
                multiple: false,
                text: {
                    uploadButton: '上传预览图'
                }
            }).on('complete',
           function (event, id, fileName, responseJSON) {
               if (responseJSON.success) {
                   var thumbBox = $("#thumbPicture");
                   thumbBox.html('');
                   thumbBox.append('<img src="' + responseJSON.showthumb + '" class="img-responsive" id="js-img-cropper" ><br/>');
                   thumbBox.append('<button class="btn btn-success btn-sm" type="button" data-toggle="cropper" data-method="crop">确认裁切</button>')

                   var $image = document.getElementById('js-img-cropper');
                   Cropper.setDefaults({
                       aspectRatio: 1/1,
                       preview: '.js-img-cropper-preview'
                   });
                   var $cropper = new Cropper($image, {
                       crop: function (e) {
                           // e.detail contains all data required to crop the image server side
                           // You will have to send it to your custom server side script and crop the image there
                           // Since this event is fired each time you set the crop section, you could also use getData()
                           // method on demand. Please check out https://fengyuanchen.github.io/cropperjs/ for more info
                           //console.log(e.detail);
                           $("#<%= hfPicX.ClientID %>").val(parseInt(e.detail.x));
                           $("#<%= hfPicY.ClientID %>").val(parseInt(e.detail.y));
                           $("#<%= hfPicW.ClientID %>").val(parseInt(e.detail.width));
                           $("#<%= hfPicH.ClientID %>").val(parseInt(e.detail.height));
                       }
                   });

                   $("#<%= hfPreviewId.ClientID %>").val(responseJSON.pictureId);
                   $("#<%= hfPreviewUrl.ClientID %>").val(responseJSON.thumb);
               } else {
                   alert(responseJSON.error);
               }
           });

        })


    </script>
</asp:Content>
