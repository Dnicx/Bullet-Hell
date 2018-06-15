using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class playerAI : MonoBehaviour {

	public GameObject playerCharacter;
	public GameObject mTrigger;
	public GameObject rTrigger;
	public GameObject lTrigger;
	public GameObject rfTrigger;
	public GameObject lfTrigger;
	public GameObject Observe;

	private observeRange observeProp;
	private playerScript playerCont;
	private float playerSpeed;

	List<GameObject> bulletClones;
	List<Vector3> simPos;
	List<Vector3> bulletClonesPos;
	public float[] moveScore;
	public float saftyBound;

	private Vector3[] MOVE = new Vector3[5];

	// Use this for initialization
	void Start () {
		observeProp = Observe.GetComponent<observeRange>();
		playerCont = playerCharacter.GetComponent<playerScript>();
		playerSpeed = playerCharacter.GetComponent<playerScript>().currentSpeed;
		bulletClones = new List<GameObject>();
		simPos = new List<Vector3>();
		bulletClonesPos = new List<Vector3>();
		moveScore = new float[5];

		MOVE[0] = new Vector3(0, 0, 0);
		MOVE[1] = new Vector3(playerSpeed*Time.fixedDeltaTime, 0, 0);
		MOVE[2] = new Vector3(-playerSpeed*Time.fixedDeltaTime, 0, 0);
		MOVE[3] = new Vector3(0, playerSpeed*Time.fixedDeltaTime, 0);
		MOVE[4] = new Vector3(0, -playerSpeed*Time.fixedDeltaTime, 0);
	}
	
	// Update is called once per frame
	//FIRST PLAYER MODEL
	void FixedUpdate () {
		if (playerCharacter == null) return;
		playerSpeed = playerCharacter.GetComponent<playerScript>().currentSpeed;
		simulateAI(1, 5);
		// AvoidantAI()
	}

	
	void AvoidantAI() {
		if (playerCharacter == null) return;
		playerCont.PressShift();
		playerCont.direction = observeProp.weight;
		if (observeProp.weight == Vector3.zero) 
			playerCont.direction = -playerCharacter.GetComponent<Transform>().position;
		observeProp.clear();
	}


	public int frameCount;
	public int decidedMove;
	void simulateAI(int iteration, int responseFrame) {
		if (frameCount <= 0) frameCount = responseFrame;
		if (frameCount < responseFrame) {
			frameCount--;
			if ( decidedMove == 0) playerCont.Stop();
			if ( decidedMove == 1) playerCont.GoRight();
			if ( decidedMove == 2) playerCont.GoLeft();
			if ( decidedMove == 3) playerCont.GoUp();
			if ( decidedMove == 4) playerCont.GoDown();
			return;
		}
		float fixedTime= Time.fixedDeltaTime;
		bulletClones.Clear();
		simPos.Clear();
		bulletClonesPos.Clear();
		moveScore = new float[5];
		simPos.Add(playerCharacter.GetComponent<Transform>().position);
		if (playerCharacter == null) return;
		foreach (GameObject bullet in observeProp.bullets) {
			if (bullet == null) continue;
			bulletClones.Add(bullet);
			bulletClonesPos.Add(bullet.GetComponent<Transform>().position);
		}
		for (int i = 1; i<=iteration; i++) {
			List<Vector3> simPosTemp = new List<Vector3>(simPos);
			foreach (Vector3 pos in simPosTemp) {
				simPos.RemoveAt(0);
				simPos.Add(new Vector3(pos.x, pos.y, 0));
				simPos.Add(new Vector3(pos.x, pos.y, 0));
				simPos.Add(new Vector3(pos.x, pos.y, 0));
				simPos.Add(new Vector3(pos.x, pos.y, 0));
				simPos.Add(new Vector3(pos.x, pos.y, 0));
			}

			

			for (int r = 0; r < responseFrame ; r++) {
				
				for (int j = 0; j < bulletClones.Count ; j++) {
					bulletClonesPos[j] += bulletClones[j].transform.up * bulletClones[j].GetComponent<bullet>().speed * fixedTime;
				}
				for (int k = 0; k < simPos.Count; k++) {
					// bulletClonesPos[j] += bulletClones[j].transform.up * bulletClones[j].GetComponent<bullet>().speed * fixedTime;
					simPos[k] += MOVE[k%5];
					for (int j = 0; j < bulletClones.Count ; j++) {
						if (bulletClonesPos[j].x-saftyBound < simPos[k].x && bulletClonesPos[j].x+saftyBound > simPos[k].x &&
							bulletClonesPos[j].y-saftyBound < simPos[k].y && bulletClonesPos[j].y+saftyBound > simPos[k].y ) {
								Debug.Log(k + " " + simPos[k].x + ":" + simPos[k].y+ " dead " + Mathf.FloorToInt(k/Mathf.Pow(5, i-1)) + " " + i + " " + r);
								moveScore[Mathf.FloorToInt(k/Mathf.Pow(5, i-1))] -= 10/(i*responseFrame + r);
								break;
						}
					}
				}
			}


		}
		int maxIndex = 0;
		for (int i = 1; i<5; i++) {
			if (moveScore[i] > moveScore[maxIndex]) maxIndex = i;
		}
		if (frameCount == responseFrame) decidedMove = maxIndex;
		if ( decidedMove == 0) playerCont.Stop();
		if ( decidedMove == 1) playerCont.GoRight();
		if ( decidedMove == 2) playerCont.GoLeft();
		if ( decidedMove == 3) playerCont.GoUp();
		if ( decidedMove == 4) playerCont.GoDown();
		frameCount--;
	}

}
