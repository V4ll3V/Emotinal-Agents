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
        frightend,
        fearfull,
        angry,
        sad,
        supprised,
        hate,
    }
    public class Agent
    {
        public List<Emotion> ActiveEmotions;
        public AgentPersonality AgentPersontality;


        public List<Goal> ActiveGoals;
        public List<GameObject> Attitudes;
    }
}
