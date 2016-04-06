using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Agents
{
    public class AgentPersonality
    {
        //Big Five 
        public float Neuroticism;
        public float Extraversion;
        public float OpennessToExperience;
        public float Conscientiousness;
        public float Agreeableness;

        public AgentPersonality(float n, float ex, float op, float co, float ag)
        {
            Neuroticism = n;
            Extraversion = ex;
            OpennessToExperience = op;
            Conscientiousness = co;
            Agreeableness = ag;
        }
    }
}
