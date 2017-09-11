using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

	public GameObject shot;
	public Transform showSpawn;
	public float fireRate;
	public float delay;

	private AudioSource audioSource;

	void Start() {
		audioSource = GetComponent<AudioSource>();
		InvokeRepeating("FireWeapon", delay, fireRate);
	}

	void FireWeapon() {
		Instantiate(shot, showSpawn.position, showSpawn.rotation);
		audioSource.Play();
	}
	
}
