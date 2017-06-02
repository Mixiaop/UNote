<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Head.Master" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="UNote.Web.Registration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="content overflow-hidden">
        <form runat="server">
            <div class="row">
                <div class="col-sm-8 col-sm-offset-2 col-md-6 col-md-offset-3 col-lg-4 col-lg-offset-4">
                    <!-- Login Block -->
                    <div class="block block-themed animated fadeIn">
                        <div class="block-header bg-primary">
                            <ul class="block-options">
                                <li>
                                    <%--<a href="base_pages_reminder.html">忘记密码?</a>--%>
                                </li>
                                <li>
                                    <a href="/Login.aspx" data-toggle="tooltip" data-placement="left" title="Login Account"><i class="si si-login"></i></a>
                                </li>
                            </ul>
                            <h3 class="block-title">Registration UNote</h3>
                        </div>
                        <div class="block-content block-content-full block-content-narrow">
                            <!-- Login Title -->
                            <h1 class="h2 font-w600 push-30-t push-5"> </h1>
                            <p></p>
                            <!-- END Login Title -->

                            <!-- Login Form -->
                            <div class="js-validation-login form-horizontal push-30-t push-50" >
                                <div class="form-group">
                                    <div class="col-xs-12">
                                        <div class="form-material form-material-primary ">
                                            <asp:TextBox runat="server" ID="tbUsername" CssClass="form-control"></asp:TextBox>
                                            <label for="login-username">邮箱</label>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-xs-12">
                                        <div class="form-material form-material-primary ">
                                             <asp:TextBox runat="server" ID="tbPassword" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                            <label for="login-password">密码</label>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-xs-12">
                                        <div class="form-material form-material-primary ">
                                             <asp:TextBox runat="server" ID="tbPassword2" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                            <label for="login-password">确认密码</label>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-xs-12">
                                        <div class="form-material form-material-primary ">
                                             <asp:TextBox runat="server" ID="tbNickName"  CssClass="form-control"></asp:TextBox>
                                            <label for="login-password">昵称</label>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-xs-12 col-sm-6 col-md-4">
                                        <button class="btn btn-block btn-primary" runat="server" id="btnRegister" type="submit"><i class="si si-plus pull-right"></i> 注 册</button> 
                                    </div>
                                </div>
                                <% if (!string.IsNullOrEmpty(ErrorMessage))
                                   { %>
                                <div class="alert alert-danger"><%= ErrorMessage %></div>
                                <%} %>
                            </div>
                            <!-- END Registration Form -->
                        </div>
                    </div>
                    <!-- END Registration Block -->
                </div>
            </div>
            </form>
        </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
</asp:Content>
