using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvasiveManoeuvre : MonoBehaviour {

	public Vector2 startWait;
	public Vector2 manouevreTime;
	public Vector2 manouevreWait;
	public Boundary boundary;
	public float dodge;
	public float smoothing;
	public float tilt;
	private Transform playerTransform;

	private float currentSpeed;
	private float targetManouevre;
	private Rigidbody rb;

	void Start() {
		rb = GetComponent<Rigidbody>();
		currentSpeed = rb.velocity.z;
		if(GameObject.FindWithTag("Player"))
			playerTransform = GameObject.FindWithTag("Player").transform;
		else 
			playerTransform = null;
		StartCoroutine(Evade());
	}

	IEnumerator Evade() {
		yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));
		while(true) {
			if(playerTransform != null)
				targetManouevre = playerTransform.position.x;
			else 
				targetManouevre = Random.Range(1, dodge) * -Mathf.Sign(transform.position.x);
			yield return new WaitForSeconds(Random.Range(manouevreTime.x, manouevreTime.y));
			targetManouevre = 0;
			yield return new WaitForSeconds(Random.Range(manouevreWait.x, manouevreWait.y));
		}
	}

	void FixedUpdate() {
		float newManouevre = Mathf.MoveTowards(rb.velocity.x, targetManouevre, Time.deltaTime * smoothing);
		rb.velocity = new Vector3(newManouevre, 0.0f, currentSpeed);
		rb.position = new Vector3(
			Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
			0,
			Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
		);
		rb.rotation = Quaternion.Euler(0.0f, 180, rb.velocity.x * -tilt);
	}
}
