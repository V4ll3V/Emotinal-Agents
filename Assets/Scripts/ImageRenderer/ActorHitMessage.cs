using UnityEngine;
using System.Collections;
using Assets.Scripts.Emotions;
using Assets.Scripts.Agents;

public class ActorHitMessage
{
    public Agent Agent;
    public Agent Enemy;
    public int HpValue;

    public ActorHitMessage(Agent agent, Agent enemy, int hpValue)
    {
        Agent = agent;
        HpValue = hpValue;
        Enemy = enemy;
    }
}
