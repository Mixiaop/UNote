<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Searchbar.ascx.cs" Inherits="UNote.Web.Notes.Searchbar" %>
<!-- Searchbar -->
                                <div class="note-searchbar" id="containerNoteSearchBar">
                                <div class="row">
                                    <div class="col-md-4 col-xs-4">
                                        <a href="<%= WriteUrl %>" class="btn btn-primary" data-toggle="tooltip" title="记上一笔"><i class="fa fa-pencil"></i> &nbsp;写笔记</a>
                                    </div>
                                    <div class="col-md-8 col-xs-8 search">
                                        <div class="input-group">
                                                   <input class="form-control" type="text"  placeholder="搜索笔记." value="<%= Keywords %>">
                                                   <span class="input-group-addon"><i class="fa fa-search"></i></span>
                                                </div>
                                    </div>
                                </div>
                                </div>
                                <!-- End Searchbar -->
<script>
    require(['jquery', 'notes/contentInfoDialog', 'utils/notify'], function ($, contentInfoDialog, notify) {

        var $containerSearch = $('#containerNoteSearchBar');
        var $keywords = $containerSearch.find('input[type=text]');
        //var $commit = $containerSearch.find('button');

        var _commit = function () {
            var url = "<%= SearchUrl%>";

            window.location.href = url + $keywords.val();
        }

        $keywords.on('keydown', function (e) {
            if (e.keyCode == 13) {
                _commit();
            }
        });
        //$commit.on('click', _commit);

        //view
        //$('a[name=btnView]').on('click', function () {
        //    var contentId = parseInt($(this).data('id'));
        //    contentInfoDialog.open({ contentId: contentId });
        //});
    });
    </script>