using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	
	bool isMoving;
	bool isGrounded;

	public Transform spawnPoint;
	public GameObject bulletSpawn;
	public float Health,Score;

	[HideInInspector]public float fireTime;

	// Use this for initialization
	void Start () {
		Score = 0f;
		Health = 100f;
		fireTime = PlayerPrefs.GetFloat("FireTime",1.4f);
	}
	
	// Update is called once per frame
	void Update () {
		Move(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));

		if(Input.GetButtonDown("Jump"))
		{
			StartCoroutine(Fire());
		}
	}

	void Move(float x, float z)
	{
		transform.Translate(0f,0f,z*21f*Time.deltaTime);
		transform.Rotate(0f,x*120f*Time.deltaTime,0f);
	}

	void OnTriggerStay(Collider col)
	{
		if(col.CompareTag("Floor"))
		{
			isGrounded = true;
		}
	}

	void OnTriggerExit(Collider col)
	{
		if(col.CompareTag("Floor"))
		{
			isGrounded = false;
		}
	}

	IEnumerator Fire()
	{
		GameObject bullet = Instantiate(bulletSpawn,spawnPoint.position, Quaternion.identity) as GameObject;
		Rigidbody b_rb = bullet.GetComponent<Rigidbody>();
		b_rb.AddForce(transform.forward*2000f);

		yield return new WaitForSeconds(fireTime);
	}
}
