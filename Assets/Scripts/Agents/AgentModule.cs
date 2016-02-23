using Assets.Scripts.Goals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using MessageBus;
using Assets.Scripts.Emotions;

namespace Assets.Scripts.Agents
{
    public class AgentModule : MonoBehaviour
    {
        public Agent Agent;
        private Goal _goal;

        public NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target;
        public GoalType GoalType;

        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

            agent.updateRotation = false;
            agent.updatePosition = true;

        }


        private void Update()
        {
            CheckReachGoalStatus();
        }
        public void CheckReachGoalStatus()
        {
            if (agent.path.status == NavMeshPathStatus.PathInvalid || agent.path.status == NavMeshPathStatus.PathPartial)
            {
                GlobalMessageBus.Instance.PublishEvent(new GoalNotReachableEvent(Agent));
                agent.Stop();
                target = null;
                agent.ResetPath();
            }
            else if (agent.hasPath && agent.remainingDistance <= agent.stoppingDistance)
            {
                GlobalMessageBus.Instance.PublishEvent(new GoalReachedEvent(Agent));
                agent.Stop();
                target = null;
                agent.ResetPath();
                
            }
            else
                MoveAgent();
        }
        public void MoveAgent()
        {
            if (GoalType == GoalType.ReachTarget)
            {
                SetTarget(target);
                MoveTo();
            }
            else
                Stop();
        }

        public void MoveTo()
        {
            if (target != null)
                agent.SetDestination(target.position);

            if (agent.remainingDistance > agent.stoppingDistance)
                character.Move(agent.desiredVelocity, false, false);
            else
                character.Move(Vector3.zero, false, false);
        }
        public void Stop()
        {
            agent.Stop();
            character.Move(Vector3.zero, false, false);
        }

        public void SetTarget(Transform target)
        {
            this.target = target.transform;
        }
    }
}
