using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TenonKit.Choir {

    public class SoundCore {

        SoundCoreContext ctx;

        public SoundCore(Transform soundRoot, int capacity) {
            ctx = new SoundCoreContext(capacity);
            ctx.Inject(soundRoot);
        }

        // Tear Down
        public void TearDown() {
            var len = ctx.TakeAllSinglePlayer(out SoundPlayer[] array);
            for (int i = 0; i < len; i++) {
                array[i]?.TearDown();
            }
            len = ctx.TakeAllGroupPlayer(out array);
            for (int i = 0; i < len; i++) {
                array[i]?.TearDown();
            }
            ctx.Clear();
        }

        #region  Single Player
        // Create Player
        public int CreateSoundPlayer(bool autoPlay, bool isLoop, string name = "SoundPlayer", AudioClip clip = null) {
            SoundPlayer soundPlayer = SoundCoreFactory.SpawnSoundPlayer(ctx, autoPlay, isLoop, name, clip);
            ctx.AddSinglePlayer(soundPlayer);
            return soundPlayer.ID;
        }

        // AudioSource
        public AudioSource GetAudioSource(int id) {
            var has = ctx.TryGetSinglePlayer(id, out SoundPlayer soundPlayer);
            if (!has) {
                CLog.Log($"SoundPlayer not found ID = {id}");
                return null;
            }
            return soundPlayer.audioSource;
        }

        // Tear Down Player
        public void TearDownPlayer(int id) {
            var has = ctx.TryGetSinglePlayer(id, out SoundPlayer soundPlayer);
            if (!has) {
                CLog.Log($"SoundPlayer not found ID = {id}");
            }
            soundPlayer.TearDown();
            ctx.RemoveSinglePlayer(soundPlayer);
        }

        // Play
        public void Play(int id, AudioClip clip = null) {
            var has = ctx.TryGetSinglePlayer(id, out SoundPlayer soundPlayer);
            if (!has) {
                CLog.Log($"SoundPlayer not found ID = {id}");
            }
            if (clip != null) {
                soundPlayer.SetAudioClip(clip);
            }
            soundPlayer.TryPlay();
        }

        // Pause
        public void Pause(int id) {
            var has = ctx.TryGetSinglePlayer(id, out SoundPlayer soundPlayer);
            if (!has) {
                CLog.Log($"SoundPlayer not found ID = {id}");
            }
            soundPlayer.Pause();
        }

        // UnPause
        public void UnPause(int id) {
            var has = ctx.TryGetSinglePlayer(id, out SoundPlayer soundPlayer);
            if (!has) {
                CLog.Log($"SoundPlayer not found ID = {id}");
            }
            soundPlayer.UnPause();
        }

        // Stop
        public void Stop(int id) {
            var has = ctx.TryGetSinglePlayer(id, out SoundPlayer soundPlayer);
            if (!has) {
                CLog.Log($"SoundPlayer not found ID = {id}");
            }
            soundPlayer.Stop();
        }

        // Set Volume
        public void SetVolume(int id, float volume) {
            var has = ctx.TryGetSinglePlayer(id, out SoundPlayer soundPlayer);
            if (!has) {
                CLog.Log($"SoundPlayer not found ID = {id}");
            }
            soundPlayer.SetVolume(volume);
        }

        // Set Mute
        public void SetMute(int id) {
            var has = ctx.TryGetSinglePlayer(id, out SoundPlayer soundPlayer);
            if (!has) {
                CLog.Log($"SoundPlayer not found ID = {id}");
            }
            soundPlayer.SetMute();
        }

        // Set UnMute
        public void SetUnMute(int id) {
            var has = ctx.TryGetSinglePlayer(id, out SoundPlayer soundPlayer);
            if (!has) {
                CLog.Log($"SoundPlayer not found ID = {id}");
            }
            soundPlayer.SetUnMute();
        }
        #endregion

        #region Group Player
        // Create Player Group
        public void CreateSoundPlayerGroup(bool autoPlay, int capacity, string groupName = "SoundPlayer", AudioClip clipArr = null) {
            SoundCoreFactory.SpawnSoundPlayerGroup(ctx, autoPlay, capacity, groupName, clipArr, (soundPlayer) => {
                ctx.AddToPlayerGroup(soundPlayer, groupName);
            });
        }

        // Tear Down Player Group
        public void TearDownPlayerGroup(string groupName) {
            var len = ctx.TakeAllPlayerInGroup(groupName, out SoundPlayer[] array);
            for (int i = 0; i < len; i++) {
                array[i].TearDown();
            }
            ctx.RemovePlayerGroup(groupName);
        }

        // Play In Group If Free
        public void PlayInGroupIfFree(string groupName, AudioClip clip = null) {
            var len = ctx.TakeAllPlayerInGroup(groupName, out SoundPlayer[] array);
            for (int i = 0; i < len; i++) {
                if (array[i].IsPlaying == false) {
                    if (clip != null) {
                        array[i].SetAudioClip(clip);
                    }
                    array[i].TryPlay();
                    return;
                }
            }
        }

        // Pause In Group
        public void PauseInGroup(string groupName) {
            var len = ctx.TakeAllPlayerInGroup(groupName, out SoundPlayer[] array);
            for (int i = 0; i < len; i++) {
                array[i].Pause();
            }
        }

        // UnPause In Group
        public void UnPauseInGroup(string groupName) {
            var len = ctx.TakeAllPlayerInGroup(groupName, out SoundPlayer[] array);
            for (int i = 0; i < len; i++) {
                array[i].UnPause();
            }
        }

        // Stop In Group
        public void StopInGroup(string groupName) {
            var len = ctx.TakeAllPlayerInGroup(groupName, out SoundPlayer[] array);
            for (int i = 0; i < len; i++) {
                array[i].Stop();
            }
        }

        // Set Volume In Group
        public void SetVolumeInGroup(string groupName, float volume) {
            var len = ctx.TakeAllPlayerInGroup(groupName, out SoundPlayer[] array);
            for (int i = 0; i < len; i++) {
                array[i].SetVolume(volume);
            }
        }

        // Set Mute In Group
        public void SetMuteInGroup(string groupName) {
            var len = ctx.TakeAllPlayerInGroup(groupName, out SoundPlayer[] array);
            for (int i = 0; i < len; i++) {
                array[i].SetMute();
            }
        }

        // Set UnMute In Group
        public void SetUnMuteInGroup(string groupName) {
            var len = ctx.TakeAllPlayerInGroup(groupName, out SoundPlayer[] array);
            for (int i = 0; i < len; i++) {
                array[i].SetUnMute();
            }
        }
        #endregion

    }

}