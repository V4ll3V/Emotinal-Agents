using UnityEngine;
using System.Collections;
using Assets.Scripts.Agents;
using Assets.Scripts.Emotions;
using Assets.Scripts.Events;

public class NewEventMessage
{
    public Agent Agent;
    public AgentEvent AgentEvent;

    public NewEventMessage(Agent agent, AgentEvent agentEvent)
    {
        Agent = agent;
        AgentEvent = agentEvent;
    }

}
