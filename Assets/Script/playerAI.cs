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
	//FIRST PLAYER MODEL
	void FixedUpdate () {
		AvoidantAI();
	}

	void AvoidantAI() {
		if (playerCharacter == null) return;
		playerCharacter.GetComponent<playerScript>().pressShift();
		playerCharacter.GetComponent<playerScript>().direction = Observe.GetComponent<observeRange>().weight;
		if (Observe.GetComponent<observeRange>().weight == Vector3.zero) 
			playerCharacter.GetComponent<playerScript>().direction = -playerCharacter.GetComponent<Transform>().position;
		Observe.GetComponent<observeRange>().clear();
	}

	void simulateAI() {
		
	}

}
