using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TenonKit.Choir {

    public class SoundCore {

        SoundCoreContext ctx;

        public SoundCore(string assetsLabel, Transform soundRoot) {
            ctx = new SoundCoreContext(assetsLabel);
            ctx.Inject(soundRoot);
        }

        // Load
        public async Task LoadAssets() {
            var handle = Addressables.LoadAssetAsync<GameObject>(ctx.AssetsLabel);
            var prefab = await handle.Task;
            ctx.audioSourcePrefab = prefab.GetComponent<AudioSource>();
            ctx.assetsHandle = handle;
        }

        // Release
        public void ReleaseAssets() {
            if (ctx.assetsHandle.IsValid()) {
                Addressables.Release(ctx.assetsHandle);
            }
        }

        // Clear
        public void Clear() {
            ctx.Clear();
        }

        // Create Player
        public int CreateSoundPlayer(bool autoPlay, bool isLoop, string name = "SoundPlayer", AudioClip clip = null) {
            SoundPlayer soundPlayer = SoundCoreFactory.SpawnSoundPlayer(ctx, autoPlay, isLoop, name, clip);
            ctx.AddSoundPlayer(soundPlayer);
            return soundPlayer.ID;
        }

        // Play
        public void Play(int id) {
            var has = ctx.TryGetSoundPlayer(id, out SoundPlayer soundPlayer);
            if (!has) {
                throw new System.Exception("SoundPlayer not found");
            }
            soundPlayer.Play();
        }

        public void SetAudioSource(int id, AudioSource audioSource) {
            var has = ctx.TryGetSoundPlayer(id, out SoundPlayer soundPlayer);
            if (!has) {
                throw new System.Exception("SoundPlayer not found");
            }
            soundPlayer.SetAudioSource(audioSource);
        }

        // Pause
        public void Pause(int id) {
            var has = ctx.TryGetSoundPlayer(id, out SoundPlayer soundPlayer);
            if (!has) {
                throw new System.Exception("SoundPlayer not found");
            }
            soundPlayer.Pause();
        }

        // UnPause
        public void UnPause(int id) {
            var has = ctx.TryGetSoundPlayer(id, out SoundPlayer soundPlayer);
            if (!has) {
                throw new System.Exception("SoundPlayer not found");
            }
            soundPlayer.UnPause();
        }

        // Stop
        public void Stop(int id) {
            var has = ctx.TryGetSoundPlayer(id, out SoundPlayer soundPlayer);
            if (!has) {
                throw new System.Exception("SoundPlayer not found");
            }
            soundPlayer.Stop();
        }

        // Set Volume
        public void SetVolume(int id, float volume) {
            var has = ctx.TryGetSoundPlayer(id, out SoundPlayer soundPlayer);
            if (!has) {
                throw new System.Exception("SoundPlayer not found");
            }
            soundPlayer.SetVolume(volume);
        }

        // Set Mute
        public void SetMute(int id) {
            var has = ctx.TryGetSoundPlayer(id, out SoundPlayer soundPlayer);
            if (!has) {
                throw new System.Exception("SoundPlayer not found");
            }
            soundPlayer.SetMute();
        }

        // Set UnMute
        public void SetUnMute(int id) {
            var has = ctx.TryGetSoundPlayer(id, out SoundPlayer soundPlayer);
            if (!has) {
                throw new System.Exception("SoundPlayer not found");
            }
            soundPlayer.SetUnMute();
        }

    }

}