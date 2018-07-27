using UnityEngine.SceneManagement;
using UnityEngine;
using AfricasTalkingUnityClass;

public class GameManager : MonoBehaviour {
	public Restarter restarter;
	public TimerScript timer;

	PlayerController playerController;
	ZombieManager zombieManager;
	UIManager uiManager;
	[HideInInspector]public bool isPaused;
	// Use this for initialization
	void Start () {
		isPaused = false;
		uiManager = GetComponent<UIManager>();
		zombieManager = GetComponent<ZombieManager>();
		playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		if(playerController.Health<=0f || restarter.isPassed || zombieManager.enemiesSpawned >=25)
		{
			GameOverDisplay();
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
			timer.isPaused = true;
			uiManager.pause();
			//Enable options panel
		}
		else
		{
			playerController.enabled = true;
			zombieManager.playZombies();
			zombieManager.isPaused = false;
			timer.isPaused = false;
			Time.timeScale = 1f;
			uiManager.unPause();
			//Disable options panel
		}
	}

	void GameOverDisplay()
	{
		uiManager.GameOver();

		Invoke("GameOver",1.4f);
	}

	void GameOver()
	{
		int main = playerController.Score;
		int minor = zombieManager.enemiesSpawned;
		float others = float.Parse(timer.minutes+"."+timer.seconds);
		PlayerPrefs.SetInt("main",main);
		PlayerPrefs.SetInt("minor",minor);
		PlayerPrefs.SetFloat("others",others);
		//Load GameOver Scene
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
	} 
}
