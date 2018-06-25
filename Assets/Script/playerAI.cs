using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class playerAI : MonoBehaviour {

	public GameObject playerCharacter;
	public GameObject Observe;

	private observeRange observeProp;
	private playerScript playerCont;
	public float playerSpeed;

	List<GameObject> bulletClones;
	List<Vector3> simPos;
	List<Vector3> bulletClonesPos;
	
	public float saftyBound;
	public int searchDepth;
	public int delayFrame;
	public int idleDelayFrame;
	public int move5Or9;
	public float[] moveScore;

	private Vector3[] MOVE = new Vector3[9];

	// Use this for initialization
	void Start () {
		observeProp = Observe.GetComponent<observeRange>();
		playerCont = playerCharacter.GetComponent<playerScript>();
		playerSpeed = playerCharacter.GetComponent<playerScript>().currentSpeed;
		bulletClones = new List<GameObject>();
		simPos = new List<Vector3>();
		bulletClonesPos = new List<Vector3>();
		moveScore = new float[9];
		// if (idleDelayFrame > delayFrame) delayFrame = idleDelayFrame;

		MOVE[0] = new Vector3(0, 0, 0);
		// MOVE[1] = new Vector3(playerSpeed*Time.fixedDeltaTime, 0, 0);
		// MOVE[2] = new Vector3(-playerSpeed*Time.fixedDeltaTime, 0, 0);
		// MOVE[3] = new Vector3(0, playerSpeed*Time.fixedDeltaTime, 0);
		// MOVE[4] = new Vector3(0, -playerSpeed*Time.fixedDeltaTime, 0);
		// Debug.Log(playerSpeed);
	}
	
	// Update is called once per frame
	//FIRST PLAYER MODEL
	void FixedUpdate () {
		if (playerCharacter == null) return;
		playerSpeed = playerCharacter.GetComponent<playerScript>().currentSpeed;
		float moveUnit = playerSpeed*Time.fixedDeltaTime;
		MOVE[1] = new Vector3(moveUnit, 0, 0);
		MOVE[2] = new Vector3(-moveUnit, 0, 0);
		MOVE[3] = new Vector3(0, moveUnit, 0);
		MOVE[4] = new Vector3(0, -moveUnit, 0);
		MOVE[5] = new Vector3(1, 1, 0).normalized * playerSpeed * Time.fixedDeltaTime;
		MOVE[6] = new Vector3(-1, 1, 0).normalized * playerSpeed * Time.fixedDeltaTime;
		MOVE[7] = new Vector3(1, -1, 0).normalized * playerSpeed * Time.fixedDeltaTime;
		MOVE[8] = new Vector3(-1, -1, 0).normalized * playerSpeed * Time.fixedDeltaTime;
		simulateAI(searchDepth, delayFrame);
		// AvoidantAI();
		// Debug.Log(playerSpeed);
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
		playerCont.PressShift();
		if (frameCount > 0) {
			frameCount--;
			if ( decidedMove == 0) playerCont.Stop();
			if ( decidedMove == 1) playerCont.GoRight();
			if ( decidedMove == 2) playerCont.GoLeft();
			if ( decidedMove == 3) playerCont.GoUp();
			if ( decidedMove == 4) playerCont.GoDown();
			if ( decidedMove == 5) playerCont.GoDiagUR();
			if ( decidedMove == 6) playerCont.GoDiagUL();
			if ( decidedMove == 7) playerCont.GoDiagDR();
			if ( decidedMove == 8) playerCont.GoDiagDL();
			return;
		}
		float fixedTime= Time.fixedDeltaTime;
		bulletClones.Clear();
		simPos.Clear();
		bulletClonesPos.Clear();
		moveScore = new float[9];
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
				for (int c = 0; c<move5Or9; c++) {
					simPos.Add(new Vector3(pos.x, pos.y, 0));
				}				
			}

			

			for (int r = 0; r < responseFrame ; r++) {
				
				for (int j = 0; j < bulletClones.Count ; j++) {
					bulletClonesPos[j] += bulletClones[j].transform.up * bulletClones[j].GetComponent<bullet>().speed * fixedTime;
				}
				for (int k = 0; k < simPos.Count; k++) {
					// bulletClonesPos[j] += bulletClones[j].transform.up * bulletClones[j].GetComponent<bullet>().speed * fixedTime;
					simPos[k] += MOVE[k%move5Or9];
					for (int j = 0; j < bulletClones.Count ; j++) {
						if (bulletClonesPos[j].x-saftyBound < simPos[k].x && bulletClonesPos[j].x+saftyBound > simPos[k].x &&
							bulletClonesPos[j].y-saftyBound < simPos[k].y && bulletClonesPos[j].y+saftyBound > simPos[k].y ) {
								// Debug.Log(k + " " + simPos[k].x + ":" + simPos[k].y+ " dead " + Mathf.FloorToInt(k/Mathf.Pow(move5Or9, i-1)) + " " + i + " " + r);
								// moveScore[Mathf.FloorToInt(k/Mathf.Pow(move5Or9, i-1))] -= 100/(i*responseFrame + r);
								moveScore[Mathf.FloorToInt(k/Mathf.Pow(move5Or9, i-1))] -= 100/(i);
								break;
						}
					}
				}
			}


		}
		observeProp.clear();
		int maxIndex = 0;
		for (int i = 1; i<move5Or9; i++) {
			if (moveScore[i] > moveScore[maxIndex]) maxIndex = i;
		}
		if (frameCount == 0) {
			if (maxIndex == 0) {
				frameCount = idleDelayFrame;
			}
			if (decidedMove != 0 && decidedMove != maxIndex) {
				maxIndex = 0;
				frameCount = idleDelayFrame;
			}
			decidedMove = maxIndex;
		}
		if ( decidedMove == 0) playerCont.Stop();
		if ( decidedMove == 1) playerCont.GoRight();
		if ( decidedMove == 2) playerCont.GoLeft();
		if ( decidedMove == 3) playerCont.GoUp();
		if ( decidedMove == 4) playerCont.GoDown();
		if ( decidedMove == 5) playerCont.GoDiagUR();
		if ( decidedMove == 6) playerCont.GoDiagUL();
		if ( decidedMove == 7) playerCont.GoDiagDR();
		if ( decidedMove == 8) playerCont.GoDiagDL();
		frameCount--;
		if (frameCount <= 0) frameCount = responseFrame;
	}

}
