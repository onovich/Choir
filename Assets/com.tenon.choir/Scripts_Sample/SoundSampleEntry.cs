using System;
using UnityEngine;

namespace TenonKit.Choir.Sample {

    public class SoundSampleEntry : MonoBehaviour {

        public string assetsLabel;
        public Transform soundRoot;
        SoundCore soundCore;
        bool isInit;

        void Awake() {
            CLog.Log = Debug.Log;
            CLog.Error = Debug.LogError;
            CLog.Warning = Debug.LogWarning;

            Transform soundRoot = GameObject.Find("SoundRoot").transform;
            soundCore = new SoundCore("Sound", soundRoot);

            Action main = async () => {
                await soundCore.LoadAssets();
                Init();
                isInit = true;
            };
            main.Invoke();
        }

        void Init() {
            var id = soundCore.CreateSoundPlayer(true, false, "SoundPlayer");
        }

        void Tick(float dt) {
            if (!isInit) {
                return;
            }
        }

        void OnDestroy() {
            soundCore.ReleaseAssets();
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
            soundCore.Clear();
        }

    }

}