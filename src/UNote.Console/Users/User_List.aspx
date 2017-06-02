<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/UZero.Master" AutoEventWireup="true" CodeBehind="User_List.aspx.cs" Inherits="UNote.Console.Users.User_List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <form runat="server">
        <!-- Page Header -->
        <div class="content bg-gray-lighter">
            <div class="row items-push">
                <div class="col-sm-7">
                    <h1 class="page-heading">所有用户 <small></small>
                    </h1>
                </div>
            </div>
        </div>
        <!-- END Page Header -->
        <!-- Page Content -->

        <div class="content">
            <div class="row items-push">
                <div class="col-xs-12">
                    <div class="form-inline">
                        <div class="form-group ">
                            <label>
                                <asp:TextBox runat="server" ID="tbKeywords" placeholder="用户名、昵称" Width="300px" CssClass="form-control"></asp:TextBox>
                            </label>
                            <label class="col-xs-pull-1">
                                <asp:Button ID="btnSearch" CssClass="btn btn-primary" runat="server" Text="搜索"></asp:Button>
                                <a href="User_Add.aspx" class="btn btn-primary">添加新用户</a>
                            </label>
                        </div>
                    </div>
                </div>
            </div>
            <div class="block">
                <div class="block-content table-responsive">
                    <asp:Literal runat="server" ID="ltlMessage"></asp:Literal>
                    <table class="table table-hover js-dataTable-full" id="tableMain">
                        <thead>
                            <tr>
                                <th class="text-center" style="width:10%">操作</th>
                                <th class="text-center">用户名</th>
                                <th class="text-center">昵称</th>
                                <th class="text-center">最后一次登录</th>
                                <th class="text-center">时间</th>
                            </tr>
                        </thead>
                        <asp:Repeater runat="server" ID="rptDatas">
                            <ItemTemplate>
                                <tbody>
                                    <tr>
                                        <td class="text-center">
                                            <%--<a href="Node_Update.aspx?nodeId=<%# Eval("Id") %>" class="btn btn-primary btn-xs">编辑</a>--%>
                                            <asp:LinkButton runat="server" OnClick="btnDelete_Click" OnClientClick="return confirm('你确认删除用户吗?')" Text="删除" CssClass="btn btn-default btn-xs" CommandArgument='<%# Eval("Id") %>'></asp:LinkButton>
                                        </td>
                                        <td class="text-center"><%# Eval("Username") %></td>
                                        <td class="text-center"><%# Eval("NickName") %></td>
                                        <td class="text-center"><%# Eval("LastLoginTime") %></td>
                                        <td class="text-center"><%# Eval("CreationTime","{0:yyyy-MM-dd HH:mm}") %></td>
                                    </tr>
                                </tbody>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>

                </div>
                <div class="text-center">
                    <ul class="pagination">
                        <%= PagerHtml %>
                    </ul>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
