using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour {

	public string groupId;
	public Vector3 startPositionOffset;
	public float moveInOffset;
	public float memberSpacing;

	// public GameObject[] group;
	private List<Enemy> memberScript;

	// Use this for initialization
	void Start () {
		memberScript = new List<Enemy>();
		foreach (Transform member in transform) {
			// memberScript.Add(member.gameObject.GetComponent<Enemy>());
			member.gameObject.GetComponent<Enemy>().SetOffset(startPositionOffset);
			member.gameObject.GetComponent<Enemy>().SetMoveInOffset(moveInOffset);
			moveInOffset += memberSpacing;
		}
	}

	public void SetStartOffset(Vector3 offset) {
		startPositionOffset = offset;
	}
}
