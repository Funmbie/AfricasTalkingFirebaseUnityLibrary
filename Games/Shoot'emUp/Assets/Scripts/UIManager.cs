using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour {
	public GameObject inGamePanel,pausePanel;
	public Text gameOvertxt;
	// Use this for initialization
	void Start () {
		gameOvertxt.text = "";
		unPause();
	}

	public void GameOver()
	{
		gameOvertxt.text = "Game Over";
	}

	public void pause()
	{
		pausePanel.SetActive(true);
		inGamePanel.SetActive(false);
	}

	public void unPause()
	{
		pausePanel.SetActive(false);
		inGamePanel.SetActive(true);
	}
}
