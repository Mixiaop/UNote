<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/UZero.Master" AutoEventWireup="true" CodeBehind="Node_Update.aspx.cs" Inherits="UNote.Console.Notes.Node_Update" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <form runat="server">
        <!-- Page Header -->
        <div class="content bg-gray-lighter">
            <div class="row items-push">
                <div class="col-sm-7">
                    <h1 class="page-heading">编辑分类<small></small>
                    </h1>
                </div>
                  <div class="col-sm-5 text-right hidden-xs">
                            <ol class="breadcrumb push-10-t">
                                <li><a class="link-effect" href="Node_Query.aspx">分类目录</a></li>
                                <li>编辑分类</li>
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
                    <div class="form-horizontal push-10-t">
                        <div class="form-group">
                            <div class="col-xs-12">
                                <div class="form-material">
                                    <asp:TextBox runat="server" ID="tbNodeName" CssClass="form-control"></asp:TextBox>
                                    <label >名称</label>
                                </div>
                                <div class="help-block">
                                    分类的显示名称
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-xs-12">
                                <div class="form-material">
                                    <asp:TextBox runat="server" ID="tbAlias"  CssClass="form-control"></asp:TextBox>
                                    <label >别名</label>
                                </div>
                                <div class="help-block">
                                    “别名”可能在URL中使用的别称，它可以令URL更美观。通常使用小写，只能包含字母，数字和连字符（-）。
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-xs-12">
                                <div class="form-material">
                                    <asp:DropDownList runat="server" ID="ddlParentId" CssClass="form-control"></asp:DropDownList>
                                    <label >父节点</label>
                                </div>
                                <div class="help-block">
                                    分类目录和标签不同，它可以有层级关系。您可以有一个“音乐”分类目录，在这个目录下可以有叫做“流行”和“古典”的子目录。
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-xs-12">
                                <div class="form-material">
                                    <asp:TextBox runat="server" ID="tbDescription" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                    <label >描述</label>
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
                                <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary"  Text="更新分类"   />
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
</asp:Content>
