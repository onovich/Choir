using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using MortiseFrame.Swing;
using System.Threading;

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

        #region Tick
        public void Tick(float dt) {
            ApplyFadeInTask(dt);
            ApplyFadeOutTask(dt);
            ctx.RemoveTaskForEach((task) => {
                if (task.fadeType == SoundFadeEnum.FadeIn) {
                    ctx.RemoveFadeInTask(task);
                } else if (task.fadeType == SoundFadeEnum.FadeOut) {
                    ctx.RemoveFadeOutTask(task);
                }
            });
            ctx.ClearRemoveTask();
        }

        #endregion

        #region Task
        void ApplyFadeInTask(float dt) {
            ctx.FadeInTaskForEach((ref SoundFadeTaskModel task) => {

                int id = task.playerID;
                float duration = task.duration;
                var easingType = task.easingType;
                var easingMode = task.easingMode;

                ref float timer = ref task.timer;
                timer += dt;

                var has = ctx.TryGetSinglePlayer(id, out SoundPlayer soundPlayer);
                if (!has) {
                    CLog.Log($"SoundPlayer not found ID = {id}");
                    return;
                }
                if (timer >= duration) {
                    soundPlayer.SetFadeVolume(1);
                    ctx.AddRemoveTask(task);
                    return;
                }
                float v = EasingHelper.Easing(0, 1, timer, duration, easingType, easingMode);
                soundPlayer.SetFadeVolume(v);
            });
        }

        void ApplyFadeOutTask(float dt) {
            ctx.FadeOutTaskForEach((ref SoundFadeTaskModel task) => {

                int id = task.playerID;
                float duration = task.duration;
                var easingType = task.easingType;
                var easingMode = task.easingMode;

                ref float timer = ref task.timer;
                timer += dt;

                var has = ctx.TryGetSinglePlayer(id, out SoundPlayer soundPlayer);
                if (!has) {
                    CLog.Log($"SoundPlayer not found ID = {id}");
                    return;
                }
                if (timer >= duration) {
                    soundPlayer.SetFadeVolume(0);
                    soundPlayer.Stop();
                    ctx.AddRemoveTask(task);
                    return;
                }
                float v = EasingHelper.Easing(1, 0, timer, duration, easingType, easingMode);
                soundPlayer.SetFadeVolume(v);
            });
        }
        #endregion

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
            return soundPlayer.AudioSource;
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
        public void SetAndPlay(int id, AudioClip clip, bool fadeIn = false, float duration = 0.5f, EasingType easingType = EasingType.Linear, EasingMode easingMode = EasingMode.None) {
            var has = ctx.TryGetSinglePlayer(id, out SoundPlayer soundPlayer);
            if (!has) {
                CLog.Log($"SoundPlayer not found ID = {id}");
            }
            if (clip != null) {
                soundPlayer.SetAudioClip(clip);
            }

            if (fadeIn) {
                var task = CreateFadeTask(soundPlayer, SoundFadeEnum.FadeIn, duration,
                    easingType, easingMode);
                ctx.AddFadeInTask(task);
                soundPlayer.TryPlay();
                soundPlayer.SetFadeVolume(0);
                return;
            }
            soundPlayer.TryPlay();
            soundPlayer.SetFadeVolume(1);
        }

        SoundFadeTaskModel CreateFadeTask(SoundPlayer player, SoundFadeEnum fadeType, float duration, EasingType easingType, EasingMode easingMode) {
            SoundFadeTaskModel task = new SoundFadeTaskModel {
                playerID = player.ID,
                fadeType = fadeType,
                duration = duration,
                timer = 0f,
                easingType = easingType,
                easingMode = easingMode
            };
            return task;
        }

        public void Play(int id, bool fadeIn = false, float duration = 0.5f, EasingType easingType = EasingType.Linear, EasingMode easingMode = EasingMode.None) {
            var has = ctx.TryGetSinglePlayer(id, out SoundPlayer soundPlayer);
            if (!has) {
                CLog.Log($"SoundPlayer not found ID = {id}");
            }
            SetAndPlay(id, soundPlayer.AudioSource.clip, fadeIn, duration, easingType, easingMode);
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
        public void Stop(int id, bool fadeOut = false, float duration = 0.5f, EasingType easingType = EasingType.Linear, EasingMode easingMode = EasingMode.None) {
            var has = ctx.TryGetSinglePlayer(id, out SoundPlayer soundPlayer);
            if (!has) {
                CLog.Log($"SoundPlayer not found ID = {id}");
            }
            if (fadeOut) {
                var task = CreateFadeTask(soundPlayer, SoundFadeEnum.FadeOut, duration,
                    easingType, easingMode);
                ctx.AddFadeOutTask(task);
                return;
            }
            soundPlayer.Stop();
        }

        // Set Volume
        public void SetVolume(int id, float volume) {
            var has = ctx.TryGetSinglePlayer(id, out SoundPlayer soundPlayer);
            if (!has) {
                CLog.Log($"SoundPlayer not found ID = {id}");
            }
            soundPlayer.SetVolume_Force(volume);
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
                array[i].SetVolume_Force(volume);
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