using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour {
	float delay,staticDelay;
	public GameObject zombiePrefab;
	public Transform[] spawnPoints;
	// Use this for initialization
	void Start () {
		float x = PlayerPrefs.GetFloat("Difficulty",0);
		if(x==0)
		delay = 21f;
		else if(x==1)
		delay = 14f;
		else if(x==2)
		delay = 7f;
		staticDelay = delay;
	}
	
	// Update is called once per frame
	void Update () {
	  	DelayManager();
	}

	void DelayManager()
	{
		if(delay<=0f){
		delay = 0f;
		SpawnZombie();
		}

		if(delay>=staticDelay)
		delay = staticDelay;

		if(delay>0f)
		delay-=0.1f;
	}

	void SpawnZombie()
	{
		int rand = Random.Range(0,spawnPoints.Length-1);
		GameObject instance = Instantiate(zombiePrefab, spawnPoints[rand].position,Quaternion.identity) as GameObject;
	}
}
