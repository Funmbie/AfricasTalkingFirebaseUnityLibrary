using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	
	bool isMoving;
	bool isGrounded;

	public Transform spawnPoint;
	public GameObject bulletSpawn;
	public float Health;
	public int Score;

	[HideInInspector]public float fireTime;

	// Use this for initialization
	void Start () {
		Score = 0;
		Health = 100f;
		fireTime = 1.2f;
	}
	
	// Update is called once per frame
	void Update () {
		Move(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));

		if(Input.GetButtonDown("Jump"))
		{
			StartCoroutine(Fire());
		}
		float input = Input.GetAxis("Mouse X");
		transform.Rotate(0f,input*90f*Time.deltaTime,0f);
	}

	void Move(float x, float z)
	{
		transform.Translate(x*14f*Time.deltaTime,0f,z*14f*Time.deltaTime);
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
		b_rb.AddForce(transform.forward*4000f);

		yield return new WaitForSeconds(fireTime);
	}
}
