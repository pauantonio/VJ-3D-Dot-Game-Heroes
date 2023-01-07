using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DeleteOnHit : MonoBehaviour
{
    public AudioClip destroySound;
    public GameObject destroyEffect;

    [SerializeField] private bool destroyed;

    // Start is called before the first frame update
    void Start()
    {
        destroyed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyed) getHit();
    }

    public void getHit() {
        if (destroySound)
            AudioSource.PlayClipAtPoint(destroySound, transform.position);
        if (destroyEffect)
            Instantiate(destroyEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
