using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour {

	public GameObject bullet;
	public float period;
	public float spinSpeed;
	private float currentSpinSpeed;
	public float moveSpeed;
	public Vector3 startPosition;	
	public Vector3 shootPosition;
	public Vector3 leavePosition;
	public float shootPeriod;
	public float spawnOffset;
	public float HP;
	public float moveInOffset;


	private IEnumerator shooting;
	private IEnumerator fire;
	private float temp;
	public GameObject instance;

	// Use this for initialization
	void Start () {
		shooting = (conShoot(period));
		currentSpinSpeed = 0;
		// StartCoroutine(shootTimer(shootPeriod));
		temp = 1;
		instance = this.gameObject;
		
	}
	
	// Update is called once per frame
	void Update () {
		// GetComponent<Transform>().Rotate(0,0,currentSpinSpeed);
		transform.Rotate(0,0,currentSpinSpeed*Time.deltaTime*60);
		// transform.Rotate(0,0,currentSpinSpeed);
		StartCoroutine(MoveIn());
		if (temp == 0) {
			temp = -1;
			Debug.Log("NOW");
			StartCoroutine(shootTimer(shootPeriod));
		}

		if (HP <= 0) {
			Destroy(instance);
		}
	}

	public void Damaged(float take) {
		HP -= take;
	}

	IEnumerator MoveIn() {
		yield return new WaitForSeconds(moveInOffset);
		if (temp > 0) {
			temp -= Time.deltaTime*moveSpeed;
			if (temp < 0) temp = 0;
			// transform.position = Vector3.Lerp(startPosition, shootPosition, 1-temp);
			GetComponent<Transform>().position = Vector3.Lerp(startPosition, shootPosition, 1-temp);
		} 
	}

	IEnumerator MoveOut() {
		temp = -2;
		while (temp < -1) {
			temp += Time.deltaTime*moveSpeed;
			if (temp > -1) temp = -1;
			// transform.position = Vector3.Lerp(shootPosition, leavePosition, 2+temp);
			GetComponent<Transform>().position = Vector3.Lerp(shootPosition, leavePosition, 2+temp);
			yield return null;
		} 
		Destroy(instance);
	}

	IEnumerator conShoot(float timer) {
		while(true) {
			yield return new WaitForSeconds(timer);
			// Instantiate(bullet, this.GetComponent<Transform>().position, GetComponent<Transform>().rotation);
			Instantiate(bullet, transform.position, transform.rotation);
		}
	}

	IEnumerator shootTimer(float timer) {
		yield return new WaitForSeconds(0.5f);
		StartCoroutine(shooting);
		currentSpinSpeed = spinSpeed;
		yield return new WaitForSeconds(timer);
		StopCoroutine(shooting);
		currentSpinSpeed = 0;
		yield return new WaitForSeconds(0.5f);
		StartCoroutine(MoveOut());
	}

}
