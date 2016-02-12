using Assets.Scripts.Agents;
using Assets.Scripts.Goals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace Assets.Scripts.Emotions
{
    public class EmotionManager
    {
        //variavles send by toImplement Message system
        private Goal _goal;
        private Agent _agent;
     
        public void DetermineEmotion()
        {
            if(_agent.ActiveGoals.Contains(_goal))
            {

            }
        }   
    }
}
