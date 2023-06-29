/*********************************************
 * 
 * 脚本名：UIMainMenu.cs
 * 创建时间：2023/03/14 11:19:54
 *********************************************/
using Douyin;
using Framework;
using LitJson;
using MainPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

namespace GameData
{
    public partial class UIMainMenu : GameUIBase
    {
        private HttpRoutine _routine;
        private string _liveRoomId;
        private string _liveRoomTitle;
        private List<Message> _msgList = new List<Message>();

        public override void OnInit()
        {
            Btn_Start.AddListener(() =>
            {
                FnAddMsg(new Message()
                {
                    Method = "[获取直播间信息中]",
                });

                HttpClearHeader();
                HttpAddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                HttpAddHeader("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36");
                HttpAddHeader("cookie", "__ac_nonce=0638733a400869171be51");
                _routine = HttpGet(Txt_Url.text + Input_Room.text, new Action<string>((jsonData) =>
                {
                    var www = _routine.GetWWW();
                    //获取Cookie
                    var cookie = www.GetResponseHeader("Set-Cookie");
                    //正则匹配
                    var ttwid = Regex.Match(cookie, @"ttwid=\S+;").Value;
                    Debug.Log(ttwid);

                    //正则匹配 删除额外文本
                    var match = Regex.Match(jsonData, @"<script id=""RENDER_DATA"" type=""application/json"">\S+</script>");
                    var str = match.Value.Replace(@"<script id=""RENDER_DATA"" type=""application/json"">", "").Replace("</script>", "");
                    //url解码
                    string result = UnityWebRequest.UnEscapeURL(str);
                    //这里是个Json数据 转化为JsonData格式
                    var res = JsonMapper.ToObject(result);
                    //获取直播房间的Id和标题
                    var roomStore = res["app"]["initialState"]["roomStore"];
                    _liveRoomId = roomStore["roomInfo"]["roomId"].ToString();
                    _liveRoomTitle = roomStore["roomInfo"]["room"]["title"].ToString();
                    FnAddMsg(new Message()
                    {
                        Method = $"[获取直播间信息成功][RoomId={_liveRoomId}][Room标题={_liveRoomTitle}]",
                    });
                    FnAddMsg(new Message()
                    {
                        Method = $"[直播间Socket连接中]",
                    });

                    //远程服务器链接
                    var wsUrl = $"wss://webcast3-ws-web-lq.douyin.com/webcast/im/push/v2/?app_name=douyin_web&version_code=180800&webcast_sdk_version=1.3.0&update_version_code=1.3.0&compress=gzip&internal_ext=internal_src:dim|wss_push_room_id:{_liveRoomId}|wss_push_did:7188358506633528844|dim_log_id:20230521093022204E5B327EF20D5CDFC6|fetch_time:1684632622323|seq:1|wss_info:0-1684632622323-0-0|wrds_kvs:WebcastRoomRankMessage-1684632106402346965_WebcastRoomStatsMessage-1684632616357153318&cursor=t-1684632622323_r-1_d-1_u-1_h-1&host=https://live.douyin.com&aid=6383&live_id=1&did_rule=3&debug=false&maxCacheMessageNumber=20&endpoint=live_pc&support_wrds=1&im_path=/webcast/im/fetch/&user_unique_id=7188358506633528844&device_platform=web&cookie_enabled=true&screen_width=1440&screen_height=900&browser_language=zh&browser_platform=MacIntel&browser_name=Mozilla&browser_version=5.0%20(Macintosh;%20Intel%20Mac%20OS%20X%2010_15_7)%20AppleWebKit/537.36%20(KHTML,%20like%20Gecko)%20Chrome/113.0.0.0%20Safari/537.36&browser_online=true&tz_name=Asia/Shanghai&identity=audience&room_id={_liveRoomId}&heartbeatDuration=0&signature=00000000";
                    //var wsurl = $"wss://webcast3-ws-web-lf.douyin.com/webcast/im/push/v2/?app_name=douyin_web&version_code=180800&webcast_sdk_version=1.3.0&update_version_code=1.3.0&compress=gzip&internal_ext=internal_src:dim|wss_push_room_id:{liveRoomId}|wss_push_did:7225556390840206860|dim_log_id:20230627133936BF3338DA4A9A0D847CB0|fetch_time:1687844377038|seq:1|wss_info:0-1687844377038-0-0|wrds_kvs:WebcastRoomStatsMessage-1687844373304888613_WebcastRoomRankMessage-1687843893353235951_InputPanelComponentSyncData-1687842585909367290&cursor=t-1687844377038_r-1_d-1_u-1_h-1&host=https://live.douyin.com&aid=6383&live_id=1&did_rule=3&debug=false&maxCacheMessageNumber=20&endpoint=live_pc&support_wrds=1&im_path=/webcast/im/fetch/&user_unique_id=7225556390840206860&device_platform=web&cookie_enabled=true&screen_width=1920&screen_height=1080&browser_language=zh-CN&browser_platform=Win32&browser_name=Mozilla&browser_version=5.0%20(Windows%20NT%2010.0;%20WOW64)%20AppleWebKit/537.36%20(KHTML,%20like%20Gecko)%20Chrome/108.0.5359.125%20Safari/537.36&browser_online=true&tz_name=Asia/Shanghai&identity=audience&room_id={liveRoomId}&heartbeatDuration=0&signature=RK4PYIXagEE1SsNF"
                    //设置校验标头，没有ttwid会导致无法连接服务器
                    SocketClearHeader();
                    SocketAddHeader("cookie", ttwid);
                    SocketAddHeader("user-agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36");
                    //创建Socket
                    SocketConnect(wsUrl, new Action(() =>
                    {
                        FnAddMsg(new Message()
                        {
                            Method = $"[直播间Socket连接成功]",
                        });
                    }));
                }));
                //HttpAddHeader("OpenAI-Organization", "org-50YXlfPMr71tBVyIuEqtBSOF");
            });

            AddEventListener(UIEvent.OnReadMsg, (args) =>
            {
                var msg = args[0] as Message;
                FnAddMsg(msg);
            });

            RegisterUpdate(Update);
        }

        private void FnAddMsg(Message msg)
        {
            _msgList.Add(msg);
            Sv_Msg.AddAtBottom(1);
        }

        public override void OnShow(params object[] args)
        {
            Sv_Msg.Init(0, () => MsgUnitPool.CreateUnitToRect(Sv_Msg.content),
                (rt, index) =>
                {
                    var unit = MsgUnitPool.Find(rt);
                    var msg = _msgList[index];
                    unit.FnShow(msg);
                });
        }

        private void Update()
        {
            //===== SD WS =====
            //if (Input.GetKeyDown(KeyCode.A))
            //{
            //    //ws://192.168.1.75:7860/queue/join
            //    SocketConnect("ws://192.168.1.75:7860/queue/join", new Action(()=> {
            //        SocketSendMsg("{\"fn_index\":746,\"session_hash\":\"ij62sfwiza\"}");
            //    }));
            //}
            //if (Input.GetKeyDown(KeyCode.S))
            //{
            //    var imgBase64 = Base64Util.FileToBase64("C:/Users/admin/Desktop/20230626-101030.jpg", "image/jpeg");
            //    var audioBase64 = Base64Util.FileToBase64("C:/Users/admin/Desktop/英语.mp3", "audio/mpeg");
            //    StringBuilder sb = new StringBuilder();

            //    //{\"fn_index\":746,\"data\":[\" base64图片数据 \",{\"data\":\" base64视频数据 \",\"name\":\"随便写名.mp3\"},\"crop\",false,false,2,256,0],\"event_data\":null,\"session_hash\":\"ij62sfwiza\"}

            //    sb.Append("{\"fn_index\":746,\"data\":[\"");
            //    sb.Append(imgBase64.HtmlBase64);
            //    sb.Append("\",{\"data\":\" ");
            //    sb.Append(audioBase64.HtmlBase64);
            //    sb.Append("\",\"name\":\"英语.mp3\"},\"crop\",false,false,2,256,0],\"event_data\":null,\"session_hash\":\"ij62sfwiza\"}");

            //    SocketSendMsg(sb.ToString());
            //}
            //if (Input.GetKeyDown(KeyCode.D))
            //{
            //    SocketClose();
            //}
        }

        public override void OnBeforDestroy()
        {

        }
    }
}
