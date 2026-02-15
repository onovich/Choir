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

        Dictionary<int, SoundFadeTaskModel> fadeOutTasks;
        Dictionary<int, SoundFadeTaskModel> fadeInTasks;
        Dictionary<int, SoundFadeTaskModel> removeList;
        Dictionary<int, SoundFadeTaskModel> modifiedList;
        internal Dictionary<int, Action> fadeOutCallbacks;
        Transform soundRoot;
        internal Transform SoundRoot => soundRoot;

        internal float globalVolume;

        internal SoundCoreContext(int capacity) {
            iDService = new SoundIDService();
            singlePlayers = new SortedList<int, SoundPlayer>();
            temp = new SoundPlayer[capacity];
            fadeOutTasks = new Dictionary<int, SoundFadeTaskModel>(capacity);
            fadeInTasks = new Dictionary<int, SoundFadeTaskModel>(capacity);
            removeList = new Dictionary<int, SoundFadeTaskModel>(capacity);
            modifiedList = new Dictionary<int, SoundFadeTaskModel>(capacity);
            fadeOutCallbacks = new Dictionary<int, Action>(capacity);
            globalVolume = 1.0f;
        }

        internal void Inject(Transform soundRoot) {
            this.soundRoot = soundRoot;
        }

        #region Fade Out
        internal void AddFadeOutTask(SoundFadeTaskModel task) {
            if (!fadeOutTasks.ContainsKey(task.playerID)) {
                fadeOutTasks.Add(task.playerID, task);
            }
        }

        internal void RemoveFadeOutTask(int playerID) {
            fadeOutTasks.Remove(playerID);
        }

        internal void PreRemoveFadeOutTask(int playerID) {
            removeList[playerID] = fadeOutTasks[playerID];
        }

        internal void ModifyFadeOutTask(SoundFadeTaskModel task) {
            if (fadeOutTasks.ContainsKey(task.playerID)) {
                fadeOutTasks[task.playerID] = task;
            }
        }

        public delegate void RefAction<T>(ref T item);
        internal void FadeOutTaskForEach(RefAction<SoundFadeTaskModel> action) {
            foreach (var key in fadeOutTasks.Keys) {
                var task = fadeOutTasks[key];
                action(ref task);
                modifiedList[key] = task;
            }
        }

        internal bool IsFadingOut(int playerID) {
            return fadeOutTasks.ContainsKey(playerID);
        }

        internal void AddFadeOutCallback(int playerID, Action callback) {
            if (fadeOutCallbacks.ContainsKey(playerID)) {
                fadeOutCallbacks[playerID] = callback;
            } else {
                fadeOutCallbacks.Add(playerID, callback);
            }
        }

        internal void RemoveFadeOutCallback(int playerID) {
            if (!fadeOutCallbacks.ContainsKey(playerID)) {
                return;
            }
            fadeOutCallbacks.Remove(playerID);
        }

        internal bool TryGetFadeOutCallback(int playerID, out Action callback) {
            return fadeOutCallbacks.TryGetValue(playerID, out callback);
        }

        internal void RemoveAllFadeOutTask() {
            fadeOutTasks.Clear();
            fadeOutCallbacks.Clear();
        }
        #endregion

        #region Fade In
        internal void AddFadeInTask(SoundFadeTaskModel task) {
            if (!fadeInTasks.ContainsKey(task.playerID)) {
                fadeInTasks.Add(task.playerID, task);
            }
        }

        internal void RemoveFadeInTask(int playerID) {
            fadeInTasks.Remove(playerID);
        }

        internal void PreRemoveFadeInTask(int playerID) {
            removeList[playerID] = fadeInTasks[playerID];
        }

        internal void ModifyFadeInTask(SoundFadeTaskModel task) {
            if (fadeInTasks.ContainsKey(task.playerID)) {
                fadeInTasks[task.playerID] = task;
            }
        }

        internal void FadeInTaskForEach(RefAction<SoundFadeTaskModel> action) {
            foreach (var key in fadeInTasks.Keys) {
                var task = fadeInTasks[key];
                action(ref task);
                modifiedList[key] = task;
            }
        }

        internal bool IsFadingIn(int playerID) {
            return fadeInTasks.ContainsKey(playerID);
        }

        internal void RemoveAllFadeInTask() {
            fadeInTasks.Clear();
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
            if (!removeList.ContainsKey(task.playerID)) {
                removeList.Add(task.playerID, task);
            }
        }

        internal void RemoveTaskForEach(Action<SoundFadeTaskModel> action) {
            foreach (var key in removeList.Keys) {
                var task = removeList[key];
                action(task);
            }
        }

        internal void ClearRemoveTask() {
            removeList.Clear();
        }
        #endregion

        #region Modified Task
        internal void ClearModifiedTask() {
            modifiedList.Clear();
        }

        internal void ModifiedTaskForEach(Action<SoundFadeTaskModel> action) {
            foreach (var key in modifiedList.Keys) {
                var task = modifiedList[key];
                action(task);
            }
        }
        #endregion

        internal void Clear() {
            singlePlayers.Clear();
            playerGroups.Clear();
            Array.Clear(temp, 0, temp.Length);
            fadeOutTasks.Clear();
            fadeInTasks.Clear();
            removeList.Clear();
            modifiedList.Clear();
            fadeOutCallbacks.Clear();
        }

    }

}