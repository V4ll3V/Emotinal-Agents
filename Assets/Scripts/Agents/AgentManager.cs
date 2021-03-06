﻿using UnityEngine;
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
    public class AgentManager : MonoBehaviour, ISubscriber<NewEmotionCreatedMessage>, ISubscriber<HpBarChangedMessage>
    {

        public GameObject ActorPrefab;
        public GameObject EnemyPrefab;
        public GameObject FriendlyPrefab;
        public GameObject ActorSpawnArea;
        public GameObject EnemySpawnContainer;
        public GameObject FriendlySpawnContainer;

        public Camera MainCamera;

        private GameObject _actorAgent;
        public GameObject ActorAgent { get { return _actorAgent;} set { } }

        private List<Agent> _activeAgents;

        private List<Transform> _actorSpawnAreas;
        private List<Transform> _enemySpawnAreas;
        private List<Transform> _friendlySpawnAreas;

        private List<GameObject> _enemyAgents;
        private List<GameObject> _friendlyAgents;
        private GameObject _lastFriendly;

        private Dictionary<EmotionType, ActiveEmotionPair> _emotiontypeToEmotionPair;
        private Dictionary<EmotionType, EmotionState> _emotionTypeToState;

        // Use this for initialization
        void Start()
        {
            GlobalMessageBus.Instance.Subscribe(this);
            _activeAgents = new List<Agent>();

            _actorSpawnAreas = new List<Transform>();
            _actorSpawnAreas.AddRange(ActorSpawnArea.GetComponentsInChildren<Transform>());
            _actorSpawnAreas.Remove(ActorSpawnArea.transform);

            _enemySpawnAreas = new List<Transform>();
            _enemySpawnAreas.AddRange(EnemySpawnContainer.GetComponentsInChildren<Transform>());
            _enemySpawnAreas.Remove(EnemySpawnContainer.transform);

            _friendlySpawnAreas = new List<Transform>();
            _friendlySpawnAreas.AddRange(FriendlySpawnContainer.GetComponentsInChildren<Transform>());
            _friendlySpawnAreas.Remove(FriendlySpawnContainer.transform);

            _enemyAgents = new List<GameObject>();
            _friendlyAgents =  new List<GameObject>();

            SetEmotionTypeToEmotionPair();
            SetEmotionTypeToState();
            InvokeRepeating("EmotionDecay", 1, 1.0f);
        }

        public void OnEvent(NewEmotionCreatedMessage evt)
        {
            AssignEmotionToAgent(evt.Agent, evt.Emotion);
        }
        public void OnEvent(HpBarChangedMessage evt)
        {
            ReduceAgentHp(evt.Agent, evt.WorldAgent, evt.HpValue);
        }

        public void InstatiateGameObject(GameObject go, AgentType type)
        {
            GameObject agentInstance = Instantiate(go);
            Transform spawnArea = GetRandomSpawnAreaForType(type);

            agentInstance.transform.SetParent(this.transform);
            agentInstance.transform.localPosition = spawnArea.transform.localPosition + GetRandomVectorLocation();

            int hp = (type == AgentType.Actor) ? 10 : 2;
            Agent newAgent = new Agent(agentInstance.GetComponent<AgentModule>(), type, hp);
            _activeAgents.Add(newAgent);

            GlobalMessageBus.Instance.PublishEvent(new ShowHpBarMessage(newAgent));

            switch (type)
            {
                case AgentType.Actor:
                    _actorAgent = agentInstance;
                    MainCamera.transform.SetParent(agentInstance.transform);
                    break;
                case AgentType.Friendly:
                    _friendlyAgents.Add(agentInstance);
                    break;
                case AgentType.Enemy:
                    _enemyAgents.Add(agentInstance);
                    break;
            }
        }

        private Vector3 GetRandomVectorLocation()
        {
            return new Vector3(Random.Range(0f, 2.5f), 0, Random.Range(0f, 2.5f));
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

            DetermineEmotionState(agent);

            ExpressEmotion(agent, emotion);
        }

        public void DetermineEmotionState(Agent agent)
        {
            if (agent.ActiveEmotionPairs != null)
            {
                foreach (ActiveEmotionPair pair in agent.ActiveEmotionPairs.Keys.ToList())
                {
                    agent.ActiveEmotionPairs[pair] = 0.0f;
                }
            }
            foreach (EmotionType emotion in System.Enum.GetValues(typeof(EmotionType)))
            {
                IEnumerable<Emotion> emotions = agent.ActiveEmotions.Where(e => e.EmotionType == emotion);
                float EmotionTypeIntensity = DetermineEmotionTypeIntensity(emotions);
                if (EmotionTypeIntensity > 5)
                    agent.EmotionState = _emotionTypeToState[emotion];
                Emotion em = emotions.FirstOrDefault();
                if (em != null &&em.IsPositiveEmotion)
                    EmotionTypeIntensity *= 1;
                else if(em != null)
                    EmotionTypeIntensity *= -1;

                ActiveEmotionPair pair = _emotiontypeToEmotionPair[emotion];
                float lastIntensity;
                if ( agent.ActiveEmotionPairs.TryGetValue(pair, out lastIntensity))
                    agent.ActiveEmotionPairs[pair] = EmotionTypeIntensity + lastIntensity;
                else
                    agent.ActiveEmotionPairs[pair] = EmotionTypeIntensity;
            }

        }

        public float DetermineEmotionTypeIntensity(IEnumerable<Emotion> emotions)
        {
            float intensity = 0;

            foreach (Emotion em in emotions)
            {
                if(em.Duration > 0)
                    intensity = intensity + (em.Intensity ^ 2);
            }
            if(intensity >0)
                intensity = Mathf.Log(intensity, 2.0f);
            
            return intensity;
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
            _lastFriendly = _friendlyAgents.FirstOrDefault();
            if (_lastFriendly != null)
                _actorAgent.GetComponent<AgentModule>().SetTarget(_lastFriendly.transform);
        }
        public void SetActorTargetObject()
        {
            //TODO
        }
        public void CreateActor(AgentPersonality personality)
        {
            if (ActorPrefab != null && _actorSpawnAreas != null && _actorAgent == null)
            {
                InstatiateGameObject(ActorPrefab, AgentType.Actor);
                _activeAgents.FirstOrDefault(e => e.AgentType == AgentType.Actor).AgentPersontality = personality;

                MainCamera.GetComponent<FollowTarget>().Target = _actorAgent.transform;
            }
        }
        public void DestroyAgent(GameObject worldAgent, Agent deadAgent)
        {
            _activeAgents.Remove(deadAgent);

            switch (deadAgent.AgentType)
            {
                case AgentType.Friendly:
                    _friendlyAgents.Remove(worldAgent);
                    break;
                case AgentType.Enemy:
                    _enemyAgents.Remove(worldAgent);
                    break;
            }
            if(deadAgent.AgentType != AgentType.Actor)
                Destroy(worldAgent);
        }
        public void ReduceAgentHp(Agent agent, GameObject worldAgent, int hpDistraction)
        {
            agent.HP -= hpDistraction;
            Debug.Log(agent.HP);
            if (agent.AgentType != AgentType.Actor && agent.HP <= 0)
            {
                DestroyAgent(worldAgent, agent);
                GlobalMessageBus.Instance.PublishEvent(new DestroyHpBarMessage(agent));
            }
        }
        public void SetEmotionTypeToEmotionPair()
        {
            _emotiontypeToEmotionPair = new Dictionary<EmotionType, ActiveEmotionPair>();
            _emotiontypeToEmotionPair.Add(EmotionType.adminration, ActiveEmotionPair.ReporachAdmiration);
            _emotiontypeToEmotionPair.Add(EmotionType.anger, ActiveEmotionPair.AngerGratitude);
            _emotiontypeToEmotionPair.Add(EmotionType.disappointment, ActiveEmotionPair.DisappointmentRelief);
            _emotiontypeToEmotionPair.Add(EmotionType.dispair, ActiveEmotionPair.JoyDispair);
            _emotiontypeToEmotionPair.Add(EmotionType.fear, ActiveEmotionPair.FearHope);
            _emotiontypeToEmotionPair.Add(EmotionType.gratitude, ActiveEmotionPair.AngerGratitude);
            _emotiontypeToEmotionPair.Add(EmotionType.hate, ActiveEmotionPair.LoveHate);
            _emotiontypeToEmotionPair.Add(EmotionType.hope, ActiveEmotionPair.FearHope);
            _emotiontypeToEmotionPair.Add(EmotionType.joy, ActiveEmotionPair.JoyDispair);
            _emotiontypeToEmotionPair.Add(EmotionType.love, ActiveEmotionPair.LoveHate);
            _emotiontypeToEmotionPair.Add(EmotionType.pride, ActiveEmotionPair.ShamePride);
            _emotiontypeToEmotionPair.Add(EmotionType.relief, ActiveEmotionPair.DisappointmentRelief);
            _emotiontypeToEmotionPair.Add(EmotionType.reproach, ActiveEmotionPair.ReporachAdmiration);
            _emotiontypeToEmotionPair.Add(EmotionType.shame, ActiveEmotionPair.ShamePride);
        }
        public void SetEmotionTypeToState()
        {
            _emotionTypeToState = new Dictionary<EmotionType, EmotionState>();
            _emotionTypeToState.Add(EmotionType.joy, EmotionState.happy);
            _emotionTypeToState.Add(EmotionType.anger, EmotionState.angry);
            _emotionTypeToState.Add(EmotionType.dispair, EmotionState.fearfull);
        }

        public Agent GetActorAgent()
        {
            if (_activeAgents != null)
                return _activeAgents.FirstOrDefault(e => e.AgentType == AgentType.Actor);
            else
                return null;
        }

        public void EmotionDecay()
        {
            foreach (Agent agent in _activeAgents)
            {
                foreach (Emotion em in agent.ActiveEmotions )
                {
                    em.Duration -= 1;
                }
                DetermineEmotionState(agent);
            }
        }
    }
}
