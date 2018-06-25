using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelLoader : MonoBehaviour {

	public GameObject[] Enemies;

	string text;
	public GameObject enemy;
	StreamReader reader;

	// Use this for initialization
	void Start () {
		Spawn();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Spawn() {
		reader = new StreamReader("Assets/Level/level.txt");
		text = reader.ReadLine();
		while (text != null) {
			// foreach (string line in text) {
				// Debug.Log(text);
			// }
			string delim = ",";
			string[] texts = text.Split(delim.ToCharArray());
			float[] param = new float[13];
			for (int i = 0; i < texts.Length; i++) {
				Debug.Log(i);
				param[i] = (float.Parse(texts[i]));
			}
			GameObject currentEnemy = Instantiate(enemy);
			currentEnemy.GetComponent<BulletSpawner>().setValues(param);
			text = reader.ReadLine();
		}
		reader.Close();
	}
}
