using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TenonKit.Choir {

    internal class SoundCoreContext {

        SoundIDService iDService;
        internal SoundIDService IDService => iDService;

        SortedList<int, SoundPlayer> singlePlayers;
        Dictionary<string, List<SoundPlayer>> playerGroups;
        SoundPlayer[] temp;

        Transform soundRoot;
        internal Transform SoundRoot => soundRoot;

        internal SoundCoreContext(int capacity) {
            iDService = new SoundIDService();
            singlePlayers = new SortedList<int, SoundPlayer>();
            temp = new SoundPlayer[capacity];
        }

        internal void Inject(Transform soundRoot) {
            this.soundRoot = soundRoot;
        }

        #region Single Player
        internal void AddSinglePlayer(SoundPlayer soundPlayer) {
            singlePlayers.Add(soundPlayer.ID, soundPlayer);
        }

        internal void RemoveSinglePlayer(SoundPlayer soundPlayer) {
            singlePlayers.Remove(soundPlayer.ID);
        }

        internal int TakeAllSinglePlayer(out SoundPlayer[] array) {
            array = temp;
            singlePlayers.Values.CopyTo(array, 0);
            return singlePlayers.Count;
        }

        internal bool TryGetSinglePlayer(int id, out SoundPlayer soundPlayer) {
            return singlePlayers.TryGetValue(id, out soundPlayer);
        }
        #endregion

        #region Group Player
        internal void AddToPlayerGroup(SoundPlayer soundPlayer, string groupName) {
            if (playerGroups == null) {
                playerGroups = new Dictionary<string, List<SoundPlayer>>();
            }
            if (!playerGroups.ContainsKey(groupName)) {
                playerGroups.Add(groupName, new List<SoundPlayer>());
            }
            playerGroups[groupName].Add(soundPlayer);
        }

        internal void RemoveFromPlayerGroup(SoundPlayer soundPlayer, string groupName) {
            if (playerGroups == null) {
                return;
            }
            if (playerGroups.ContainsKey(groupName)) {
                playerGroups[groupName].Remove(soundPlayer);
            }
        }

        internal void RemovePlayerGroup(string groupName) {
            if (playerGroups == null) {
                return;
            }
            if (playerGroups.ContainsKey(groupName)) {
                playerGroups.Remove(groupName);
            }
        }

        internal int TakeAllPlayerInGroup(string groupName, out SoundPlayer[] array) {
            if (playerGroups == null) {
                array = null;
                return 0;
            }
            if (!playerGroups.TryGetValue(groupName, out List<SoundPlayer> soundPlayers)) {
                array = null;
                return 0;
            }
            array = temp;
            soundPlayers.CopyTo(array, 0);
            return soundPlayers.Count;
        }

        internal int TakeAllGroupPlayer(out SoundPlayer[] array) {
            if (playerGroups == null) {
                array = null;
                return 0;
            }
            int count = 0;
            foreach (var soundPlayers in playerGroups.Values) {
                count += soundPlayers.Count;
            }
            array = temp;
            int index = 0;
            foreach (var soundPlayers in playerGroups.Values) {
                soundPlayers.CopyTo(array, index);
                index += soundPlayers.Count;
            }
            return count;
        }

        internal bool TryGetPlayerGroup(string groupName, out List<SoundPlayer> soundPlayers) {
            return playerGroups.TryGetValue(groupName, out soundPlayers);
        }
        #endregion

        internal void Clear() {
            singlePlayers.Clear();
            playerGroups.Clear();
            Array.Clear(temp, 0, temp.Length);
        }

    }

}