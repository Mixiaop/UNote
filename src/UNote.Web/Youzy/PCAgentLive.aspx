<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/User.master" AutoEventWireup="true" CodeBehind="PCAgentLive.aspx.cs" Inherits="UNote.Web.Youzy.PCAgentLive" %>
<asp:Content ID="Content1" ContentPlaceHolderID="userHead" runat="server">
    <script src="http://player.ucloud.com.cn/release/js/ump.player_v1.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="userBody" runat="server">
    <%
        var onlineUserService = U.UPrimeEngine.Instance.Resolve<UNote.Services.Users.IOnlineUserService>();
        var users = onlineUserService.GetOnlineUsers();
        %>
     <style>
        .item{width:80px;height:40px;line-height:40px;text-align:center;font-size:16px;margin-right:20px;margin-bottom:10px;}
    </style>

    <div class="col-sm-8 col-sm-offset-2" style="margin-top:50px;">
        <div class="block">
            <div class="block-content">
                <h2 class="push-20">优志愿代理商学习平台<span class=" h5 font-w300 text-muted push-10-l"><%= users.Count %> 人在线</span>
                </h2>
                <!--直播区域-->
                <div id="mod_player" class="push-20"></div>
                <% foreach(var user in users){ %>
                <div class="item item-circle bg-gray-lighter ">
                    <%= user.NickName %>
                </div>
                <%} %>
                


            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="userFoot" runat="server">
    <script>
        var sUserAgent = navigator.userAgent.toLowerCase();
        var bIsIphoneOs = sUserAgent.match(/iphone os/i) == "iphone os";
        var bIsMidp = sUserAgent.match(/midp/i) == "midp";
        var bIsUc7 = sUserAgent.match(/rv:1.2.3.4/i) == "rv:1.2.3.4";
        var bIsUc = sUserAgent.match(/ucweb/i) == "ucweb";
        var bIsAndroid = sUserAgent.match(/android/i) == "android";
        var bIsCE = sUserAgent.match(/windows ce/i) == "windows ce";
        var bIsWM = sUserAgent.match(/windows mobile/i) == "windows mobile";
        if (bIsIphoneOs || bIsMidp || bIsUc7 || bIsUc || bIsAndroid || bIsCE || bIsWM) {
            window.location.href = 'h5agentlive.aspx';
        }

        getUrlParamValue = function (sParamName) {
            var sSearch = location.search;
            var oParam = {};
            sSearch = sSearch.substr(1, sSearch.length);
            var params = sSearch.split("&");
            for (var i = 0; i < params.length; i++) {
                var param = params[i].split("=");
                var key = param[0];
                var value = param[1];
                if (key in oParam) {
                    (oParam[key].constructor == Array) ? oParam[key].push(value) : oParam[key] = [oParam[key], value];
                }
                else {
                    oParam[key] = value;
                }
            }
            return (oParam[sParamName]) ? oParam[sParamName] : "";
        }

        var buffertime = getUrlParamValue("buffertime");
        buffertime = buffertime == "" ? 2 : buffertime;

        var player = new ump.Player1vN();
        var params = {
            //播放地址
            playurl: "rtmp://btrainrtmp.youzy.cn/btrain/ulive-kmewsc",
            //缓冲区大小，建议0.1-6秒，数值越小延时越小，但容易发生缓冲
            buffertime: buffertime,
            //封面图地址，播放器未开始播放前显示的图片
            //coverurl:"http://xxx.com/test.swf",
            //是否为自动播放
            autostart: true
        };
        //添加对初始化完成后的回调
        player.onInited = function (info) {
            ump.log("播放器初始化完成!");
            for (var o in info) {
                ump.log(o + "=" + info[o]);
            }

            /************收到初始化完成得消息后才可调用flash播放器的方法************/
            //开始播放
            //player.joinRoom({roomid:"ucloud"});

            //停止播放
            //player.stop();

            //获取当前播放状态，播放状态请参考player.onState里对应的值
            //var state = player.getState();

            /************更多调用接口请查看《API文档》**********/
        };
        //播放状态通知
        player.onState = function (info) {
            if (!info || (info && typeof info.type == undefined)) {
                return;
            }

            ump.log("播放状态：" + info.type);

            switch (info.type) {
                //视频加载中，调用播放接口时触发
                case "loading":
                    break;
                    //视频播放中，视频从loading转为可开始播放状态时触发
                case "playing":
                    break;
                    //视频停止，一般为视频正常结束、用户手动点击停止按钮或外部调用stop
                case "stop":
                    break;
            }
        };
        //播放器错误消息处理
        player.onError = function (info) {
            if (!info) {
                return;
            }

            if (info.type) {
                //不建议针对错误码做处理，用来做统计或定位问题使用
                ump.log("播放器错误码：" + info.type);
            }

            if (info.desc) {
                //显示错误提示给用户即可
                ump.log("播放器错误提示：" + info.desc);
            }
        };

        //用户主动触发的行为消息，通过代码调用的接口不响应此事件
        player.onUserAction = function (info) {
            if (!info || (info && typeof info.type == undefined)) {
                return;
            }

            ump.log("用户主动触发的行为:" + info.type);

            switch (info.type) {
                //点击播放按钮
                case "playBtnClick":
                    break;
                    //点击暂停按钮
                case "pauseBtnClick":
                    break;
                    //点击全屏按钮
                case "fullscreen":
                    break;
                    //点击退出全屏按钮
                case "normalscreen":
                    break;
                    //点击静音按钮
                case "muteBtnClick":
                    break;
                    //点击取消静音按钮
                case "unmuteBtnClick":
                    break;
                case "volumeChange":
                    ump.log("变化后的音量" + info.value);
                    break;
                default:
                    ump.log("Unkown user action!");
            }
        };

        //创建播放器
        player.create({
            width: '100%',
            height: 550,
            modId: "mod_player",
            params: params
        });

        playVideo = function () {
            if (inputCheck()) {
                var url = encodeURIComponent(document.getElementById("playUrlTxt").value);
                player.load({ playurl: url });
            }
        };

        inputCheck = function () {
            if (trim(document.getElementById("playUrlTxt").value) == "") {
                document.getElementById("playUrlTxt").focus();
                alert("请输入播放地址！");
                return false;
            }
            return true;
        };

        trim = function (str) {
            return null == str ? str : str.replace(/(^s*)|(s*$)/g, "");
        };

        ump.log("创建完成");
</script>

</asp:Content>
