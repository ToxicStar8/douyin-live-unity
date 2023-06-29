/*********************************************
 * 
 * 脚本名：UIMainMenu_MsgUnit.cs
 * 创建时间：2023/06/28 14:28:54
 *********************************************/
using Framework;
using MainPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public partial class UIMainMenu_MsgUnit : UnitBase
    {
        public override void OnInit()
        {

        }

        public void FnShow(Douyin.Message msg)
        {
            switch (msg.Method)
            {
                case "WebcastChatMessage":
                    var chatMessage = Douyin.ChatMessage.Parser.ParseFrom(msg.Payload);
                    Txt_Msg.text = chatMessage.User.NickName + "发送消息：" + chatMessage.Content;
                    Txt_Msg.color = Color.white;
                    //GameGod.Instance.Log(E_Log.Custom, chatMessage.User.NickName + "发送弹幕", chatMessage.Content, "#15FFC3");
                    break;

                case "WebcastMatchAgainstScoreMessage":
                    //
                    var matchAgainstScoreMessage = Douyin.MatchAgainstScoreMessage.Parser.ParseFrom(msg.Payload);
                    Txt_Msg.text = "matchAgainstScoreMessage消息";
                    Txt_Msg.color = Color.yellow;
                    //GameGod.Instance.Log(E_Log.Custom, "matchAgainstScoreMessage消息", null, "#15FFC3");
                    break;

                case "WebcastLikeMessage":
                    //点赞
                    var likeMessage = Douyin.LikeMessage.Parser.ParseFrom(msg.Payload);
                    Txt_Msg.text = likeMessage.User.NickName + "给主播点赞";
                    Txt_Msg.color = Color.blue;
                    //GameGod.Instance.Log(E_Log.Custom, likeMessage.User.NickName + "给主播点赞", null, "#15FFC3");
                    break;

                case "WebcastMemberMessage":
                    //xx成员进入直播间消息
                    var memberMessage = Douyin.MemberMessage.Parser.ParseFrom(msg.Payload);
                    Txt_Msg.text = memberMessage.User.NickName + "进入了直播间";
                    Txt_Msg.color = Color.black;
                    //GameGod.Instance.Log(E_Log.Custom, memberMessage.User.NickName + "进入了直播间", null, "#15FFC3");
                    break;

                case "WebcastGiftMessage":
                    //礼物
                    var giftMessage = Douyin.GiftMessage.Parser.ParseFrom(msg.Payload);
                    Txt_Msg.text = giftMessage.Common.Describe;
                    Txt_Msg.color = Color.red;
                    //GameGod.Instance.Log(E_Log.Custom, giftMessage.Common.Describe, null, "#15FFC3");
                    break;

                case "WebcastSocialMessage":
                    //关注
                    var socialMessage = Douyin.SocialMessage.Parser.ParseFrom(msg.Payload);
                    Txt_Msg.text = socialMessage.User.NickName + "关注了主播";
                    Txt_Msg.color = Color.green;
                    //GameGod.Instance.Log(E_Log.Custom, socialMessage.User.NickName + "关注了主播", null, "#15FFC3");
                    break;

                case "WebcastRoomUserSeqMessage":
                    //
                    var roomUserSeqMessage = Douyin.RoomUserSeqMessage.Parser.ParseFrom(msg.Payload);
                    Txt_Msg.text = "roomUserSeqMessage消息";
                    Txt_Msg.color = Color.yellow;
                    //GameGod.Instance.Log(E_Log.Custom, "roomUserSeqMessage消息", null, "#15FFC3");
                    break;

                case "WebcastUpdateFanTicketMessage":
                    //
                    var updateFanTicketMessage = Douyin.UpdateFanTicketMessage.Parser.ParseFrom(msg.Payload);
                    Txt_Msg.text = "updateFanTicketMessage消息";
                    Txt_Msg.color = Color.yellow;
                    //GameGod.Instance.Log(E_Log.Proto, "updateFanTicketMessage消息", null, "#15FFC3");
                    break;

                case "WebcastCommonTextMessage":
                    //
                    var commonTextMessage = Douyin.CommonTextMessage.Parser.ParseFrom(msg.Payload);
                    Txt_Msg.text = "commonTextMessage消息";
                    Txt_Msg.color = Color.yellow;
                    //GameGod.Instance.Log(E_Log.Custom, "commonTextMessage消息", null, "#15FFC3");
                    break;

                default:
                    Txt_Msg.text = msg.Method;
                    Txt_Msg.color = Color.cyan;
                    break;
            }
        }
    }
}
