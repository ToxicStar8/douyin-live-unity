/*********************************************
 * BFramework
 * 游戏数据枚举
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameData
{
    /// <summary>
    /// 主角类型
    /// </summary>
    public enum E_RoleType
    {
        None = 0,           //无
        Green = 1,          //绿龙
        Yellow = 2,         //黄龙
        Blue = 3,           //蓝龙
        Red = 4,            //红龙
    }

    /// <summary>
    /// 花色类型
    /// </summary>
    public enum E_SuitType
    {
        None = 0,           //无
        Spade = 1,          //黑桃
        Heart = 2,          //红心
        Club = 3,           //梅花
        Diamond = 4,        //方块
        Joker = 5,          //大小王
    }

    /// <summary>
    /// 状态类型
    /// </summary>
    public enum E_StateType
    {
        Stun = 1 << 1,      //眩晕，目标不再响应任何操控
        Root = 1 << 2,      //止步，目标不可位移
        Silence = 1 << 3,   //沉默，目标无法释放技能
        Invincible = 1 << 4,//无敌，目标不受任何伤害
        Purify = 1 << 5,    //净化，目标不受任何控制
        Invisible = 1 << 6, //隐身，目标不可见
    }

    /// <summary>
    /// 难度类型
    /// </summary>
    public enum E_DifficultyType
    {
        None = 0,           //无
        VeryEasy = 1,       //难度1
        Easy = 2,           //难度2
        Normal = 3,         //难度3
        Hard = 4,           //难度4
        VeryHard = 5,       //难度5
    }
}
