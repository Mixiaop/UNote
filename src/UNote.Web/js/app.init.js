﻿require.config({
    urlArgs: 'v=' + (new Date().getTime()),
    waitSeconds: 30,
    paths: {
        //lib
        'jquery': '/lib/core/jquery.min',
        'jquery.signalR': '/lib/core/jquery.signalR-2.2.2',
        'bootstrap': '/lib/core/bootstrap.min',
        'bootstrap.colorpicker': '/lib/plugins/bootstrap-colorpicker/bootstrap-colorpicker.min',
        'bootstrap.datepicker': '/lib/plugins/bootstrap-datepicker/bootstrap-datepicker',
        'bootstrap.datepicker.zh-CN': '/lib/plugins/bootstrap-datepicker/locales/bootstrap-datepicker.zh-CN.min',
        'underscore': '/lib/core/underscore/underscore-1.8',
        'handlebars': '/lib/core/handlebars/handlebars-v2.0.0',
        'jquery.placeholder': '/lib/core/jquery.placeholder.min',
        'jquery.slimscroll': '/lib/core/jquery.slimscroll.min',
        'jquery.scrollLock': '/lib/core/jquery.scrollLock.min',
        'jquery.appear': '/lib/core/jquery.appear.min',
        'jquery.countTo': '/lib/core/jquery.countTo.min',
        'jquery.fineuploader': '/lib/plugins/jquery.fineuploader/jquery.fineuploader-3.4.1.min',
        'cropper': '/lib/plugins/cropper/cropper.min',
        'jquery.tagsinput': '/lib/plugins/jquery-tags-input/jquery.tagsinput.min',
        'jquery.ui': '/lib/plugins/jquery-ui/jquery-ui',
        'jquery.slimscroll': '/lib/plugins/jquery-slimscroll/jquery.slimscroll',
        'jquery.confirm': '/lib/plugins/jquery-confirm/jquery-confirm',
        'cookie': '/lib/core/js.cookie.min',
        'bs.notify': '/lib/plugins/bootstrap-notify/bootstrap-notify.min',
        'kindeditor': '/editors/kindeditor/kindeditor',
        'kindeditor/zh_CN': '/editors/kindeditor/lang/zh_CN',
        'signalR.hubs': '/js/signalR.hubs',
        //utilities
        'utils/notify': '/js/utils/notify.util',
        //notes
        'notes/addNodeDialog': '/js/notes/addNode.dialog',
        'notes/editNodeDialog': '/js/notes/editNode.dialog',
        'notes/contentInfoDialog': '/js/notes/contentInfo.dialog',
        'notes/moveContentDialog': '/js/notes/moveContent.dialog',
        'notes/addFollowerDialog': '/js/notes/addFollower.dialog',
        'notes/boards/tagSettingsDialog': '/js/notes/boards/tagSettings.dialog',
        'notes/boards/taskInfoDialog': '/js/notes/boards/taskInfo.dialog',
        //teams
        'teams/addMemberDialog': '/js/teams/addMember.dialog'

    },
    shim: {
        'jquery.signalR': { deps: ['jquery'] },
        'bootstrap': { deps: ['jquery'] },
        'bootstrap.colorpicker': { deps: ['jquery', 'bootstrap'] },
        'bootstrap.datepicker': { deps: ['jquery', 'bootstrap'] },
        'bootstrap.datepicker.zh-CN': { deps: ['bootstrap.datepicker'] },
        'jquery.scrollLock': { deps: ['jquery'] },
        'jquery.slimscroll': { deps: ['jquery'] },
        'jquery.appear': { deps: ['jquery'] },
        'jquery.fineuploader': { deps: ['jquery'] },
        'jquery.tagsinput': { deps: ['jquery'] },
        'jquery.ui': { deps: ['jquery'] },
        'jquery.confirm': { deps: ['jquery'] },
        'kindeditor/zh_CN': { deps: ['kindeditor'] },//, 'css!./editors/kindeditor/css/default.css',
        'cropper':{deps:['jquery']},
        'kindeditor/zh_CN': { deps: ['kindeditor'] },//, 'css!./editors/kindeditor/css/default.css',
        // 'handlebars': { exports: 'Handlebars' },
        'signalR.hubs': { deps: ['jquery.signalR'] }
    }
});

require.onError = function (err) {
    console.log(JSON.stringify(err));
    if (err.requireType === 'timeout') {
        console.log('modules: ' + err.requireModules);
    }
    throw err;
};

