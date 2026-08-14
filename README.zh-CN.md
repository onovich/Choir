# Choir

[English](README.md)

Choir 是一个轻量 Unity 音频生命周期管理库，用于背景音乐、环境音和可复用的一次性音效。

![Choir 封面](docs/cover.png)

## Choir 能做什么

- 为背景音乐和环境音创建可单独控制的播放器。
- 创建具名播放器组，用于复用音效播放器。
- 只有组内存在空闲播放器时才播放音效，从而限制同时叠加数量。
- 控制播放、暂停、继续、停止、音量和静音状态。
- 通过 Swing 支持音频片段替换和缓动淡入淡出。

## 安装

Choir 依赖 [Swing](https://github.com/onovich/Swing)。先添加 Swing：

```text
https://github.com/onovich/Swing.git?path=/Assets/com.mortise.swing#main
```

然后通过 **Window → Package Manager → Add package from git URL** 添加 Choir：

```text
https://github.com/onovich/Choir.git?path=/Assets/com.tenon.choir#main
```

包元数据声明支持 Unity `2019.4` 及以上版本。仓库内工程使用 Unity `2023.2.22f1`。

## 快速开始

```csharp
using TenonKit.Choir;
using UnityEngine;

public sealed class AudioSample : MonoBehaviour {
    [SerializeField] Transform soundRoot;
    [SerializeField] AudioClip backgroundMusic;
    [SerializeField] AudioClip clickSound;

    SoundCore soundCore;
    int musicId;

    void Start() {
        soundCore = new SoundCore(soundRoot, 5);

        musicId = soundCore.CreateSoundPlayer(
            autoPlay: true,
            isLoop: true,
            name: "BGM",
            clip: backgroundMusic
        );

        soundCore.CreateSoundPlayerGroup(
            autoPlay: false,
            capacity: 4,
            groupName: "UI"
        );
    }

    public void PlayClick() {
        soundCore.PlayInGroupIfFree("UI", clickSound);
    }

    void Update() {
        soundCore.Tick(Time.deltaTime);
    }

    void OnDestroy() {
        soundCore?.TearDown();
    }
}
```

## 当前状态

单播放器、播放器组、空闲播放器播放、基础传输控制、音量、静音和淡入淡出已经实现。“空闲后再播放”队列、循环/随机组、空间音频、混音和混响尚未实现。

当前包版本为 `0.0.6`。Choir 更适合小型项目、Demo 和 GameJam，不是完整生产级音频中间件的替代品。仓库内没有自动化测试。

## 开发

运行时代码位于 `Assets/com.tenon.choir/Scripts_Runtime/`，示例场景与脚本位于同一包目录下。

## 许可证

[MIT](LICENSE)
