using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Emotions;
using System.Linq;
using MessageBus;

namespace Assets.Scripts.Agents
{
    public enum AgentType
    {
        Actor,
        Friendly,
        Enemy
    }
    public class AgentManager : MonoBehaviour, ISubscriber<NewEmotionCreatedMessage>
    {

        public GameObject ActorPrefab;
        public GameObject EnemyPrefab;
        public GameObject FriendlyPrefab;
        public GameObject ActorSpawnArea;
        public GameObject EnemySpawnContainer;
        public GameObject FriendlySpawnContainer;

        public Camera MainCamera;

        private GameObject _actorAgent;

        private List<Agent> _activeAgents;

        private List<Transform> _actorSpawnAreas;
        private List<Transform> _enemySpawnAreas;
        private List<Transform> _friendlySpawnAreas;

        private List<GameObject> _enemyAgents;
        private List<GameObject> _friendlyAgents;

        // Use this for initialization
        void Start()
        {
            GlobalMessageBus.Instance.Subscribe(this);
            _activeAgents = new List<Agent>();

            _actorSpawnAreas = new List<Transform>();
            _actorSpawnAreas.AddRange(ActorSpawnArea.GetComponentsInChildren<Transform>());

            _enemySpawnAreas = new List<Transform>();
            _enemySpawnAreas.AddRange(EnemySpawnContainer.GetComponentsInChildren<Transform>());

            _friendlySpawnAreas = new List<Transform>();
            _friendlySpawnAreas.AddRange(FriendlySpawnContainer.GetComponentsInChildren<Transform>());

            _enemyAgents = new List<GameObject>();
            _friendlyAgents =  new List<GameObject>();

            if (ActorPrefab != null && _actorSpawnAreas != null)
                InstatiateGameObject(ActorPrefab, AgentType.Actor);
        }

        public void OnEvent(NewEmotionCreatedMessage evt)
        {
            AssignEmotionToAgent(evt.Agent, evt.Emotion);
        }

        public void InstatiateGameObject(GameObject go, AgentType type)
        {
            GameObject actorInstance = Instantiate(go);
            Transform spawnArea = GetRandomSpawnAreaForType(type);

            actorInstance.transform.SetParent(this.transform);
            actorInstance.transform.localPosition = spawnArea.transform.localPosition + GetRandomVectorLocation();
            MainCamera.transform.SetParent(actorInstance.transform);

            Agent newAgent = new Agent(actorInstance.GetComponent<AgentModule>(), type);
            _activeAgents.Add(newAgent);

            switch (type)
            {
                case AgentType.Actor:
                    _actorAgent = actorInstance;
                    break;
                case AgentType.Friendly:
                    _friendlyAgents.Add(actorInstance);
                    break;
                case AgentType.Enemy:
                    _enemyAgents.Add(actorInstance);
                    break;
            }
        }

        private Vector3 GetRandomVectorLocation()
        {
            return new Vector3(Random.Range(0, 2.5f), 0, Random.Range(0, 2.5f));
        }

        private Transform GetRandomSpawnAreaForType(AgentType type)
        {
            Transform result = null;

            switch(type)
            {
                case AgentType.Actor:
                    {
                        result = _actorSpawnAreas.OrderBy(x => System.Guid.NewGuid()).FirstOrDefault();
                        break;
                    }
                case AgentType.Friendly:
                    {
                        result = _friendlySpawnAreas.OrderBy(x => System.Guid.NewGuid()).FirstOrDefault();
                        break;
                    }
                case AgentType.Enemy:
                    {
                        result = _enemySpawnAreas.OrderBy(x => System.Guid.NewGuid()).FirstOrDefault();
                        break;
                    }
            }
            return result;
        }

        public void AssignEmotionToAgent(Agent agent, Emotion emotion)
        {
            agent.ActiveEmotions.Add(emotion);

            int strongestEmotionIntensity = agent.ActiveEmotions.Max(e => e.Intensity);
            Emotion strongestEmotion = agent.ActiveEmotions.FirstOrDefault(e => e.Intensity == strongestEmotionIntensity);

            ExpressEmotion(agent, emotion);
        }

        public void ExpressEmotion(Agent agent, Emotion emotion)
        {
            GlobalMessageBus.Instance.PublishEvent(new ShowPinMessage(agent.AgentModule.agent, emotion.EmotionType));
        }

        public void SetActorTarget(GameObject target)
        {
            _actorAgent.GetComponent<AgentModule>().SetTarget(target.transform);
        }
        public void SetActorTragetFriendly()
        {
            _actorAgent.GetComponent<AgentModule>().SetTarget(_friendlyAgents.FirstOrDefault().transform);
        }
        public void SetActorTargetObject()
        {
            //TODO
        }
    }
}
