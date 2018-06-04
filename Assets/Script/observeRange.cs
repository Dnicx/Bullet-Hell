using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class observeRange : MonoBehaviour {

	public Vector3 weight;
	public bool danger;

	// Use this for initialization
	void Start () {
		weight = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "bullet") {
			// Debug.Log(Vector3.Distance(other.transform.position, transform.position));
			weight += (1/Vector3.Distance(other.transform.position, transform.position))*(transform.position - other.transform.position);
			danger = true;
		}
	}

	public void clear() {
		danger = false;
		weight = new Vector3(0, 0, 0);
	}
}
