# Choir
Choir, a lightweight sound effect lifecycle management library, named after "唱诗班."<br/>
**Choir，轻量级的音效生命周期管理库，取名自「唱诗班」。**

Choir provides lifecycle management for AudioSource and AudioClip in Unity.<br/>
**Choir 为 Unity 里的 AudioSource 和 AudioClip 提供生命周期托管。**

For details, please refer to the sample project within the repository.<br/>
**具体可见项目内的示例工程。**

# Readiness
Suitable for projects with low sound effect requirements, small-scale demos, and GameJams, making it easy to quickly set up a sound effect system.<br/>
**适用于对音效要求不高的项目、小体量 Demo、GameJam，方便快速搭建音效系统。**

# Features
## Implemented
* Single-player management (suitable for BGM/BGS) and player group management (suitable for SE).<br/>
  **单播放器管理（适用于 BGM / BGS）、播放器组管理（适用于 SE）；** <br/>
* Player's "Play If Free": It plays only when there is an available player in the group. This controls the number of the same type of sound effects played simultaneously, preventing resource waste and undesirable effects caused by overlapping sound volumes. <br/>
  **播放器的 Play If Free，即：当组内存在空闲播放器时，才执行播放。通过这个方式可以控制同一时间同一类型音效的播放数量，避免大量音效叠加造成的资源浪费以及音量叠加造成的效果不适；** <br/>

## Planned
* Player group's "Play When Free": When there are no available players in the group, it waits for a period until a player is released and then plays with a delay (suitable for staggering the playback of many SEs at the same time).<br/>
 **播放器组的 Play When Free，即：当组内没有空闲播放器时，等待一段时间直到有播放器被释放出来再延时播放（适用于同一时间播放大量 SE 时，希望错开时间播放）；** <br/>
* Loop groups and random groups. <br/>
  **循环组、随机组；** <br/>

## Not In Plan Yet
* Spatial sound effects.<br/>
  **空间音效；** <br/>
* Effect support (reverb and mixing)<br/>
  **效果器支持（混响和混音）。** 

# Sample
```
// Single Player
[SerializeField] AudioClip bgmClip;
SoundCore soundCore;
int bgmID;

public void Init() {
    soundCore = new SoundCore(soundRoot, 1); // Only Need One Player In This Case
    bgmID = soundCore.CreateSoundPlayer(true, true, "BGM_Player", bgmClip);
}

public void BGM_Play() {
    soundCore.Play(bgmID);
}

public void BGM_Pause() {
    soundCore.Pause(bgmID);
}

public void BGM_UnPause() {
    soundCore.UnPause(bgmID);
}

public void BGM_Stop() {
    soundCore.Stop(bgmID);
}

public void BGM_SetVolume(float volume) {
    soundCore.SetVolume(bgmID, volume);
}

public void BGM_SetMute() {
    soundCore.SetMute(bgmID);
}

public void BGM_SetUnMute() {
    soundCore.SetUnMute(bgmID);
}

public void Clear() {
    soundCore.TearDown();
}
```

```
// Player Group
[SerializeField] AudioClip seClip;
SoundCore soundCore;

public void Init() {
    soundCore = new SoundCore(soundRoot, 4); // 4 is an example. Choose the maximum number of sounds supported in the current scene as needed.
    soundCore.CreateSoundPlayerGroup(false, 4, "SE_Player"); // 4 is an example. Choose the maximum number of players that can simultaneously play the sound effect as needed.
}

public void SE_Play() {
    soundCore.PlayInGroupIfFree("SE_Player", seClip);
}

public void SE_Pause() {
    soundCore.PauseInGroup("SE_Player");
}

public void SE_UnPause() {
    soundCore.UnPauseInGroup("SE_Player");
}

public void SE_Stop() {
    soundCore.StopInGroup("SE_Player");
}

public void SE_SetVolume(float volume) {
    soundCore.SetVolumeInGroup("SE_Player", volume);
}

public void SE_SetMute() {
    soundCore.SetMuteInGroup("SE_Player");
}

public void SE_SetUnMute() {
    soundCore.SetUnMuteInGroup("SE_Player");
}

public void Clear() {
    soundCore.TearDown();
}
```

# UPM URL
ssh://git@github.com/onovich/Choir.git?path=/Assets/com.tenon.choir#main
