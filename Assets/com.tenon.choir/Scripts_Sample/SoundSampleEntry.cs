using System;
using UnityEngine;

namespace TenonKit.Choir.Sample {

    public class SoundSampleEntry : MonoBehaviour {

        SoundCore soundCore;

        void Awake() {
            CLog.Log = Debug.Log;
            CLog.Error = Debug.LogError;
            CLog.Warning = Debug.LogWarning;

            Transform soundRoot = GameObject.Find("SoundRoot").transform;
            soundCore = new SoundCore(soundRoot);
        }

        void Init() {
            var id = soundCore.CreateSoundPlayer(true, false, "SoundPlayer");
        }

        void OnDestroy() {

            soundCore.TearDown();
        }

        public void Play() {
            int id = soundCore.CreateSoundPlayer(true, false, "SoundPlayer");
            soundCore.Play(id);
        }

        public void Pause() {
            int id = soundCore.CreateSoundPlayer(true, false, "SoundPlayer");
            soundCore.Pause(id);
        }

        public void UnPause() {
            int id = soundCore.CreateSoundPlayer(true, false, "SoundPlayer");
            soundCore.UnPause(id);
        }

        public void Clear() {
            soundCore.TearDown();
        }

    }

}