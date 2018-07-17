using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class EvolveLoopController : MonoBehaviour {

	public string managerFileName;
	public static EvolveLoopController instance;
	public GameObject envManager;
	public GameObject patternDetector;

	public int gen;
	private int nextgen;

	public float playSpeed;

	public Text statusText;
	public Text generationText;

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
		gen = int.Parse(reader.ReadLine());
		generationText.text = "" + gen;
		nextgen = gen+1;
		string text = reader.ReadLine();
		string writeBackBuffer = gen + "\n";
		int mcounter = 0;
	
		if (text.Split(delim.ToCharArray())[0] == "-2") {
			reader.Close();
			envManager.GetComponent<EnvManager>().GeneticPause();
			statusText.text = "[o] evolving";
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
			statusText.text = "playing";
		} else {
			for (int i = 0; i<populationSize; i++) {
				if (member[i].status == 1) {
					envManager.GetComponent<EnvManager>().GeneticPause();
					statusText.text = "[o] finishing";
					return;
				}
			}
			writeBackBuffer = new string(writeBackBuffer.ToCharArray(), 0, 2) + "-" + new string(writeBackBuffer.ToCharArray(), 2, writeBackBuffer.Length-2);
			writer = new StreamWriter(Application.dataPath + "/Level/" + managerFileName + ".txt");
			writer.Write(writeBackBuffer);
			writer.Close();
			statusText.text = "Evolve";
			Evolve();
			envManager.GetComponent<EnvManager>().GeneticContinue();
		}

		Time.timeScale = playSpeed;
	}

	public void WriteStatus() {
		StreamReader reader = new StreamReader(Application.dataPath + "/Level/" + managerFileName + ".txt");
		string delim = ",";
		reader.ReadLine();
		string text = reader.ReadLine();
		string writeBackBuffer = gen + "\n";
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

		writer = new StreamWriter(Application.dataPath + "/Level/" + managerFileName + ".txt");
		writer.Write(writeBackBuffer);
		writer.Close();
	}

	public float CalScore(string resultFileName) {
		StreamReader reader = new StreamReader(Application.dataPath + "/Level/" + "meso-pattern.txt");
		List<string> mesoPattern = new List<string>();
		string text = reader.ReadLine();
		while (text != null) {
			mesoPattern.Add(text);
			text = reader.ReadLine();
		}
		reader.Close();

		reader = new StreamReader(Application.dataPath + "/Level/" + resultFileName + ".txt");
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
		RecordBestMem(member[0].levelName, member[0].score);
		Reproduction();
	}

	public void SortLevel() {
		System.Array.Sort<GenerationMember>(member, (x, y) => y.score.CompareTo(x.score));
	}

	public void RecordBestMem(string file, float score) {
		string best = "bestInGen"+gen;
		if (gen%10 == 0) {
		
			StreamReader reader = new StreamReader(Application.dataPath + "/Level/" + file + ".txt");
			string text = reader.ReadLine();
			string writeBackBuffer = "best is " + file + " : score " + score + "\n";
			while (text != null) {
				writeBackBuffer += text;
				writeBackBuffer += "\n";
				text = reader.ReadLine();
			}
			reader.Close();

			writer = new StreamWriter(Application.dataPath + "/Level/" + best + ".txt");
			writer.Write(writeBackBuffer);
			writer.Close();
		}
	}

	public void Reproduction() {

		StreamReader p1reader;
		StreamReader p2reader;
		string parentWrite = "";

		for (int i = 10; i<populationSize; i++) {
			
			int parent1 = Random.Range(0, 10);
			int parent2 = Random.Range(0, 10);
			while (parent2 == parent1) {
				parent2 = Random.Range(0, 10);
			}
			p1reader = new StreamReader(Application.dataPath + "/Level/" + member[parent1].levelName + ".txt");
			p2reader = new StreamReader(Application.dataPath + "/Level/" + member[parent2].levelName + ".txt");

			parentWrite += parent1 + " + " + parent2 + "\n";

			float cross1 = Random.Range(0.0f, 60.0f);
			float cross2 = Random.Range(0.0f, 60.0f);

			while (cross2 > cross1 ) cross2 = Random.Range(0.0f, 60.0f);
			string p1line = p1reader.ReadLine();
			string p2line = p2reader.ReadLine();
			string child = "";
			string delim = ",";
			bool isMutate = false;
			while (p1line != null) {
				
				string[] param;
				string currentLine = "";
				param = p1line.Split(delim.ToCharArray());
				if (float.Parse(param[0]) > cross2)  currentLine += p1line;
				else if (float.Parse(param[0]) > cross1) currentLine += p2line;
				else currentLine += p1line;

				if(!isMutate && Random.Range(0,1000) == 348) {
					isMutate = true;
					currentLine = param[0] + ",";
					int position = Random.Range(0, 7);
					char muteTo = '-';
					if (param[1][position] == '-') {
						int muteGene = Random.Range(0, 'I'-'A'+1);
						muteTo = (char)('A' + muteGene);
					}
					currentLine += new string(param[1].ToCharArray(), 0, position) + muteTo + new string(param[1].ToCharArray(), position+1, param[1].Length-position-1);
				}

				currentLine += "\n";

				child += currentLine;
				p1line = p1reader.ReadLine();
				p2line = p2reader.ReadLine();
			}
			p1reader.Close();
			p2reader.Close();
			
			writer = new StreamWriter(Application.dataPath + "/Level/" + member[i].levelName + ".txt");
			writer.Write(child);
			writer.Close();
			
		}
		writer = new StreamWriter(Application.dataPath + "/Level/parentIn" + gen + ".txt");
		writer.Write(parentWrite);
		writer.Close();

		StreamReader reader = new StreamReader(Application.dataPath + "/Level/" + managerFileName + ".txt");
		reader.ReadLine();
		string text = reader.ReadLine();
		string writeBackBuffer = nextgen + "\n";
		for (int i = 0; i<populationSize ; i++) {
			string[] param;
			param = text.Split(',');

			writeBackBuffer += 0+",";
			writeBackBuffer += param[1]+",";
			writeBackBuffer += 0+",";
			writeBackBuffer += param[3];
			writeBackBuffer += "\n";
			text = reader.ReadLine();
		}
		reader.Close();

		writer = new StreamWriter(Application.dataPath + "/Level/" + managerFileName + ".txt");
		writer.Write(writeBackBuffer);
		writer.Close();
	}
}
