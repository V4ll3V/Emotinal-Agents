using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Agents;

namespace Assets.Scripts.Emotions
{
    public class NewEmotionCreatedMessage
    {
        public Emotion Emotion;
        public Agent Agent;
        public NewEmotionCreatedMessage(Emotion emotion, Agent agent)
        {
            Emotion = emotion;
            Agent = agent;
        }
    }
}
