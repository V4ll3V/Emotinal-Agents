using UnityEngine;
using System.Collections;

public class AgentManager : MonoBehaviour {

    public GameObject ActorPrefab;
    public GameObject AgentPrefab;
    public GameObject SpawnArea;
    public Camera MainCamera;
	// Use this for initialization
	void Start () {
        //if (ActorPrefab != null && SpawnArea != null)
            //InstatiateActor(ActorPrefab);
        if (AgentPrefab != null && SpawnArea != null)
            InstatiateGameObject(AgentPrefab);

    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    private void InstatiateActor(GameObject go)
    {
        GameObject actorInstance = Instantiate(go);
        actorInstance.transform.SetParent(this.transform);
        actorInstance.transform.localPosition = SpawnArea.transform.localPosition + GetRandomVectorLocation();
        MainCamera.transform.SetParent(actorInstance.transform);
    }

    private void InstatiateGameObject(GameObject go)
    {
         GameObject actorInstance = Instantiate(go);
         actorInstance.transform.SetParent(this.transform);
         actorInstance.transform.localPosition = SpawnArea.transform.localPosition + GetRandomVectorLocation();
    }

    private Vector3 GetRandomVectorLocation()
    {
        return new Vector3(Random.Range(0, 5), 0, Random.Range(0, 5));
    }

}
