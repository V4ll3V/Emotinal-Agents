using UnityEngine;
using System.Collections;
using Assets.Scripts.Emotions;
using Assets.Scripts.Agents;

public class DestroyHpBarMessage
{
    public Agent Agent;

    public DestroyHpBarMessage(Agent agent)
    {
        Agent = agent;
    }
}
