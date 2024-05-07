using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TenonKit.Choir {

    public class SoundCore {

        SoundCoreContext ctx;

        public int CreateSoundPlayer(bool autoPlay, bool isLoop, string name = "SoundPlayer", AudioClip clip = null) {
            SoundPlayer soundPlayer = SoundCoreFactory.SpawnSoundPlayer(ctx, autoPlay, isLoop, name, clip);
            ctx.AddSoundPlayer(soundPlayer);
            return soundPlayer.ID;
        }


    }

}