using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour {
	[HideInInspector] public int Health;
	GameObject target;
	// Use this for initialization
	void Start () {
		Health = 150;
		target = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if(Health<=0)
		Death();

		transform.Translate(0f,0f,0.05f);
		transform.LookAt(target.transform);
	}

	void Death()
	{
		Destroy(this.gameObject);
	}
}
