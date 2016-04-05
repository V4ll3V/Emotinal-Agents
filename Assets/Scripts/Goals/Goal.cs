using System;
using System.Collections;

namespace Assets.Scripts.Goals
{
    public enum GoalType
    {
        ReachTarget,
        KillTarget,
        RunAway,
    }
    public class Goal
    {
        public GoalType GoalType;
        public int Desirability;
        public int ThreadRange;

    }
}
