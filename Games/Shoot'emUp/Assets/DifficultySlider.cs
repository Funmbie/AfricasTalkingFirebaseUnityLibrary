using UnityEngine.UI;
using UnityEngine;

public class DifficultySlider : MonoBehaviour {

	public Scrollbar difficultyBar;
	public Text DifficultyText;
	int x;
	// Use this for initialization
	void Start () {
		difficultyBar = GetComponent<Scrollbar>();
		DifficultyText = GetComponentInChildren<Text>();

		if(PlayerPrefs.GetInt("Difficulty",0)==0)
		difficultyBar.value = 0.3f;
		else if(PlayerPrefs.GetInt("Difficulty",0)==1)
		difficultyBar.value = 0.6f;
		else if(PlayerPrefs.GetInt("Difficulty",0)==2)
		difficultyBar.value = 1f;
		
	}
	
	// Update is called once per frame
	void Update () {
		if((difficultyBar.value<0.33f)&&(difficultyBar.value>0f))
		{
			PlayerPrefs.SetInt("Difficulty",0);
		}
		else if((difficultyBar.value<0.66f)&&(difficultyBar.value>0.33f))
		{
			PlayerPrefs.SetInt("Difficulty",1);
		}
		else if((difficultyBar.value<1f)&&(difficultyBar.value>0.66f))
		{
			PlayerPrefs.SetInt("Difficulty",2);
		}

		DifficultyText.text = "Difficulty: "+ PlayerPrefs.GetInt("Difficulty",0).ToString();
	}
}
