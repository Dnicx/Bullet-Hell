using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EvolveLoopController : MonoBehaviour {

	public string managerFileName;
	public static EvolveLoopController instance;

	public struct GenerationMember {
		public int status;
		public string levelName;	//
		public float score;		//0-free, 1-working, 2-finish
		public string resultFile;
	}

	public int populationSize;
	
	private StreamReader reader;
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
		reader = new StreamReader(@"C:\Users\IkedaLab\Desktop\internship\2dGame\BH\Assets\Level\" + managerFileName + ".txt");
		string delim = ",";
		string text = reader.ReadLine();
		int counter = 0;
		string writeBackBuffer = "";
		for (int i = 0; i<populationSize ; i++) {
			string[] param;
			param = text.Split(delim.ToCharArray());

			if (currentMember.score == -1.0f && param[0] == "0") {
				currentMember.status = int.Parse(param[0]);
				currentMember.levelName = param[1];
				currentMember.score = float.Parse(param[2]);
				currentMember.resultFile = param[3];
				writeBackBuffer += 1+",";
				writeBackBuffer += currentMember.levelName+",";
				writeBackBuffer += 0+",";
				writeBackBuffer += currentMember.resultFile;
			} else {
				writeBackBuffer += text;
			}
			writeBackBuffer += "\n";


			counter++;
			text = reader.ReadLine();
		}
		reader.Close();

		writer = new StreamWriter(@"C:\Users\IkedaLab\Desktop\internship\2dGame\BH\Assets\Level\" + managerFileName + ".txt");
		writer.Write(writeBackBuffer);
		writer.Close();
	}

	public void WriteStatus() {
		reader = new StreamReader(@"C:\Users\IkedaLab\Desktop\internship\2dGame\BH\Assets\Level\" + managerFileName + ".txt");
		string delim = ",";
		string text = reader.ReadLine();
		int counter = 0;
		string writeBackBuffer = "";
		float score;
		for (int i = 0; i<populationSize ; i++) {
			string[] param;
			param = text.Split(delim.ToCharArray());

			if (currentMember.levelNmae == param[1]) {
				writeBackBuffer += 2+",";
				writeBackBuffer += currentMember.levelName+",";
				score = CalScore(currentMember.resultFile);
				writeBackBuffer += score+",";
				writeBackBuffer += currentMember.resultFile;
			} else {
				writeBackBuffer += text;
			}
			writeBackBuffer += "\n";
			counter++;
			text = reader.ReadLine();
		}
		reader.Close();

		writer = new StreamWriter(@"C:\Users\IkedaLab\Desktop\internship\2dGame\BH\Assets\Level\" + managerFileName + ".txt");
		writer.Write(writeBackBuffer);
		writer.Close();
	}

	public CalScore(string resultFileName) {
		
	}
	
}
