using UnityEngine.UI;
using UnityEngine;

public class ZombieManager : MonoBehaviour {
	
	[HideInInspector]public int enemiesSpawned;
	public GameObject zombiePrefab;
	public Transform[] spawnPoints;
	public Text spawnCounter;
	public bool isPaused;

	GameObject[] zombies;
	float delay;

	// Use this for initialization
	void Start () {
		int x = PlayerPrefs.GetInt("Difficulty",2);
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

		enemiesSpawned = zombies.Length;
		spawnCounter.text = "Enemies Spawned: "+enemiesSpawned.ToString()+"/25";
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
