using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TenonKit.Choir {

    public class SoundCoreContext {

        SoundIDService iDService;
        public SoundIDService IDService => iDService;

        SortedList<int, SoundPlayer> singlePlayers;
        Dictionary<string, List<SoundPlayer>> playerGroups;

        Transform soundRoot;
        public Transform SoundRoot => soundRoot;

        public SoundCoreContext() {
            iDService = new SoundIDService();
            singlePlayers = new SortedList<int, SoundPlayer>();
        }

        public void Inject(Transform soundRoot) {
            this.soundRoot = soundRoot;
        }

        #region Single Player
        public void AddSinglePlayer(SoundPlayer soundPlayer) {
            singlePlayers.Add(soundPlayer.ID, soundPlayer);
        }

        public void RemoveSinglePlayer(SoundPlayer soundPlayer) {
            singlePlayers.Remove(soundPlayer.ID);
        }

        public void ForEachSinglePlayer(System.Action<SoundPlayer> action) {
            foreach (var soundPlayer in singlePlayers.Values) {
                action(soundPlayer);
            }
        }

        public bool TryGetSinglePlayer(int id, out SoundPlayer soundPlayer) {
            return singlePlayers.TryGetValue(id, out soundPlayer);
        }
        #endregion

        #region Group Player
        public void AddToPlayerGroup(SoundPlayer soundPlayer, string groupName) {
            if (playerGroups == null) {
                playerGroups = new Dictionary<string, List<SoundPlayer>>();
            }
            if (!playerGroups.ContainsKey(groupName)) {
                playerGroups.Add(groupName, new List<SoundPlayer>());
            }
            playerGroups[groupName].Add(soundPlayer);
        }

        public void RemoveFromPlayerGroup(SoundPlayer soundPlayer, string groupName) {
            if (playerGroups == null) {
                return;
            }
            if (playerGroups.ContainsKey(groupName)) {
                playerGroups[groupName].Remove(soundPlayer);
            }
        }

        public void RemovePlayerGroup(string groupName) {
            if (playerGroups == null) {
                return;
            }
            if (playerGroups.ContainsKey(groupName)) {
                playerGroups.Remove(groupName);
            }
        }

        public void ForEachPlayerInGroup(string groupName, System.Action<SoundPlayer> action) {
            if (playerGroups == null) {
                return;
            }
            if (playerGroups.ContainsKey(groupName)) {
                foreach (var soundPlayer in playerGroups[groupName]) {
                    action(soundPlayer);
                }
            }
        }

        public void ForEachPlayerInAllGroup(System.Action<SoundPlayer> action) {
            if (playerGroups == null) {
                return;
            }
            foreach (var soundPlayers in playerGroups.Values) {
                foreach (var soundPlayer in soundPlayers) {
                    action(soundPlayer);
                }
            }
        }

        public bool TryGetPlayerGroup(string groupName, out List<SoundPlayer> soundPlayers) {
            return playerGroups.TryGetValue(groupName, out soundPlayers);
        }
        #endregion

        public void Clear() {
            singlePlayers.Clear();
        }

    }

}