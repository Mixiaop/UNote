//Board模式 - 标签设置（list、create、update）
define(['jquery', 'utils/notify', 'handlebars', 'bootstrap', 'bootstrap.colorpicker'], function ($, notify, handlebars, bs, bsColor) {

    var $container;
    var $newForm;
    var $tagList;

    var vc = {
        tagId: 0,    //mode = 2, need tagId>0
        options: {
            nodeId: 0,
            callback: function () { }
        }
    };

    function _initialize() {
        $container = $('#modalTagSettings');
        $newForm = $container.find('.new-form');
        $tagList = $container.find('#tagList');

        //events
        //add new 
        $container.find('.btn-default').unbind('click');
        $container.find('.btn-default').bind('click', function () {
            _openAddForm();
        });

        _renderList();
    }

    function _renderList() {
        TagService.GetAllTags(vc.options.nodeId, function (res) {
            var json = res.value;
            if (json.Success) {
                $tagList.html('');
                var $html = $('#tempTagItem').html();
                var template = handlebars.compile($html);
                var $data = template({ tags: json.Result });
                $tagList.append($data);

                //events
                $tagList.find('.btn-edit').unbind('click');
                $tagList.find('.btn-edit').bind('click', function () {
                    _openEditForm($(this));
                });

                $tagList.find('.btn-delete').unbind('click');
                $tagList.find('.btn-delete').bind('click', function () {
                    if (confirm('你确认永远删除标签【' + $(this).data('name') + '】吗')) {
                        TagService.DeleteTag($(this).data('id'), function (res) {
                            if (res.value.Success) {
                                _renderList();
                            }
                        });
                    }
                });
            }
        });
        
    }

    //open add new form
    function _openAddForm() {
        var $title = $container.find('input[name=title]');
        var $color = $container.find('input[name=color]');
        var $commit = $container.find('.btn-success');

        $title.text('');
        $color.text('');
        $commit.text('提交');
        $commit.attr('disabled', 'disabled');
        $newForm.css({ 'top': '100px' });
        $newForm.css({ 'left': '20px' });
        $newForm.removeClass('hidden');
        $title.focus();

        //events
        $color.colorpicker({ 'fomart': 'hex', 'inline': false });

        //add commit event
        $newForm.find('.btn-success').unbind('click');
        $newForm.find('.btn-success').bind('click', function () {
            if ($commit.text() == '提交') {
                $commit.text('提交中');
                TagService.CreateTag(vc.options.nodeId, $.trim($title.val()), $.trim($color.val()), function (res) {
                    var json = res.value;
                    if (json.Success) {
                        $newForm.addClass('hidden');
                        _renderList();
                    } else {
                        console.log('error: ' + JSON.stringify(json));
                    }
                    $commit.text('提交');
                });
            }
        });

        $color.keydown(function (e) {
            if (e.keyCode == 13) {
                $commit.click();
            }
        });

        $title.keyup(function () {
            if ($title.val() != '') {
                $commit.removeAttr('disabled');
            } else {
                $commit.attr('disabled', 'disabled');
            }
        });

        $newForm.find('.btn-default').unbind('click');
        $newForm.find('.btn-default').bind('click', function () {
            $newForm.addClass('hidden');
        });


    }

    //option edit form
    function _openEditForm($btn) {
        var $title = $container.find('input[name=title]');
        var $color = $container.find('input[name=color]');
        var $commit = $container.find('.btn-success');

        $title.text('');
        $color.text('');
        $commit.text('提交');
        $commit.removeAttr('disabled');
        
        var tagId = $btn.data('id');
        var name = $btn.data('name');
        var color = $btn.data('color');
        $newForm.css({ 'left': '223px', 'top': ($btn.offset().top - 30) + 'px' });
        $newForm.removeClass('hidden');

        $title.focus();
        $title.val(name);
        $color.val(color);

        //events
        $color.colorpicker({ 'fomart': 'hex', 'inline': false });

        //add commit event
        $newForm.find('.btn-success').unbind('click');
        $newForm.find('.btn-success').bind('click', function () {
            if ($commit.text() == '提交') {
                $commit.text('提交中');
                TagService.UpdateTag(tagId, $.trim($title.val()), $.trim($color.val()), function (res) {
                    var json = res.value;
                    if (json.Success) {
                        $newForm.addClass('hidden');
                        _renderList();
                    } else {
                        console.log('error: ' + JSON.stringify(json));
                    }
                    $commit.text('提交');
                });
            }
        });

        $color.keydown(function (e) {
            if (e.keyCode == 13) {
                $commit.click();
            }
        });

        $title.keyup(function () {
            if ($title.val() != '') {
                $commit.removeAttr('disabled');
            } else {
                $commit.attr('disabled', 'disabled');
            }
        });

        $newForm.find('.btn-default').unbind('click');
        $newForm.find('.btn-default').bind('click', function () {
            $newForm.addClass('hidden');
        });

    }

    //exposed methods
    vc.open = function (options) {
        $.extend(vc.options, options);
        if ($container == undefined) {
            $.ajax({
                type: 'get',
                dataType: 'html',
                url: '/js/notes/boards/tagSettings.dialog.html?t=' + Math.random(),
                success: function (data) {
                    $('body').append(data);
                    _initialize();
                    $container.modal({ show: true });
                }
            });
        } else {
            _initialize();
            $container.modal({ show: true });
        }
    }

    vc.close = function () {
        if ($container != undefined) {
            $container.modal('hide');
        }
    }


    return vc;
});