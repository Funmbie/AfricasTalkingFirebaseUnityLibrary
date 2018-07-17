using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AfricasTalkingUnityClass;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var at_u = new AfricasTalkingUnityGateway();
		//Debug.Log(at_u.addLeaderboard("randomGameId","gamre-id",12,1,0.95f));
		//at_u.SMS("sandbox","39ecb55d445bf5b5aa8cf215032f1e040611ca9b8d55b7a1121b85ff3e013d0b","Suppp!!!!","Wassup");
		//string x = at_u.signUp("funmbioyesanya7@gmail.com","Kenya","Baxter Dynasty","1234","+254701951089");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
