using Assets.Scripts.Agents;
using Assets.Scripts.Goals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MessageBus;
using UnityEngine;
using Assets.Scripts.Events;

namespace Assets.Scripts.Emotions
{
    public class EmotionManager : MonoBehaviour, ISubscriber<NewEventMessage>
    {
        void Start()
        {
            GlobalMessageBus.Instance.Subscribe(this);

        }

        public void OnEvent(NewEventMessage evt)
        {
            DetermineEmmotion(evt.AgentEvent, evt.Agent);      
        }


        public void DetermineEmmotion(AgentEvent currentEvent, Agent agent)
        {
            if(currentEvent.EventType == AgentEventType.ActionsOfAgents)
            {
                if (currentEvent.EventIsPositive)
                    CreateEmotion(EmotionType.joy, agent);
                else
                    CreateEmotion(EmotionType.distress, agent);
            }
            if (currentEvent.EventType == AgentEventType.AspectsOfObjects)
            { }
            if (currentEvent.EventType == AgentEventType.ConsequencesOfEvents)
            {

            }
        }

        public void CreateEmotion(EmotionType type, Agent agent)
        {
            Emotion emotion = new Emotion(type, DetermineEmotionIntensity(agent), DetermineEmotionDuration(agent));
            GlobalMessageBus.Instance.PublishEvent(new NewEmotionCreatedMessage(emotion, agent));
        }
        public int DetermineEmotionIntensity(Agent agent)
        {          
            return 4;
        }
        public int DetermineEmotionDuration(Agent agent)
        {
            return 100;
        }

    }
}
