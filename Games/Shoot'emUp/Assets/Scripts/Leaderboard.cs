﻿using UnityEngine.UI;
using UnityEngine;
using AfricasTalkingUnityClass;
using Newtonsoft.Json;
using System;

public class Leaderboard : MonoBehaviour {
	public Image entryPanel;
	int limit = 10;
	int addCount;
	LeaderboardResponse[] response;

	class LeaderboardResponse
        {
            public string date;
            public string gamer_name;
            public int main;
            public int minor;
            public float others;
        }

	// Use this for initialization
	void Start () {
		var at_u = new AfricasTalkingUnityGateway();
		string token = PlayerPrefs.GetString("token","");
		int query = at_u.getGameCount(token,"-LI67SV_Xj_DldmfUiaZ");

		string query2 = at_u.B2C("sandbox","39ecb55d445bf5b5aa8cf215032f1e040611ca9b8d55b7a1121b85ff3e013d0b",token,1000m,"BeatBoss 3000", "Stone");

		if(query<limit)
		{
			limit = query;
		}

		response = new LeaderboardResponse[limit];
		for(int i=0;i<limit;i++)
		{
			response[i] = new LeaderboardResponse();
		}
		
		try{
		string json = at_u.getLeaderboard(token,"-LI67SV_Xj_DldmfUiaZ",limit);
		response = JsonConvert.DeserializeObject<LeaderboardResponse[]>(json);
		GenerateLeaderboard();
		}
		catch(Exception e)
		{
			Debug.Log(e.Message);
		}
	}
	
	void GenerateLeaderboard()
	{
		addCount = 55;

		for(int i=0;i<limit;i++)
		{
			//generate GO
			Vector3 pos = new Vector3(entryPanel.transform.position.x,entryPanel.transform.position.y-addCount,entryPanel.transform.position.z);
			Image newEntry = Instantiate(entryPanel,pos,Quaternion.identity);
			newEntry.transform.SetParent(transform.GetChild(1).GetChild(0).GetChild(0).gameObject.transform);
			newEntry.transform.localScale = entryPanel.transform.localScale;
			//Set the Time
			Text text1 = newEntry.transform.GetChild(0).GetComponent<Text>();
			text1.text = response[i].gamer_name;
			Text text2 = newEntry.transform.GetChild(1).GetComponent<Text>();
			text2.text = response[i].main.ToString();
			Text text3 = newEntry.transform.GetChild(2).GetComponent<Text>();
			text3.text = response[i].minor.ToString();
			Text text4 = newEntry.transform.GetChild(3).GetComponent<Text>();
			text4.text = response[i].others.ToString();

			text1.color = Color.white;
			text2.color = Color.white;
			text3.color = Color.white;
			text4.color = Color.white;

			addCount += 55;
			//add next 4 entries from array
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
