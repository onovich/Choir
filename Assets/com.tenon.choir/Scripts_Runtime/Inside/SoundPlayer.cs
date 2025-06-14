using UnityEngine;

namespace TenonKit.Choir {

    internal class SoundPlayer {

        int id;
        internal int ID => id;

        internal float playerVolumeFactor;
        internal float fadeVolumeFactor;

        AudioSource audioSource;
        internal AudioSource AudioSource => audioSource;
        internal bool IsLoop => audioSource.loop;
        internal bool IsPlaying => audioSource.isPlaying;

        internal SoundPlayer() {
            fadeVolumeFactor = 1f;
        }

        internal void SetID(int id) {
            this.id = id;
        }

        internal void SetAudioSource(AudioSource audioSource) {
            this.audioSource = audioSource;
        }

        internal void SetAudioClip(AudioClip clip) {
            audioSource.clip = clip;
        }

        internal void TearDown() {
            if (audioSource != null) {
                audioSource.Stop();
                GameObject.Destroy(audioSource.gameObject);
            }
        }

        internal bool TryPlay() {
            if (audioSource == null || audioSource.clip == null) {
                return false;
            }
            if (audioSource.isPlaying) {
                audioSource.Stop();
            }
            audioSource.Play();
            audioSource.volume = playerVolumeFactor * fadeVolumeFactor;
            return true;
        }

        internal void Stop() {
            audioSource.Stop();
        }

        internal void Pause() {
            audioSource.Pause();
        }

        internal void UnPause() {
            audioSource.UnPause();
        }

        internal void SetVolume_Force(float factor) {
            audioSource.volume = fadeVolumeFactor * factor;
            this.playerVolumeFactor = factor;
        }

        internal void SetFadeVolume(float fator) {
            audioSource.volume = playerVolumeFactor * fator;
            fadeVolumeFactor = fator;
        }

        internal void SetMute() {
            audioSource.mute = true;
        }

        internal void SetUnMute() {
            audioSource.mute = false;
        }

    }

}