//Board模式 - 任务弹出详情
define(['jquery', 'utils/notify', 'underscore', 'kindeditor', 'bootstrap'], function ($, notify, _) {

    var $container;

    //context
    var vc = {
        task: {                       //当前内容项
            NodeId: 0,                //分类Id
            Title: '',                //标题
            Body: '',                 //内容
            ColumnTaskFinished: false, //任务是否已完成
            FortmatCreationTime: '',  //创建任务的时间
            Tag: '',                  //已选中的标签, 逗号分割
            TagList:[],               //已选中的标签列表
            Column: {
                Title: ''             //列名称
            }
        },
        tags: [],                      //分类的标签列表
        isLoadingTags : false,         //是否已载入
        options: {
            id: 0,
            resTitle: function () { },
            resFinishd: function () { },
            resDelete: function () { },
            resBody: function () { }
        }
    };

    //-----------------------------------------------------------------
    //---------------modules-------------------------------------------
    vc.modules = {};

    vc.modules.title = function () {
        var $title;
        var $input;

        function _initialize() {
            if (vc.task.Id == undefined) {
                alert('error: task is null');
                return;
            }

            //-----------title
            $title = $container.find('.block-title .title a');
            $input = $container.find('.block-title .title input');
            $choose = $('.block-title .choose');

            //init bind datas
            $title.html(vc.task.Title);
            $input.val(vc.task.Title);
            $title.parent().find('span').html('&nbsp;&nbsp;/&nbsp;' + vc.task.Column.Title);

            if (vc.task.ColumnTaskFinished) {
                $title.addClass('text-line-through');
                $choose.find('input[type=checkbox]').prop('checked', true);
            } else {
                $title.removeClass('text-line-through');
                $choose.find('input[type=checkbox]').prop('checked', false);
            }

            //##event title
            $title.unbind('click');
            $title.bind('click', function () {
                $title.addClass('hidden');
                $input.removeClass('hidden');
                $input.focus();
                return false;
            });

            $input.unbind('blur');
            $input.bind('blur', function () {
                $title.removeClass('hidden');
                $input.addClass('hidden');

                var value = $.trim($input.val());
                if ($title.html() != value && value != '') {
                    $title.html(value);
                    vc.task.Title = value;
                    if (vc.options.resTitle != null) {
                        vc.options.resTitle(vc.task.Id, vc.task.Title);
                    }
                    BoardService.UpdateTaskTitle(vc.task.Id, value, function (res) {
                        if (!res.value.Success) {
                            console.log('error: BoardService.UpdateTaskTitle');
                        }
                    });
                } else {
                    //reset
                    $title.html(vc.task.Title);
                    $input.val(vc.task.Title);
                }
                return false;
            });


            //##event check
            $('.block-title .choose span').unbind('click');
            $('.block-title .choose span').bind('click', function (e) {
                var taskId = vc.task.Id;
                if (vc.task.ColumnTaskFinished) {
                    $choose.find('input[type=checkbox]').prop('checked', false);
                    $title.removeClass('text-line-through');

                    vc.task.ColumnTaskFinished = false;
                    if (vc.options.resFinishd != null)
                        vc.options.resFinishd(taskId, false);


                    //post
                    BoardService.CancelFinishTask(taskId, function (res) { });

                } else {
                    $choose.find('input[type=checkbox]').prop('checked', true);
                    $title.addClass('text-line-through');

                    vc.task.ColumnTaskFinished = true;
                    if (vc.options.resFinishd != null)
                        vc.options.resFinishd(taskId, true);

                    //post
                    BoardService.FinishTask(taskId, function (res) { });
                }
                return false;
            });
            //-----------end title

        }

        return {
            initialize: _initialize
        }
    }();

    vc.modules.body = function () {
        var $text;
        var $editor;
        var editor;

        function _bindEvents() {
            var $editorBlock = $container.find('.js-bodyEditor');
            var $bodyBlock = $container.find('.js-body');
            var $bodyText = $bodyBlock.find('.js-bodyText');

            //##events
            //##event body click
            $bodyBlock.unbind('click');
            $bodyBlock.bind('click', function (e) {
                var obj = e.srcElement || e.target;
                if (obj != undefined) {
                    var $element = $(obj);
                    if ($element.prop("tagName") != 'A') {
                        $editorBlock.removeClass('hidden');
                        $bodyBlock.addClass('hidden');
                    }
                } else {
                    $editorBlock.removeClass('hidden');
                    $bodyBlock.addClass('hidden');
                }
            });

            //##event editor commit click
            $editorBlock.find('.btn-success').unbind('click');
            $editorBlock.find('.btn-success').bind('click', function () {
                editor.sync();
                var value = $.trim($editor.val());
                vc.task.Body = value;
                if (vc.task.Body != '') {
                    $bodyBlock.find('.js-none').addClass('hidden');
                    $bodyText.removeClass('hidden');
                    $bodyText.html(vc.task.Body);
                    $editor.val(vc.task.Body);
                } else {
                    $bodyBlock.find('.js-none').removeClass('hidden');
                    $bodyText.addClass('hidden');
                }

                //回调
                vc.options.resBody(vc.task.Id, vc.task.Body);

                //post
                BoardService.UpdateTaskBody(vc.task.Id, value, function (res) {
                    if (!res.value.Success) {
                        console.log('error: BoardService.UpdateTaskBody');
                    }
                });

                $editorBlock.addClass('hidden');
                $bodyBlock.removeClass('hidden');
            });

            //##event editor cancel click
            $editorBlock.find('.btn-default').unbind('click');
            $editorBlock.find('.btn-default').bind('click', function () {
                $editorBlock.addClass('hidden');
                $bodyBlock.removeClass('hidden');
            });
        }

        function _initialize() {
            var $editorBlock = $container.find('.js-bodyEditor');
            var $bodyBlock = $container.find('.js-body');
            var $bodyText = $bodyBlock.find('.js-bodyText');

            vc.task.Body = $.trim(vc.task.Body);

            //editor
            //fix editor 重复loading
            $editorBlock.html($('.js-bodyEditor-copy').clone().html());
            $editor = $container.find('#txtTaskBody');
            $editor.val(vc.task.Body);

            KindEditor.options.filterMode = false;
            editor = KindEditor.create('#txtTaskBody', {
                width: '100%',
                items: ['source', '|',
                        'plainpaste', 'wordpaste', '|', 'justifyleft', 'justifycenter', 'justifyright',
                        'justifyfull', 'insertorderedlist', 'insertunorderedlist', 'clearhtml', '|', 'table', 'hr', 'fullscreen', '/',
                        'formatblock', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold',
                        'italic', 'underline', 'strikethrough', '|', 'image', 'multiimage',
                       'insertfile', 'link', 'unlink'],
                uploadJson: '/editors/kindeditor/handler/upload_json.ashx',
                filterMode: false
            });


            //init datas
            if (vc.task.Body != '') {
                $bodyBlock.find('.js-none').addClass('hidden');
                $bodyText.removeClass('hidden');
                $bodyText.html(vc.task.Body);
            } else {
                $bodyBlock.find('.js-none').removeClass('hidden');
                $bodyText.addClass('hidden');
            }

            $bodyBlock.removeClass('hidden');
            $editorBlock.addClass('hidden');

            _bindEvents();
        }

        return {
            initialize: _initialize
        }
    }();

    vc.modules.tags = function () {
        var $form;          // 选择标签的表单
        var $formTagList;   //
        var $btnAddNew;     // 添加新标签
        var $selectedList;  //已选择的列表

        //渲染设置标签的列表
        var _renderFormChooseTag = function () {
            if (vc.tags.length > 0) {
                _.each(vc.tags, function (t) {
                    $formTagList.append('<li ><a href="javascript:;" style="border-bottom-color:' + t.StyleColor + ';color:' + t.StyleColor + '" >' + t.Name + '</a></li>');
                });
            }
        }


        function _initialize() {
            $form = $container.find('.tags .form-choosetags');
            $formTagList = $container.find('.tags .form-choosetags ul');
            $btnAddNew = $container.find('.tags .contents a');
            $selectedList = $container.find('.tags .contents ul');

            //init datas
            $form.addClass('hidden');

            if (!vc.isLoadingTags) {
                TagService.GetAllTags(vc.task.NodeId, function (res) {
                    var json = res.value;
                    if (json.Success) {
                        if (json.Result.length > 0) {
                            vc.tags = json.Result;
                            _renderFormChooseTag();
                            vc.isLoadingTags = true;
                        }
                    }
                });
            }

            //##---events
            $btnAddNew.unbind('click');
            $btnAddNew.bind('click', function (e) {
                $form.removeClass('hidden');
                $(document).one('click', function () {
                    $form.addClass('hidden');
                });
                e.stopPropagation();
            });

            $form.unbind('click');
            $form.bind('click', function (e) {
                e.stopPropagation();
            });

            //close
            $form.find('.title a').unbind('click');
            $form.find('.title a').bind('click', function () {
                $form.addClass('hidden');
            });
        }

        return {
            initialize: _initialize
        }
    }();

    vc.modules.btns = function () {
        function _initialize() {
            //#event - delete
            $container.find('.js-btn-delete').unbind('click');
            $container.find('.js-btn-delete').bind('click', function () {
                if (confirm('你确认永远删除任务【' + vc.task.Title + '】吗')) {
                    vc.options.resDelete(vc.task.Id);
                    vc.close();
                    //post
                    BoardService.DeleteTask(vc.task.Id, function (res) {
                        if (!res.value.Success) {
                            console.log('error: BoardService.UpdateTaskBody');
                        }
                    });
                }
            });
        }
        return {
            initialize: _initialize
        }
    }();

    //-----------------------------------------------------------------
    //---------------constructor---------------------------------------
    function _initialize(callback) {
        $container = $('#modalTaskInfo');

        //init datas
        if (vc.options.id <= 0) {
            alert('error: vc.options.id must be greater than zero');
            return;
        }

        BoardService.GetTask(vc.options.id, function (res) {
            var json = res.value;
            if (json.Success) {
                vc.task = json.Result;
                callback();

                //##events 
                $('[data-toggle="tooltip"], .js-tooltip').tooltip({
                    container: 'body',
                    animation: false
                });

                

                //init modules
                vc.modules.title.initialize();
                vc.modules.body.initialize();
                vc.modules.tags.initialize();
                vc.modules.btns.initialize();

                //绑定任务创建时间
                $('.block-notice ul').html('<li>创建了任务,&nbsp;&nbsp;' + vc.task.FortmatCreationTime + '</li>');
            }
        });
    }

    //-----------------------------------------------------------------
    //---------------exposed methods-----------------------------------
    vc.open = function (options) {
        $.extend(vc.options, options);
        if ($container == undefined) {
            $.ajax({
                type: 'get',
                dataType: 'html',
                url: '/js/notes/boards/taskInfo.dialog.html?t=' + Math.random(),
                success: function (data) {
                    $('body').append(data);
                    _initialize(function () {
                        $container.off('shown.bs.modal').on('shown.bs.modal', function (e) {
                            $(document).off('focusin.modal');
                        });
                        $container.modal({ show: true });
                    });

                }
            });
        } else {
            _initialize(function () {
                $container.modal({ show: true });
            });
        }
    }

    vc.close = function () {
        if ($container != undefined) {
            $container.modal('hide');
        }
    }


    return vc;
});