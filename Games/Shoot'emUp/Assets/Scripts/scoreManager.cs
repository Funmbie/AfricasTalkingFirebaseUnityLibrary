using UnityEngine;
using UnityEngine.UI;

public class scoreManager : MonoBehaviour {
	
	PlayerController playerController;
	Text scoreText;

	// Use this for initialization
	void Start () {
		scoreText = GetComponent<Text>();
		playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		scoreText.text = "Score: "+playerController.Score.ToString();
	}
}
