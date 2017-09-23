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
	public float spawnWait;
	public float startWait;
	public float waveWait;

	private int hazardCount;
	private int difficultyLevel;
	private float score;
	private bool gameOver;
	private bool restart;

	int chooseSpawn() {
		//25 - x, 25 - x, 25 - x, 35, 0 + x, 2*(-5 + x)
		int [] minMaxValues = new int[] {0, 25 - difficultyLevel, 50 - (2 * difficultyLevel), 75 - (3 * difficultyLevel), (75 - (3 * difficultyLevel)) + 35, ((75 - (3 * difficultyLevel)) + 35) + difficultyLevel, ((75 - (3 * difficultyLevel)) + 35 + difficultyLevel ) + (2 * (-5 + difficultyLevel))};
		int chosen = Random.Range(0, 100);
		for(int i = 0; i < minMaxValues.Length - 1; i++) {
			if(chosen >= minMaxValues[i] && chosen < minMaxValues[i + 1])
				return i;
		}
		Debug.Log("Could not chooseSpawn!");
		return 0;
	}

	void Start() {
		score = 0;
		hazardCount = 10;
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
		hazardCount = 10 + Mathf.FloorToInt(score/250);
		if(difficultyLevel < 15) {
			difficultyLevel = Mathf.Max(difficultyLevel, Mathf.FloorToInt(score/Random.Range(500, 1000)));
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
