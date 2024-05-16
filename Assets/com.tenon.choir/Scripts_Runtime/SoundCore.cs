using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TenonKit.Choir {

    public class SoundCore {

        SoundCoreContext ctx;

        public SoundCore(Transform soundRoot) {
            ctx = new SoundCoreContext();
            ctx.Inject(soundRoot);
        }

        // Tear Down
        public void TearDown() {
            ctx.ForEachSinglePlayer((soundPlayer) => {
                soundPlayer.TearDown();
            });
            ctx.ForEachPlayerInAllGroup((soundPlayer) => {
                soundPlayer.TearDown();
            });
            ctx.Clear();
        }

        #region  Single Player
        // Create Player
        public int CreateSoundPlayer(bool autoPlay, bool isLoop, string name = "SoundPlayer", AudioClip clip = null) {
            SoundPlayer soundPlayer = SoundCoreFactory.SpawnSoundPlayer(ctx, autoPlay, isLoop, name, clip);
            ctx.AddSinglePlayer(soundPlayer);
            return soundPlayer.ID;
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
        public void CreateSoundPlayerGroup(bool autoPlay, int count, string groupName = "SoundPlayer", AudioClip clipArr = null) {
            SoundCoreFactory.SpawnSoundPlayerGroup(ctx, autoPlay, count, groupName, clipArr, (soundPlayer) => {
                ctx.AddToPlayerGroup(soundPlayer, groupName);
            });
        }

        // Tear Down Player Group
        public void TearDownPlayerGroup(string groupName) {
            ctx.ForEachPlayerInGroup(groupName, (soundPlayer) => {
                soundPlayer.TearDown();
            });
            ctx.RemovePlayerGroup(groupName);
        }

        // Play In Group If Free
        public void PlayInGroupIfFree(string groupName, AudioClip clip = null) {
            ctx.ForEachPlayerInGroup(groupName, (soundPlayer) => {
                if (soundPlayer.IsPlaying == false) {
                    if (clip != null) {
                        soundPlayer.SetAudioClip(clip);
                    }
                    soundPlayer.TryPlay();
                }
            });
        }

        // Pause In Group
        public void PauseInGroup(string groupName) {
            ctx.ForEachPlayerInGroup(groupName, (soundPlayer) => {
                soundPlayer.Pause();
            });
        }

        // UnPause In Group
        public void UnPauseInGroup(string groupName) {
            ctx.ForEachPlayerInGroup(groupName, (soundPlayer) => {
                soundPlayer.UnPause();
            });
        }

        // Stop In Group
        public void StopInGroup(string groupName) {
            ctx.ForEachPlayerInGroup(groupName, (soundPlayer) => {
                soundPlayer.Stop();
            });
        }

        // Set Volume In Group
        public void SetVolumeInGroup(string groupName, float volume) {
            ctx.ForEachPlayerInGroup(groupName, (soundPlayer) => {
                soundPlayer.SetVolume(volume);
            });
        }

        // Set Mute In Group
        public void SetMuteInGroup(string groupName) {
            ctx.ForEachPlayerInGroup(groupName, (soundPlayer) => {
                soundPlayer.SetMute();
            });
        }

        // Set UnMute In Group
        public void SetUnMuteInGroup(string groupName) {
            ctx.ForEachPlayerInGroup(groupName, (soundPlayer) => {
                soundPlayer.SetUnMute();
            });
        }
        #endregion

    }

}