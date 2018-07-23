using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletController : MonoBehaviour {
	int Damage;
	// Use this for initialization
	void Start () {
		Damage = PlayerPrefs.GetInt("Damage",50);
		Invoke("Die",2.1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col)
	{
		ZombieController zController = col.gameObject.GetComponent<ZombieController>();
		if(zController)
		{
			zController.Health -= Damage;
			PlayerController playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
			playerController.Score += Random.Range(12,21);
		}
	}

	void Die()
	{
		Destroy(this.gameObject);
	}
}
