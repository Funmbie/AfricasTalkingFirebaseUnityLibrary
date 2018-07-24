using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour {
	public AudioClip[] tunes;
	AudioSource audi;
	// Use this for initialization
	void Start () {
		audi = GetComponent<AudioSource>();
		DontDestroyOnLoad(this.gameObject);
	}
	
	void PickTrack()
	{
		int rand = Random.Range(0,tunes.Length);
		audi.clip = tunes[rand];
		audi.Play();
	}
	// Update is called once per frame
	void Update () {
		if(!audi.isPlaying)
		PickTrack();
	}
}
