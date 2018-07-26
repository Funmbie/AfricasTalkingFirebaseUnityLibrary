using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowCamMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
		InvokeRepeating("Turn",10f,5f);
	}

	void Turn()
	{
		transform.Rotate(0f,90f,0f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(0f,0f,2f*Time.deltaTime);
	}
}
