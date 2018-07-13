﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EvolveLoopController : MonoBehaviour {

	public string managerFileName;
	public static EvolveLoopController instance;
	public GameObject envManager;
	public GameObject patternDetector;

	public struct GenerationMember {
		public int status;
		public string levelName;	//
		public float score;		//0-free, 1-working, 2-finish, 3-evolving
		public string resultFile;
	}

	public int populationSize;
	
	// private StreamReader reader;
	private StreamWriter writer;

	private GenerationMember[] member;
	public GenerationMember currentMember;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
		member = new GenerationMember[populationSize];
		currentMember = new GenerationMember();
		currentMember.score = -1.0f;
	}

	public void GetStatus() {
		StreamReader reader = new StreamReader(Application.dataPath + "/Level/" + managerFileName + ".txt");
		string delim = ",";
		string text = reader.ReadLine();
		string writeBackBuffer = "";
		int mcounter = 0;

		if (text.Split(delim.ToCharArray())[0] == "-2") {
			reader.Close();
			return;
		} else 
			for (int i = 0; i<populationSize ; i++) {
				string[] param;
				param = text.Split(delim.ToCharArray());

				member[mcounter].status = int.Parse(param[0]);
				member[mcounter].levelName = param[1];
				member[mcounter].score = float.Parse(param[2]);
				member[mcounter].resultFile = param[3];
				mcounter++;

				if (currentMember.score == -1.0f && param[0] == "0") {
					currentMember.status = int.Parse(param[0]);
					currentMember.levelName = param[1];
					currentMember.score = float.Parse(param[2]);
					currentMember.resultFile = param[3];
					writeBackBuffer += 1+",";
					writeBackBuffer += currentMember.levelName+",";
					writeBackBuffer += 0+",";
					writeBackBuffer += currentMember.resultFile;
					patternDetector.GetComponent<PatternDetector>().SetPatternName(param[3]);
				} else {
					writeBackBuffer += text;
				}
				writeBackBuffer += "\n";

				text = reader.ReadLine();
			}
		reader.Close();

		if (currentMember.score != -1.0f) {
			writer = new StreamWriter(Application.dataPath + "/Level/" + managerFileName + ".txt");
			writer.Write(writeBackBuffer);
			writer.Close();
		} else {
			writeBackBuffer = "-" + writeBackBuffer;
			writer = new StreamWriter(Application.dataPath + "/Level/" + managerFileName + ".txt");
			writer.Write(writeBackBuffer);
			writer.Close();
			envManager.GetComponent<EnvManager>().GeneticPause();
			Evolve();
			envManager.GetComponent<EnvManager>().GeneticContinue();
		}
	}

	public void WriteStatus() {
		StreamReader 	reader = new StreamReader(@"C:\Users\IkedaLab\Desktop\internship\2dGame\BH\Assets\Level\" + managerFileName + ".txt");
		string delim = ",";
		string text = reader.ReadLine();
		string writeBackBuffer = "";
		float score;
		for (int i = 0; i<populationSize ; i++) {
			string[] param;
			param = text.Split(delim.ToCharArray());

			if (currentMember.levelName == param[1]) {
				writeBackBuffer += 2+",";
				writeBackBuffer += currentMember.levelName+",";
				score = CalScore(currentMember.resultFile);
				writeBackBuffer += score+",";
				Debug.Log(score);
				writeBackBuffer += currentMember.resultFile;
			} else {
				writeBackBuffer += text;
			}
			writeBackBuffer += "\n";
			text = reader.ReadLine();
		}
		reader.Close();

		writer = new StreamWriter(@"C:\Users\IkedaLab\Desktop\internship\2dGame\BH\Assets\Level\" + managerFileName + ".txt");
		writer.Write(writeBackBuffer);
		writer.Close();
	}

	public float CalScore(string resultFileName) {
		StreamReader reader = new StreamReader(@"C:\Users\IkedaLab\Desktop\internship\2dGame\BH\Assets\Level\meso-pattern.txt");
		List<string> mesoPattern = new List<string>();
		string text = reader.ReadLine();
		while (text != null) {
			mesoPattern.Add(text);
			text = reader.ReadLine();
		}
		reader.Close();

		reader = new StreamReader(@"C:\Users\IkedaLab\Desktop\internship\2dGame\BH\Assets\Level\" + resultFileName + ".txt");
		List<string> pattern = new List<string>();
		text = reader.ReadLine();
		while (text != null) {
			pattern.Add(text);
			text = reader.ReadLine();
		}
		reader.Close();

		float counter = 0;

		foreach (string i in pattern) {
			foreach (string j in mesoPattern) {
				if (i.CompareTo(j) == 0)
					counter += 1.0f;
			}
		}
		return counter;
	}

	private void Evolve() {
		SortLevel();
		
	}

	public void SortLevel() {
		// Debug.Log("in");
		System.Array.Sort<GenerationMember>(member, (x, y) => y.score.CompareTo(x.score));
		for (int i = 0; i < populationSize; i++) {
			// int select = i;
			// for (int j = i+1; j < populationSize ; j++) {
			// 	if (member[select].score > member[j].score)
			// 		select = j;
			// }
			// GenerationMember temp = member[select];
			// member[select] = member[i];
			// member[i] = temp;
			Debug.Log(member[i].levelName);
			
		}
	}
	
}
