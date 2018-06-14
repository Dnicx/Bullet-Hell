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

	private observeRange observeProp;
	private playerScript playerCont;

	// Use this for initialization
	void Start () {
		observeProp = Observe.GetComponent<observeRange>();
		playerCont = playerCharacter.GetComponent<playerScript>();
	}
	
	// Update is called once per frame
	//FIRST PLAYER MODEL
	void FixedUpdate () {
		simulateAI(5);
		AvoidantAI();
		
	}

	
	void AvoidantAI() {
		if (playerCharacter == null) return;
		playerCont.pressShift();
		playerCont.direction = observeProp.weight;
		if (observeProp.weight == Vector3.zero) 
			playerCont.direction = -playerCharacter.GetComponent<Transform>().position;
		observeProp.clear();
	}

	void simulateAI(int iteration) {
		if (playerCharacter == null) return;
		for (int i = iteration; i>0; i--) {
			foreach (GameObject bullet in observeProp.bullets) {
				
			}
		}
	}

}
