<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/User.master" AutoEventWireup="true" CodeBehind="TeamInfo.aspx.cs" Inherits="UNote.Web.Teams.TeamInfo" %>
<%@ Register Src="~/Teams/TeamTabMenu.ascx" TagPrefix="uc1" TagName="TeamTabMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="userHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="userBody" runat="server">
    <div class="content bg-gray-lighter">
            <div class="row items-push">
                <div class="col-sm-7">
                    <h1 class="page-heading"><%= Model.Team.Name %> - 基本信息<small> team settings</small>
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
                                
                                <form runat="server">
                                <!-- Form -->
                                <div class="form-horizontal" >
                                    <div class="block-content tab-content">
                                        <asp:Literal runat="server" ID="ltlMessage"></asp:Literal>
                                        <!-- Step 1 -->
                                        <div class="tab-pane push-30-t push-50 active" >
                                            <div class="form-group">
                                                <div class="col-sm-8 col-sm-offset-2">
                                                    <div class="form-material">
                                                        <asp:TextBox runat="server" ID="tbName" CssClass="form-control"></asp:TextBox>
                                                        <label >团队名称</label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-sm-8 col-sm-offset-2">
                                                    <div class="form-material">
                                                        <asp:TextBox ID="tbIntroduction" CssClass="form-control" TextMode="MultiLine"  Height="450px" runat="server"></asp:TextBox>
                                                        <label >团队简介</label>
                                                    </div>
                                                </div>
                                            </div>
                                             <div class="form-group">
                                                <div class="col-sm-8 col-sm-offset-2">
                                                    <div class="form-material">
                                                    <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Text="保存更改" />
                                                </div></div>
                                            </div>
                                        </div>
                                          
                                        </div>
                                </div>
                                <!-- END Form -->
                                    </form>
                            </div>
                            <!-- END Simple Wizard -->
                        </div>
                    </div>
                    <!-- END Simple Wizards -->
                </div>
            </div>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="userFoot" runat="server">
</asp:Content>
