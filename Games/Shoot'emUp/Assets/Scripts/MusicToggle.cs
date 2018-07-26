using UnityEngine.UI;
using UnityEngine;

public class MusicToggle : MonoBehaviour {
	Toggle on_off;
	Text on_off_text;
	int getter;
	public GameObject musicObject;
	// Use this for initialization
	void Start () {
		getter = PlayerPrefs.GetInt("Music",0);
		on_off = GetComponentInChildren<Toggle>();
		on_off_text = on_off.gameObject.GetComponentInChildren<Text>();
		if(getter==0){
			on_off.isOn = false;
			on_off_text.text = "Off";
			musicObject.GetComponent<AudioSource>().volume = 0f;
		}
		else if(getter==1)
		{
			on_off_text.text = "On";
			musicObject.GetComponent<AudioSource>().volume = 1f;
			on_off.isOn = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(on_off.isOn)
		{
			on_off_text.text = "On";
			PlayerPrefs.SetInt("Music",1);
			musicObject.GetComponent<AudioSource>().volume = 1f;
		}
		else
		{
			on_off_text.text = "Off";
			PlayerPrefs.SetInt("Music",0);
			musicObject.GetComponent<AudioSource>().volume = 0f;
		}
	}
}
