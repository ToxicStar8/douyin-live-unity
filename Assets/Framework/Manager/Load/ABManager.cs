/*********************************************
 * BFramework
 * AB包管理器
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using LitJson;
using MainPackage;
using UnityEngine;

namespace Framework
{
    public class ABManager : ManagerBase
    {
        /// <summary>
        /// AB包信息
        /// </summary>
        public ABInfo ABInfo { private set; get; }

        public override void OnStart()
        {
            if (!GameEntry.Instance.IsEditorMode || GameEntry.Instance.IsRunABPackage)
            {
                //非编辑器模式或者加载AB包模式下直接加载AB包索引信息
                var abPackage = AssetBundle.LoadFromFile(GameEntry.Instance.DowloadManager.SavePath + ConstDefine.ABInfoName);
                if (abPackage == null)
                {
                    GameGod.Instance.Log(E_Log.Error, "没有找到AB包！");
                    return;
                }
                var textAsset = abPackage.LoadAsset<TextAsset>(ConstDefine.ABInfoName + ".json");
                //转化为ABInfo（AB包的索引
                ABInfo = JsonMapper.ToObject<ABInfo>(textAsset.text);
                abPackage.Unload(true);
            }
        }

        public override void OnUpdate() { }
        public override void OnDispose() { }
    }
}
