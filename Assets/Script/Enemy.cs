using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	// public GameObject EnvManager;
	public float spinSpeed;
	private float currentSpinSpeed;
	public float moveSpeed;
	public float moveOutSpeed;
	public Vector3 startPosition;	
	public Vector3 shootPosition;
	public Vector3 leavePosition;
	public float stopPeriod;
	public float spawnOffset;
	public float HP;
	public float moveInOffset;
	public bool polar;
	public int movePattern;

	private bool isFire;
	private float temp;
	private EnvManager EnvScript;
	public GameObject instance;

	// Use this for initialization
	void Start () {
		// shooting = (conShoot(period));
		currentSpinSpeed = 0;
		// StartCoroutine(shootTimer(stopPeriod));
		temp = 1;
		instance = this.gameObject;
		SetChildFire(false);
		if (movePattern == 0) {
			startPosition = transform.position;
			shootPosition = transform.position;
			leavePosition = transform.position;
		}

		// EnvScript = EnvManager.GetComponent<EnvManager>();
	}

	void FixedUpdate() {
		
		transform.Rotate(0,0,currentSpinSpeed);
		
		StartCoroutine(MoveIn());
		if (temp == 0) {
			temp = -1;
			StartCoroutine(shootTimer(stopPeriod));
		}

		if (HP <= 0) {
			Destroy(instance);
		}

		if (movePattern == 2) {
			// if (EnvScript.player == null) return;
			// transform.rotation = Quaternion.LookRotation(transform.forward, (EnvScript.player.transform.position - transform.position));	
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			Destroy(instance);
			Destroy(other.gameObject);
		}
	}

	void OnTriggerExit(Collider other) {
			Debug.Log(other.gameObject.tag);
		if (other.gameObject.tag == "screen") {
			SetChildFire(false);
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
		stopPeriod = param[9];
		spawnOffset = param[10];
		HP = param[11];
		moveInOffset = param[12];

	}

	public void setEnvManager(EnvManager script) {
		EnvScript = script.GetComponent<EnvManager>();
	}

	public void Damaged(float take) {
		HP -= take;
	}

	public Vector3 GetPlayerPosition() {
		if (EnvScript == null) return new Vector3();
		return EnvScript.player.transform.position;
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
			temp += Time.deltaTime*moveOutSpeed;
			if (temp > -1) temp = -1;
			GetComponent<Transform>().position = Vector3.Lerp(shootPosition, leavePosition, 2+temp);
			if (movePattern == 2) {
				GetComponent<Transform>().position += new Vector3(Mathf.Sin((temp+2)*10), 0, 0);
			}
			if (movePattern == 3) {
				if (shootPosition.x > leavePosition.x) GetComponent<Transform>().position -= new Vector3((shootPosition.y - leavePosition.y) * (1-(Mathf.Cos((temp+2)*1.57f))),0, 0);
				else GetComponent<Transform>().position += new Vector3((shootPosition.y - leavePosition.y) * (1-(Mathf.Cos((temp+2)*1.57f))),0, 0);
			}
			yield return null;
		} 
		Destroy(instance);
	}


	IEnumerator shootTimer(float timer) {
		// yield return new WaitForSeconds(0.5f);
		SetChildFire(true);
		currentSpinSpeed = spinSpeed;
		yield return new WaitForSeconds(timer);
		// SetChildFire(false);
		// currentSpinSpeed = 0;
		// yield return new WaitForSeconds(0.5f);
		StartCoroutine(MoveOut());
	}

	void SetChildFire(bool fire) {
		Turret turret;
		foreach (Transform child in transform) {
			turret = child.gameObject.GetComponent<Turret>();
			if (turret != null) {
				turret.SetFire(fire);
			}
		}
	}
}
