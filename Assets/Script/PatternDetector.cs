using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PatternDetector : MonoBehaviour {

	[Range(1, 2)] public int dimension;
	public Vector2Int density;
	public GameObject EnvManager;
	public List<string>[] screen;
	public List<GameObject>[] groupScreen;
	
	private Vector3 boundMin;
	private Vector3 boundMax;
	private Vector3 boundCenter;
	StreamWriter writer;
	public string buffer;

	// Use this for initialization
	void Start () {
		boundMax = EnvManager.GetComponent<Collider>().bounds.max;
		boundMin = EnvManager.GetComponent<Collider>().bounds.min;
		boundCenter = EnvManager.GetComponent<Collider>().bounds.center;
		writer = new StreamWriter(@"C:\Users\IkedaLab\Desktop\internship\2dGame\BH\Assets\Level\output.txt");
		writer.Write("");
		writer.Close();
	}
	
	private float counter;

	void FixedUpdate () {
		counter += Time.deltaTime;
		if (counter >= 0.5) {
			counter = 0;

			UpdatePattern();

		}
	}

	private int counter2;

	void UpdatePattern() {
		buffer = "";
		Vector3 offset = boundMax - boundCenter;
		Vector3 temp;
		counter2++;
		screen = new List<string>[density.x];
		groupScreen = new List<GameObject>[density.x];
		for (int i = 0 ; i < density.x ; i++)  {
			groupScreen[i] = new List<GameObject>();
			screen[i] = new List<string>();
		}
		foreach (GameObject enemy in EnvManager.GetComponent<EnvManager>().enemies) {
			if (enemy == null) continue;
			if (enemy.transform.parent == null) continue;
			
			temp = enemy.GetComponent<Transform>().position;
			temp += new Vector3(offset.x, 0, 0);
			
			int slot = Mathf.FloorToInt(temp.x*density.x/(boundMax-boundMin).x);
			if (slot == density.x) slot-=1;
			if (slot > density.x) continue;
			if (slot < 0) continue;
			// Debug.Log(slot);
			GameObject group = enemy.GetComponent<Transform>().parent.gameObject;
			if (group.GetComponent<EnemyGroup>() != null) 
			if (!groupScreen[slot].Contains(group)) {
				groupScreen[slot].Add(group);
				screen[slot].Add(group.GetComponent<EnemyGroup>().groupId);
			}
		}

		if (counter2 > 10) {
			// writer = new StreamWriter(@"C:\Users\IkedaLab\Desktop\internship\2dGame\BH\Assets\Level\output.txt");
			// writer.Write(buffer);
			// writer.Close();
		}
		else {
			for (int i = 0 ; i < density.x ; i++) {
				if (screen[i].Count > 0) {
					buffer += screen[i][0];
					// writer.Write(screen[i][0]);
				}
				else {
					buffer += "-";
					// writer.Write("-");
				}
			}
			buffer += "\n";
			// writer.Write("\n");

			writer = new StreamWriter(@"C:\Users\IkedaLab\Desktop\internship\2dGame\BH\Assets\Level\output.txt",true);
			writer.Write(buffer);
			writer.Close();
		}
	}

}
