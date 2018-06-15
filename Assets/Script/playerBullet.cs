﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class playerBullet : MonoBehaviour {

	public float speed = 0;
	public float damage = 1.0f;

	// Use this for initialization
	void Start () {
		// GetComponent<Rigidbody>().velocity = transform.up * speed;
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
			other.GetComponent<BulletSpawner>().Damaged(damage);
		}
	}

}

class pMover : ComponentSystem {
	
	struct Components
	{
		public playerBullet bull;
		public Transform transform;
	}

	protected override void OnUpdate() {
		
		float deltatime = Time.deltaTime;
		
		foreach (var e in GetEntities<Components>()) {
				
			e.transform.position += e.transform.up * e.bull.speed * deltatime;
		}
	
	}

}