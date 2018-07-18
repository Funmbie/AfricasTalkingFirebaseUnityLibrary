using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour {
	float delay;
	public GameObject zombiePrefab;
	public Transform[] spawnPoints;
	GameObject[] zombies;
	public bool isPaused;
	// Use this for initialization
	void Start () {
		int x = PlayerPrefs.GetInt("Difficulty",0);
		if(x==0)
		delay = 4f;
		else if(x==1)
		delay = 2f;
		else if(x==2)
		delay = 1f;
		SpawnZombie();
	}

	void Update()
	{
		//Collect all Zombies
		zombies = GameObject.FindGameObjectsWithTag("Zombie");
	}

	void SpawnZombie()
	{
		if(!isPaused){
		int rand = Random.Range(0,spawnPoints.Length-1);
		Instantiate(zombiePrefab, spawnPoints[rand].position,Quaternion.identity);
		Invoke("SpawnZombie",delay);
		}
	}

	public void pauseZombies()
	{
		for(int i=0;i<zombies.Length;i++)
		{
			zombies[i].GetComponent<ZombieController>().enabled = false;
		}
	}

	public void playZombies()
	{
		for(int i=0;i<zombies.Length;i++)
		{
			zombies[i].GetComponent<ZombieController>().enabled = true;
		}
	}
}
