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
	private float score;

	private bool gameOver;
	private bool restart;

	void Start() {
		score = 0;
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
	}

	void FixedUpdate() {
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
		Instantiate(hazard[Random.Range(0,hazard.Length)], spawnPosition, spawnRotation);
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
