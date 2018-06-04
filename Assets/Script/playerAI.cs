using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAI : MonoBehaviour {

	public GameObject playerCharacter;
	public GameObject mTrigger;
	public GameObject rTrigger;
	public GameObject lTrigger;
	public GameObject rfTrigger;
	public GameObject lfTrigger;
	public GameObject Observe;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//playerCharacter.GetComponent<playerScript>().goUp();
		if (playerCharacter == null) return;
		// if (rTrigger.GetComponent<playerTrigger>().warn) playerCharacter.GetComponent<playerScript>().goLeft();
		// else if (lTrigger.GetComponent<playerTrigger>().warn) playerCharacter.GetComponent<playerScript>().goRight();
		// else if (rfTrigger.GetComponent<playerTrigger>().warn) playerCharacter.GetComponent<playerScript>().goUp();
		// else if (lfTrigger.GetComponent<playerTrigger>().warn) playerCharacter.GetComponent<playerScript>().goUp();
		// else if (mTrigger.GetComponent<playerTrigger>().warn) playerCharacter.GetComponent<playerScript>().goDown();
		// else playerCharacter.GetComponent<playerScript>().stop();
		playerCharacter.GetComponent<playerScript>().pressShift();
		playerCharacter.GetComponent<playerScript>().direction = Observe.GetComponent<observeRange>().weight;
		// Debug.Log(Observe.GetComponent<observeRange>().weight.magnitude);
		// if (Observe.GetComponent<observeRange>().weight.magnitude > 4) {
		// 	playerCharacter.GetComponent<playerScript>().liftShift();
		// 	playerCharacter.GetComponent<playerScript>().direction = new Vector3(playerCharacter.GetComponent<playerScript>().direction.y, -playerCharacter.GetComponent<playerScript>().direction.x, 0);
		// 	// Debug.Log("turn! + " + Observe.GetComponent<observeRange>().weight.magnitude);
		// }
		if (Observe.GetComponent<observeRange>().weight == Vector3.zero) 
			playerCharacter.GetComponent<playerScript>().direction = -playerCharacter.GetComponent<Transform>().position;
		Observe.GetComponent<observeRange>().clear();
	}
}
