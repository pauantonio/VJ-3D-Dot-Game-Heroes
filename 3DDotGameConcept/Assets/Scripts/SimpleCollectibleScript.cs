using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SimpleCollectibleScript : MonoBehaviour {

	public enum CollectibleTypes {NoType, Coin, Normal_Key, Boss_Key, Special_Object, Chest}; // you can replace this with your own labels for the types of collectibles in your game!

	public CollectibleTypes CollectibleType; // this gameObject's type

	public bool rotate; // do you want it to rotate?

	public float rotationSpeed;

	public AudioClip collectSound;

	public GameObject collectEffect;

	public int numPuzzlesSolved = 0;

	public int numPuzzlesToSolve = 1;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		if (rotate)
			transform.Rotate (Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
			Collect (other.gameObject);
		}
	}

	public void Collect(GameObject player)
	{
		if (CollectibleType == CollectibleTypes.NoType) {
			Debug.Log ("Do NoType Command");
		}
		if (CollectibleType == CollectibleTypes.Coin) {
			player.GetComponent<Player>().ObtainCoin();
			Effects();
		}
		if (CollectibleType == CollectibleTypes.Normal_Key) {
			player.GetComponent<Player>().hasNormalKey = true;
			Effects();
		}
		if (CollectibleType == CollectibleTypes.Boss_Key) {
			player.GetComponent<Player>().hasBossKey = true;
			Effects();
		}
		if (CollectibleType == CollectibleTypes.Special_Object) {
			player.GetComponent<Player>().ObtainSpecialObject();
			Effects();
		}
		if (CollectibleType == CollectibleTypes.Chest)
		{
			if (numPuzzlesSolved >= numPuzzlesToSolve) {
				Effects();
			}
		}
	}

	private void Effects() {
		if(collectSound)
			AudioSource.PlayClipAtPoint(collectSound, transform.position);
		if (collectEffect)
			Instantiate(collectEffect, transform.position, Quaternion.identity);
		Destroy (gameObject);
	}
}
