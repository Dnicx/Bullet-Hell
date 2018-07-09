using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelLoader : MonoBehaviour {

	public GameObject[] RedEnemies;

	string text;
	public GameObject enemy;
	StreamReader reader;

	public Vector2 density;

	// Use this for initialization
	void Start () {
		Spawn();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public string[] texts;
	public void Spawn() {
		reader = new StreamReader("Assets/Level/level.txt");
		text = reader.ReadLine();		// ignore marker line
		text = reader.ReadLine();
		float[] param = new float[2];
		while (text != null) {
			string delim = ",";
			texts = text.Split(delim.ToCharArray());
			param[0] = float.Parse(texts[0]);
			
			for (int i = 0; i<density.x; i++) {
				char group = texts[1][i];
				if (group == ' ') continue;
				GameObject currentGroup = Instantiate(RedEnemies[group - 'A']);
				param[1] = (i*2+1) * (EnvManager.maxBound.x - EnvManager.minBound.x)/(density.x*2);
				param[1] -= (EnvManager.maxBound.x - EnvManager.minBound.x)/2;
				// Debug.Log(param[1]);
				currentGroup.GetComponent<EnemyGroup>().setParam(param);
			}

			
			text = reader.ReadLine();
		}
		reader.Close();
	}
}
