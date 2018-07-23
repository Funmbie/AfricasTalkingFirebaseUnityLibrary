using UnityEngine.UI;
using UnityEngine;

public class TimerScript : MonoBehaviour {
	
	public bool isPaused;

	public int minutes=0, seconds=0;
	Text timerText;

	// Use this for initialization
	void Start () {
		timerText = GetComponent<Text>();
		InvokeRepeating("Counter",1f,1f);
	}

	void Update()
	{
		if(minutes<10)
		{
			if(seconds<10)
			{
				timerText.text = "0"+minutes+":"+"0"+seconds;
			}
			else
			{
				timerText.text = "0"+minutes+":"+seconds;
			}
		}
		else
		{
			if(seconds<10)
			{
				timerText.text = minutes+":0"+seconds;
			}
			else
			{
				timerText.text = minutes+":"+seconds;
			}
		}
	}

	void Counter()
	{
		if(!isPaused){
			if(seconds<59){
				seconds++;
			}
			else
			{
				seconds = 0;
				minutes++;
			}
		}
	}
}
