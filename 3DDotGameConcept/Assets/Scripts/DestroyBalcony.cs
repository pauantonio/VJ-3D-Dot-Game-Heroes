using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DestroyBalcony : MonoBehaviour
{
    public AudioClip destroySound;
    public GameObject destroyEffect;
    public GameObject[] balcony;

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
            Destroy();
            solved = false;
        }
    }

    public void Destroy() {
        foreach (GameObject b in balcony) {
            Destroy(b);
            if (destroySound)
                AudioSource.PlayClipAtPoint(destroySound, b.transform.position);
        }
        if (destroySound)
            AudioSource.PlayClipAtPoint(destroySound, transform.position);
    }
}
