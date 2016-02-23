using UnityEngine;
using System.Collections;
using Assets.Scripts.Agents;
using Assets.Scripts.Emotions;

public class GoalNotReachableEvent
{
    public Agent Agent;

    public GoalNotReachableEvent(Agent agent)
    {
        Agent = agent;
    }

}
