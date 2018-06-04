﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBullet : MonoBehaviour {

	public float speed = 0;

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody>().velocity = transform.up * speed;
	}
	
	// Update is called once per frame
	void Update () {
		// GetComponent<Transform>().position += GetComponent<Transform>().up*speed;
	}

	void OnTriggerExit(Collider other)
    {
		if (other.gameObject.tag == "screen") {
	        Destroy(this.gameObject);
		}
    }
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Enemy") {
			Destroy(other.gameObject);
		}
	}
}
