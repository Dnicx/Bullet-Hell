using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour {

	public float speed;
	public float slowSpeed;
	public float currentSpeed;
	public float frequency;
	public GameObject pbullet;
	public Vector3 direction;
	public GameObject bound;
	public float BoundsOffset;
	public Vector3 mx_bound;
	public Vector3 mn_bound;
	public static readonly Vector3 UP = new Vector3(0,1,0);
	public static readonly Vector3 DOWN = new Vector3(0,-1,0);
	public static readonly Vector3 RIGHT = new Vector3(1,0,0);
	public static readonly Vector3 LEFT = new Vector3(-1,0,0);

	public IEnumerator fire;
	

	// Use this for initialization
	void Start () {
		currentSpeed = speed;
		fire = conShoot(frequency);
		mx_bound = bound.GetComponent<Collider>().bounds.max;
		mn_bound = bound.GetComponent<Collider>().bounds.min;
	}

	void Update() {
			if (Input.GetButtonDown("slow")) {
				currentSpeed = slowSpeed;
			}
			if (Input.GetButtonUp("slow")) {
				currentSpeed = speed;
			}
			if (Input.GetButtonDown("Fire")) {
				Instantiate(pbullet, this.GetComponent<Transform>().position, GetComponent<Transform>().rotation);
				StartCoroutine(fire);
			} 
			if (Input.GetButtonUp("Fire")) {
				StopCoroutine(fire);

			} 
			if (Input.GetButtonDown("slowmo")) {
				Time.timeScale = 0.5f;
				Time.fixedDeltaTime = 0.0334f;
			} 
			if (Input.GetButtonUp("slowmo")) {
				Time.timeScale = 1.0f;
				Time.fixedDeltaTime = 0.0167f;
			} 
	}
	
	// Update is called once per frame
	void FixedUpdate () {
            // GetComponent<Transform>().position += new Vector3(currentSpeed*Time.deltaTime*Input.GetAxis("Horizontal"),currentSpeed*Time.deltaTime*Input.GetAxis("Vertical"),0);
			GetComponent<Transform>().position += new Vector3(currentSpeed*Input.GetAxis("Horizontal")/60,currentSpeed*Input.GetAxis("Vertical")/60,0);
			GetComponent<Transform>().position += (direction.normalized)*Time.fixedDeltaTime*currentSpeed;
			


			if (GetComponent<Transform>().position.x <= mn_bound.x+BoundsOffset) GetComponent<Transform>().position = new Vector3(mn_bound.x+BoundsOffset, GetComponent<Transform>().position.y, 0);
			if (GetComponent<Transform>().position.x >= mx_bound.x-BoundsOffset) GetComponent<Transform>().position = new Vector3(mx_bound.x-BoundsOffset, GetComponent<Transform>().position.y, 0);
			if (GetComponent<Transform>().position.y <= mn_bound.y+BoundsOffset) GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x,mn_bound.y+BoundsOffset, 0);
			if (GetComponent<Transform>().position.y >= mx_bound.y-BoundsOffset) GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x,mx_bound.y-BoundsOffset, 0);
	}

	// void OnTriggerEnter(Collider other)
    // {
	// 	if (other.gameObject.tag == "screen") {
	// 		return;
	// 	}
		
	// 	Debug.Log("Hit");
	//         Destroy(this.gameObject);
    // }

	public void PressShift() {
		currentSpeed = slowSpeed;
	}
	public void LiftShift() {
		currentSpeed = speed;
	}
	public void Stop() {
		direction = new Vector3(0,0,0);
	}
	public void GoUp() {
		direction = UP;
	}
	public void GoDown() {
		direction = DOWN;
	}
	public void GoLeft() {
		direction = LEFT;
	}
	public void GoRight() {
		direction = RIGHT;
	}

	IEnumerator conShoot(float timer) {
		while(true) {
			yield return new WaitForSeconds(timer);
			Instantiate(pbullet, this.GetComponent<Transform>().position, GetComponent<Transform>().rotation);
		}
	}
}
