using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CheckKey : MonoBehaviour {

	public GameObject normal_door;

	public GameObject boss_door_1;

	public GameObject boss_door_2;

	public AudioClip collectSound;

	public GameObject collectEffect;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			bool hasNormalKey = other.gameObject.GetComponent<Player>().hasNormalKey;
			bool hasBossKey = other.gameObject.GetComponent<Player>().hasBossKey;

			if((hasNormalKey && gameObject.tag == "NormalDoor") || (hasBossKey && gameObject.tag == "BossDoor")) {
				if (collectSound)
					AudioSource.PlayClipAtPoint(collectSound, transform.position);
				if (collectEffect)
					Instantiate(collectEffect, transform.position, Quaternion.identity);

				Destroy(gameObject);

				if (normal_door) {
					other.gameObject.GetComponent<Player>().hasNormalKey = false;
					Destroy(normal_door);
				}

				if (boss_door_1 && boss_door_2) {
					other.gameObject.GetComponent<Player>().hasBossKey = false;
					Destroy(boss_door_1);
					Destroy(boss_door_2);
				}
			}
		}
	}
}
