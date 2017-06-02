<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/User.master" AutoEventWireup="true" CodeBehind="H5AgentLive.aspx.cs" Inherits="UNote.Web.Youzy.H5AgentLive" %>
<asp:Content ID="Content1" ContentPlaceHolderID="userHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="userBody" runat="server">
    <style>
        .item {
            width: 80px;
            height: 40px;
            line-height: 40px;
            font-size: 16px;
            margin-right: 10px;
            margin-bottom: 10px;
            text-align:center;
        }
    </style>
    <%
        var onlineUserService = U.UPrimeEngine.Instance.Resolve<UNote.Services.Users.IOnlineUserService>();
        var users = onlineUserService.GetOnlineUsers();
        %>
    <video src="http://btrainhls.youzy.cn/btrain/ulive-kmewsc/playlist.m3u8" controls="controls" width="100%" autoplay="true">
        您的浏览器不支持 video 标签。
    </video>


    <div class="block">
        <div class="block-content">
            <div class="row" style="padding-left:15px;">

                <h4 class="push-20">优志愿代理商学习平台<span class=" h6 font-w300 text-muted push-10-l"><%= users.Count %> 人在线</span>
                </h4>

                <% foreach(var user in users){ %>
                <div class="item item-circle bg-gray-lighter ">
                    <%= user.NickName %>
                </div>
                <%} %>
                


            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="userFoot" runat="server">
</asp:Content>
