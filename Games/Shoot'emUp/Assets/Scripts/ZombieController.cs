using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour {
	[HideInInspector] public int Health;
	GameObject target;
	GameManager gameManager;
	
	// Use this for initialization
	void Start () {
		Health = 150;
		target = GameObject.FindGameObjectWithTag("Player");
		gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Health<=0)
		Death();
		int Difficulty = PlayerPrefs.GetInt("Difficulty",0);
		transform.Translate(0f,0f,0.25f+Difficulty*Time.deltaTime);
		transform.LookAt(target.transform);
	}

	void OnTriggerStay(Collider col)
	{
		if(!gameManager.isPaused){
		if(col.CompareTag("Player"))
		{
			col.gameObject.GetComponent<PlayerController>().Health -= 0.5f;
		}
		}
	}

	void Death()
	{
		Destroy(this.gameObject);
	}
}
