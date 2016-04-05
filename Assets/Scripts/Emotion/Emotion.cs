using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Emotions
{
    public enum EmotionType
    {
        joy,
        hope,
        relief,
        pride,
        gratitude,
        love,
        adminration,
        dispair,
        fear,
        disappointment,
        reproach,
        anger,
        hate,
        shame
    }

    public class Emotion
    {
        public int Intensity;
        public int Duration;
        public EmotionType EmotionType;

        public Emotion (EmotionType type, int intensity, int duration)
        {
            EmotionType = type;
            Intensity = intensity;
            Duration = duration;
        }
    }
}
