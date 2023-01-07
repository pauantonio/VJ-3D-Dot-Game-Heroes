using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MagicWand : MonoBehaviour
{
    public GameObject player;
    public GameObject projectile;
    public Transform firePoint;
    public float projectileSpeed = 5;
    public AudioClip hitSound;

    private Vector3 destination;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShootProjectile() {
        Vector3 playerPosition = player.transform.position;
        Vector3 position = new Vector3(playerPosition.x, 2f, playerPosition.z);
        Vector3 forward = player.transform.forward;
        RaycastHit hit;

        if(Physics.Raycast(position, forward, out hit, 100))
            destination = hit.point;

        InstantiateProjectile();

        if (hitSound)
				AudioSource.PlayClipAtPoint(hitSound, transform.position);
    }

    void InstantiateProjectile() {
        var projectileObj = Instantiate (projectile, firePoint.position, Quaternion.identity) as GameObject;
        projectileObj.GetComponent<Rigidbody>().velocity = (destination - firePoint.position).normalized * projectileSpeed;
    }
}
