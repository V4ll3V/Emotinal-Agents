using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts.Agents
{
    public class ActorContextMenu : MonoBehaviour
    {
        public Image ActorEmotionStateImage;
        public AgentManager AgentManager;
        public GameObject ObstacleContainer;

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
                _showObstalces = true;
            
        }
    }
}
