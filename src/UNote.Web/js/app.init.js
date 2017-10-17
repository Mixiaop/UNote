﻿require.config({
    urlArgs: 'v=' + (new Date().getTime()),
    waitSeconds: 30,
    paths: {
        //lib
        'jquery': '/lib/core/jquery.min',
        'bootstrap': '/lib/core/bootstrap.min',
        'underscore': '/lib/core/underscore/underscore-1.6.min',
        'jquery.placeholder': '/lib/core/jquery.placeholder.min',
        'jquery.slimscroll': '/lib/core/jquery.slimscroll.min',
        'jquery.scrollLock': '/lib/core/jquery.scrollLock.min',
        'jquery.appear': '/lib/core/jquery.appear.min',
        'jquery.countTo': '/lib/core/jquery.countTo.min',
        'jquery.fineuploader': '/lib/plugins/jquery.fineuploader/jquery.fineuploader-3.4.1.min',
        'jquery.tagsinput': '/lib/plugins/jquery-tags-input/jquery.tagsinput.min',
        'cookie': '/lib/core/js.cookie.min',
        'bs.notify': '/lib/plugins/bootstrap-notify/bootstrap-notify.min',
        'kindeditor': '/editors/kindeditor/kindeditor',
        'kindeditor/zh_CN': '/editors/kindeditor/lang/zh_CN',
        //utilities
        'utils/notify': '/js/utils/notify.util',
        //notes
        'notes/addNodeDialog': '/js/notes/addNode.dialog',
        'notes/editNodeDialog': '/js/notes/editNode.dialog',
        'notes/contentInfoDialog': '/js/notes/contentInfo.dialog',
        'notes/moveContentDialog': '/js/notes/moveContent.dialog',
        'notes/addFollowerDialog': '/js/notes/addFollower.dialog',
        //teams
        'teams/addMemberDialog': '/js/teams/addMember.dialog'

    },
    shim: {
        'bootstrap': { deps: ['jquery'] },
        'jquery.scrollLock': { deps: ['jquery'] },
        'jquery.slimscroll': { deps: ['jquery'] },
        'jquery.appear': { deps: ['jquery'] },
        'jquery.fineuploader': { deps: ['jquery'] },
        'jquery.tagsinput':{ deps: ['jquery']},
        'kindeditor/zh_CN': { deps: ['kindeditor'] }//, 'css!./editors/kindeditor/css/default.css'
        // 'handlebars': { exports: 'Handlebars' },
    }
});

require.onError = function (err) {
    console.log(JSON.stringify(err));
    if (err.requireType === 'timeout') {
        console.log('modules: ' + err.requireModules);
    }
    throw err;
};
