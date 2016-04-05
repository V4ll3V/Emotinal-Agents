using System;
using System.Collections;

namespace Assets.Scripts.Events
{
    public enum AgentEventType
    {
        ConsequencesOfEvents,
        ActionsOfAgents,
        AspectsOfObjects
    }
    public class AgentEvent
    {
        public AgentEventType EventType;
        public bool EventIsPositive;

        public AgentEvent(AgentEventType type, bool evenIsPositive)
        {
            EventType = type;
            EventIsPositive = evenIsPositive;
        }
    }
}
