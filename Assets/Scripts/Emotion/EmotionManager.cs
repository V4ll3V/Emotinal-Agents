using Assets.Scripts.Agents;
using Assets.Scripts.Goals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MessageBus;
using UnityEngine;

namespace Assets.Scripts.Emotions
{
    public class EmotionManager : MonoBehaviour, ISubscriber<GoalReachedEvent>, ISubscriber<GoalNotReachableEvent>
    {
        void Start()
        {
            GlobalMessageBus.Instance.Subscribe(this);

        }

        public void OnEvent(GoalReachedEvent evt)
        {
            CreateEmotion(EmotionType.joy, evt.Agent);         
        }

        public void OnEvent(GoalNotReachableEvent evt)
        {
            CreateEmotion(EmotionType.distress, evt.Agent);
        }

        public void CreateEmotion(EmotionType type, Agent agent)
        {
            Emotion emotion = new Emotion(type, DetermineEmotionIntensity(agent), DetermineEmotionDuration(agent));
            GlobalMessageBus.Instance.PublishEvent(new NewEmotionCreatedMessage(emotion, agent));
        }
        public int DetermineEmotionIntensity(Agent agent)
        {
            
            return 100;
        }
        public int DetermineEmotionDuration(Agent agent)
        {

            return 100;
        }

    }
}
