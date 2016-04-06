using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace Assets.Scripts.Agents
{
    public class ActorContextMenu : MonoBehaviour
    {
        public Image ActorEmotionStateImage;
        public AgentManager AgentManager;
        public GameObject ObstacleContainer;


        public Slider JoyDispair;
        public Slider FearHope;
        public Slider ShamePride;
        public Slider DisappointmentRelief;
        public Slider AngerGratitude;
        public Slider ReporachAdmiration;
        public Slider LoveHate;

        public Slider Neuroticism;
        public Slider Extraversion;
        public Slider OpennessToExperience;
        public Slider Conscientiousness;
        public Slider Agreeableness;

        private int _enemyCount;
        private int _timerCount;
        private bool _showObstalces;

        void Update()
        {
            Agent Actor = AgentManager.GetActorAgent();
            float value;
            if (Actor != null && Actor.ActiveEmotionPairs != null)
            {
                if (Actor.ActiveEmotionPairs.TryGetValue(ActiveEmotionPair.JoyDispair, out value))
                    JoyDispair.value = value;
                if (Actor.ActiveEmotionPairs.TryGetValue(ActiveEmotionPair.FearHope, out value))
                    FearHope.value = value;
                if (Actor.ActiveEmotionPairs.TryGetValue(ActiveEmotionPair.ShamePride, out value))
                    ShamePride.value = value;
                if (Actor.ActiveEmotionPairs.TryGetValue(ActiveEmotionPair.DisappointmentRelief, out value))
                    DisappointmentRelief.value = value;
                if (Actor.ActiveEmotionPairs.TryGetValue(ActiveEmotionPair.AngerGratitude, out value))
                    AngerGratitude.value = value;
                if (Actor.ActiveEmotionPairs.TryGetValue(ActiveEmotionPair.ReporachAdmiration, out value))
                    ReporachAdmiration.value = value;
                if (Actor.ActiveEmotionPairs.TryGetValue(ActiveEmotionPair.LoveHate, out value))
                    LoveHate.value = value;
            }
            if (Actor != null)
            {
                ActorEmotionStateImage.sprite = Resources.Load<Sprite>("Sprites/EmotionType/" + Actor.EmotionState.ToString());
                

            }

        }

        public void OnSetGoalReachFriendly()
        {
            AgentManager.SetActorTragetFriendly();
        }

        public void OnSetGoalKillEnemy()
        {
            //TODO
        }

        public void OnSetGoalReachObject()
        {
            AgentManager.SetActorTargetObject();
        }

        public void OnSpawnEnemy()
        {
            if(_enemyCount != 0)
            {
                for (int i = 0; i <= _enemyCount; i++)
                    AgentManager.InstatiateGameObject(AgentManager.EnemyPrefab, AgentType.Enemy);
            }
            else
                AgentManager.InstatiateGameObject(AgentManager.EnemyPrefab, AgentType.Enemy);
        }
        public void SetEnemyCount(InputField enemyCount)
        {
            int count = 0;
            if (Int32.TryParse(enemyCount.text, out count))
              _enemyCount = count;
        }

        public void OnSpawnFriendly()
        {
            AgentManager.InstatiateGameObject(AgentManager.FriendlyPrefab, AgentType.Friendly);
        }

        public void OnBlockPath()
        {
            if (_showObstalces)
            {
                ObstacleContainer.SetActive(_showObstalces);
                _showObstalces = false;
            }
            else
            {
                ObstacleContainer.SetActive(_showObstalces);
                _showObstalces = true;
            }
            
        }

        public void OnCreateActor()
        {
            AgentPersonality personality = new AgentPersonality(Neuroticism.value, Extraversion.value, OpennessToExperience.value, Conscientiousness.value, Agreeableness.value);
            AgentManager.CreateActor(personality);
        }
        
    }
}
