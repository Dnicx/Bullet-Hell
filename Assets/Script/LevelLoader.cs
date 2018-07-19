using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LevelLoader : MonoBehaviour {
	
	public string levelName;
	public GameObject[] RedEnemies;

	public float spawnAhead;
	public Text levelUI;

	string text;
	StreamReader reader;
	List<string> levelText;

	public Vector2 density;

	private float timeSpawn;

	// Use this for initialization
	void Start () {
		levelText = new List<string>();
		EvolveLoopController.instance.GetStatus();
		levelName = EvolveLoopController.instance.currentMember.levelName;
		levelUI.text = levelName;
		if (levelName != null) {
			timeSpawn = 1;
			readFile();
			Spawn(1 + spawnAhead);
		}
	}
	
	public float timeCounter;
	// Update is called once per frame
	void Update () {
		if (levelText.Count == 0) return;
		timeCounter += Time.deltaTime;
		if (timeCounter > timeSpawn - 2) {
			Debug.Log("spawn until " + (timeSpawn + spawnAhead));
			Spawn(timeSpawn + spawnAhead);
			timeSpawn += spawnAhead;
		}
	}

	public string[] texts;
	public void Spawn(float spawnUntil) {
		// reader = new StreamReader("Assets/Level/"+levelName+".txt");
		text = levelText[0];
		float[] param = new float[2];
		while (text != null) {
			string delim = ",";
			texts = text.Split(delim.ToCharArray());
			param[0] = float.Parse(texts[0]);
			if (param[0] > spawnUntil) break;
			
			for (int i = 0; i<density.x; i++) {
				char group = texts[1][i];
				if (group == '-') continue;
				if (group > 'I') Debug.Log(group);
				GameObject currentGroup = Instantiate(RedEnemies[group - 'A']);
				param[1] = (i*2+1) * (EnvManager.maxBound.x - EnvManager.minBound.x)/(density.x*2);
				param[1] -= (EnvManager.maxBound.x - EnvManager.minBound.x)/2;
				// Debug.Log(param[1]);
				currentGroup.GetComponent<EnemyGroup>().SetParam(param);
				currentGroup.GetComponent<EnemyGroup>().SetEnvScript(GetComponent<EnvManager>());
			}
			levelText.RemoveAt(0);
			if (levelText.Count == 0) break;
			text = levelText[0];
		}
	}

	private void readFile() {
		reader = new StreamReader(Application.dataPath + "/Level/"+levelName+".txt");
		reader.ReadLine();		// ignore marker line
		text = reader.ReadLine();
		while (text != null) {
			levelText.Add(text);
			text = reader.ReadLine();
		}
		reader.Close();
	}
}
