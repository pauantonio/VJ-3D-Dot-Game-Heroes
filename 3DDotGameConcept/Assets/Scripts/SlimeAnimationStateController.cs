using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAnimationStateController : MonoBehaviour
{
    Animator animator;
    RandomMovement randomMovement;
    SlimeStats slime;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        randomMovement = GetComponent<RandomMovement>();
        slime = GetComponent<SlimeStats>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool("isWalking");
        bool walk = randomMovement.isWalking;
        bool hit = slime.hit;
        int life = slime.Life;
        if (!hit)
        {
            if (isWalking != walk)
            {
                animator.SetBool("isWalking", walk);
            }
        }
        else
        {
            animator.SetBool("GetsHit", hit);
            animator.SetInteger("Life", life);
        }
        
    }
}
