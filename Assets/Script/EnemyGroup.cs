﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour {

	public string groupId;
	public Vector3 startPositionOffset;
	public float moveInOffset;
	public float memberSpacing;

	// public GameObject[] group;
	private List<Enemy> memberScript;
	public Vector3 averagePosition;

	// Use this for initialization
	void Start () {
		memberScript = new List<Enemy>();
		foreach (Transform member in transform) {
			// memberScript.Add(member.gameObject.GetComponent<Enemy>());
			member.gameObject.GetComponent<Enemy>().SetOffset(startPositionOffset);
			member.gameObject.GetComponent<Enemy>().SetMoveInOffset(moveInOffset);
			moveInOffset += memberSpacing;
		}
		averagePosition = new Vector3();
	}

	void FixedUpdate() {
		averagePosition = Vector3.zero;
		foreach (Transform member in transform) {
			averagePosition += member.position;
		}
		averagePosition /= transform.childCount;
	}

	public void SetStartOffset(Vector3 offset) {
		startPositionOffset = offset;
	}

	public void setParam(float[] param) {
		moveInOffset = param[0];
		startPositionOffset = new Vector3(param[1], startPositionOffset.y, startPositionOffset.z);

	}
}
