<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Head.Master" AutoEventWireup="true" CodeBehind="BoardTests.aspx.cs" Inherits="UNote.Web._Test.Notes.BoardTests" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headCss" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="content">
                    <!-- Draggable Items with jQueryUI (.js-draggable-items class is initialized in App() -> uiHelperDraggableItems()) -->
                    <!-- For more info and examples you can check out https://jqueryui.com/sortable/ -->
                    <div class="row">
                        <div class="col-lg-12">
                             <!-- Block -->
                           <div class="task-column col-sm-4">
                                <div class="block-header draggable-handler">
                                    计划中
                                </div>
                                <div class="block-content task-item">
                                     <div class="block">
                                            <div class="block-content">
                                                <p>基础数据维护管理</p>
                                            </div>
                                        </div>
                                     <div class="block">
                                            <div class="block-content">
                                                <p>问答2.0 社区</p>
                                            </div>
                                        </div>
                                     <div class="block">
                                            <div class="block-content">
                                                <p>智能填报(浙江版).</p>
                                            </div>
                                        </div>
                                </div>
                            </div>
                             <!-- END Block -->
                            <!-- Block -->
                            <div class="task-column col-sm-4">
                                <div class="block-header draggable-handler">
                                    正在做
                                </div>
                                <div class="block-content task-item">
                                     <div class="block">
                                            <div class="block-content">
                                                <p>Draggable &amp; Sortable..</p>
                                            </div>
                                        </div>
                                     <div class="block">
                                            <div class="block-content">
                                                <p>Draggable &amp; Sortable..</p>
                                            </div>
                                        </div>
                                     <div class="block">
                                            <div class="block-content">
                                                <p>Draggable &amp; Sortable..</p>
                                            </div>
                                        </div>
                                </div>
                            </div>
                            <!-- END Block -->
                            <!-- Block -->
                            <div class="task-column col-sm-4">
                                <div class="block-header draggable-handler">
                                    已完成
                                </div>
                                <div class="block-content task-item">
                                     <div class="block">
                                            <div class="block-content">
                                                <p>11111111111111</p>
                                            </div>
                                        </div>
                                     <div class="block">
                                            <div class="block-content">
                                                <p>22222222222222</p>
                                            </div>
                                        </div>
                                     <div class="block">
                                            <div class="block-content">
                                                <p>33333333333333333</p>
                                            </div>
                                        </div>
                                </div>
                            </div>
                            <!-- END Block -->
                        </div>
                    </div>
                    <!-- END Draggable Items with jQueryUI -->
                </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="foot" runat="server">
     <style>
         .task-column {
         }
         .block-header {
             background:#666;
             color:#fff;
         }
    </style>
    <script>
        require(['jquery', 'jquery.ui'], function ($) {
            //$('.task-item').sortable({
            //    opacity: .75,
            //    start: function (e, ui) {
            //        ui.placeholder.css({
            //            'height': ui.item.outerHeight(),
            //            'margin-bottom': ui.item.css('margin-bottom')
            //        });
            //    }
            //});

            
            $('.col-lg-12').sortable({
                connectWith: '.task-column',
                dropOnEmpty: true,
                opacity: .75,
                handle: '.draggable-handler',
                start: function (e, ui) {
                    console.log('start');
                },
                stop: function (e, ui) {
                    console.log('end');
                }
            });

            $('.task-column .task-item').sortable({
                connectWith: '.task-item',
                dropOnEmpty: true,
                opacity: .75,
                //handle: '.draggable-handler',
                placeholder: 'draggable-placeholder',
                tolerance: 'pointer',
                start: function (e, ui) {
                    ui.placeholder.css({
                        'height': ui.item.outerHeight(),
                        'margin-bottom': ui.item.css('margin-bottom')
                    });
                }
            });
        });
    </script>
</asp:Content>
