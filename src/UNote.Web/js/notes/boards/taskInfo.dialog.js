//Board模式 - 任务弹出详情
define(['jquery', 'utils/notify', 'bootstrap'], function ($, notify) {

    var $container;

    var vc = {
        options: {
            callback: function () { }
        }
    };

    function _initialize() {
        $container = $('#modalTaskInfo');
    }

    //exposed methods
    vc.open = function (options) {
        $.extend(vc.options, options);
        if ($container == undefined) {
            $.ajax({
                type: 'get',
                dataType: 'html',
                url: '/js/notes/boards/taskInfo.dialog.html?teamId=' + vc.options.teamId + '&t=' + Math.random(),
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