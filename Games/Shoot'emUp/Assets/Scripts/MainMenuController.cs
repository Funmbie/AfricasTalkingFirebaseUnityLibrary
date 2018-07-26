using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuController : MonoBehaviour {
	public GameObject mainmenu,leaderboard,options,loginPanel,regPanel;
	// Use this for initialization
	void Start () {
		returntoMainMenu();
		LoginPanel();
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

	public void LoginPanel()
	{
		loginPanel.SetActive(true);
		regPanel.SetActive(false);
	}

	public void RegPanel()
	{
		loginPanel.SetActive(false);
		regPanel.SetActive(true);
	}
}
