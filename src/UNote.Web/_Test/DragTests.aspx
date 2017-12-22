<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DragTests.aspx.cs" Inherits="UNote.Web._Test.DragTests" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="/css/bootstrap.min.css" />
    <link rel="stylesheet" href="/css/oneui.css" />
    <script type="text/javascript" src="/lib/core/jquery.min.js"></script>
    <script type="text/javascript" src="/lib/plugins/jquery-ui/jquery-ui.js"></script>
<style>
    body {
        background:#fff;
        padding-top:50px;
        padding-left:50px;
    }
  #sortable1, #sortable2, #sortable3 { list-style-type: none; margin: 0; padding: 0; float: left; margin-right: 10px; background: #eee; padding: 5px; width: 143px;}
  #sortable1 li, #sortable2 li, #sortable3 li { margin: 5px; padding: 5px; font-size: 1.2em; width: 120px; }
  </style>
  <script>
  $(function() {
      $(".draggable-item").sortable({
      connectWith: "ul"
    });
 
    $( "ul.dropfalse" ).sortable({
      connectWith: "ul",
      dropOnEmpty: true
    });
  $("div" ).sortable();
    //$( "#sortable1, #sortable2, #sortable3" ).disableSelection();
  });
  </script>
</head>
<body>
    <div class="container">
	<div class="col-lg-12">
        <!-- Block -->
                            <div class="block draggable-item">
                                <div class="block-header">
                                    <ul class="block-options">
                                        <li>
                                            <span class="draggable-handler text-gray"><i class="si si-cursor-move"></i></span>
                                        </li>
                                    </ul>
                                    <h3 class="block-title">Block</h3>
                                </div>
                                <div class="block-content">
                                    <p>Draggable &amp; Sortable..</p>
                                </div>
                            </div>
                            <!-- END Block -->
        <!-- Block -->
                            <div class="block draggable-item">
                                <div class="block-header">
                                    <ul class="block-options">
                                        <li>
                                            <span class="draggable-handler text-gray"><i class="si si-cursor-move"></i></span>
                                        </li>
                                    </ul>
                                    <h3 class="block-title">Block</h3>
                                </div>
                                <div class="block-content">
                                    <p>Draggable &amp; Sortable..</p>
                                </div>
                            </div>
                            <!-- END Block -->
        <!-- Block -->
                            <div class="block draggable-item">
                                <div class="block-header">
                                    <ul class="block-options">
                                        <li>
                                            <span class="draggable-handler text-gray"><i class="si si-cursor-move"></i></span>
                                        </li>
                                    </ul>
                                    <h3 class="block-title">Block</h3>
                                </div>
                                <div class="block-content">
                                    <p>Draggable &amp; Sortable..</p>
                                </div>
                            </div>
                            <!-- END Block -->
    <%--<div class="block">
<ul id="sortable1" class="droptrue">
  <li class="ui-state-default">可被放置到..</li>
  <li class="ui-state-default">..一个空列表中</li>
  <li class="ui-state-default">Item 3</li>
  <li class="ui-state-default">Item 4</li>
  <li class="ui-state-default">Item 5</li>
</ul>
        </div>
 <div class="block">
<ul id="sortable2" class="dropfalse">
  <li class="ui-state-highlight">不可被放置到..</li>
  <li class="ui-state-highlight">..一个空列表中</li>
  <li class="ui-state-highlight">Item 3</li>
  <li class="ui-state-highlight">Item 4</li>
  <li class="ui-state-highlight">Item 5</li>
</ul>
     </div>
 
        <div class="block">
<ul id="sortable3" class="droptrue">
</ul>
            </div>--%>
 
 </div></div>
</body>
</html>