//
//-----------------------------------------------------------------
//Board模式 - 任务弹出详情
//-----------------------------------------------------------------
define(['jquery', 'utils/notify', 'underscore', 'kindeditor', 'bootstrap', 'jquery.confirm', 'bootstrap.datepicker'], function ($, notify, _) {

    //-----------------------------------------------------------------
    //---------------properties-------------------------------------------
    var $container;
    var initDataPicker = false;

    //context
    var vc = {
        task: {                        //当前内容项
            NodeId: 0,                 //分类Id
            TeamId: 0,                 //TeamId
            Title: '',                 //标题
            Body: '',                  //内容
            ColumnTaskFinished: false, //任务是否已完成
            FormatCreationTime: '',   //创建任务的时间
            Tag: '',                   //已选中的标签, 逗号分割
            FormatTags: [],             //已选中的标签列表
            Column: {
                Title: ''             //列名称
            },
            Followers: [],            //all followers {Id: id}
        },
        tags: [],                      //分类的标签列表
        users: [],                     //Team参与者列表 {Id: '', NickName:'',...}
        isLoadingTags: false,          //是否首次已载入
        isLoadingUsers: false,         //是否首次已载入
        options: {
            id: 0,
            resTitle: function () { },
            resFinishd: function () { },
            resDelete: function () { },
            resBody: function () { },
            resTags: function () { },
            resUsers: function () { },
            resExpirationDate: function () { }
        }
    };

    //-----------------------------------------------------------------
    //---------------methods-------------------------------------------
    function _bindJQueryEvents() {
        //##events 
        $('[data-toggle="tooltip"], .js-tooltip').tooltip({
            container: 'body',
            animation: false
        });
    }

    //是否已选中（存在）的标签
    function _existsTag(name) {
        var exists = false;
        if (vc.task.FormatTags.length > 0) {
            _.each(vc.task.FormatTags, function (t) {
                if (t == name) {
                    exists = true;
                }
            });
        }
        return exists;
    }

    //选择标签
    function _chooseTagPost(name) {
        if (!_existsTag(name)) {
            vc.task.FormatTags.push(name);
        } else {
            //已存在取消
            var tags = [];
            _.each(vc.task.FormatTags, function (t) {
                if (t != name)
                    tags.push(t);
            });

            vc.task.FormatTags = tags;
        }

        var tags = '';
        _.each(vc.task.FormatTags, function (t) {
            tags += t + ',';
        });

        if (tags != '') {
            tags = tags.substring(0, tags.length - 1);
        }

        //回调
        if (vc.options.resTags != undefined)
            vc.options.resTags(vc.task.Id, vc.task.FormatTags);

        BoardService.UpdateTaskTags(vc.task.Id, tags, function (res) {
            if (!res.value.Success) {
                console.log('error: BoardService.UpdateTaskTitle');
            } else {
                vc.modules.logs.initialize();
            }
        });
    }

    //是否存在参与者
    function _existsUser(userId) {
        var exists = false;
        if (vc.task.Followers.length > 0) {
            _.each(vc.task.Followers, function (user) {
                if (user.UserId == userId) {
                    exists = true;
                }
            });
        }
        return exists;
    }

    //选择参与者
    function _chooseUserPost(userId, nickName) {
        if (!_existsUser(userId)) {
            vc.task.Followers.push({ UserId: userId, NickName: nickName });

            //回调
            if (vc.options.resUsers != undefined)
                vc.options.resUsers(vc.task.Id, vc.task.Followers);

            //add
            BoardService.AddTaskFollower(vc.task.Id, userId, function (res) {
                vc.modules.logs.initialize();
                if (!res.value.Success) {
                    console.log('error: BoardService.AddTaskFollower');
                }
            });
        } else {
            //已存在取消
            var users = [];
            _.each(vc.task.Followers, function (user) {
                if (user.UserId != userId)
                    users.push(user);
            });

            vc.task.Followers = users;

            //回调
            if (vc.options.resUsers != undefined)
                vc.options.resUsers(vc.task.Id, vc.task.Followers);

            //remove
            BoardService.DeleteTaskFollower(vc.task.Id, userId, function (res) {
                vc.modules.logs.initialize();
                if (!res.value.Success) {
                    console.log('error: BoardService.DeleteTaskFollower');
                }
            });
        }
    }

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
                        vc.modules.logs.initialize();
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
                    BoardService.CancelFinishTask(taskId, function (res) {
                        vc.modules.logs.initialize();
                    });

                } else {
                    $choose.find('input[type=checkbox]').prop('checked', true);
                    $title.addClass('text-line-through');

                    vc.task.ColumnTaskFinished = true;
                    if (vc.options.resFinishd != null)
                        vc.options.resFinishd(taskId, true);

                    //post
                    BoardService.FinishTask(taskId, function (res) {
                        vc.modules.logs.initialize();
                    });
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
                    vc.modules.logs.initialize();
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

        //render已选中的标签
        var _renderSelctedTags = function () {
            $selectedList.html('');

            if (vc.task.FormatTags.length > 0) {
                _.each(vc.task.FormatTags, function (tag) {
                    var color = '';
                    _.each(vc.tags, function (obj) {
                        if (tag == obj.Name)
                            color = obj.StyleColor; //get color
                    });

                    if (color == '')
                        color = '#999';

                    $selectedList.append('<li><label class="label" style="background:' + color + ';">' + tag + '</label></li>');
                });
            }
            //<li><label class="label" style="background:#000;">111</label></li>
        }

        //first render设置标签的列表
        var _renderFormChooseTag = function () {
            if (vc.tags.length > 0) {
                $formTagList.html('');
                _.each(vc.tags, function (t) {
                    var $tag = '<li ><a href="javascript:;" style="background:' + t.StyleColor + ';color:#fff" data-name="' + t.Name + '" >';
                    if (_existsTag(t.Name)) {
                        $tag += '<span class="selected"></span>';
                    }
                    $tag += t.Name + '</a></li>';

                    $formTagList.append($tag);
                });
            }

            //##events
            $container.find('.tags .form-choosetags ul a').unbind('click');
            $container.find('.tags .form-choosetags ul a').bind('click', function () {
                var name = $(this).data('name');
                _chooseTagPost(name);
                if (_existsTag(name)) {
                    $(this).prepend('<span class="selected"></span>');
                } else {
                    $(this).text(name);
                }

                _renderSelctedTags();
                //console.log(vc.task.FormatTags);
            });
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
                            _renderSelctedTags();
                            vc.isLoadingTags = true;
                        }
                    }
                });
            } else {
                _renderSelctedTags();
            }

            //##---events
            $btnAddNew.unbind('click');
            $btnAddNew.bind('click', function (e) {
                $form.removeClass('hidden');
                $form.css('left', ($(this).position().left + 5) + 'px');

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

    vc.modules.users = function () {
        var $form;          // 选择参与者的表单
        var $formUserList;   //
        var $btnAddNew;     // 添加新参与者
        var $selectedList;  //已选择的列表

        //----------renders
        //first render设置参与者的列表

        var _renderFormUsers = function (users) {
            
            var index = 1;
            $formUserList.html('');
            _.each(users, function (u) {
                if (u != undefined && u.Id != undefined && u.NickName != null && index <= 6) {
                    var nickName = u.NickName.substring(0, 1);

                    var $user = '<li data-userid=' + u.Id + ' data-nickname="' + u.NickName + '"><div class="item item-circle bg-info-light text-info">' +
                                nickName + '</div>&nbsp;&nbsp;&nbsp;' + u.NickName;
                    if (_existsUser(u.Id)) {
                        $user += '<span class="selected"></span>';
                    }

                    $user += '</li>';

                    $formUserList.append($user);
                    index++;
                }
            });

            if ($formUserList.html() == '') {
                $formUserList.append('<li>该成员不在列表中</li>');
            }

            //##events
            $container.find('.users .form-chooseusers ul li').unbind('click');
            $container.find('.users .form-chooseusers ul li').bind('click', function () {
                var userid = $(this).data('userid');
                var name = $(this).data('nickname');
                _chooseUserPost(userid, name);

                if (_existsUser(userid)) {
                    $(this).append('<span class="selected"></span>');
                } else {
                    $(this).find('span').remove();
                }

                _renderSelectedList();
            });

            $container.find('.users .form-chooseusers input').unbind('keyup');
            $container.find('.users .form-chooseusers input').bind('keyup', function (e) {
                var w = $.trim($(this).val());
                var searchUsers = [];
                var index = 1;
                if (index <= 6) {
                    _.each(vc.users, function (user) {
                        if (user != undefined && user.Id != undefined && user.NickName != null && (user.NickName.indexOf(w) != -1 || user.PinYin.indexOf(w) != -1)) {
                            searchUsers.push(user);
                        }
                    });
                    index++;
                }
                if (w == '') {
                    _renderFormUsers(vc.users);
                } else {
                    _renderFormUsers(searchUsers);
                }
            });
        }

        var _firstRenderFormUsers = function () {
            if (vc.users.length > 0) {
                $formUserList.html('');

                _renderFormUsers(vc.users);
            }
        }

        //render已选中的标签
        var _renderSelectedList = function () {
            $selectedList.html('');

            if (vc.task.Followers.length > 0) {
                _.each(vc.task.Followers, function (user) {
                    var nickName = user.NickName.substring(0, 1);
                    $selectedList.append('<div class="item item-circle bg-info-light text-info js-tooltip" data-userid="' + user.UserId +
                                         '" title="' + user.NickName + '">' + user.NickName + '</div>');
                });
            }
            _bindJQueryEvents();
        }

        function _initialize() {
            $form = $container.find('.users .form-chooseusers');
            $formUserList = $container.find('.users .form-chooseusers ul');
            $btnAddNew = $container.find('.users .contents a');
            $selectedList = $container.find('.users .contents .items');

            //init datas
            $form.addClass('hidden');

            if (!vc.isLoadingUsers) {
                //first init
                TeamService.GetAllMembers(vc.task.TeamId, function (res) {
                    var json = res.value;
                    if (json.Success) {
                        if (json.Result.length > 0) {
                            vc.users = json.Result;
                            _firstRenderFormUsers();
                            _renderSelectedList();
                            vc.isLoadingUsers = true;
                        }
                    }
                });
            } else {
                _renderFormUsers(vc.users);
                _renderSelectedList();
            }

            //##---events
            $btnAddNew.unbind('click');
            $btnAddNew.bind('click', function (e) {
                $form.removeClass('hidden');
                $form.css('left', ($(this).position().left + 5) + 'px');
                $form.css('top', ($(this).position().top + 25) + 'px');
                $container.find('.users .form-chooseusers input').val('');
                $container.find('.users .form-chooseusers input').focus();

                $(document).one('click', function () {
                    $form.addClass('hidden');
                });
                e.stopPropagation();
            });

            $form.unbind('click');
            $form.bind('click', function (e) {
                e.stopPropagation();
            });
        }
        return {
            initialize: _initialize
        }
    }();

    vc.modules.options = function () {
        var $block;
        var $btnAction;
        var $btnDeleteTask;
        var committing = false;

        var _renderDate = function (date) {
            var $expirationDateBlock = $('.block-expiration');

            if (date != '') {
                $expirationDateBlock.removeClass('hidden');
                $expirationDateBlock.find('a').text(date);
            } else {
                $expirationDateBlock.addClass('hidden');
            }

            var $expirationDateInput = $block.find('.js-setExpirationDate').next();
            $expirationDateInput.val(date);
        }

        function _initialize() {
            $block = $('.block-options');
            $btnAction = $block.find('.btn-actions');
            $btnDeleteTask = $block.find('.js-deletetask');

            //renders 
            _renderDate(vc.task.ColumnTaskExpirationDate);

            //##event actions
            $btnAction.unbind('click');
            $btnAction.bind('click', function () {
                var $menus = $(this).next();
                $menus.css('top', $(this).position().top + 25 + 'px');
            });

            //delete task
            $btnDeleteTask.unbind('click');
            $btnDeleteTask.bind('click', function () {
                $.confirm({
                    confirmButtonClass: 'btn-danger',
                    title: '删除任务',
                    content: '你确认永远删除任务【' + vc.task.Title + '】吗?',
                    confirm: function () {
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
            });

            //preview task
            $block.find('.js-preview').prop('href', '/notes/content/' + vc.task.Id);

            //expiration date
            var datePicker = $block.find('.js-setExpirationDate').next().datepicker();
            var dateChoosed = false; //prevent repeat trigger
            datePicker.on('changeDate', function (e) {
                if (!dateChoosed) {
                    vc.task.ColumnTaskExpirationDate = date;
                    var date = $block.find('.js-setExpirationDate').next().val();
                    _renderDate(date);
                    if (vc.options.resExpirationDate != undefined)
                        vc.options.resExpirationDate(vc.task.Id, date);

                    //post
                    BoardService.UpdateTaskExpirationDate(vc.task.Id, date, function (res) {
                        if (!res.value.Success) {
                            console.log('error: BoardService.UpdateTaskExpirationDate');
                        } else {
                            dateChoosed = true;
                            vc.modules.logs.initialize();
                        }
                    });
                    datePicker.datepicker('hide');
                }
            });
            initDataPicker = true;

            $block.find('.js-setExpirationDate').unbind('click');
            $block.find('.js-setExpirationDate').bind('click', function () {
                datePicker.datepicker('show');

                $('.datepicker').css('left', ($(this).offset().left + 'px'));
                $('.datepicker').css('top', ($(this).offset().top - 50 + 'px'));
            });

            $('.block-expiration a').unbind('click');
            $('.block-expiration a').bind('click', function () {
                vc.task.ColumnTaskExpirationDate = '';
                _renderDate('');
                if (vc.options.resExpirationDate != undefined)
                    vc.options.resExpirationDate(vc.task.Id, '');
                //post
                BoardService.UpdateTaskExpirationDate(vc.task.Id, '', function (res) {
                    if (!res.value.Success) {
                        console.log('error: BoardService.UpdateTaskExpirationDate');
                    } else {
                        vc.modules.logs.initialize();
                    }
                });
            });
        }
        return {
            initialize: _initialize
        }
    }();

    vc.modules.logs = function () {
        var $logItems;
        var $btnShowMore;
        var showCount = 10;
        var dataLogs = [];
        var showmore = false;

        var _render = function (count) {
            $logItems.html('');
            if (count == 0) {
                _.each(dataLogs, function (log) {
                    $logItems.append('<li><a>' + log.User.NickName + '</a> ' + log.Desc + ',&nbsp;&nbsp;' + log.FormatCreationTime + '</li>');
                });
            } else {
                _.each(dataLogs, function (log) {
                    if (count > 0) {
                        $logItems.append('<li><a>' + log.User.NickName + '</a> ' + log.Desc + ',&nbsp;&nbsp;' + log.FormatCreationTime + '</li>');
                        count--;
                    }
                });
            }
        }

        function _initialize() {
            $logItems = $('.block-notice ul');
            $btnShowMore = $('.block-notice .showmore');
            showmore = false;

            BoardService.GetRecentTaskLogs(vc.task.Id, function (res) {
                if (res.value.Success) {
                    //$logItems.html('');
                    dataLogs = res.value.Result;
                    _render(showCount);

                    if (dataLogs.length > showCount) {
                        $btnShowMore.text('显示较早的 ' + dataLogs.length + ' 条动态');
                        $btnShowMore.removeClass('hidden');
                        showmore = true;
                    } else {
                        $btnShowMore.addClass('hidden');
                    }
                }
            });

            //##events
            $btnShowMore.unbind('click');
            $btnShowMore.bind('click', function () {
                if (showmore) {
                    _render(0);
                    showmore = false;
                    $btnShowMore.text('隐藏较早的动态');
                } else {
                    showmore = true;
                    _render(showCount);
                    $btnShowMore.text('显示较早的 ' + dataLogs.length + ' 条动态');
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

                //init modules
                vc.modules.title.initialize();
                vc.modules.body.initialize();
                vc.modules.tags.initialize();
                vc.modules.users.initialize();
                vc.modules.options.initialize();
                vc.modules.logs.initialize();

                _bindJQueryEvents();
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