using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TenonKit.Choir {

    public class SoundCoreContext {

        SoundIDService iDService;
        public SoundIDService IDService => iDService;

        SortedList<int, SoundPlayer> soundPlayers;

        public SoundCoreContext() {
            iDService = new SoundIDService();
            soundPlayers = new SortedList<int, SoundPlayer>();
        }

        public void AddSoundPlayer(SoundPlayer soundPlayer) {
            soundPlayers.Add(soundPlayer.ID, soundPlayer);
        }

        public void RemoveSoundPlayer(SoundPlayer soundPlayer) {
            soundPlayers.Remove(soundPlayer.ID);
        }

        public bool TryGetSoundPlayer(int id, out SoundPlayer soundPlayer) {
            return soundPlayers.TryGetValue(id, out soundPlayer);
        }

        public void Clear() {
            soundPlayers.Clear();
        }

    }

}