using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SolvePuzzle : MonoBehaviour
{
    public AudioClip solveSound;
    public GameObject destroyBoxEffect;
    public GameObject chest;

    [SerializeField] private bool solved;

    // Start is called before the first frame update
    void Start()
    {
        solved = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (solved) {
            Solve();
            solved = false;
        }
    }

    public void Solve() {
        if (chest)
            chest.GetComponent<SimpleCollectibleScript>().numPuzzlesSolved = chest.GetComponent<SimpleCollectibleScript>().numPuzzlesSolved + 1;
        if (solveSound)
            AudioSource.PlayClipAtPoint(solveSound, transform.position);
        if (destroyBoxEffect) {
            Instantiate(destroyBoxEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
