using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSoldierAnimationStateController : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    SoldierMovement stats;
    private int Life;
    private float velocity;
    private bool isAttacking;
    private bool GetsHit;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        stats = GetComponent<SoldierMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Life = stats.Life;
        velocity = (stats.agent.velocity.x + stats.agent.velocity.z) / 2;
        isAttacking = stats.isAttacking;
        GetsHit = stats.hit;

        animator.SetInteger("Life", Life);
        animator.SetFloat("velocity", velocity);
        animator.SetBool("IsAttacking", isAttacking);
        animator.SetBool("GetsHit", GetsHit);
    }

}
