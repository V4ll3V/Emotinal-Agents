using Assets.Scripts.Emotions;
using Assets.Scripts.Goals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Agents
{
    public enum EmotionState
    {
        happy,
        pride,
        resilent,
        fearfull,
        angry,
        sad,
        supprised,
        hate,
    }

    public class Agent
    {
        public AgentModule AgentModule;

        public List<Emotion> ActiveEmotions;
        public AgentPersonality AgentPersontality;

        public List<Goal> ActiveGoals;
        public List<GameObject> Attitudes;

        public AgentType AgentType;

        public Agent (AgentModule module, AgentType type)
        {
            AgentModule = module;
            AgentModule.Agent = this;
            ActiveEmotions = new List<Emotion>();
            AgentType = type;
        }
    }
}
