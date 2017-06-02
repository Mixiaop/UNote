<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserInfo.ascx.cs" Inherits="UNote.Web.Users.UserInfo" %>
<!-- About -->
                                <div class="block block-link-hover3" >
                                    <div class="block-header bg-gray-lighter">
                                        <h3 class="block-title">About</h3>
                                    </div>
                                    <div class="block-content block-content-full text-center">
                                        <div>
                                            <a href="#" title="编辑个人信息">
                                            <img class="img-avatar img-avatar96" src="/img/avatars/avatar1.jpg" alt="">
                                                </a>
                                        </div>
                                        <div class="h5 push-15-t push-5"><%= LoginedUser.FormatNickName %></div>
                                    </div>
                                    <div class="block-content border-t">
                                        <div class="row items-push text-center">
                                            <div class="col-xs-12">
                                                <a href="<%= Routes.Nodes_AddContent %>?nodeid=<%= NodeId %>" title="写笔记" style="color:#666;"><div class="push-5"><i class="si si-pencil fa-2x"></i></div></a>
                                                <div class="h5 font-w300 text-muted"><%= ContentTotal %> 条笔记</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- END About -->