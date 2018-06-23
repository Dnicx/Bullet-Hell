using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

	public GameObject bullet;
	[Range(0, 60)] public int rate;
	public float spinSpeed;
	private float currentSpinSpeed;
	public float shootPeriod;
	public bool polar;


	private IEnumerator shooting;
	private IEnumerator fire;
	private bool isFire;
	private float temp;
	public GameObject instance;

	// Use this for initialization
	void Start () {
		// shooting = (conShoot(period));
		currentSpinSpeed = 0;
		// StartCoroutine(shootTimer(shootPeriod));
		temp = 0;
		instance = this.gameObject;
		isFire = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public float fireTimer = 0;
	void FixedUpdate() {
		if (fireTimer < 60/rate) {
			fireTimer+=1.0f;
		}
		else {
			fireTimer = 0;
			if (isFire) {
				Instantiate(bullet, transform.position, transform.rotation);
			}
		}

		// GetComponent<Transform>().Rotate(0,0,currentSpinSpeed);
		transform.Rotate(0,0,currentSpinSpeed);
		// transform.Rotate(0,0,currentSpinSpeed);
		if (temp == 0) {
			temp = -1;
			StartCoroutine(shootTimer(shootPeriod));
		}
	}

	// public void setValues(float[] param) {
	// 	// rate = param[0];
	// 	spinSpeed = param[1];
	// 	moveSpeed = param[2];
	// 	startPosition.x = param[3];
	// 	startPosition.y = param[4];
	// 	shootPosition.x = param[5];
	// 	shootPosition.y = param[6];
	// 	leavePosition.x = param[7];
	// 	leavePosition.y = param[8];
	// 	shootPeriod = param[9];
	// 	spawnOffset = param[10];
	// 	HP = param[11];
	// 	moveInOffset = param[12];

	// }
	public void StartShoot() {
		isFire = true;
	}

	public void HaltShoot() {
		isFire = false;
	}

	IEnumerator shootTimer(float timer) {
		yield return new WaitForSeconds(0.5f);
		// StartCoroutine(shooting);
		isFire = true;
		currentSpinSpeed = spinSpeed;
		yield return new WaitForSeconds(timer);
		// StopCoroutine(shooting);
		isFire = false;
		currentSpinSpeed = 0;
		yield return new WaitForSeconds(0.5f);
	}

}
