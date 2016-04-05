using UnityEngine;
using System.Collections;
using Assets.Scripts.Emotions;

public class ShowPinMessage
{
    public NavMeshAgent AgentMesh;
    public EmotionType Type;

    public ShowPinMessage(NavMeshAgent agentMesh, EmotionType type)
    {
        AgentMesh = agentMesh;
        Type = type;
    }
}
