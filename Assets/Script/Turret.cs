using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

	public GameObject bullet;
	[Range(0, 60)] public int rate;
	public float spinSpeed;
	private float currentSpinSpeed;
	public float shootPeriod;
	public float cycleOffset;
	public bool polar;
	public int spread;
	public int barrels = 1;
	public Vector2 hiccup;	


	private IEnumerator shooting;
	private IEnumerator fire;
	public float counter;
	public bool isFire;
	public bool target;
	public bool stationary;
	public bool laser;
	public GameObject instance;

	// Use this for initialization
	void Start () {
		currentSpinSpeed = 0;
		instance = this.gameObject;
		isFire = false;
	}


	public float fireTimer = 0;
	void FixedUpdate() {
		if (isFire && rate > 0) {
			if (fireTimer < 60/rate) {
				if (counter >= 0) fireTimer+=1.0f;
			}
			else {
				fireTimer = 0;

				GameObject temp = null;
				
				if (barrels == 1) temp = Instantiate(bullet, transform.position, transform.rotation);
				else {
					Quaternion transformTemp = transform.rotation;
					transformTemp.eulerAngles += new Vector3(0, 0, -spread/2);
					for (int i = 0; i<barrels; i++) {
						temp = Instantiate(bullet, transform.position, transformTemp);
						transformTemp.eulerAngles += new Vector3(0, 0, spread/(barrels-1));
					}
				}
				
				if (laser) {
					temp.transform.SetParent(transform.parent);
				}
				if (stationary) {
					temp.GetComponent<bullet>().SetInertia(EnvManager.INERTIA);
				}
				
			}
			counter += Time.deltaTime;
			if (counter > hiccup.y) 
					if (counter >= hiccup.y) counter = -hiccup.x;
		}
		if (target) {
			if (transform.parent.gameObject.GetComponent<Enemy>() != null) 
				transform.rotation = Quaternion.LookRotation(transform.forward, (transform.parent.gameObject.GetComponent<Enemy>().GetPlayerPosition() - transform.position));
		} else {
			transform.Rotate(0,0,currentSpinSpeed);
		}

		if (isFire) {
			StartCoroutine(shootTimer(shootPeriod));
		}
	}

	public void SetFire(bool fire) {
		if (fire) StartCoroutine(shootDelay());
		else isFire = false;
	}

	IEnumerator shootDelay() {
		yield return new WaitForSeconds(cycleOffset);
		isFire = true;
	}

	IEnumerator shootTimer(float timer) {
		currentSpinSpeed = spinSpeed;
		yield return new WaitForSeconds(timer);
		isFire = false;
		currentSpinSpeed = 0;
	}

}
