using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour {

	public float speed;
	public float slowSpeed;
	public float currentSpeed;
	public int frequency;
	public GameObject pBulletG;
	public GameObject pBulletR;
	public GameObject playerDot;
	public Material Green;
	public Material Red;
	public Vector3 direction;
	public GameObject bound;
	public float BoundsOffset;
	public Vector3 mx_bound;
	public Vector3 mn_bound;
	public static readonly Vector3 UP = new Vector3(0,1,0);
	public static readonly Vector3 DOWN = new Vector3(0,-1,0);
	public static readonly Vector3 RIGHT = new Vector3(1,0,0);
	public static readonly Vector3 LEFT = new Vector3(-1,0,0);
	public static readonly Vector3 UPLEFT = new Vector3(-1,1,0);
	public static readonly Vector3 UPRIGHT = new Vector3(1,1,0);
	public static readonly Vector3 DOWNLEFT = new Vector3(-1,-1,0);
	public static readonly Vector3 DOWNRIGHT = new Vector3(1,-1,0);

	public IEnumerator fire;

	//true = G, false = R
	public bool polar;
	
	private EnvManager EnvScript;
	private bool isFire;

	// Use this for initialization
	void Start () {
		currentSpeed = slowSpeed;
		mx_bound = bound.GetComponent<Collider>().bounds.max;
		mn_bound = bound.GetComponent<Collider>().bounds.min;
		polar = true;
		isFire = false;

		EnvScript = bound.GetComponent<EnvManager>();
	}

	void Update() {
			if (Input.GetButtonDown("Switch")) {
				polar = !polar;
				if (polar) playerDot.GetComponent<MeshRenderer>().material = Green;
				else playerDot.GetComponent<MeshRenderer>().material = Red;
			}
			// if (Input.GetButtonDown("Slow")) {
			// 	currentSpeed = slowSpeed;
			// }
			// if (Input.GetButtonUp("Slow")) {
			// 	currentSpeed = speed;
			// }
			if (Input.GetButtonDown("Fire")) {
				// if (polar) Instantiate(pBulletG, this.GetComponent<Transform>().position, GetComponent<Transform>().rotation);
				// else Instantiate(pBulletR, this.GetComponent<Transform>().position, GetComponent<Transform>().rotation);
				isFire = true;
			} 
			if (Input.GetButtonUp("Fire")) {
				isFire = false;
			} 
			if (Input.GetButtonDown("Slowmo")) {
				Time.timeScale = .5f;
				// Time.fixedDeltaTime = 0.0334f;
			} 
			if (Input.GetButtonUp("Slowmo")) {
				Time.timeScale = 1.0f;
				// Time.fixedDeltaTime = 0.0167f;
			} 
	}
	
	private int fireDelay;
	// Update is called once per frame
	void FixedUpdate () {
		// GetComponent<Transform>().position += new Vector3(currentSpeed*Time.deltaTime*Input.GetAxis("Horizontal"),currentSpeed*Time.deltaTime*Input.GetAxis("Vertical"),0);
		GetComponent<Transform>().position += new Vector3(currentSpeed*Input.GetAxisRaw("Horizontal")*Time.fixedDeltaTime,currentSpeed*Input.GetAxisRaw("Vertical")*Time.fixedDeltaTime,0);
		GetComponent<Transform>().position += (direction.normalized)*Time.fixedDeltaTime*currentSpeed;
		


		if (GetComponent<Transform>().position.x <= mn_bound.x+BoundsOffset) GetComponent<Transform>().position = new Vector3(mn_bound.x+BoundsOffset, GetComponent<Transform>().position.y, 0);
		if (GetComponent<Transform>().position.x >= mx_bound.x-BoundsOffset) GetComponent<Transform>().position = new Vector3(mx_bound.x-BoundsOffset, GetComponent<Transform>().position.y, 0);
		if (GetComponent<Transform>().position.y <= mn_bound.y+BoundsOffset) GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x,mn_bound.y+BoundsOffset, 0);
		if (GetComponent<Transform>().position.y >= mx_bound.y-BoundsOffset) GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x,mx_bound.y-BoundsOffset, 0);

		if (isFire && fireDelay >= 60-frequency) {
			fireDelay = 0;
			if (polar) Instantiate(pBulletG, this.GetComponent<Transform>().position, GetComponent<Transform>().rotation);
			else Instantiate(pBulletR, this.GetComponent<Transform>().position, GetComponent<Transform>().rotation);
		}
		fireDelay +=1;
	}


	public void PressShift() {
		// currentSpeed = slowSpeed;
	}
	public void LiftShift() {
		// currentSpeed = speed;
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
	public void GoDiagUL() {
		direction = UPLEFT;
	}
	public void GoDiagUR() {
		direction = UPRIGHT;
	}
	public void GoDiagDL() {
		direction = DOWNLEFT;
	}
	public void GoDiagDR() {
		direction = DOWNRIGHT;
	}
	public void Fire() {
		isFire = true;
	}
	public void Hault() {
		isFire = false;
	}

	public void SetEnvManager(EnvManager script) {
		EnvScript = script;
	}

	public EnvManager GetEnvManager() {
		return EnvScript;
	}
}
