using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TenonKit.Choir.Sample {

    public class SoundNavigationPanel : MonoBehaviour {

        [SerializeField] Button btn_single_play;
        [SerializeField] Button btn_single_pause;
        [SerializeField] Button btn_single_unpause;
        [SerializeField] Button btn_single_stop;
        [SerializeField] Scrollbar scr_single_setvolume;
        [SerializeField] Button btn_single_setMute;
        [SerializeField] Button btn_single_setUnMute;

        [SerializeField] Button btn_group_play;
        [SerializeField] Button btn_group_pause;
        [SerializeField] Button btn_group_unpause;
        [SerializeField] Button btn_group_stop;
        [SerializeField] Scrollbar scr_group_setvolume;
        [SerializeField] Button btn_group_setMute;
        [SerializeField] Button btn_group_setUnMute;

        public Action SinglePlayHandle;
        public Action SinglePauseHandle;
        public Action SingleUnpauseHandle;
        public Action SingleStopHandle;
        public Action<float> SingleSetVolumeHandle;
        public Action SingleSetMuteHandle;
        public Action SingleSetUnMuteHandle;

        public Action GroupPlayHandle;
        public Action GroupPauseHandle;
        public Action GroupUnpauseHandle;
        public Action GroupStopHandle;
        public Action<float> GroupSetVolumeHandle;
        public Action GroupSetMuteHandle;
        public Action GroupSetUnMuteHandle;

        public void SetSingleVolume(float value) {
            scr_single_setvolume.value = value;
        }

        public void SetGroupVolume(float value) {
            scr_group_setvolume.value = value;
        }

        public void Ctor() {
            btn_single_play.onClick.AddListener(() => {
                SinglePlayHandle?.Invoke();
            });
            btn_single_pause.onClick.AddListener(() => {
                SinglePauseHandle?.Invoke();
            });
            btn_single_unpause.onClick.AddListener(() => {
                SingleUnpauseHandle?.Invoke();
            });
            btn_single_stop.onClick.AddListener(() => {
                SingleStopHandle?.Invoke();
            });
            scr_single_setvolume.onValueChanged.AddListener((value) => {
                SingleSetVolumeHandle?.Invoke(value);
            });
            btn_single_setMute.onClick.AddListener(() => {
                SingleSetMuteHandle?.Invoke();
            });
            btn_single_setUnMute.onClick.AddListener(() => {
                SingleSetUnMuteHandle?.Invoke();
            });

            btn_group_play.onClick.AddListener(() => {
                GroupPlayHandle?.Invoke();
            });
            btn_group_pause.onClick.AddListener(() => {
                GroupPauseHandle?.Invoke();
            });
            btn_group_unpause.onClick.AddListener(() => {
                GroupUnpauseHandle?.Invoke();
            });
            btn_group_stop.onClick.AddListener(() => {
                GroupStopHandle?.Invoke();
            });
            scr_group_setvolume.onValueChanged.AddListener((value) => {
                GroupSetVolumeHandle?.Invoke(value);
            });
            btn_group_setMute.onClick.AddListener(() => {
                GroupSetMuteHandle?.Invoke();
            });
            btn_group_setUnMute.onClick.AddListener(() => {
                GroupSetUnMuteHandle?.Invoke();
            });

            scr_single_setvolume.value = .5f;
        }

        public void TearDown() {
            btn_single_play.onClick.RemoveAllListeners();
            btn_single_pause.onClick.RemoveAllListeners();
            btn_single_unpause.onClick.RemoveAllListeners();
            btn_single_stop.onClick.RemoveAllListeners();
            scr_single_setvolume.onValueChanged.RemoveAllListeners();
            btn_single_setMute.onClick.RemoveAllListeners();
            btn_single_setUnMute.onClick.RemoveAllListeners();

            btn_group_play.onClick.RemoveAllListeners();
            btn_group_pause.onClick.RemoveAllListeners();
            btn_group_unpause.onClick.RemoveAllListeners();
            btn_group_stop.onClick.RemoveAllListeners();
            scr_group_setvolume.onValueChanged.RemoveAllListeners();
            btn_group_setMute.onClick.RemoveAllListeners();
            btn_group_setUnMute.onClick.RemoveAllListeners();

            SinglePlayHandle = null;
            SinglePauseHandle = null;
            SingleUnpauseHandle = null;
            SingleStopHandle = null;
            SingleSetVolumeHandle = null;
            SingleSetMuteHandle = null;
            SingleSetUnMuteHandle = null;

            GroupPlayHandle = null;
            GroupPauseHandle = null;
            GroupUnpauseHandle = null;
            GroupStopHandle = null;
            GroupSetVolumeHandle = null;
            GroupSetMuteHandle = null;
            GroupSetUnMuteHandle = null;
            GameObject.Destroy(gameObject);
        }

    }

}