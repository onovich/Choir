using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TenonKit.Choir {

    public static class SoundCoreFactory {

        public static SoundPlayer SpawnSoundPlayer(SoundCoreContext ctx, bool autoPlay, bool isLoop, string name, AudioClip clip) {
            var id = ctx.IDService.PickPlayerID();
            GameObject go = new GameObject();
            AudioSource audioSource = go.AddComponent<AudioSource>();
            audioSource.playOnAwake = autoPlay;
            audioSource.loop = isLoop;
            if (clip != null) {
                audioSource.clip = clip;
            }
            audioSource.name = "${name} - {id}";
            SoundPlayer soundPlayer = new SoundPlayer();
            soundPlayer.SetAudioSource(audioSource);
            soundPlayer.SetID(id);
            return soundPlayer;
        }

    }

}