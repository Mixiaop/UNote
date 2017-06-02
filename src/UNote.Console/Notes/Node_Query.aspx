<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/UZero.Master" AutoEventWireup="true" CodeBehind="Node_Query.aspx.cs" Inherits="UNote.Console.Notes.Node_Query" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <form runat="server">
        <!-- Page Header -->
        <div class="content bg-gray-lighter">
            <div class="row items-push">
                <div class="col-sm-7">
                    <h1 class="page-heading">分类目录 <small></small>
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
                                <asp:DropDownList ID="ddlCreator" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="创建者" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="管理员" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="用户" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList runat="server" ID="ddlPublic" CssClass="form-control">
                                    <asp:ListItem Text="类型" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="公开" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="私有" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox runat="server" ID="tbKeywords" placeholder="名称、创建者" Width="300px" CssClass="form-control"></asp:TextBox>
                            </label>
                            <label class="col-xs-pull-1">
                                <asp:Button ID="btnSearch" CssClass="btn btn-primary" runat="server" Text="搜索"></asp:Button>
                                <a href="Node_Create.aspx" class="btn btn-primary">添加新分类</a>
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
                                <th class="text-center">名称</th>
                                <th class="text-center">别名</th>
                                <th class="text-center">类型</th>
                                <th class="text-center">创建者</th>
                                <th class="text-center">时间</th>
                            </tr>
                        </thead>
                        <asp:Repeater runat="server" ID="rptDatas">
                            <ItemTemplate>
                                <tbody>
                                    <tr>
                                        <td class="text-center">

                                            <a href="Node_Update.aspx?nodeId=<%# Eval("Id") %>" class="btn btn-primary btn-xs">编辑</a>
                                            <asp:LinkButton runat="server" OnClick="btnDelete_Click" OnClientClick="return confirm('你确认删除分类吗?')" Text="删除" CssClass="btn btn-default btn-xs" CommandArgument='<%# Eval("Id") %>'></asp:LinkButton>
                                        </td>
                                        <td class="text-center"><%# Eval("NodeName") %></td>
                                        <td class="text-center"><%# Eval("Alias") %></td>
                                        <td class="text-center"><%# Eval("Public").ToBool()?"<label class=\"label label-success\">公开<label>":"<label class=\"label label-default\">私有<label>" %></td>
                                        <td class="text-center"><%# Eval("UserId").ToInt()==0?"管理员":"" %></td>
                                        <td class="text-center"><%# Eval("CreationTime","{0:yyyy-MM-dd HH:mm}") %></td>
                                    </tr>
                                </tbody>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>

                </div>
                <div class="text-center">
                    <ul class="pagination">
                        <%= Model.PagerHtml %>
                    </ul>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
