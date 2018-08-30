using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AfricasTalkingUnityClass;
using Newtonsoft.Json;

class PaymentResponse
{
	public string status;
	public string description;
	public string transactionId;
	public string providerChannel;
}

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

		var at_u = new AfricasTalkingUnityGateway();
		string username = "sandbox";
		string apikey = "";
		string number = "";
		at_u.SMS(username,apikey,"Hello World",number);
		at_u.Airtime(username,apikey,number,"100");
		at_u.B2C(username,apikey,number,"TestUser",100m,"No Reason","ProductName","KES");
	}

	public void CallHailstone()
	{
		string username = "sandbox";
		string apikey = "";
		string product = "";
		string channel = "";
		decimal amount = 50m; 

		var at_u = new AfricasTalkingUnityGateway();
		string request = at_u.IAP(username,apikey,"+254XXXXXXXX",product,"KES",amount,channel);
		PaymentResponse response = JsonConvert.DeserializeObject<PaymentResponse>(request);
		Debug.Log(request);
		if(response.transactionId.Substring(0,3)=="ATP"){
		processHailStone();
		Debug.Log(response.transactionId);
		}
	}

	public void AddHealth()
	{
		string username = "sandbox";
		string apikey = "";
		string product = "";
		string channel = "";
		decimal amount = 70m; 
		string number = "+2547XXXXYYY";

		var at_u = new AfricasTalkingUnityGateway();
		string request = at_u.IAP(username, apikey,number, product,"KES",amount, channel);
		PaymentResponse response = JsonConvert.DeserializeObject<PaymentResponse>(request);
		if(response.transactionId.Substring(0,3)=="ATP"){
		ProcessHealth();
		Debug.Log(response.transactionId);
		}
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
