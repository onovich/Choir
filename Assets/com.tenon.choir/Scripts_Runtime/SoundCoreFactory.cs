using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TenonKit.Choir {

    public static class SoundCoreFactory {

        public static SoundPlayer SpawnSoundPlayer(SoundCoreContext ctx, bool autoPlay, bool isLoop, string playerName, AudioClip clip) {

            // GameObject
            GameObject go = new GameObject();
            AudioSource audioSource = go.AddComponent<AudioSource>();
            go.transform.SetParent(ctx.SoundRoot);

            // AudioSource
            audioSource.clip = clip;
            audioSource.loop = isLoop;
            audioSource.playOnAwake = autoPlay;
            if (autoPlay) audioSource.Play();

            // ID
            var id = ctx.IDService.PickPlayerID();
            audioSource.name = $"{playerName} - {id}";
            SoundPlayer soundPlayer = new SoundPlayer();
            soundPlayer.SetAudioSource(audioSource);
            soundPlayer.SetID(id);
            return soundPlayer;
        }

        public static void SpawnSoundPlayerGroup(SoundCoreContext ctx, bool autoPlay, int capacity, string groupName, AudioClip clip, Action<SoundPlayer> onSpawn) {
            for (int i = 0; i < capacity; i++) {
                SoundPlayer soundPlayer = SpawnSoundPlayer(ctx, autoPlay, false, $"{groupName} - {i}", clip);
                onSpawn?.Invoke(soundPlayer);
            }
        }

    }

}