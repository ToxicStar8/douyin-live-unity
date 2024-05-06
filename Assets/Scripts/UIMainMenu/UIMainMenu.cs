/*********************************************
 * 
 * 脚本名：UIMainMenu.cs
 * 创建时间：2023/03/14 11:19:54
 *********************************************/
using Douyin;
using Framework;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace GameData
{
    public partial class UIMainMenu : MonoBehaviour
    {
        public UnityEngine.GameObject MsgUnit;
        public Button Btn_Start;
        public UnityEngine.UI.Text Txt_Url;
        public UnityEngine.UI.InputField Input_Room;
        public UnityEngine.UI.ScrollRect Sv_Msg;

        private Queue<GameObject> _msgGoQueue = new Queue<GameObject>();
        private HttpRoutine _routine;
        private string _liveRoomId;
        private string _liveRoomTitle;

        private void Awake()
        {
            MsgUnit.SetActive(false);

            Btn_Start.onClick.AddListener(() =>
            {
                FnAddMsg(new Message()
                {
                    Method = "[获取直播间信息中]",
                });

               HttpManager.Instance.ClearHeader();
                _routine = HttpManager.Instance.Get(Txt_Url.text, new Action<string>((jsonData) =>
                {
                    var www = _routine.GetWWW();
                    //获取Cookie
                    var cookie = www.GetResponseHeader("Set-Cookie");
                    //正则匹配
                    var ttwid = Regex.Match(cookie, @"ttwid=\S+;").Value;
                    Debug.Log(ttwid);
                    Connect(ttwid);
                }));
            });

            EventManager.Instance.AddEventListener(10000, (args) =>
            {
                var msg = args[0] as Message;
                FnAddMsg(msg);
            });
        }

        private void Connect(string ttwid)
        {
            HttpManager.Instance.ClearHeader();
            HttpManager.Instance.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            HttpManager.Instance.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
            HttpManager.Instance.AddHeader("Cookie",  ttwid +"__live_version__=%221.1.1.6573%22;  home_can_add_dy_2_desktop=%220%22; live_use_vvc=%22false%22; csrf_session_id=02ada872b083234a19ab94c449bdd303; FORCE_LOGIN=%7B%22videoConsumedRemainSeconds%22%3A180%7D;__ac_nonce=0658a797a00aba95e4b5d;");

            HttpManager.Instance.Get(Txt_Url.text + Input_Room.text, new Action<string>((jsonData) =>
            {
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

                //设置校验标头，没有ttwid会导致无法连接服务器
                SocketManager.Instance.ClearHeader();
                SocketManager.Instance.AddHeader("Cookie", ttwid);
                SocketManager.Instance.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.6045.160 Safari/537.36");
                //创建Socket
                SocketManager.Instance.Connect(wsUrl, new Action(() =>
                {
                    FnAddMsg(new Message()
                    {
                        Method = $"[直播间Socket连接成功]",
                    });
                }));
            }));
        }

        private void FnAddMsg(Message msg)
        {
            var unitGo = UnityEngine.Object.Instantiate<GameObject>(MsgUnit, Sv_Msg.content);
            unitGo.gameObject.SetActive(true);
            var unit = unitGo.GetComponent<UIMainMenu_MsgUnit>();
            unit.OnShow(msg);

            _msgGoQueue.Enqueue(unitGo);
            if (_msgGoQueue.Count > 100)
            {
                var needDestroyGo = _msgGoQueue.Dequeue();
                UnityEngine.Object.Destroy(needDestroyGo);
            }

            Sv_Msg.verticalNormalizedPosition = 0f;
        }

        private void Update()
        {
            SocketManager.Instance.OnUpdate();
        }
    }
}
