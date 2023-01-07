using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Projectile : MonoBehaviour
{
    public AudioClip hitSound;

    private bool collided;
    // Start is called before the first frame update
    void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag != "Magic" && other.gameObject.tag != "Player" && other.gameObject.tag != "Balcony" && !collided) {
            collided = true;
            if(other.gameObject.tag == "Button" || other.gameObject.tag == "Puzzle - Box") {
                other.gameObject.GetComponent<SolvePuzzle>().Solve();
            }
            else if(other.gameObject.tag == "Puzzle - Button") {
                other.gameObject.GetComponent<DestroyBalcony>().Destroy();
            }
            else if(other.gameObject.tag == "Box") {
                other.gameObject.GetComponent<DeleteOnHit>().getHit();
            }
            else if(other.gameObject.CompareTag("Enemy - Slime")) {
            other.gameObject.GetComponent<SlimeStats>().getHit();
            }
            else if(other.gameObject.CompareTag("Enemy - Beholder")) {
                other.gameObject.GetComponent<EyeballMovement>().getHit();
            }
            else if(other.gameObject.CompareTag("Enemy - Footman")) {
                other.gameObject.GetComponent<SoldierMovement>().getHit();
            }
            else if(other.gameObject.CompareTag("Enemy - Grunt")) {
                other.gameObject.GetComponent<BullMovement>().getHit();
            }
            else if(other.gameObject.CompareTag("Boss")) {
                other.gameObject.GetComponent<FinalBossMovement>().getHit(gameObject.tag);
            }
            if (hitSound)
				AudioSource.PlayClipAtPoint(hitSound, transform.position);
            Destroy(gameObject);
        }
    }
}
