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

                    //正则匹配
                    var match = Regex.Match(jsonData, @"roomId\\"":\\""(\d+)\\"",");
                    //获取直播房间的Id
                    _liveRoomId = match.Groups[1].ToString();
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
            if (Input.GetKeyDown(KeyCode.A))
            {
                HttpClearHeader();
                HttpAddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                HttpAddHeader("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36");

                HttpRoutine routine = null;
                routine = HttpGet("https://www.tiktok.com/@sanma238/live", (jsonData) =>
                {
                    //正则匹配
                    var match = Regex.Match(jsonData, @"roomID"":""(\d+)""");

                    //获取直播房间的Id
                    var roomId = match.Groups[1].ToString();


                    var www = routine.GetWWW();
                    //获取Cookie
                    var cookie = www.GetResponseHeader("Set-Cookie");

                    SocketClearHeader();
                    //连接webSocket
                    SocketConnect($"wss://webcast16-ws-useast5.us.tiktok.com/webcast/im/push/?aid=1988&app_language=zh-Hans&app_name=tiktok_web&browser_language=zh-CN&browser_name=Mozilla&browser_online=true&browser_platform=Win32&browser_version=5.0%20%28Windows%20NT%2010.0%3B%20Win64%3B%20x64%29%20AppleWebKit%2F537.36%20%28KHTML%2C%20like%20Gecko%29%20Chrome%2F114.0.5735.289%20Safari%2F537.36&compress=gzip&cookie_enabled=true&cursor=1695782654316_7283331040054141577_1_1_0_0&debug=false&device_platform=web&heartbeatDuration=0&host=https%3A%2F%2Fwebcast.us.tiktok.com&identity=audience&imprp=u4b-7CnGyKSSI&internal_ext=fetch_time%3A1695782654316%7Cstart_time%3A1695782640256%7Cack_ids%3A7283330975113677611_2cc%2C7283330978453621509_2ce%2C7283330981062200106_2d0%2C7283330986615524101_2d2%2C7283330989174409989_2d2%2C7283330993376496390_2d4%2C7283330992858745605_2d4%2C7283330998685305605_2d6%2C7283331004545321771_2d8%2C7283330999999417090_2da%2C7283331012607822634_2dc%2C7283331008841337606_2dc%2C7283331013463640838_2de%2C7283331027602213637_2e4%2C%7Cflag%3A1%7Cseq%3A1%7Cnext_cursor%3A1695782654316_7283331040054141577_1_1_0_0%7Cwss_info%3A0-1695782654316-0-0&live_id=12&room_id={roomId}&screen_height=1080&screen_width=1920&tz_name=Asia%2FShanghai&update_version_code=1.3.0&version_code=270000&webcast_sdk_version=1.3.0", new Action(() =>
                    {

                    }));
                });
            }
        }

        public override void OnBeforDestroy()
        {

        }
    }
}
