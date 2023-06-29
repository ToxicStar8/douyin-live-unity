/*********************************************
 * BFramework
 * 声音管理器
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using MainPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 声音管理器
    /// </summary>
    public class AudioManager : ManagerBase
    {
        //环境音
        private AudioSource _environmentAudio;
        //音效
        private AudioSource _soundAudio;
        //音乐
        private AudioSource _backgroundAudio;
        //音频挂载的游戏对象
        private GameObject gameObject;

        //背景音乐音量
        private float _volumeBackground;
        public float GetVolumeBackground() => _volumeBackground;
        public void SetVolumeBackground(float volume)
        {
            _volumeBackground = volume;
            _backgroundAudio.volume = _volumeBackground;
            PlayerPrefs.SetFloat(ConstDefine.VolumBackground, _volumeBackground);
        }
        //音效音量
        public float _volumeSound;
        public float GetVolumeSound() => _volumeSound;
        public void SetVolumeSound(float volume)
        { 
            _volumeSound = volume;
            _soundAudio.volume = _volumeSound;
            PlayerPrefs.SetFloat(ConstDefine.VolumSound, _volumeSound);
        }

        public override void OnStart()
        {
            gameObject = new GameObject("[Audio]");
            //音乐
            _backgroundAudio = gameObject.AddComponent<AudioSource>();
            _backgroundAudio.loop = true;
            _volumeBackground = PlayerPrefs.GetFloat(ConstDefine.VolumBackground, 1);
            //音效
            _soundAudio = gameObject.AddComponent<AudioSource>();
            _soundAudio.playOnAwake = false;
            _volumeSound = PlayerPrefs.GetFloat(ConstDefine.VolumSound, 1);
            //环境音
            _environmentAudio = gameObject.AddComponent<AudioSource>();
            _environmentAudio.playOnAwake = false;
            //挂载在游戏入口下
            gameObject.SetParent(GameEntry.Instance.transform);
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="audioName">带格式全名</param>
        public void PlayBackground(string audioName)
        {
            var audio = GameGod.Instance.LoadManager.LoadSync<AudioClip>(audioName);
            //声音
            _backgroundAudio.volume = _volumeBackground;
            //如果在播放 先停止
            if (_backgroundAudio.isPlaying)
            {
                _backgroundAudio.Stop();
            }
            //卸载资源
            //Debug.Log("oldName=" + _backgroundAudio.clip.name);
            //GameEntry.Instance.LoadManager.UnloadAsset(_backgroundAudio.clip.name);
            //更新音频
            _backgroundAudio.clip = audio;
            //播放
            _backgroundAudio.Play();
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        public void PlaySound(string audioName)
        {
            var audio = GameGod.Instance.LoadManager.LoadSync<AudioClip>(audioName);
            _soundAudio.PlayOneShot(audio);
        }

        public override void OnUpdate() { }
        public override void OnDispose() 
        {
            _environmentAudio = null;
            _soundAudio = null;
            _backgroundAudio = null;
            UnityEngine.Object.Destroy(gameObject);
        }
    }
}
