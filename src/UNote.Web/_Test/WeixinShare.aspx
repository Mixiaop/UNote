<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WeixinShare.aspx.cs" Inherits="UNote.Web._Test.WeixinShare" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    hello weixinshare
     <script src="http://staticv2.youzy.cn/lib-js/jquery/jquery1.11.2.min.js"></script>
   <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script>
 var wxConfig = function (title, body, url, imgUrl) {
        $.ajax({
            type: "post",
            url: "/_Test/WeixinShareGetToken.aspx",
            data: "url=" + encodeURIComponent(window.location.href),
            dataType: "json",
            success: function (data) {
                //alert(JSON.stringify(data));
                wx.config({
                    debug: true, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
                    appId: data.appId, // 必填，公众号的唯一标识
                    timestamp: data.timestamp, // 必填，生成签名的时间戳
                    nonceStr: data.nonceStr, // 必填，生成签名的随机串
                    signature: data.signature,// 必填，签名，见附录1
                    jsApiList: ['onMenuShareTimeline', 'onMenuShareAppMessage', 'onMenuShareQQ', 'onMenuShareWeibo'] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
                });

                wx.ready(function () {
                    alert(1);
                    wx.onMenuShareTimeline({
                        title: title, // 分享标题
                        link: url, // 分享链接
                        imgUrl: imgUrl, // 分享图标
                        success: function () {
                            // 用户确认分享后执行的回调函数
                        },
                        cancel: function () {
                            // 用户取消分享后执行的回调函数
                        }
                    });
                    //分享给朋友
                    wx.onMenuShareAppMessage({
                        title: title, // 分享标题
                        desc: body, // 分享描述
                        link: url, // 分享链接
                        imgUrl: imgUrl, // 分享图标
                        type: '', // 分享类型,music、video或link，不填默认为link
                        dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
                        success: function () {
                            // 用户确认分享后执行的回调函数
                        },
                        cancel: function () {
                            // 用户取消分享后执行的回调函数
                        }
                    });
                    //QQ
                    wx.onMenuShareQQ({
                        title: title, // 分享标题
                        desc: body, // 分享描述
                        link: url, // 分享链接
                        imgUrl: imgUrl, // 分享图标
                        success: function () {
                            // 用户确认分享后执行的回调函数
                        },
                        cancel: function () {
                            // 用户取消分享后执行的回调函数
                        }
                    });
                    //QQ微博
                    wx.onMenuShareWeibo({
                        title: title, // 分享标题
                        desc: body, // 分享描述
                        link: url, // 分享链接
                        imgUrl: imgUrl, // 分享图标
                        success: function () {
                            // 用户确认分享后执行的回调函数
                        },
                        cancel: function () {
                            // 用户取消分享后执行的回调函数
                        }
                    });
                });
                wx.error(function (res) {
                    alert(JSON.stringify(res));
                });

            }
        });
    }

 wxConfig("优志愿", "", "", "http://s.youzy.cn/content/images/icons-apple-touch-128x128.jpg");
        </script>
</body>
</html>
