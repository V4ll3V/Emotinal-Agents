using UnityEngine;
using System.Collections;
using MessageBus;

namespace Assets.Scripts.Events
{
    public class EventManager : MonoBehaviour, ISubscriber<ActorHitMessage>, ISubscriber<GoalReachedEvent>, ISubscriber<GoalNotReachableEvent>
    {

        void Start()
        {
            GlobalMessageBus.Instance.Subscribe(this);
        }

        public void OnEvent(ActorHitMessage evt)
        {
            AgentEvent aEvt = new AgentEvent(AgentEventType.ActionsOfAgents, false);
            GlobalMessageBus.Instance.PublishEvent(new NewEventMessage(evt.Agent, aEvt));
        }
        public void OnEvent(GoalReachedEvent evt)
        {
            AgentEvent aEvt = new AgentEvent(AgentEventType.ActionsOfAgents, true);
            GlobalMessageBus.Instance.PublishEvent(new NewEventMessage(evt.Agent, aEvt));
        }

        public void OnEvent(GoalNotReachableEvent evt)
        {
            AgentEvent aEvt = new AgentEvent(AgentEventType.ActionsOfAgents, false);
            GlobalMessageBus.Instance.PublishEvent(new NewEventMessage(evt.Agent, aEvt));
        }
    }
}