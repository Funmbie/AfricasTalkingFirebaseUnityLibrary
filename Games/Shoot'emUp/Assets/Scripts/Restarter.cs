using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restarter : MonoBehaviour {
	public bool isPassed;

	void OnTriggerEnter(Collider col)
	{
		if(col.CompareTag("Player"))
		{
			//Call GameOver
			isPassed = true;
		}
	}
}
