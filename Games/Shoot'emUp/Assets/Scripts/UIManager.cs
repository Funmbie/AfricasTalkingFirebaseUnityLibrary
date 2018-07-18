using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
	public GameObject inGamePanel,pausePanel;
	// Use this for initialization
	void Start () {
		unPause();
	}
	
	// Update is called once per frame
	void Update () {
		
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
