///*********************************************
// * BFramework
// * 加载窗口代码
// * 创建时间：2022/12/28 20:40:23
// *********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainPackage
{
    public class WinLoading : MonoBehaviour
    {
        /// <summary>
        /// 进度条
        /// </summary>
        [SerializeField]
        private Image Img_Progress;
        /// <summary>
        /// 检查更新中
        /// </summary>
        [SerializeField]
        private Text Txt_VersionCheck;
        /// <summary>
        /// 加载资源中
        /// </summary>
        [SerializeField]
        private Text Txt_LoadingAsset;
        /// <summary>
        /// 退出按钮
        /// </summary>
        [SerializeField]
        private Button Btn_Quit;
        /// <summary>
        /// 加载报错
        /// </summary>
        [SerializeField]
        private RectTransform Rt_Error;

        /// <summary>
        /// AB包是否初始化完毕
        /// </summary>
        public bool IsInitEnd = false;

        /// <summary>
        /// 是否加载中
        /// </summary>
        private bool _isLoading = false;

        private void Awake()
        {
            Btn_Quit.onClick.AddListener(Application.Quit);
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void CheckUpdate()
        {
            Txt_VersionCheck.gameObject.SetActive(true);
            Txt_LoadingAsset.gameObject.SetActive(false);
            Img_Progress.fillAmount = 0;
        }

        public void StartLoading()
        {
            Txt_VersionCheck.gameObject.SetActive(false);
            Txt_LoadingAsset.gameObject.SetActive(true);
            Img_Progress.fillAmount = 0;
            _isLoading = true;
        }

        public void ShowError()
        {
            Rt_Error.gameObject.SetActive(true);
        }

        private void Update()
        {
            //加载完毕就关闭界面
            if (IsInitEnd)
            {
                if (gameObject.activeSelf)
                {
                    gameObject.SetActive(false);
                }
                return;
            }

            if (_isLoading)
            {
                Txt_LoadingAsset.text = GameEntry.Instance.DowloadManager.LoadedABTimes.ToString() + "/" + GameEntry.Instance.DowloadManager.ABMd5InfoList.Count.ToString();
                Img_Progress.fillAmount = (float)GameEntry.Instance.DowloadManager.LoadedABTimes / GameEntry.Instance.DowloadManager.ABMd5InfoList.Count;
            }
        }
    }
}