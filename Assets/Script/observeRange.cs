using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class observeRange : MonoBehaviour {

	public Vector3 weight;
	public bool danger;
	public List<GameObject> bullets;
	public List<GameObject> enemies;

	// Use this for initialization
	void Start () {
		weight = new Vector3(0, 0, 0);
		bullets = new List<GameObject>();
		enemies = new List<GameObject>();
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "bullet") {
			// Debug.Log(Vector3.Distance(other.transform.position, transform.position));
			weight += (1/Vector3.Distance(other.transform.position, transform.position))*(transform.position - other.transform.position);
			danger = true;
			if (!bullets.Contains(other.gameObject)) bullets.Add(other.gameObject);
		}
		if (other.tag == "Enemy") {
			if (!enemies.Contains(other.gameObject)) enemies.Add(other.gameObject);
		}
	}

	public void clear() {
		danger = false;
		weight = new Vector3(0, 0, 0);
		bullets.Clear();
		enemies.Clear();
	}
}
