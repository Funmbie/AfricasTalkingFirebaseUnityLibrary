using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AfricasTalkingUnityClass;

public class StoreLogic : MonoBehaviour {
	public GameObject hailStone;

	PlayerController playerController;
	UIManager uiManager;
	GameManager gameManager;
	// Use this for initialization
	void Start () {
		playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		uiManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIManager>();
		gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CallHailstone()
	{
		string username = "sandbox";
		string apikey = "39ecb55d445bf5b5aa8cf215032f1e040611ca9b8d55b7a1121b85ff3e013d0b";
		string gamer_id = PlayerPrefs.GetString("token","");
		string product = "Stone";
		string channel = "09876";
		decimal amount = 50m; 

		var at_u = new AfricasTalkingUnityGateway();
		string request = at_u.inAppPurchase(username, apikey,gamer_id, product,"KES",amount, channel);
		if(request=="OK")
		processHailStone();

		Debug.Log(request);
	}

	public void AddHealth()
	{
		string username = "sandbox";
		string apikey = "39ecb55d445bf5b5aa8cf215032f1e040611ca9b8d55b7a1121b85ff3e013d0b";
		string gamer_id = PlayerPrefs.GetString("token","");
		string product = "Timee";
		string channel = "12345";
		decimal amount = 70m; 

		var at_u = new AfricasTalkingUnityGateway();
		string request = at_u.inAppPurchase(username, apikey,gamer_id, product,"KES",amount, channel);
		if(request=="OK")
		ProcessHealth();
	}

	void ProcessHealth()
	{
		uiManager.unPause();
		gameManager.isPaused=true;
		gameManager.Pause();
		StartCoroutine(add());
	}

	void processHailStone()
	{
		uiManager.unPause();
		gameManager.isPaused=true;
		gameManager.Pause();
		Vector3 originPos = playerController.gameObject.transform.position;
		Vector3 finalPos = new Vector3(originPos.x,originPos.y+5f,originPos.z);
		Instantiate (hailStone,finalPos,Quaternion.identity);
	}

	IEnumerator add()
	{
		int i=0;
		while(i<50)
		{
			i++;
			playerController.Health +=1;
			yield return new WaitForSeconds(0.025f);
		}
	}
}
