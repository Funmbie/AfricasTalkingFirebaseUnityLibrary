using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletController : MonoBehaviour {
	int Damage;
	// Use this for initialization
	void Start () {
		Damage = PlayerPrefs.GetInt("Damage",25);
		Invoke("Die",4f);
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
		}
	}

	void Die()
	{
		Destroy(this.gameObject);
	}
}
