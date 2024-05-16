using System;
using UnityEngine;

namespace TenonKit.Choir.Sample {

    public class SoundSampleEntry : MonoBehaviour {

        SoundCore soundCore;
        int bgmID;

        [SerializeField] SoundNavigationPanel soundNavigationPanel;
        [SerializeField] AudioClip bgmClip;
        [SerializeField] AudioClip seClip;
        [SerializeField][Range(0, 1)] float bgmOriginalVolume = 0.5f;
        [SerializeField][Range(0, 1)] float seOriginalVolume = 0.5f;
        [SerializeField] int seGroupCapacity = 4;

        void Awake() {
            CLog.Log = Debug.Log;
            CLog.Error = Debug.LogError;
            CLog.Warning = Debug.LogWarning;

            Transform soundRoot = GameObject.Find("SoundRoot").transform;
            soundCore = new SoundCore(soundRoot, 100);

            Init();
            Binding();

            soundNavigationPanel.SetSingleVolume(bgmOriginalVolume);
            soundNavigationPanel.SetGroupVolume(seOriginalVolume);
        }

        void Init() {
            bgmID = soundCore.CreateSoundPlayer(true, true, "BGM_Player", bgmClip);
            soundCore.CreateSoundPlayerGroup(false, seGroupCapacity, "SE_Player");
            soundNavigationPanel.Ctor();
        }

        void Binding() {
            soundNavigationPanel.SinglePlayHandle += BGM_Play;
            soundNavigationPanel.SinglePauseHandle += BGM_Pause;
            soundNavigationPanel.SingleUnpauseHandle += BGM_UnPause;
            soundNavigationPanel.SingleStopHandle += BGM_Stop;
            soundNavigationPanel.SingleSetVolumeHandle += BGM_SetVolume;
            soundNavigationPanel.SingleSetMuteHandle += BGM_SetMute;
            soundNavigationPanel.SingleSetUnMuteHandle += BGM_SetUnMute;

            soundNavigationPanel.GroupPlayHandle += SE_Play;
            soundNavigationPanel.GroupPauseHandle += SE_Pause;
            soundNavigationPanel.GroupUnpauseHandle += SE_UnPause;
            soundNavigationPanel.GroupStopHandle += SE_Stop;
            soundNavigationPanel.GroupSetVolumeHandle += SE_SetVolume;
            soundNavigationPanel.GroupSetMuteHandle += SE_SetMute;
            soundNavigationPanel.GroupSetUnMuteHandle += SE_SetUnMute;
        }

        void Unbinding() {
            soundNavigationPanel.SinglePlayHandle -= BGM_Play;
            soundNavigationPanel.SinglePauseHandle -= BGM_Pause;
            soundNavigationPanel.SingleUnpauseHandle -= BGM_UnPause;
            soundNavigationPanel.SingleStopHandle -= BGM_Stop;
            soundNavigationPanel.SingleSetVolumeHandle -= BGM_SetVolume;
            soundNavigationPanel.SingleSetMuteHandle -= BGM_SetMute;
            soundNavigationPanel.SingleSetUnMuteHandle -= BGM_SetUnMute;

            soundNavigationPanel.GroupPlayHandle -= SE_Play;
            soundNavigationPanel.GroupPauseHandle -= SE_Pause;
            soundNavigationPanel.GroupUnpauseHandle -= SE_UnPause;
            soundNavigationPanel.GroupStopHandle -= SE_Stop;
            soundNavigationPanel.GroupSetVolumeHandle -= SE_SetVolume;
            soundNavigationPanel.GroupSetMuteHandle -= SE_SetMute;
            soundNavigationPanel.GroupSetUnMuteHandle -= SE_SetUnMute;
        }

        void OnDestroy() {
            Unbinding();
            Clear();
        }

        #region BGM
        public void BGM_Play() {
            soundCore.Play(bgmID);
        }

        public void BGM_Pause() {
            soundCore.Pause(bgmID);
        }

        public void BGM_UnPause() {
            soundCore.UnPause(bgmID);
        }

        public void BGM_Stop() {
            soundCore.Stop(bgmID);
        }

        public void BGM_SetVolume(float volume) {
            soundCore.SetVolume(bgmID, volume);
        }

        public void BGM_SetMute() {
            soundCore.SetMute(bgmID);
        }

        public void BGM_SetUnMute() {
            soundCore.SetUnMute(bgmID);
        }
        #endregion

        #region SE
        public void SE_Play() {
            soundCore.PlayInGroupIfFree("SE_Player", seClip);
        }

        public void SE_Pause() {
            soundCore.PauseInGroup("SE_Player");
        }

        public void SE_UnPause() {
            soundCore.UnPauseInGroup("SE_Player");
        }

        public void SE_Stop() {
            soundCore.StopInGroup("SE_Player");
        }

        public void SE_SetVolume(float volume) {
            soundCore.SetVolumeInGroup("SE_Player", volume);
        }

        public void SE_SetMute() {
            soundCore.SetMuteInGroup("SE_Player");
        }

        public void SE_SetUnMute() {
            soundCore.SetUnMuteInGroup("SE_Player");
        }
        #endregion

        public void Clear() {
            soundCore.TearDown();
        }

    }

}