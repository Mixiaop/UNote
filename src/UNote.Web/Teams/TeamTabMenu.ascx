<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TeamTabMenu.ascx.cs" Inherits="UNote.Web.Teams.TeamTabMenu" %>
 <!-- Step Tabs -->
                                <ul class="nav nav-tabs nav-tabs-alt nav-justified">
                                    <li <%= SelectedTab == UNote.Web.Teams.TeamTab.Info?"class=\"active\"":"" %>>
                                        <a href="<%= RouteContext.GetRouteUrl("Teams.Info", TeamKey) %>" >基本信息</a>
                                    </li>
                                    <li <%= SelectedTab == UNote.Web.Teams.TeamTab.Members?"class=\"active\"":"" %>>
                                        <a href="<%= RouteContext.GetRouteUrl("Teams.Members", TeamKey) %>" >成员管理</a>
                                    </li>
                                    <li <%= SelectedTab == UNote.Web.Teams.TeamTab.Nodes?"class=\"active\"":"" %>>
                                        <a href="<%= RouteContext.GetRouteUrl("Teams.Nodes", TeamKey) %>" >目录管理</a>
                                    </li>
                                </ul>
                                <!-- END Step Tabs -->