using UnityEngine;
using UnityEngine.UI;

public class HealthToUIManager : MonoBehaviour {

	PlayerController playerController;
	Slider healthSlider;

	// Use this for initialization
	void Start () {
		playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		healthSlider = GetComponent<Slider>();
		healthSlider.maxValue = 100f;
		healthSlider.value = playerController.Health;
	}
	
	// Update is called once per frame
	void Update () {
		healthSlider.value = playerController.Health;
	}
}
