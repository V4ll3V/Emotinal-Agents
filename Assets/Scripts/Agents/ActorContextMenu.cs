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

        public Scrollbar JoyDispair;
        public Scrollbar FearHope;
        public Scrollbar ShamePride;
        public Scrollbar DisappointmentRelief;
        public Scrollbar AngerGratitude;
        public Scrollbar ReporachAdmiration;
        public Scrollbar LoveHate;


        private int _enemyCount;
        private int _timerCount;
        private bool _showObstalces;

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
            AgentManager.CreateActor();
        }
    }
}
