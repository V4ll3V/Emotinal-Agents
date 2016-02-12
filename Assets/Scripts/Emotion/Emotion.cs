using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Emotions
{
    public enum PositveEmotionType
    {
        joy,
        hope,
        relief,
        pride,
        gratitude,
        love,
    }
    public enum NegativEmotionType
    {
        distress,
        fear,
        disappointment,
        remorse,
        anger,
        hate
    }
    public class Emotion
    {
        public int Intensity;
        public int Duration;
    }
}
