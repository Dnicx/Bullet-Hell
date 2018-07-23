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
	public int generationChild;
	public int generationMutation;
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
	private GenerationMember[] family;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
		member = new GenerationMember[populationSize];
		family = new GenerationMember[generationChild+2];
		currentMember = new GenerationMember();
		currentMember.score = -1.0f;
	}

	public void GetStatus() {
		StreamReader reader = new StreamReader(Application.dataPath + "/Level/" + managerFileName + ".txt");
		if (reader == null) {
			statusText.text = "file occupy";
			System.Threading.Thread.Sleep(500);
			envManager.GetComponent<EnvManager>().GeneticContinue();
			return;
		}
		string delim = ",";
		gen = int.Parse(reader.ReadLine());
		generationText.text = "" + gen;
		nextgen = gen+1;
		string text = reader.ReadLine();
		string writeBackBuffer = gen + "\n";
		int mcounter = 0;
	
		if (text.Split(delim.ToCharArray())[0] == "-2") {
			reader.Close();
			statusText.text = "[o] evolving";
			System.Threading.Thread.Sleep(500);
			envManager.GetComponent<EnvManager>().GeneticContinue();
			return;
		} else {
			statusText.text = "iterating member";
			try {
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

				mcounter = 0;
				while (text != null) {
					string[] param;
					param = text.Split(delim.ToCharArray());					
					family[mcounter].status = int.Parse(param[0]);
					family[mcounter].levelName = param[1];
					family[mcounter].score = float.Parse(param[2]);
					family[mcounter].resultFile = param[3];
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
			} catch (System.Exception e) {
				statusText.text = "read member error" + e.Message;
				writeErrorLog("read member error at " + gen + "-" + currentMember.levelName + " : " + e);
				reader.Close();
				envManager.GetComponent<EnvManager>().GeneticContinue();
				return;
			}
			
		}
		reader.Close();

		statusText.text = currentMember.score + "to play or to wait";
		if (currentMember.score != -1.0f) {
			try {
				writer = new StreamWriter(Application.dataPath + "/Level/" + managerFileName + ".txt");
				writer.Write(writeBackBuffer);
				writer.Close();
				statusText.text = "playing";
			} catch (System.Exception e) {
				writer.Close();
				statusText.text = "write error:" + e.Message;
				writeErrorLog("write 1 error at " + gen + "-" + currentMember.levelName + " : " + e);
				System.Threading.Thread.Sleep(500);
				envManager.GetComponent<EnvManager>().GeneticContinue();
				return;
			}
		} else {
			try {
				for (int i = 0; i<populationSize; i++) {
					if (member[i].status == 1) {
						statusText.text = "[o] finishing";
						System.Threading.Thread.Sleep(2000);
						envManager.GetComponent<EnvManager>().GeneticContinue();
						return;
					}
				}
				for (int i = 0; i<generationChild; i++) {
					if (family[i].status == 1) {
						statusText.text = "[o] finishing";
						System.Threading.Thread.Sleep(2000);
						envManager.GetComponent<EnvManager>().GeneticContinue();
						return;
					}
				}
				statusText.text = "Write before evolve";
				writeBackBuffer = gen + "\n-";
				for (int i = 0; i<populationSize; i++) {
					writeBackBuffer += 2 + ",";
					writeBackBuffer += member[i].levelName + ",";
					writeBackBuffer += member[i].score + ",";
					writeBackBuffer += member[i].resultFile + "\n";
				}
			} catch (System.Exception e) {
				writeErrorLog("error before play at for " + gen + "-" + currentMember.levelName + " : " + e);
				envManager.GetComponent<EnvManager>().GeneticContinue();
			}

			try {
				writer = new StreamWriter(Application.dataPath + "/Level/" + managerFileName + ".txt");
				writer.Write(writeBackBuffer);
				writer.Close();
			} catch (System.Exception e) {
				writer.Close();
				writeErrorLog("write befor evolve " + gen + "-" + currentMember.levelName + " : " + e);
				envManager.GetComponent<EnvManager>().GeneticContinue();
				return;
			}
			
			try {
				statusText.text = "Evolve";
				Evolve();
				envManager.GetComponent<EnvManager>().GeneticContinue();
			} catch (System.Exception e) {
				writeErrorLog("error before play at evolve " + gen + "-" + currentMember.levelName + " : " + e);
				envManager.GetComponent<EnvManager>().GeneticContinue();
			}
		}

		Time.timeScale = playSpeed;
	}

	public void WriteStatus() {
		string writeBackBuffer = gen + "\n";
		string delim = ",";
		float score = CalScore(currentMember.resultFile);

		StreamReader reader = new StreamReader(Application.dataPath + "/Level/" + managerFileName + ".txt");
		reader.ReadLine();
		string text = reader.ReadLine();
		int c;
		c = (gen>0)?generationChild:0;
		for (int i = 0; i<populationSize+c ; i++) {
			string[] param;
			param = text.Split(delim.ToCharArray());

			if (currentMember.levelName == param[1]) {
				writeBackBuffer += 2+",";
				writeBackBuffer += currentMember.levelName+",";
				writeBackBuffer += score+",";
				writeBackBuffer += currentMember.resultFile;
			} else {
				writeBackBuffer += text;
			}
			writeBackBuffer += "\n";

			try {
				text = reader.ReadLine();
			} catch (System.Exception e) {
				writeErrorLog("error read finish at " + gen + "-" + currentMember.levelName + " : " + e);
				bool error = true;
				i = 0;
				reader.Close();
				reader = new StreamReader(Application.dataPath + "/Level/" + managerFileName + ".txt");
				reader.ReadLine();
				text = reader.ReadLine();
				writeBackBuffer = gen + "\n";
			}
		}
		reader.Close();

		try {
			writer = new StreamWriter(Application.dataPath + "/Level/" + managerFileName + ".txt");
			writer.Write(writeBackBuffer);
			writer.Close();
		} catch (System.Exception e) {
			statusText.text = "[w]　finish write error";
			writeErrorLog("Error Write Finish at " + gen + "-" + currentMember.levelName + " : " + e);
			writer.Close();
			System.Threading.Thread.Sleep(500);
			WriteStatus();
			writeErrorLog("out of loop");
		}
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

		int[] patternCount = new int[mesoPattern.Count];
		reader = new StreamReader(Application.dataPath + "/Level/" + "patternCount" + ".txt");
		try {
			text = reader.ReadLine();
			int pc = 0;
			while (text != null) {
				patternCount[pc] = int.Parse(text);
				text = reader.ReadLine();
				pc++;
			}
			reader.Close();
		} catch (System.Exception e) {
			reader.Close();
			envManager.GetComponent<EnvManager>().GeneticContinue();
		}

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
			int pc = 0;
			foreach (string j in mesoPattern) {
				if (i.CompareTo(j) == 0) {
					Debug.Log(i);
					counter += 1.0f;
					patternCount[pc]++;
				}
				pc++;
			}
		}

		StreamWriter writer = new StreamWriter(Application.dataPath + "/Level/" + "patternCount" + ".txt");
		for (int i = 0; i<mesoPattern.Count; i++) {
			try {
				writer.WriteLine(patternCount[i]);
			} catch (System.Exception e) {
				writer.Close();
				System.Threading.Thread.Sleep(500);
				writer = new StreamWriter(Application.dataPath + "/Level/" + "patternCount" + ".txt");
				i = 0;
			}
		}
		writer.Close();

		return counter;
	}

	private void Evolve() {
		// SortLevel();
		// Reproduction(5);
		if (gen > 0) selectFamily();
		RecordMem();
		RecordPatternCount();
		ReproductionMGG(5, generationChild, generationMutation);
		SortLevel();
		RecordBestMem(member[0].levelName, member[0].score);
	}

	public void SortLevel() {
		System.Array.Sort<GenerationMember>(member, (x, y) => y.score.CompareTo(x.score));
	}

	public void RecordBestMem(string file, float score) {
		string best = "bestInGen"+gen;
		if (gen%50 == 0) {
		
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

	public void RecordMem() {
		float scoreSum = 0;
		int div = 0;
		int diffCount = 0;
		string m1line, m2line;
		float maxScore = 0;
		for (int i = 0; i<populationSize; i++) {
			if (member[i].score > maxScore) maxScore = member[i].score;
			scoreSum += member[i].score;
			for (int j = i+1; j<populationSize; j++) {
				div++;
				StreamReader m1 = new StreamReader(Application.dataPath + "/Level/" + member[i].levelName + ".txt");
				StreamReader m2 = new StreamReader(Application.dataPath + "/Level/" + member[j].levelName + ".txt");
				m1line = m1.ReadLine();
				m2line = m2.ReadLine();

				while (m1line != null && m2line != null) {
					if (m1line.CompareTo(m2line) != 0) diffCount++;
					m1line = m1.ReadLine();
					m2line = m2.ReadLine();
				}
				m1.Close();
				m2.Close();
			}
		}

		string writeBackBuffer = "gen " + gen + " best " + maxScore + " average " + scoreSum/populationSize + " avg. difference " + diffCount/div ;
		writer = new StreamWriter(Application.dataPath + "/Level/record.txt",true);
		writer.WriteLine(writeBackBuffer);
		writer.Close();
	}

	public void RecordPatternCount() {
		string writeBack = "Gen " + gen + " patterns count ";
		List<int> patternCount = new List<int>();
		StreamReader reader = new StreamReader(Application.dataPath + "/Level/" + "patternCount" + ".txt");
		string text = reader.ReadLine();
		int pc = 0;
		while (text != null) {
			pc++;
			patternCount.Add(int.Parse(text));
			writeBack += " p" + pc + " " + text;
			text = reader.ReadLine();
		}
		reader.Close();

		StreamWriter writer = new StreamWriter(Application.dataPath + "/Level/" + "patternCount" + ".txt");
		for (int i = 0; i<patternCount.Count; i++) writer.WriteLine("0");
		writer.Close();

		writer = new StreamWriter(Application.dataPath + "/Level/" + "RecordPatternCount" + ".txt", true);
		writer.WriteLine(writeBack);
		writer.Close();
	}

	public void Reproduction(int nCrossPoint) {

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

			parentWrite += member[parent1].levelName + " + " + member[parent2].levelName;

			float[] crossPoint = new float[nCrossPoint];
			for (int cp = 0; cp < nCrossPoint; cp++) crossPoint[cp] = Random.Range(0.0f, 60.0f);
			System.Array.Sort<float>(crossPoint, (x, y) => x.CompareTo(y));
			
			string p1line = p1reader.ReadLine();
			string p2line = p2reader.ReadLine();
			string child = "";
			string delim = ",";
			bool isMutate = false;
			int pointPass = 0;
			while (p1line != null) {
				
				string[] param;
				string currentLine = "";
				string[] timeParam = p1line.Split(delim.ToCharArray());
				param = p1line.Split(delim.ToCharArray());
				
				if (pointPass % 2 == 0) {
					 currentLine += p1line;
				}
				else {
					currentLine += p2line;
					param = p2line.Split(delim.ToCharArray());
				}

				if (float.Parse(timeParam[0]) > crossPoint[pointPass] && pointPass < nCrossPoint-1) pointPass++; 

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
			parentWrite += " = " + member[i].levelName + "\n";
			
		}
		writer = new StreamWriter(Application.dataPath + "/Level/parentIn" + gen + ".txt");
		writer.Write(parentWrite);
		writer.Close();


		StepNextGen();
	}

	public void ReproductionMGG(int nCrossPoint, int C, int M) {

		StreamReader p1reader;
		StreamReader p2reader;
		string parentWrite = "";
		int parent1 = Random.Range(0, populationSize);
		int parent2 = Random.Range(0, populationSize);
		while (parent2 == parent1) {
			parent2 = Random.Range(0, populationSize);
		}
		family[6].resultFile = member[parent2].resultFile;
		parentWrite += member[parent1].levelName + " + " + member[parent2].levelName;
		StreamWriter parentWriter = new StreamWriter(Application.dataPath + "/Level/" + "parent" + ".txt");
		parentWriter.WriteLine(parent1);
		parentWriter.WriteLine(parent2);
		parentWriter.Close();


		for (int i = 0; i<C; i++) {
			
			p1reader = new StreamReader(Application.dataPath + "/Level/" + member[parent1].levelName + ".txt");
			p2reader = new StreamReader(Application.dataPath + "/Level/" + member[parent2].levelName + ".txt");

			float[] crossPoint = new float[nCrossPoint];
			for (int cp = 0; cp < nCrossPoint; cp++) crossPoint[cp] = Random.Range(0.0f, 60.0f);
			System.Array.Sort<float>(crossPoint, (x, y) => x.CompareTo(y));
			
			string p1line = p1reader.ReadLine();
			string p2line = p2reader.ReadLine();
			string child = "";
			string delim = ",";
			int pointPass = 0;
			int lineCount = 0;
			int[] mutateLine = new int[M];
			for (int mut = 0; mut < M; mut++) mutateLine[mut] = Random.Range(0, 240);
			while (p1line != null) {
				
				string[] param;
				string currentLine = "";
				string[] timeParam = p1line.Split(delim.ToCharArray());
				param = p1line.Split(delim.ToCharArray());
				
				if (pointPass % 2 == 0) {
					 currentLine += p1line;
				}
				else {
					currentLine += p2line;
					param = p2line.Split(delim.ToCharArray());
				}

				if (float.Parse(timeParam[0]) > crossPoint[pointPass] && pointPass < nCrossPoint-1) pointPass++; 

				foreach (int mut in mutateLine) {
					if (mut == lineCount) {
						currentLine = param[0] + ",";
						int position = Random.Range(0, 7);
						char muteTo = '-';
						if (param[1][position] == '-') {
							int muteGene = Random.Range(0, 'I'-'A'+1);
							muteTo = (char)('A' + muteGene);
						}
						currentLine += new string(param[1].ToCharArray(), 0, position) + muteTo + new string(param[1].ToCharArray(), position+1, param[1].Length-position-1);
					}
				}

				currentLine += "\n";

				child += currentLine;
				lineCount++;
				p1line = p1reader.ReadLine();
				p2line = p2reader.ReadLine();
			}
			p1reader.Close();
			p2reader.Close();
			
			writer = new StreamWriter(Application.dataPath + "/Level/childTemp" + i + ".txt");
			writer.Write(child);
			writer.Close();
			
		}
		writer = new StreamWriter(Application.dataPath + "/Level/parentIn" + gen + ".txt");
		writer.Write(parentWrite);
		writer.Close();


		StepNextGenMGG(C);
	}

	public void selectFamily() {
		StreamReader reader = new StreamReader(Application.dataPath + "/level/" + "parent.txt");
		int p1 = int.Parse(reader.ReadLine());
		int p2 = int.Parse(reader.ReadLine());
		reader.Close();

		family[5].status = member[p1].status;
		family[5].levelName = member[p1].levelName;
		family[5].score = member[p1].score;
		family[5].resultFile = member[p1].resultFile;
		family[6].status = member[p2].status;
		family[6].levelName = member[p2].levelName;
		family[6].score = member[p2].score;
		family[6].resultFile = member[p2].resultFile;

		System.Array.Sort<GenerationMember>(family, (x, y) => y.score.CompareTo(x.score));

		reader = new StreamReader(Application.dataPath + "/level/" + family[0].levelName + ".txt");
		List<string> s1 = new List<string>();
		string text = reader.ReadLine();
		while (text != null) {
			s1.Add(text);
			text = reader.ReadLine();
		}
		reader.Close();

		reader = new StreamReader(Application.dataPath + "/level/" + family[1].levelName + ".txt");
		List<string> s2 = new List<string>();
		text = reader.ReadLine();
		while (text != null) {
			s2.Add(text);
			text = reader.ReadLine();
		}
		reader.Close();

		StreamWriter writer = new StreamWriter(Application.dataPath + "/level/" + member[p1].levelName + ".txt");
		foreach(string s in s1) {
			writer.WriteLine(s);
		}
		writer.Close();

		writer = new StreamWriter(Application.dataPath + "/level/" + member[p2].levelName + ".txt");
		foreach(string s in s2) {
			writer.WriteLine(s);
		}
		writer.Close();
		
		writer = new StreamWriter(Application.dataPath + "/Level/parentIn" + (gen-1) + ".txt", true);
		writer.WriteLine("select : " + family[0].levelName + " and " + family[1].levelName);
		writer.Close();

		member[p1].score = family[0].score;
		member[p1].status = family[0].status;
		member[p2].score = family[1].score;
		member[p2].status = family[1].status;

	}

	public void StepNextGenMGG(int C) {
		string writeBackBuffer = nextgen + "\n";
		for (int i = 0; i<populationSize ; i++) {
			if (member[i].status == -2) member[i].status = 2;
			writeBackBuffer += member[i].status+",";
			writeBackBuffer += member[i].levelName+",";
			writeBackBuffer += member[i].score+",";
			writeBackBuffer += member[i].resultFile;
			writeBackBuffer += "\n";
		}

		for (int i = 0; i<C; i++) {
			writeBackBuffer += "0,childTemp"+i+",0,pattern_child"+i+"\n";
		}
		

		writer = new StreamWriter(Application.dataPath + "/Level/" + managerFileName + ".txt");
		bool error = true;
		try {
			writer.Write(writeBackBuffer);
		} catch (System.Exception e) {
			writeErrorLog("error write step");
			while (error) {
				writer.Close();
				System.Threading.Thread.Sleep(500);
				writer = new StreamWriter(Application.dataPath + "/Level/" + managerFileName + ".txt");
				writer.Write(writeBackBuffer);
				error = false;
			}
			writeErrorLog("out of loop write step");	
		}
		writer.Close();
	}

	public void StepNextGen() {
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
			try {
				text = reader.ReadLine();
			} catch (System.Exception e) {
				writeErrorLog("error read step");
				reader.Close();
				reader = new StreamReader(Application.dataPath + "/Level/" + managerFileName + ".txt");
				reader.ReadLine();
				text = reader.ReadLine();
				writeBackBuffer = nextgen + "\n";
				i = 0;
			}
		}
		reader.Close();
		

		writer = new StreamWriter(Application.dataPath + "/Level/" + managerFileName + ".txt");
		bool error = true;
		try {
			writer.Write(writeBackBuffer);
		} catch (System.Exception e) {
			writeErrorLog("error write step");
			while (error) {
				writer.Close();
				System.Threading.Thread.Sleep(500);
				writer = new StreamWriter(Application.dataPath + "/Level/" + managerFileName + ".txt");
				writer.Write(writeBackBuffer);
				error = false;
			}
			writeErrorLog("out of loop write step");	
		}
		writer.Close();
	}


	public void writeErrorLog(string message) {
		writer = new StreamWriter(Application.dataPath + "/ErrorLog.txt", true);
		writer.WriteLine(Time.time + " Error : " + message);
		writer.Close();
	}
}
