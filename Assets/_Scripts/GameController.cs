using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public GameObject[] hazard = new GameObject[3];
	public Vector3 spawnValues;
	public GUIText scoreText;
	public GUIText restartText;
	public GUIText gameOverText;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;

	private int level;
	private int[] levelUpArray = new int[] {500, 1000, 1500, 2500, 5000, 10000};
	private float score;
	private bool gameOver;
	private bool restart;

	private Dictionary<string, int[]> enemyGeneration = new Dictionary<string, int[]>(){
		{"0", new int[] {0, 30, 60, 90, 100}},
		{"1", new int[] {0, 25, 50, 75, 100}},
		{"2", new int[] {0, 23, 46, 69, 99, 100}},
		{"3", new int[] {0, 22, 44, 65, 95, 99, 100}},
		{"4", new int[] {0, 15, 30, 45, 90, 95, 100}},
		{"5", new int[] {0, 10, 20, 30, 80, 95, 100}},
		{"6", new int[] {0, 10, 20, 25, 75, 90, 100}},
		{"asteroidsField", new int[] {0, 30, 60, 90, 90, 100, 100}},
		{"enemyBase", new int[] {0, 0, 0, 0, 95, 95, 100}}
	};

	int chooseSpawn() {
		int [] minMaxValues = enemyGeneration[level.ToString()];
		int chosen = Random.Range(0, 100);
		for(int i = 0; i < minMaxValues.Length - 1; i++) {
			if(chosen >= minMaxValues[i] && chosen < minMaxValues[i + 1])
				return i;
		}
		Debug.Log("Out of loop!");
		return 0;
	}

	void Start() {
		score = 0;
		level = 0;
		gameOver = false;
		restart = false;
		restartText.text = "";
		gameOverText.text = "";
		UpdateScore();
		StartCoroutine(GenerateWave());
	}

	void Update() {
		if(restart && Input.GetKeyDown(KeyCode.R)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		if(score > levelUpArray[level] && level != 6) {
			level++;
			Debug.Log("LEVEL UP!");
		}
	}

	void FixedUpdate() {
		if(!gameOver)
			AddScore(0.1f);
	}

	public void GameOver() {
		gameOverText.text = "Game Over!";
		gameOver = true;
		restart = true;
	}

	public void AddScore(float newScoreValue) {
		score = score + newScoreValue;
		UpdateScore();
	}

	void UpdateScore() {
		scoreText.text = "Score: " + (int)score;
	}

	void SpawnWaves() {
		Vector3 spawnPosition = new Vector3(Random.Range(-3, 3), spawnValues.y, spawnValues.z);
		Quaternion spawnRotation = Quaternion.identity;
		Instantiate(hazard[chooseSpawn()], spawnPosition, spawnRotation);
	}

	IEnumerator GenerateWave() {
		yield return new WaitForSeconds(startWait);
		while(true) {

			for(int i = 0; i < hazardCount; i++) {
				SpawnWaves();
				yield return new WaitForSeconds(spawnWait);
			}
			yield return new WaitForSeconds(waveWait);

			if(gameOver) {
				restartText.text = "Press R to Restart!";
				break;
			}
		}
	}

}
