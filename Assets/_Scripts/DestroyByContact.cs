using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

	public GameObject explosion;
	public GameObject miniExplosion;
	public GameObject playerExplosion;
	public int ScoreValue;
	public int hp;
	private GameController gameController;

	void Start() {
		GameObject gameControllerObject = GameObject.FindWithTag("GameController");

		if (gameControllerObject != null) {
            gameController = gameControllerObject.GetComponent<GameController>();
        }

        if (gameController == null) {
            Debug.Log("Cannot find 'GameController' script");
        }
	}

	void OnTriggerEnter(Collider other) {
		if(other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("Asteroid")){
			return;
		}

		if(other.CompareTag("Player")) {
				Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
				gameController.GameOver();
		}

		if(hp > 1) {
			Instantiate(miniExplosion, transform.position, transform.rotation);
			hp--;
			Destroy(other.gameObject);
			return;
		}

		if(explosion != null) {
			Instantiate(explosion, transform.position, transform.rotation);
		}

		if(other.name == "Bolt(Clone)" && this.name == "Enemy Bolt(Clone)") {
			Instantiate(miniExplosion, transform.position, transform.rotation);
		}

		gameController.AddScore(ScoreValue);
		Destroy(other.gameObject);
		Destroy(gameObject);		
	}
}
