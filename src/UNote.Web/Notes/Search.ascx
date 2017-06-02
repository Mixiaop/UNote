<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Search.ascx.cs" Inherits="UNote.Web.Notes.Search" %>
<!-- Search -->
                                <div class="block">
                                    <div class="block-header bg-gray-lighter">
                                        <h3 class="block-title">Search</h3>
                                    </div>
                                    <div class="block-content block-content-full">
                                        <div id="containerSearch">
                                            <div class="input-group input-group-lg">
                                                <input class="form-control" type="text"  placeholder="关键字" value="<%= Keywords %>">
                                                <div class="input-group-btn">
                                                    <button class="btn btn-default"><i class="fa fa-search"></i></button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- END Search -->
<script>
    require(['jquery', 'notes/contentInfoDialog', 'utils/notify'], function ($, contentInfoDialog, notify) {

        var $containerSearch = $('#containerSearch');
        var $keywords = $containerSearch.find('input[type=text]');
        var $commit = $containerSearch.find('button');

        var _commit = function () {
            var url = "<%= SearchUrl%>";

                window.location.href = url + $keywords.val();
            }
            $keywords.on('keydown', function (e) {
                if (e.keyCode == 13) {
                    _commit();
                }
            });
            $commit.on('click', _commit);

            //view
            $('a[name=btnView]').on('click', function () {
                var contentId = parseInt($(this).data('id'));
                contentInfoDialog.open({ contentId: contentId });
            });
        });
    </script>