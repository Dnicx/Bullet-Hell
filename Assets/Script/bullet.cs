using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class bullet : MonoBehaviour {

	public float speed = 0;
	public bool polar;

	// Use this for initialization
	void Start () {
		// GetComponent<Rigidbody>().velocity = transform.up * speed;
	}
	
	// Update is called once per frame
	void Update () {
		// GetComponent<Transform>().position += GetComponent<Transform>().up*speed*Time.deltaTime;
	}

	void OnTriggerExit(Collider other)
    {
		if (other.gameObject.tag == "screen") {
	        Destroy(this.gameObject);
		}
    }
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			if (polar != other.GetComponent<playerScript>().polar) Destroy(other.gameObject);
		}
	}

}

class Mover : ComponentSystem {
	
	struct Components
	{
		public bullet bull;
		public Transform transform;
	}

	protected override void OnUpdate() {
		
		float deltatime = Time.deltaTime;
		foreach (var e in GetEntities<Components>()) {

			// e.transform.position += e.transform.up * e.bull.speed * deltatime * Time.timeScale;
			e.transform.position += e.transform.up * e.bull.speed * deltatime;
		}
	
	}

}