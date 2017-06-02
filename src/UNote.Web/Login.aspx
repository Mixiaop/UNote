<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Head.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="UNote.Web.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/css/themes/flat.min.css" />
    <style type="text/css">
        canvas {
            position:absolute;
            top:0px;
            z-index:10;
        }
        .content {
            z-index:100;
            position:relative;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="content overflow-hidden" >
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
                                    <a href="/Registration.aspx" data-toggle="tooltip" data-placement="left" title="New Account"><i class="si si-plus"></i></a>
                                </li>
                            </ul>
                            <h3 class="block-title">Login UNote</h3>
                        </div>
                        <div class="block-content block-content-full block-content-narrow">
                            <!-- Login Title -->
                            <h1 class="h2 font-w600 push-30-t push-5"></h1>
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
                                        <label class="css-input switch switch-sm switch-primary">
                                            <asp:CheckBox runat="server" ID="cbRememberMe" /><span></span> 记住用户名
                                        </label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-xs-12 col-sm-6 col-md-4">
                                        <%--<asp:Button runat="server" ID="btnLogin" CssClass="btn btn-block btn-primary" Text="<i class="si si-login pull-right"></i> 登 录" />
                                        <asp:LinkButton runat="server" ID="btnLogin" CssClass="btn btn-block btn-primary" Text="登录"></asp:LinkButton>--%>
                                        <button class="btn btn-block btn-primary" runat="server" id="btnLogin" type="submit"><i class="si si-login pull-right"></i> 登 录</button> 
                                    </div>
                                </div>
                                <% if (!string.IsNullOrEmpty(ErrorMessage))
                                   { %>
                                <div class="alert alert-danger"><%= ErrorMessage %></div>
                                <%} %>
                            </div>
                            <!-- END Login Form -->
                        </div>
                    </div>
                    <!-- END Login Block -->
                </div>
            </div>
            </form>
        </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
    <script src="/lib/core/three/renderers/CanvasRenderer.js"></script>
     <script src="/lib/core/three/renderers/Projector.js"></script>
    <script src="/lib/core/three/effect/line.js"></script>
</asp:Content>
