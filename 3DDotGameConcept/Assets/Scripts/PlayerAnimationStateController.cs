using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationStateController : MonoBehaviour
{
    Animator animator;
    Player stats;
    private float Life;
    private bool inMovement;
    private bool isAttacking;
    private bool GetsHit;
    private bool swordActive;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        stats = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Life = stats.GetLife();
        Vector3 velocity = stats.GetComponent<Rigidbody>().velocity;
        inMovement = velocity.x != 0 || velocity.z != 0;
        isAttacking = stats.isAttacking;
        GetsHit = stats.hit;
        swordActive = stats.swordEquiped;

        animator.SetFloat("Life", Life);
        animator.SetBool("InMovement", inMovement);
        animator.SetBool("IsAttacking", isAttacking);
        animator.SetBool("GetsHit", GetsHit);
        animator.SetBool("SwordActive", swordActive);
    }
}
