/*********************************************
 * BFramework
 * 热更入口
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using Framework;
using MainPackage;
using UnityEngine;

namespace GameData
{
    /// <summary>
    /// 挂载脚本加载热更
    /// </summary>
    public class HotUpdateMain : MonoBehaviour
    {
        void Start()
        {
            //初始化游戏总控制器
            GameEntry.Instance.gameObject.AddComponent<GameGod>();
            GameGod.Instance.Log(E_Log.Framework, "热更代码", "启动成功");
            //初始化表格
            GameGod.Instance.TableManager.Init(TableTypes.TableCtrlTypeArr);
            //背景音乐
            GameGod.Instance.AudioManager.PlayBackground("RetroComedy.ogg");
            //正式启动
            GameGod.Instance.UIManager.OpenUI<UIMainMenu>(E_UILevel.Common);
        }
    }
}
