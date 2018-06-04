using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour {

	public GameObject bullet;
	public float period;
	public float spinSpeed;
	public float spinRound;
	private float currentSpinSpeed;
	public float moveSpeed;
	public Vector3 startPosition;	
	public Vector3 shootPosition;
	public Vector3 leavePosition;
	public float shootPeriod;


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
		GetComponent<Transform>().Rotate(0,0,currentSpinSpeed);
		spinRound -= currentSpinSpeed/360;
		if (temp > 0) {
			temp -= Time.deltaTime*moveSpeed;
			if (temp < 0) temp = 0;
			transform.position = Vector3.Lerp(startPosition, shootPosition, 1-temp);
		} 
		if (temp == 0) {
			temp = -1;
			Debug.Log("NOW");
			StartCoroutine(shootTimer(shootPeriod));
		}

	}

	IEnumerator MoveOut() {
		temp = -2;
		while (temp < -1) {
			temp += Time.deltaTime*moveSpeed;
			if (temp > -1) temp = -1;
			transform.position = Vector3.Lerp(shootPosition, leavePosition, 2+temp);
			yield return null;
		} 
		Destroy(instance);
	}

	IEnumerator conShoot(float timer) {
		while(true) {
			yield return new WaitForSeconds(timer);
			Instantiate(bullet, this.GetComponent<Transform>().position, GetComponent<Transform>().rotation);
		}
	}

	IEnumerator shootTimer(float timer) {
		yield return new WaitForSeconds(0.5f);
		StartCoroutine(shooting);
		currentSpinSpeed = spinSpeed;
		yield return new WaitForSeconds(timer);
		// while (spinRound > 0) yield return null;
		StopCoroutine(shooting);
		currentSpinSpeed = 0;
		yield return new WaitForSeconds(0.5f);
		StartCoroutine(MoveOut());
	}

}
