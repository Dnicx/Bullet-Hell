using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour {

	public GameObject bullet;
	[Range(0.001f, 1000.0f)] public float rate;
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
		// shooting = (conShoot(period));
		currentSpinSpeed = 0;
		// StartCoroutine(shootTimer(shootPeriod));
		temp = 1;
		instance = this.gameObject;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public float fireTimer = 0;
	void FixedUpdate() {
		if (fireTimer < 1/rate) {
			fireTimer+=0.001f;
		}
		else {
			fireTimer = 0;
			Instantiate(bullet, transform.position, transform.rotation);
		}

		// GetComponent<Transform>().Rotate(0,0,currentSpinSpeed);
		transform.Rotate(0,0,currentSpinSpeed);
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

	public void setValues(float[] param) {
		rate = param[0];
		spinSpeed = param[1];
		moveSpeed = param[2];
		startPosition.x = param[3];
		startPosition.y = param[4];
		shootPosition.x = param[5];
		shootPosition.y = param[6];
		leavePosition.x = param[7];
		leavePosition.y = param[8];
		shootPeriod = param[9];
		spawnOffset = param[10];
		HP = param[11];
		moveInOffset = param[12];

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
		// while(true) {
			yield return new WaitForSeconds(timer);
			Instantiate(bullet, transform.position, transform.rotation);
		// }
	}

	IEnumerator shootTimer(float timer) {
		yield return new WaitForSeconds(0.5f);
		// StartCoroutine(shooting);
		currentSpinSpeed = spinSpeed;
		yield return new WaitForSeconds(timer);
		// StopCoroutine(shooting);
		currentSpinSpeed = 0;
		yield return new WaitForSeconds(0.5f);
		StartCoroutine(MoveOut());
	}

}
