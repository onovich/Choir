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

        List<SoundFadeTaskModel> fadeOutTasks;
        List<SoundFadeTaskModel> fadeInTasks;
        List<SoundFadeTaskModel> removeList;

        Transform soundRoot;
        internal Transform SoundRoot => soundRoot;

        internal SoundCoreContext(int capacity) {
            iDService = new SoundIDService();
            singlePlayers = new SortedList<int, SoundPlayer>();
            temp = new SoundPlayer[capacity];
            fadeOutTasks = new List<SoundFadeTaskModel>(capacity);
            fadeInTasks = new List<SoundFadeTaskModel>(capacity);
            removeList = new List<SoundFadeTaskModel>(capacity);
        }

        internal void Inject(Transform soundRoot) {
            this.soundRoot = soundRoot;
        }

        #region Fade Out
        internal void AddFadeOutTask(SoundFadeTaskModel task) {
            if (!fadeOutTasks.Contains(task)) {
                fadeOutTasks.Add(task);
            }
        }

        internal void RemoveFadeOutTask(SoundFadeTaskModel task) {
            fadeOutTasks.Remove(task);
        }

        public delegate void RefAction<T>(ref T item);
        internal void FadeOutTaskForEach(RefAction<SoundFadeTaskModel> action) {
            for (int i = 0; i < fadeOutTasks.Count; i++) {
                var task = fadeOutTasks[i];
                action(ref task);
                fadeOutTasks[i] = task;
            }
        }
        #endregion

        #region Fade In
        internal void AddFadeInTask(SoundFadeTaskModel task) {
            if (!fadeInTasks.Contains(task)) {
                fadeInTasks.Add(task);
            }
        }

        internal void RemoveFadeInTask(SoundFadeTaskModel task) {
            fadeInTasks.Remove(task);
        }

        internal void FadeInTaskForEach(RefAction<SoundFadeTaskModel> action) {
            for (int i = 0; i < fadeInTasks.Count; i++) {
                var task = fadeInTasks[i];
                action(ref task);
                fadeInTasks[i] = task;
            }
        }
        #endregion

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

        #region Remove Task
        internal void AddRemoveTask(SoundFadeTaskModel task) {
            if (!removeList.Contains(task)) {
                removeList.Add(task);
            }
        }

        internal void RemoveTaskForEach(Action<SoundFadeTaskModel> action) {
            for (int i = 0; i < removeList.Count; i++) {
                action(removeList[i]);
            }
        }

        internal void ClearRemoveTask() {
            removeList.Clear();
        }
        #endregion


        internal void Clear() {
            singlePlayers.Clear();
            playerGroups.Clear();
            Array.Clear(temp, 0, temp.Length);
            fadeOutTasks.Clear();
            fadeInTasks.Clear();
            removeList.Clear();
        }

    }

}