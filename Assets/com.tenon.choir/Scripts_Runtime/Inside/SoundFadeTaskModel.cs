using MortiseFrame.Swing;

namespace TenonKit.Choir {

    internal struct SoundFadeTaskModel {

        public int playerID;
        public SoundFadeEnum fadeType;
        public float duration;
        public float timer;
        public EasingType easingType;
        public EasingMode easingMode;

    }

}