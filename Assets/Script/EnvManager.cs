using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class EnvManager : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject enemyPrefab;

	public GameObject player;
	public int life;
	public List<GameObject> enemies;
	string text;
	StreamReader reader;
	StreamWriter writer;
	public bool record;
	public bool autoRestart;
	public int endTime;
	public float timeCount;

	public static Vector3 minBound;
	public static Vector3 maxBound;
	private int playerInvinsible;
	private bool pause;

	public static float INERTIA = -2;
	
	int winCount;
	int game;
	int Ecount;

	// Use this for initialization
	void Start () {
		reader = new StreamReader(@"C:\Users\IkedaLab\Desktop\internship\2dGame\BH\Assets\Level\winrateSimulateFire.txt");
		// reader = new StreamReader(@"C:\Users\IkedaLab\Desktop\internship\2dGame\BH\Assets\Level\winrateAvoid.txt");
		// reader = new StreamReader(@"C:\Users\IkedaLab\Desktop\internship\2dGame\BH\Assets\Level\winrateSimulate.txt");
		text = reader.ReadLine();
		winCount = int.Parse(text);
		text = reader.ReadLine();
		game = int.Parse(text);
		reader.Close();
		timeCount = 0;

		minBound = GetComponent<Collider>().bounds.min;
		maxBound = GetComponent<Collider>().bounds.max;

		playerInvinsible = 0;
	}
	public float count;
	public float fixedCount;
	// Update is called once per frame
	void Update () {
		Ecount = 0;
		for (int i = 0; i < enemies.Count; i++) {
			if(enemies[i]==null) enemies.RemoveAt(i);
		}
		// if (player == null) {
		// 	if (life > 0) return;
		// 	if (record) {
		// 		writer = new StreamWriter(@"C:\Users\IkedaLab\Desktop\internship\2dGame\BH\Assets\Level\winrateSimulateFire.txt");
		// 		// writer = new StreamWriter(@"C:\Users\IkedaLab\Desktop\internship\2dGame\BH\Assets\Level\winrateAvoid.txt");
		// 		// writer = new StreamWriter(@"C:\Users\IkedaLab\Desktop\internship\2dGame\BH\Assets\Level\winrateSimulate.txt");
		// 		writer.WriteLine(winCount);
		// 		writer.WriteLine(game+1);
		// 		writer.Close();
		// 	}
		// 	if (game == 499 ) Application.Quit();
		// 	if (autoRestart) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		// }
		if (Input.GetButtonDown("Restart")) {
			game++;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		
		if (timeCount > endTime) {
			if (record) {
				writer = new StreamWriter(@"C:\Users\IkedaLab\Desktop\internship\2dGame\BH\Assets\Level\winrateSimulateFire.txt");
				// writer = new StreamWriter(@"C:\Users\IkedaLab\Desktop\internship\2dGame\BH\Assets\Level\winrateAvoid.txt");
				// writer = new StreamWriter(@"C:\Users\IkedaLab\Desktop\internship\2dGame\BH\Assets\Level\winrateSimulate.txt");
				writer.WriteLine(winCount+1);
				writer.WriteLine(game+1);
				writer.Close();
			}
			// if (game == 499) Application.Quit();
			EvolveLoopController.instance.WriteStatus();
			if (autoRestart && !pause) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		} else {
			timeCount += Time.deltaTime;
		}

		if (playerInvinsible > 0) {
			playerInvinsible--;
		}
		if (playerInvinsible == 0) {
			playerInvinsible = -1;
			player.GetComponent<Collider>().enabled = true;
		}

	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Enemy") {
			if (other.GetComponent<Enemy>() != null) other.GetComponent<Enemy>().SetEnvManager(this);
			if (!enemies.Contains(other.gameObject)) enemies.Add(other.gameObject);
		}
		// if (other.tag == "Player") {
		// 	other.GetComponent<playerScript>().SetEnvManager(this);
		// }
	}

	public void SpawnPlayer() {
		if (life <= 0) return;
		if (life > 0) 
			life--;
		playerInvinsible = 120;
		player = Instantiate(playerPrefab, new Vector3(0, -3, 0), GetComponent<Transform>().rotation);
		player.GetComponent<Collider>().enabled = false;
		player.GetComponent<playerScript>().SetEnvManager(this.gameObject);
	}

	public float GetGameTime() {
		return timeCount;
	}

	public void GeneticPause() {
		pause = true;
	}

	public void GeneticContinue() {
		pause = false;
	}
}
