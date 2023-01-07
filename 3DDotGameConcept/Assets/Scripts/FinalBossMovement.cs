using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossMovement : MonoBehaviour
{
    public Transform enemy;
    public GameObject player;
    public UnityEngine.AI.NavMeshAgent agent;
    public LayerMask obstacleMask;
    public LayerMask playerMask;
    public float CloseRange;
    public float MaxRange;
    public int Life;
    public float visionRange;
    public float visionAngle;
    public AudioClip LevelUp;
    public AudioClip Groan;
    public bool hit;
    public bool isAttacking;
    ParticleSystem particlePhase2;
    AudioSource audioSource;
    private bool PlayerSeen;
    private Vector3 initialVelocity;
    private bool dead;
    private int phase;//Three phases of boss
    private int MaxLife;
    public int AttackType;
    public float fireRate;
    public float TimeToFire = 4;
    public GameObject victory;
    public GameObject shootingPoint;
    public GameObject projectile;
    public float projectileSpeed = 30;
    private Vector3 destination;
    private Vector3 ProtoPlayerPosition; //For Projectile
    // Start is called before the first frame update
    void Start()
    {
        TimeToFire = 0;
        agent.destination = enemy.position;
        isAttacking = false;
        hit = false;
        PlayerSeen = false;
        dead = false;
        if (visionRange == 0) visionRange = 20;
        if (visionAngle == 0) visionAngle = 90;
        audioSource = GetComponent<AudioSource>();
        particlePhase2 = GetComponentInChildren<ParticleSystem>();
        MaxLife = Life;
        CheckPhase();
    }

    // Update is called once per frame
    void Update()
    {
        ProtoPlayerPosition = player.transform.position;
        ProtoPlayerPosition.y += 1;
        CheckPhase();
        if(phase == 2 && !particlePhase2.isEmitting)
        {
            particlePhase2.Play();
        }else if(phase != 2)
        {
            particlePhase2.Stop();
        }

        if (!dead)
        {
            Vector3 playerTarget = (player.transform.position - enemy.position).normalized;
            if (Vector3.Angle(transform.forward, playerTarget) < visionAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(enemy.position, player.transform.position);
                if (distanceToTarget <= visionRange)
                {
                    if (Physics.Raycast(enemy.position, playerTarget, distanceToTarget, obstacleMask) == false)
                    {
                        agent.SetDestination(player.transform.position);
                        PlayerSeen = true;
                    }
                    else
                    {
                        PlayerSeen = false;
                        isAttacking = false;
                    }
                }
            }
            else
            {
                PlayerSeen = false;
                isAttacking = false;
            }

            if (PlayerSeen &&  CheckFarRange() && !isAttacking)
            {
                AttackPlayer();
            }
        }

        if (Life <= 0 && !dead)
        {
            dead = true;
            StartCoroutine(Dead());
        }
    }


    private void AttackPlayer()
    {
        //CheckAnimation for attacking, make Player Receive damage
        isAttacking = true;
        StartCoroutine(Attack());
    }

    public void getHit(string tag)
    {
        //CheckPhase();

        if ((phase == 2 && tag.Equals("Magic")) || (phase != 2 && (tag.Equals("Sword") || tag.Equals("Magic"))))
        {
            StartCoroutine(Hit());
            --Life;
        }
    }

    IEnumerator Hit()
    {
        hit = true;
        AudioSource.PlayClipAtPoint(Groan, transform.position);
        yield return new WaitForSeconds(0.5f);
        hit = false;
    }

    IEnumerator Dead()
    {
        audioSource.PlayOneShot(LevelUp);
        yield return new WaitForSeconds(2.0f);
        victory.GetComponent<Victory>().BossDefeat();
        for(int i = 0; i < 5; ++i) player.GetComponent<Player>().ObtainHealth();
        Destroy(gameObject);
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        initialVelocity = agent.velocity;

        if (phase == 1 && CheckCloseRange())
        {
            AttackType = 1;
        }else if(phase == 2 && CheckFarRange()){
            AttackType = 0;

            Debug.Log(Time.time - TimeToFire);

            if (Time.time >= TimeToFire) {
                TimeToFire = Time.time + (1/fireRate);
                ThrowProjectile();
            } 
        }
        else if(phase == 3)
        {
            AttackType = Random.Range(0, 2);
            if (AttackType == 1 && !CheckCloseRange() || (AttackType == 0 && !CheckFarRange()))
            {
                agent.SetDestination(player.transform.position);
                agent.velocity = initialVelocity;
                isAttacking = false;
                yield break;
            }

            if (AttackType == 0) {
                ThrowProjectile();
            } 
        }
        else
        {
            isAttacking = false;
            yield break;
        }

        yield return new WaitForSeconds(1.0f);
        agent.velocity = initialVelocity;
        isAttacking = false;
    }

    private void CheckPhase()
    {
        if (Life >= (MaxLife * 2) / 3)
        {
            phase = 1;
        }else if (Life < (MaxLife * 2) / 3 && Life >= MaxLife / 3)
        {
            phase = 2;
        }
        else
        {
            phase = 3;
        }
    }

    private bool CheckCloseRange()
    {
        return (Vector3.Distance(enemy.position, player.transform.position) <= CloseRange);
    }

    private bool CheckFarRange()
    {
        return (Vector3.Distance(enemy.position, player.transform.position) <= MaxRange);
    }

    private void ThrowProjectile() {
        Vector3 playerTarget = (ProtoPlayerPosition - shootingPoint.transform.position).normalized;
        Ray ray = new Ray(shootingPoint.transform.position, playerTarget);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            destination = hit.point;
        }
        else
        {
            destination = ray.GetPoint(40);
        }

        InstantiateProjectile();
    }

    private void InstantiateProjectile()
    {
        var projectileObj = Instantiate(projectile, shootingPoint.transform.position, Quaternion.identity) as GameObject;
        projectileObj.GetComponent<Rigidbody>().velocity = (destination - shootingPoint.transform.position).normalized * projectileSpeed;
    }
/*
    private void OnCollisionEnter(Collision collision)
    {
        CheckPhase();

        if ((phase == 2 && collision.gameObject.tag.Equals("Magic")) || (phase != 2 && (collision.gameObject.tag.Equals("Sword") || collision.gameObject.tag.Equals("Magic"))))
        {
            getHit();
        }
    }*/
}