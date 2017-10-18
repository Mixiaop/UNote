<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RightColumn.ascx.cs" Inherits="UNote.Web.Controls.RightColumn" %>
<%--最新的笔记--%>
<% if(Model.List!=null&& Model.List.Count>0){ %>
<div class="block rc-recentnotes">
                                    <div class="block-header bg-gray-lighter">
                                        <ul class="block-options">
                                            <li>
                                            </li>
                                        </ul>
                                        <h3 class="block-title">最近的笔记</h3>
                                    </div>
                                    <div class="block-content bg-white">
                                        <ul class="list list-simple">
                                            <% foreach(var content in Model.List){ %>
                                            <li>
                                                                
                                                <div class="push-5 clearfix">
                                                    <span class="font-s13 text-muted push-10-l pull-right"><% if (Model.LoginedUser != null && content.User != null)
                                                                                                                    { %><% if (Model.LoginedUser.Id != content.User.Id)
                                                                                                                    { %><a><%= content.User.FormatNickName %></a><%}
                                                                                                                    else
                                                                                                                    { %><a>我</a><%}
                                                                                                                    } %> &nbsp; <%= content.FormatCreationTime %></span>
                                                    <% if(content.Node!=null){ %>
                                                                <a href="<%= RouteContext.GetRouteUrl("Notes.Contents", content.NodeId) %>" data-toggle="tooltip" title="所属目录" class="node"><span class="label label-info push-5-r"><%= content.Team!=null?content.Team.Name+" / ":"" %> <%= content.Node.NodeName %></span></a>
                                                                <%} %>
                                                     <% if(content.Node.NodeType == UNote.NodeType.Html){ %>
                                                                <a class="title" href="<%= content.NodeHtmlHomePage %>" target="_blank" title="<%= content.Title %>"><%= content.Title %></a>
                                                                <%}else{ %>
                                                        <a href="<%= RouteContext.GetRouteUrl("Notes.ContentInfo", content.Id) %>" title="<%= content.Title %>" target="_blank" class="title"><%= content.Title %></a>
                                                    <%} %>
                                                </div>
                                                <%--<div class="font-s13"><%= content.Summary %></div>--%>
                                                
                                            </li>
                                            <%} %>
                                            
                                        </ul>
                                    </div>
                                </div>
<%} %>