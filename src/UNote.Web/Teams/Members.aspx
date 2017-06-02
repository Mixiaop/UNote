<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/User.master" AutoEventWireup="true" CodeBehind="Members.aspx.cs" Inherits="UNote.Web.Teams.Members" %>

<%@ Register Src="~/Teams/TeamTabMenu.ascx" TagPrefix="uc1" TagName="TeamTabMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="userHead" runat="server">
    <style type="text/css">

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="userBody" runat="server">
    <div class="content bg-gray-lighter">
            <div class="row items-push">
                <div class="col-sm-7">
                    <h1 class="page-heading"><%= Model.Team.Name %> - 团队设置<small> team settings</small>
                    </h1>
                </div>
                <div class="col-sm-5 text-right hidden-xs">
                </div>
            </div>
         
        </div>
    <!-- Page Content -->
        <div class="content">
            <uc1:TeamTabMenu runat="server" ID="tabMenu" />
            <!-- Dynamic Table Full -->
            <div class="block">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="js-wizard-simple block">
                                <div class="push-20-l push-20-t">
                                            <a href="javascript:;" class="btn btn-primary" id="btnAddMember" data-toggle="tooltip" title="添加新成员">添加 <i class="fa fa-plus"></i></a>
                                    </div>
                                    <div class="block-content table-responsive">
                                        <table class="table table-hover  table-striped strip js-dataTable-full" id="tableMain">
                                            <thead>
                                                <tr>
                                                    <th class="text-center" style="width:10%"></th>
                                                    <th class="text-center">昵称</th>
                                                    <th class="text-center">邮箱</th>
                                                    <th class="text-center">加入时间</th>
                                                    <th class="text-center">角色</th>
                                                </tr>
                                            </thead>
                                             <tbody>
                                            <% if (Model.Members != null)
                                               {
                                                   foreach (var member in Model.Members.Items)
                                                   { %>
                                                   <% if (member.User != null)
                                                      { %>
                                                        <tr>
                                                    <td class="text-center"></td>
                                                    <td class="text-center"><%= member.User.NickName%> <%if (member.User.HeartbeatTime.HasValue)
                                                                                                          { %><%= member.User.HeartbeatTime.Value >= DateTime.Now.AddSeconds(-Model.Settings.OnlineUserTime) ? "<label class='label label-success'>在线</label>" : ""%><%} %></td>
                                                    <td class="text-center"><%= member.User.Username%></td>
                                                    <td class="text-center"><%= member.CreationTime.ToString("yyyy-MM-dd HH:mm")%></td>
                                                    <td class="text-center"><%= member.IsAdmin ? "管理员" : "成员"%></td>
                                                </tr>
                                                <%}
                                                   }
                                               } %>
                                                 </tbody>
                                        </table>

                                    </div>
                                    <div class="text-center">
                                        <ul class="pagination">
                                            <%--<%= Model.PagerHtml %>--%>
                                        </ul>
                                    </div>
                            </div>
                            <!-- END Simple Wizard -->
                        </div>
                    </div>
                    <!-- END Simple Wizards -->
                </div>
            </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="userFoot" runat="server">
    <script>
        require(['jquery', 'utils/notify', 'teams/addMemberDialog'], function ($, notify, addMemberDialog) {
            var $btnAddMember = $('#btnAddMember');
            var key = "<%= Model.TeamKey %>";

            $btnAddMember.on('click', function () {
                addMemberDialog.open({ teamKey: key });
            });
            
        });
    </script>
</asp:Content>
