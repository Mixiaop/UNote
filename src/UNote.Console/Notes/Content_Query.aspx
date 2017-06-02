<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/UZero.Master" AutoEventWireup="true" CodeBehind="Content_Query.aspx.cs" Inherits="UNote.Console.Notes.Content_Query" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <form runat="server">
        <!-- Page Header -->
        <div class="content bg-gray-lighter">
            <div class="row items-push">
                <div class="col-sm-7">
                    <h1 class="page-heading">所有笔记 <small></small>
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
                                <asp:TextBox runat="server" ID="tbKeywords" placeholder="标题、标签" Width="300px" CssClass="form-control"></asp:TextBox>
                            </label>
                            <label class="col-xs-pull-1">
                                <asp:Button ID="btnSearch" CssClass="btn btn-primary" runat="server" Text="搜索"></asp:Button>
                                <a href="Content_Add.aspx" class="btn btn-primary">写笔记</a>
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
                                <th class="text-center">标题</th>
                                <th class="text-center">所属分类</th>
                                <th class="text-center">标签</th>
                                <th class="text-center">文件大小</th>
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

                                            <%--<a href="Node_Update.aspx?nodeId=<%# Eval("Id") %>" class="btn btn-primary btn-xs">编辑</a>--%>
                                            <asp:LinkButton runat="server" OnClick="btnDelete_Click" OnClientClick="return confirm('你确认删除笔记吗?')" Text="删除" CssClass="btn btn-default btn-xs" CommandArgument='<%# Eval("Id") %>'></asp:LinkButton>
                                        </td>
                                        <td class="text-center"><%# Eval("Title") %></td>
                                        <td class="text-center"><%# ((UNote.Domain.Notes.Node)Eval("Node"))==null?"-":((UNote.Domain.Notes.Node)Eval("Node")).NodeName %></td>
                                        <td class="text-center"><%# Eval("Tag") %></td>
                                        <td class="text-center"><%# Eval("FileId").ToInt()!=0?Eval("FormatFileSize"):"-" %></td>
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
                        <%= PagerHtml %>
                    </ul>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
