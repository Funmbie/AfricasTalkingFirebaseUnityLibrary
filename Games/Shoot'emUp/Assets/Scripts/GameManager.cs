using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {
	PlayerController playerController;
	ZombieManager zombieManager;
	UIManager uiManager;
	bool isPaused;
	// Use this for initialization
	void Start () {
		isPaused = false;
		uiManager = GetComponent<UIManager>();
		zombieManager = GetComponent<ZombieManager>();
		playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		if(playerController.Health<=0f)
		{
			GameOver();
		}
	}

	public void Pause()
	{
		if(isPaused)
		isPaused = false;
		else
		isPaused = true;

		if(isPaused)
		{
			playerController.enabled = false;
			zombieManager.pauseZombies();
			zombieManager.isPaused = true;
			uiManager.pause();
			//Enable options panel
		}
		else
		{
			playerController.enabled = true;
			zombieManager.playZombies();
			zombieManager.isPaused = false;
			Time.timeScale = 1f;
			uiManager.unPause();
			//Disable options panel
		}
	}

	void GameOver()
	{
		Debug.Log("GameOver");
		//Check if value is higher than player's highscore
		//Load GameOver Scene
	}
}
