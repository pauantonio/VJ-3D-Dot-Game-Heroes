using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   

    }

    private void OnTriggerEnter(Collider other) {
        if (!player.GetComponent<Player>().isAttacking || player.GetComponent<Player>().attackHit) return;
        
        player.GetComponent<Player>().attackHit = true;
        if(other.gameObject.CompareTag("Enemy - Slime")) {
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
        else if(other.gameObject.CompareTag("Box")) {
            other.gameObject.GetComponent<DeleteOnHit>().getHit();
        }
        else if(other.gameObject.CompareTag("Puzzle - Box")) {
            other.gameObject.GetComponent<SolvePuzzle>().Solve();
        }
        else player.GetComponent<Player>().attackHit = false;  
    }

}
