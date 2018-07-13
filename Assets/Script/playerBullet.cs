using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class playerBullet : MonoBehaviour {

	public float speed = 0;
	public float damage = 1.0f;
	public bool polar;

	// Use this for initialization
	void Start () {
		// GetComponent<Rigidbody>().velocity = transform.up * speed;
	}
	
	// Update is called once per frame
	void Update () {
		// GetComponent<Transform>().position += GetComponent<Transform>().up*speed;
	}

	void FixedUpdate() {
		transform.position += transform.up * speed * Time.deltaTime;
	}

	void OnTriggerExit(Collider other)
    {
		if (other.gameObject.tag == "screen") {
	        Destroy(this.gameObject);
		}
    }
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Enemy") {
			// if (other.GetComponent<BulletSpawner>() != null ) {
			// 	if(other.GetComponent<BulletSpawner>().polar != polar) other.GetComponent<BulletSpawner>().Damaged(2*damage);
			// 	else other.GetComponent<BulletSpawner>().Damaged(damage);
			// } else {
			Destroy(this.gameObject);
			if (other.GetComponent<Enemy>() == null) return;
			if(other.GetComponent<Enemy>().polar != polar) other.GetComponent<Enemy>().Damaged(2*damage);
			else other.GetComponent<Enemy>().Damaged(damage);
			// }
		}
	}

}

// class pMover : ComponentSystem {
	
// 	struct Components
// 	{
// 		public playerBullet bull;
// 		public Transform transform;
// 	}

// 	protected override void OnUpdate() {
		
// 		float deltatime = Time.deltaTime;
		
// 		foreach (var e in GetEntities<Components>()) {
				
// 			e.transform.position += e.transform.up * e.bull.speed * deltatime;
// 		}
	
// 	}

// }