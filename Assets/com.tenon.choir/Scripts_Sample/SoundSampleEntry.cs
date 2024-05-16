using System;
using UnityEngine;

namespace TenonKit.Choir.Sample {

    public class SoundSampleEntry : MonoBehaviour {

        SoundCore soundCore;
        int bgmID;

        void Awake() {
            CLog.Log = Debug.Log;
            CLog.Error = Debug.LogError;
            CLog.Warning = Debug.LogWarning;

            Transform soundRoot = GameObject.Find("SoundRoot").transform;
            soundCore = new SoundCore(soundRoot);
        }

        void Init() {
            bgmID = soundCore.CreateSoundPlayer(true, false, "SoundPlayer");
        }

        void OnDestroy() {
            soundCore.TearDown();
        }

        public void PlayBGM() {
            soundCore.Play(bgmID);
        }

        public void PauseBGM() {
            soundCore.Pause(bgmID);
        }

        public void UnPauseBGM() {
            soundCore.UnPause(bgmID);
        }

        public void Clear() {
            soundCore.TearDown();
        }

    }

}