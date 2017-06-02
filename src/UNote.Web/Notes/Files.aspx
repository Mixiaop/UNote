<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/User.master" AutoEventWireup="true" CodeBehind="Files.aspx.cs" Inherits="UNote.Web.Notes.Files" %>
<asp:Content ID="Content1" ContentPlaceHolderID="userHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="userBody" runat="server">
     <div class="content note-files" >
         <div class="row">
                        <div class="col-lg-12">
                            <!-- DropzoneJS -->
                            <!-- For more info and examples you can check out http://www.dropzonejs.com/#usage -->
                            <div class="block">
                                <div class="block-content block-content-full">
                                    <!-- DropzoneJS Container -->
                                    <form class="dropzone" action="base_forms_pickers_more.html">
                                    </form>
                                </div>
                            </div>
                            <!-- END DropzoneJS -->
                        </div>
                    </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="userFoot" runat="server">
    <script src="/lib/plugins/dropzonejs/dropzone.min.js"></script>
</asp:Content>
