using UnityEngine.UI;
using UnityEngine;
using AfricasTalkingUnityClass;
using Newtonsoft.Json;

public class Leaderboard : MonoBehaviour {
	public Image entryPanel;
	int limit = 1;
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
		response = new LeaderboardResponse[limit];
		for(int i=0;i<limit;i++)
		{
			response[i] = new LeaderboardResponse();
		}

		var at_u = new AfricasTalkingUnityGateway();
		string json = at_u.getLeaderboard("-LI67SV_Xj_DldmfUiaZ",limit);
		response = JsonConvert.DeserializeObject<LeaderboardResponse[]>(json);

		GenerateLeaderboard();
	}
	
	void GenerateLeaderboard()
	{
		addCount = 55;

		for(int i=0;i<limit;i++)
		{
			//generate GO
			Vector3 pos = new Vector3(entryPanel.transform.position.x,entryPanel.transform.position.y-addCount,entryPanel.transform.position.z);
			Image newEntry = Instantiate(entryPanel,pos,Quaternion.identity);
			newEntry.transform.SetParent(this.gameObject.transform);
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
