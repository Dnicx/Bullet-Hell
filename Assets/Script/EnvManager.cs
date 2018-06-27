using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class EnvManager : MonoBehaviour {

	public GameObject screen;
	public GameObject playerPrefab;
	public GameObject enemyPrefab;
	public GameObject levelLoader;

	public GameObject player;
	public List<GameObject> Enemy;
	string text;
	StreamReader reader;
	StreamWriter writer;
	public bool record;
	public bool autoRestart;
	
	int winCount;
	int game;
	int Ecount;

	// Use this for initialization
	void Start () {
		reader = new StreamReader(@"C:\Users\IkedaLab\Desktop\internship\2dGame\BH\Assets\Level\winrateSimulate.txt");
		// reader = new StreamReader(@"C:\Users\IkedaLab\Desktop\internship\2dGame\BH\Assets\Level\winrateAvoid.txt");
		text = reader.ReadLine();
		winCount = int.Parse(text);
		text = reader.ReadLine();
		game = int.Parse(text);
		// while (text != null) {
		// 	string delim = ",";
		// 	string[] texts = text.Split(delim.ToCharArray());
		// 	param[0] = (float.Parse(texts[0]));
		// 	text = reader.ReadLine();
		// }
		reader.Close();
	}
	public float count;
	public float fixedCount;
	// Update is called once per frame
	void Update () {
		Ecount = 0;
		for (int i = 0; i < Enemy.Count; i++) {
			Ecount += Enemy[i]!=null?1:0;
		}
		if (player == null) {
			if (record) {
				writer = new StreamWriter(@"C:\Users\IkedaLab\Desktop\internship\2dGame\BH\Assets\Level\winrateSimulate.txt");
				// writer = new StreamWriter(@"C:\Users\IkedaLab\Desktop\internship\2dGame\BH\Assets\Level\winrateAvoid.txt");
				writer.WriteLine(winCount);
				writer.WriteLine(game+1);
				writer.Close();
			}
			if (game == 499 ) Application.Quit();
			if (autoRestart) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		if (Input.GetButtonDown("Restart")) {
			game++;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		if (Ecount == 0) {
			if (record) {
				writer = new StreamWriter(@"C:\Users\IkedaLab\Desktop\internship\2dGame\BH\Assets\Level\winrateSimulate.txt");
				// writer = new StreamWriter(@"C:\Users\IkedaLab\Desktop\internship\2dGame\BH\Assets\Level\winrateAvoid.txt");
				writer.WriteLine(winCount+1);
				writer.WriteLine(game+1);
				writer.Close();
			}
			if (game == 499) Application.Quit();
			if (autoRestart) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		

	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Enemy") {
			if (other.GetComponent<Enemy>() != null) other.GetComponent<Enemy>().setEnvManager(this);
		}
	}
}
