<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/User.master" AutoEventWireup="true" CodeBehind="Nodes.aspx.cs" Inherits="UNote.Web.Teams.Nodes" %>
<%@ Register Src="~/Teams/TeamTabMenu.ascx" TagPrefix="uc1" TagName="TeamTabMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="userHead" runat="server">
    <link rel="stylesheet" href="/lib/plugins/treeview/treeview.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="userBody" runat="server">
    <div class="content bg-gray-lighter">
            <div class="row items-push">
                <div class="col-sm-7">
                    <h1 class="page-heading"><%= Model.Team.Name %> - 目录管理<small> team settings</small>
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
                            <div class="js-wizard-simple block-content">
                                <div class="row" style="padding-bottom: 20px;">
                                <div class="col-sm-12 col-lg-12">
                                    <button class="btn btn-primary" id="btnAddMainNode" data-toggle="tooltip" title="添加新目录"><i class="fa fa-plus"></i> 添加</button>
                                </div>
                            </div>

                               <div class="widget flat radius-bordered" style="padding-bottom:20px;">
                    <div class="widget-body">
                        <div id="permissions" class="tree tree-plus-minus">
                            <% 
                                if (Model.NodeList.Count == 0)
                                {
                            %>
                            <div class="alert alert-info">您还没有目录，请先添加。</div>
                            <%}
                                else
                                {
                                    foreach (var node in Model.NodeList.Where(x => x.ParentId == 0))
                                    {
                            %>
                            <div class="tree-folder">
                                <div class="tree-folder-header">
                                    <div class="tree-folder-name">
                                        <%= node.NodeName %>
                                        <span>
                                            <button data-toggle="tooltip" title="添加子目录" class="btn btn-default btn-sm" data-id="<%= node.Id %>" data-name="<%= node.NodeName %>" type-add1 ><i class="fa fa-plus"></i></button>
                                            <button data-toggle="tooltip" title="编辑" class="btn btn-default btn-sm" data-id="<%= node.Id %>" type-edit1><i class="fa fa-edit"></i></button>
                                            <button data-toggle="tooltip" title="删除" class="btn btn-default btn-sm" data-id="<%= node.Id %>" type-del1><i class="fa fa-trash-o danger"></i></button>
                                        </span>
                                    </div>
                                </div>
                                <%
                                        var level2Childs = Model.NodeList.Where(x => x.ParentId == node.Id);
                                        if (level2Childs.Count() > 0)
                                        {
                                            foreach (var level2 in level2Childs)
                                            {
                                %>
                                <div class="tree-folder-content">
                                    <div class="tree-folder">
                                        <div class="tree-folder-header">
                                            <div class="tree-folder-name">
                                                <%= level2.NodeName %>
                                                <span>
                                                    <button data-toggle="tooltip" title="添加子目录" class="btn btn-default btn-sm" data-id="<%= level2.Id %>" data-name="<%= level2.NodeName %>" type-add2><i class="fa fa-plus"></i></button>
                                                    <button data-toggle="tooltip" title="编辑" class="btn btn-default btn-sm" data-id="<%= level2.Id %>" type-edit2><i class="fa fa-edit"></i></button>
                                                    <button data-toggle="tooltip" title="删除" class="btn btn-default btn-sm" data-id="<%= level2.Id %>" type-del2><i class="fa fa-trash-o danger"></i></button>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                    <% var level3Childs = Model.NodeList.Where(x => x.ParentId == level2.Id);
                                       if (level3Childs.Count() > 0)
                                       {
                                           foreach (var level3 in level3Childs)
                                           {%>
                                    <div class="tree-folder-content">
                                        <div class="tree-folder">
                                            <div class="tree-folder-header">
                                                <div class="tree-folder-name">
                                                    <%= level3.NodeName %>
                                                    <span>
                                                        <button data-toggle="tooltip" title="编辑" class="btn btn-default btn-sm" data-id="<%= level3.Id %>" type-edit3><i class="fa fa-edit"></i></button>
                                                        <button data-toggle="tooltip" title="删除" class="btn btn-default btn-sm" data-id="<%= level3.Id %>" type-del3><i class="fa fa-trash-o danger"></i></button>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <%}
                                       } %>
                                </div>
                                <%}
                                        }%>
                            </div>
                            <%}
                                } %>
                        </div>
                    </div>
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
        require(['jquery', 'utils/notify', 'notes/addNodeDialog', 'notes/editNodeDialog'], function ($, notify, addNodeDialog, editNodeDialog) {
            //添加目录
            $('#btnAddMainNode').on('click', function () {
                addNodeDialog.open({
                    teamKey: '<%= Model.TeamKey %>',
                    callback: function (nodeId) {
                        setTimeout(function () {
                            window.location.href = window.location.href;
                        }, 500);
                    }
                });
            });

            //添加子目录
            $('.content').find('[type-add1],[type-add2]').on('click', function () {
                var $this = $(this);
                var parentId = parseInt($this.data('id'));
                addNodeDialog.open({
                    parentId: parentId,
                    parentName: $this.data('name'),
                    teamKey: '<%= Model.TeamKey %>',
                    callback: function (nodeId) {
                        setTimeout(function () {
                            window.location.href = window.location.href;
                        }, 500);
                    }
                });
            });

            //目录-删除
            $('.content').find('[type-del1],[type-del2],[type-del3]').on('click', function () {
                var $this = $(this);
                var nodeId = parseInt($this.data('id'));
                NodeService.Get(nodeId, function (res) {
                    var json = res.value;
                    if (confirm('您确认删除目录【' + json.Result.NodeName + '】吗？')) {
                        NodeService.Delete(nodeId, function (res) {
                            var json = res.value;
                            if (json.Success) {
                                notify.success('删除成功');
                                setTimeout(function () {
                                    window.location.href = window.location.href;
                                }, 500);
                            } else {
                                notify.error(json.Error.Message);
                            }
                        });
                    }
                });
            });

            //主目录-编辑
            $('.content').find('[type-edit1],[type-edit2],[type-edit3]').on('click', function () {
                var $this = $(this);
                var nodeId = parseInt($this.data('id'));
                editNodeDialog.open({
                    nodeId: nodeId,
                    callback: function () {
                        setTimeout(function () {
                            window.location.href = window.location.href;
                        }, 500);
                    }
                });
            });
            //notify.success('123', 'haha');
        });
    </script>
</asp:Content>
