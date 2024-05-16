using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TenonKit.Choir {

    public class SoundCoreContext {

        SoundIDService iDService;
        public SoundIDService IDService => iDService;

        string assetsLabel;
        public string AssetsLabel => assetsLabel;

        public AudioSource audioSourcePrefab;
        public AsyncOperationHandle assetsHandle;

        SortedList<int, SoundPlayer> soundPlayers;
        Transform soundRoot;

        public SoundCoreContext(string assetsLabel) {
            iDService = new SoundIDService();
            soundPlayers = new SortedList<int, SoundPlayer>();
            this.assetsLabel = assetsLabel;
        }

        public void Inject(Transform soundRoot) {
            this.soundRoot = soundRoot;
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