using UnityEngine;
using System.Collections;
using Assets.Scripts.Emotions;
using Assets.Scripts.Agents;

public class HpBarChangedMessage
{
    public Agent Agent;
    public GameObject WorldAgent;
    public int HpValue;

    public HpBarChangedMessage(Agent agent, GameObject worldAgent, int hpValue)
    {
        Agent = agent;
        HpValue = hpValue;
        WorldAgent = worldAgent;
    }
}
