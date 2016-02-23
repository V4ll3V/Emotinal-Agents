using UnityEngine;
using System.Collections;
using Assets.Scripts.Agents;
using Assets.Scripts.Emotions;

public class GoalReachedEvent{

    public Agent Agent;
     
    public GoalReachedEvent(Agent agent)
    {
        Agent = agent;
    }

}
