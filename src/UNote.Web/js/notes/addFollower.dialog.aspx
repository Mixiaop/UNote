<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addFollower.dialog.aspx.cs" Inherits="UNote.Web.js.notes.addFollower_dialog" %>

<div class="modal fade" id="notes_addFollower" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog modal-dialog-popin">
        <div class="modal-content">
            <div class="block block-themed block-transparent remove-margin-b">
                <div class="block-header bg-primary-dark">
                    <ul class="block-options">
                        <li>
                            <button data-dismiss="modal" type="button"><i class="si si-close"></i></button>
                        </li>
                    </ul>
                    <h3 class="block-title"><i class="fa fa-plus"></i> 添加关注</h3>
                </div>
                <div class="block-content">
                    <div class="form-horizontal push-10-t">
                        <div class="alert alert-danger hide">
                        </div>
                        <div class="form-group">
                            <div class="col-sm-12" id="userList">
                                <% foreach (var member in Members)
                                   {
                                       if (member.User != null)
                                       { 
                                       %>
                                <label class="css-input css-checkbox css-checkbox-lg css-checkbox-primary">
                                    <input type="checkbox" value="<%= member.UserId%>:<%= member.User.FormatNickName%>"><span></span> <div class="item item-circle bg-info-light text-info unote-avatar"><%= member.User.FormatNickName%></div>
                                </label> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <%}
                                   } %>
                               
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button class="btn  btn-primary" type="button">保存</button>
                <button class="btn  btn-default" type="button" data-dismiss="modal">关闭</button>
            </div>
        </div>
    </div>
</div>