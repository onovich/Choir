using UnityEngine;

namespace TenonKit.Choir {

    internal class SoundPlayer {

        int id;
        internal int ID => id;

        AudioSource audioSource;
        internal bool IsLoop => audioSource.loop;
        internal bool IsPlaying => audioSource.isPlaying;

        internal SoundPlayer() {
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
            audioSource?.Play();
            return audioSource != null;
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

        internal void SetVolume(float volume) {
            audioSource.volume = volume;
        }

        internal void SetMute() {
            audioSource.mute = true;
        }

        internal void SetUnMute() {
            audioSource.mute = false;
        }

    }

}