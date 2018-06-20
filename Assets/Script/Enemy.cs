using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

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
	public bool polar;

	private bool isFire;
	private float temp;
	public GameObject instance;

	// Use this for initialization
	void Start () {
		// shooting = (conShoot(period));
		currentSpinSpeed = 0;
		// StartCoroutine(shootTimer(shootPeriod));
		temp = 1;
		instance = this.gameObject;
		isFire = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public float fireTimer = 0;
	void FixedUpdate() {
		
		transform.Rotate(0,0,currentSpinSpeed);
		
		StartCoroutine(MoveIn());
		if (temp == 0) {
			temp = -1;
			StartCoroutine(shootTimer(shootPeriod));
		}

		if (HP <= 0) {
			Destroy(instance);
		}
	}

	public void setValues(float[] param) {
		// rate = param[0];
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
			// temp -= Time.deltaTime*moveSpeed*Time.timeScale;
			temp -= Time.deltaTime*moveSpeed;
			if (temp < 0) temp = 0;
			// transform.position = Vector3.Lerp(startPosition, shootPosition, 1-temp);
			GetComponent<Transform>().position = Vector3.Lerp(startPosition, shootPosition, 1-temp);
		} 
	}

	IEnumerator MoveOut() {
		temp = -2;
		while (temp < -1) {
			// temp += Time.deltaTime*moveSpeed*Time.timeScale	;
			temp += Time.deltaTime*moveSpeed;
			if (temp > -1) temp = -1;
			// transform.position = Vector3.Lerp(shootPosition, leavePosition, 2+temp);
			GetComponent<Transform>().position = Vector3.Lerp(shootPosition, leavePosition, 2+temp);
			yield return null;
		} 
		Destroy(instance);
	}


	IEnumerator shootTimer(float timer) {
		yield return new WaitForSeconds(0.5f);
		isFire = true;
		currentSpinSpeed = spinSpeed;
		yield return new WaitForSeconds(timer);
		isFire = false;
		currentSpinSpeed = 0;
		yield return new WaitForSeconds(0.5f);
		StartCoroutine(MoveOut());
	}
}
