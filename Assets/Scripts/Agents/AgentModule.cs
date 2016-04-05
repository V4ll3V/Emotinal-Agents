using Assets.Scripts.Goals;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using MessageBus;
using Assets.Scripts.Emotions;
using UnityEngine.UI;
using System.Collections;

namespace Assets.Scripts.Agents
{
    public class AgentModule : MonoBehaviour
    {
        public Agent Agent;
        public SphereCollider Sphere;
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
            if (Agent.AgentType == AgentType.Actor && GoalType == GoalType.ReachTarget)
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
            else if(Agent.AgentType == AgentType.Enemy)
            {
                if(!agent.hasPath)
                    SetTarget(GetComponentInParent<AgentManager>().ActorAgent.transform);
                if (agent.remainingDistance <= 20)
                    MoveAgent();
                else
                    Stop();
            }
            else if(Agent.AgentType == AgentType.Actor && GoalType == GoalType.RunAway)
            {
                if (agent.path.status == NavMeshPathStatus.PathInvalid || agent.path.status == NavMeshPathStatus.PathPartial)
                {
                    //GlobalMessageBus.Instance.PublishEvent(new GoalNotReachableEvent(Agent));
                    agent.Stop();
                    target = null;
                    agent.ResetPath();
                }
                else
                    MoveAgent();
            }
        }

        public void MoveAgent()
        {
            if (GoalType == GoalType.ReachTarget)
            {
                MoveTo();
            }
            else if(GoalType == GoalType.RunAway)
            {
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

        public void SetTarget(Transform moveTarget)
        {
            if(moveTarget != null && target == null)
                target = moveTarget.transform;
        }

        void OnCollisionEnter(Collision col)
        {
            AgentModule module = col.gameObject.GetComponent<AgentModule>();
            if (module != null)
            {
                Agent colAgent = module.Agent;
                if (colAgent != null)
                {
                    if (colAgent.AgentType == AgentType.Enemy)
                    {
                        if (Agent.AgentType == AgentType.Actor)
                        {
                            GlobalMessageBus.Instance.PublishEvent(new HpBarChangedMessage(colAgent, col.gameObject, 1));
                            RandomMoveTo();
                        }
                    }
                    if (colAgent.AgentType == AgentType.Actor)
                    {
                        if (Agent.AgentType == AgentType.Enemy)
                        {
                            GlobalMessageBus.Instance.PublishEvent(new HpBarChangedMessage(colAgent, col.gameObject, 1));
                            GlobalMessageBus.Instance.PublishEvent(new ActorHitMessage(colAgent, Agent, 1));
                            Stop();
                            RandomMoveTo();
                            StartCoroutine(ReAssignEnemy());
                        }
                    }
                }
            }
        }

        public IEnumerator ReAssignEnemy()
        {
            yield return new WaitForSeconds(3f);
            SetTarget(GetComponentInParent<AgentManager>().ActorAgent.transform);
        }

        public void RandomMoveTo()
        {
            Vector3 randomDirection = new Vector3(Random.Range(-20.0F, 20.0F), 0, Random.Range(-20.5f, 20.5f));
            GoalType = GoalType.RunAway;
            agent.SetDestination(randomDirection);
        }
        /*void OnControllerColliderHit(ControllerColliderHit hit)
        {

            Rigidbody body = hit.collider.attachedRigidbody;

            // Only bounce on static objects...
            if ((body == null || body.isKinematic)  hit.controller.velocity.y < -1f) {
                float kr = 0.5f;
                Vector3 v = hit.controller.velocity;
                Vector3 n = hit.normal;
                Vector3 vn = Vector3.Dot(v, n) * n;
                Vector3 vt = v - vn;
                bounce = vt - (vn * kr);
            }
        }*/
    }
}
