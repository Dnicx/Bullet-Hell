using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTrigger : MonoBehaviour {

	public bool warn = false;

	void OnTriggerStay(Collider other) {
		if (other.tag == "bullet") {
			warn = true;
		}
	}
	
	void OnTriggerExit(Collider other) {
		warn = false;
	}
}
