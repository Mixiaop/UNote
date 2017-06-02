define(['jquery', 'utils/notify', 'bootstrap'], function ($, notify) {

    var $container, $errorBox, $userList;
    var $commit;
    var users = [];

    var vm = {
        //committing: false,
        //form: {},
        options: {
            teamId: 0,
            callback: function () { }
        }
    };

    //function _showError(msg) {
    //    $errorBox.html(msg);
    //    $errorBox.removeClass('hide');
    //}

    //function _hideError() {
    //    $errorBox.html('');
    //    $errorBox.addClass('hide');
    //}


    function _initialize() {
        $container = $('#notes_addFollower');
        $errorBox = $container.find('.alert');
        $userList = $container.find('#userList');
        $commit = $('.btn-primary');

        $commit.click(function () {
            var userIds = '';
            $userList.find('input[type=checkbox]:checked').each(function () {
                userIds += $(this).val() + ',';
            });
            
            userIds = userIds.substr(0, userIds.length - 1);
            
            vm.options.callback(userIds);
            vm.close();
        });
    }


    //exposed methods
    vm.open = function (options) {
        $.extend(vm.options, options);
        if ($container == undefined) {
            $.ajax({
                type: 'get',
                dataType: 'html',
                url: '/js/notes/addFollower.dialog.aspx?teamId=' + vm.options.teamId + '&t=' + Math.random(),
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

    vm.close = function () {
        if ($container != undefined) {
            $container.modal('hide');
        }
    }


    return vm;
});

//<div class="col-xs-6"> 15800448791 (海鹏）</div>
//<div class="col-xs-6 text-right">
//                        <a href="" class="btn btn-primary btn-sm">添加</a>
//                  </div>