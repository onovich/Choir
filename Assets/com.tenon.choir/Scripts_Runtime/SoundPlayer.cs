using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TenonKit.Choir {

    public class SoundPlayer {

        int id;
        public int ID => id;

        AudioSource audioSource;
        public bool IsLoop => audioSource.loop;
        public bool IsPlaying => audioSource.isPlaying;

        public SoundPlayer() {
        }

        public void SetID(int id) {
            this.id = id;
        }

        public void SetAudioSource(AudioSource audioSource) {
            this.audioSource = audioSource;
        }

        public void Play() {
            audioSource.Play();
        }

        public void Stop() {
            audioSource.Stop();
        }

        public void Pause() {
            audioSource.Pause();
        }

        public void UnPause() {
            audioSource.UnPause();
        }

        public void SetVolume(float volume) {
            audioSource.volume = volume;
        }

        public void SetMute() {
            audioSource.mute = true;
        }

        public void SetUnMute() {
            audioSource.mute = false;
        }

    }

}