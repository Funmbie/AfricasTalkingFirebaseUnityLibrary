using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AfricasTalkingUnityClass;
using UnityEngine.UI;

public class RetryLogicManager : MonoBehaviour {
	public GameObject leaderboardPanel;

	// Use this for initialization
	void Start () {
		var at_u = new AfricasTalkingUnityGateway();
		int main = PlayerPrefs.GetInt("main",0);
		int minor = PlayerPrefs.GetInt("minor",0);
		float others = PlayerPrefs.GetFloat("others",0f);
		string token = PlayerPrefs.GetString("token","");
		string request = at_u.addLeaderboard("-LI67SV_Xj_DldmfUiaZ",token,main,minor,others);
		if(request=="OK")
		{
			//Take leaderboard gameobject
			StartCoroutine(callForward(leaderboardPanel,"Successful Leaderboard Update"));
		}
		else
		{
			//Take error and display
			StartCoroutine(callForward(leaderboardPanel,request));
		}

		Debug.Log(request);

		if(main>3000)
		{
			//Send them airtime
			Invoke("sendAirtime",2f);
		}
	}

	void sendAirtime()
	{
		var at_u = new AfricasTalkingUnityGateway();
		string username="sandbox";
		string apikey = "39ecb55d445bf5b5aa8cf215032f1e040611ca9b8d55b7a1121b85ff3e013d0b";
		string gamer_id = PlayerPrefs.GetString("token","");
		string amount="100";
		if((at_u.sendAirtime(username, apikey, gamer_id, amount))=="OK")
		StartCoroutine(callForward(leaderboardPanel,"You got airtime!!"));
	}

	IEnumerator callForward(GameObject g,string msg)
	{
		g.SetActive(true);
		//Set text
		g.GetComponentInChildren<Text>().text = msg;
		//Move g
		float x=0f;
		while(x<2f)
		{
			g.transform.position = new Vector3(g.transform.position.x+50f,g.transform.position.y,g.transform.position.z);
			x+=0.1f;
			yield return null;
		}
		//Wait then callBack
		yield return new WaitForSeconds(3f);
		StartCoroutine(callBack(g));
	}

	IEnumerator callBack(GameObject g)
	{
		float x=0f;
		while(x<2f)
		{
			g.transform.position = new Vector3(g.transform.position.x-50f,g.transform.position.y,g.transform.position.z);
			x+=0.1f;
			yield return null;
		}
		g.SetActive(false);
	}

	public void Yes()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
	}

	public void No()
	{
		GameObject musicObject = GameObject.FindGameObjectWithTag("Music");
		
		if(musicObject)
		Destroy(musicObject);
		
		SceneManager.LoadScene(0);
	}
}
