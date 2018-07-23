using UnityEngine.SceneManagement;
using UnityEngine;
using AfricasTalkingUnityClass;

public class GameManager : MonoBehaviour {
	public Restarter restarter;
	public TimerScript timer;

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
		if(playerController.Health<=0f || restarter.isPassed || zombieManager.enemiesSpawned >=25)
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

	void GameOver()
	{
		var at_u = new AfricasTalkingUnityGateway();
		bool sent = false;
		//Just add leaderboard,using score as main, enemies spawned as minor and time as others
		//Collect all of them
		string x = at_u.login("funmbioyesanya7@gmail.com","1234");
		string token = x.Substring(2,x.Length-2);

		int main = playerController.Score;
		int minor = zombieManager.enemiesSpawned;
		string y = timer.minutes+"."+timer.seconds;
		float time = float.Parse(y);

		if(!sent){
		if(at_u.addLeaderboard("-LI67SV_Xj_DldmfUiaZ",token,main,minor,time)=="OK"){
		//Load GameOver Scene
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
		sent = true;
		}
		}
	} 
}
