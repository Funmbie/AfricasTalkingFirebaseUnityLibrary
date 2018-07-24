using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuController : MonoBehaviour {
	public GameObject mainmenu,leaderboard,options;
	// Use this for initialization
	void Start () {
		returntoMainMenu();
	}

	public void LoadGameScene()
	{
		PlayerPrefs.Save();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
	}

	public void returntoMainMenu()
	{
		mainmenu.SetActive(true);
		leaderboard.SetActive(false);
		options.SetActive(false);
	}
	
	public void LeaderboardEnable()
	{
		mainmenu.SetActive(false);
		leaderboard.SetActive(true);
		options.SetActive(false);
	}

	public void OptionsEnable()
	{
		mainmenu.SetActive(false);
		leaderboard.SetActive(false);
		options.SetActive(true);
	}
}
