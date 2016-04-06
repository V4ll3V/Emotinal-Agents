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
        public bool IsPositiveEmotion;

        public Emotion (EmotionType type, int intensity, int duration)
        {
            EmotionType = type;
            Intensity = intensity;
            Duration = duration;
            IsPositiveEmotion = IsPositveEmotionType();
        }
        public bool IsPositveEmotionType()
        {
            switch (EmotionType)
            {
                case (EmotionType.joy):
                    {
                        return true;
                    }
                case (EmotionType.hope):
                    {
                        return true;
                    }
                case (EmotionType.pride):
                    {
                        return true;
                    }
                case (EmotionType.gratitude):
                    {
                        return true;
                    }
                case (EmotionType.love):
                    {
                        return true;
                    }
                case (EmotionType.adminration):
                    {
                        return true;
                    }
                case (EmotionType.dispair):
                    {
                        return false;
                    }
                case (EmotionType.fear):
                    {
                        return false;
                    }
                case (EmotionType.disappointment):
                    {
                        return false;
                    }
                case (EmotionType.reproach):
                    {
                        return false;
                    }
                case (EmotionType.anger):
                    {
                        return false;
                    }
                case (EmotionType.hate):
                    {
                        return false;
                    }
                case (EmotionType.shame):
                    {
                        return false;
                    }
            }
            return false;
        }
    }
}
