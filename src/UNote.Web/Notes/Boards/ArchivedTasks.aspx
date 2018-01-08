<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/User.master" AutoEventWireup="true" CodeBehind="ArchivedTasks.aspx.cs" Inherits="UNote.Web.Notes.Boards.ArchivedTasks" %>
<asp:Content ID="Content1" ContentPlaceHolderID="userHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="userBody" runat="server">
    <!-- Page Header -->
    <div class="page-header">
        <div class="row">
            <div class="col-lg-3 col-xs-8">
                <ul>
                    <li><%= Model.Node.TeamId>0?Model.Node.Team.Name:"我的笔记" %></li>
                    <% foreach (var node in Model.Parents)
                       { %>
                    <li><span>/</span> <a href="<%= RouteContext.GetRouteUrl("Notes.Contents", node.Id) %>"><%= node.NodeName %></a></li>
                    <%} %>
                    <li><span>/</span> <a href="<%= RouteContext.GetRouteUrl("Notes.Boards", Model.Node.Id) %>"><%= Model.Node.NodeName %></a></li>
                    <li class="current"><span>/</span> 归档的任务列表</li>
                </ul>
            </div>
            <div class="col-lg-9 col-xs-4 text-right viewtype-wrapper">
               
            </div>
        </div>
    </div>
    <!-- END Page Header -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="userFoot" runat="server">
</asp:Content>
