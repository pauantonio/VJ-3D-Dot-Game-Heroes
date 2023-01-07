using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    public float initialLife = 20;
    public float speed = 15.0f;
    private Rigidbody rb;

    public float smoothInputSpeed = 0.005f;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction obtainNormalKeyAction;
    private InputAction obtainBossKeyAction;
    private InputAction obtainInvulnerableAction;
    private InputAction interactAction;
    private InputAction attackAction;
    private InputAction changeWeapon;

    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;

    public bool hit;

    [SerializeField] private float life;

    public AudioClip hitSound;
    public AudioClip changeSound;
    public AudioClip swordSound;

    public bool hasNormalKey;
    public bool hasBossKey;

    [SerializeField] private bool hasSpecialObject;
    [SerializeField] private bool invulnerable;
    [SerializeField] private int coins;

    public GameObject[] buttonsObjects;
    public GameObject sword;
    public GameObject magic;
    public bool isAttacking;
    public bool attackHit;
    public bool swordEquiped;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        obtainNormalKeyAction = playerInput.actions["ObtainNormalKey"];
        obtainBossKeyAction = playerInput.actions["ObtainBossKey"];
        obtainInvulnerableAction = playerInput.actions["Invulnerable"];
        interactAction = playerInput.actions["Interact"];
        attackAction = playerInput.actions["Attack"];
        changeWeapon = playerInput.actions["ChangeWeapon"];

        life = initialLife;
        hit = false;
        invulnerable = false;
        hasNormalKey = false;
        hasBossKey = false;
        hasSpecialObject = false;
        coins = 0;
        buttonsObjects = GameObject.FindGameObjectsWithTag("Button");
        magic.SetActive(false);
        isAttacking = false;
        attackHit = false;
        swordEquiped = true;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGodMode();
        MoveAndRotate();
        CheckAttack();
        CheckLife();
        CheckButtons();
    }

    private void CheckGodMode() {
        if (obtainNormalKeyAction.triggered) {
            hasNormalKey = true;
        }

        if (obtainBossKeyAction.triggered) {
            hasBossKey = true;
        }

        if (obtainInvulnerableAction.triggered) {
            invulnerable = !invulnerable;
        }
    }

    private void MoveAndRotate() {
        // Move
        Vector3 pos = transform.position;
        pos.y = 0;
        transform.position = pos;

        Vector2 input = moveAction.ReadValue<Vector2>();
        currentInputVector = Vector2.SmoothDamp(currentInputVector, input, ref smoothInputVelocity, smoothInputSpeed);
        Vector3 move = new Vector3(currentInputVector.x, 0, currentInputVector.y);
        rb.AddForce(move * Time.deltaTime * speed, ForceMode.Impulse);

        // Rotate
        if (currentInputVector != Vector2.zero)
        {
            float angle = Mathf.Atan2(currentInputVector.x, currentInputVector.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }
    }

    private void CheckAttack() {
        if (changeWeapon.triggered && hasSpecialObject) {
            swordEquiped = !swordEquiped;
            if (swordEquiped) {
                // equip sword
                sword.SetActive(true);
                magic.SetActive(false);
            }
            else {
                // equip special object
                sword.SetActive(false);
                magic.SetActive(true);
            }

            if (changeSound)
				AudioSource.PlayClipAtPoint(changeSound, transform.position);
        }

        if(attackAction.triggered && !isAttacking) {
            attackHit = false;
            if(swordEquiped) {
                if (swordSound)
				    AudioSource.PlayClipAtPoint(swordSound, transform.position);
            }
            else magic.GetComponent<MagicWand>().ShootProjectile();
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
    }

    private void CheckButtons() {
        if(!interactAction.triggered) return;

        foreach (GameObject button in buttonsObjects) {
            if (Vector3.Distance(transform.position, button.transform.position) < 2) {
                button.GetComponent<SolvePuzzle>().Solve();
                break;
            }
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Enemy - Slime") && !other.gameObject.GetComponent<SlimeStats>().isDead()) {
            GetHit(2);
        }
        else if(other.gameObject.CompareTag("Enemy - Beholder") && !other.gameObject.GetComponent<EyeballMovement>().isDead()) {
            GetHit(3);
        }
        else if(other.gameObject.CompareTag("Enemy - Footman") && !other.gameObject.GetComponent<SoldierMovement>().isDead()) {
            GetHit(5);
        }
        else if(other.gameObject.CompareTag("Enemy - Grunt") && !other.gameObject.GetComponent<BullMovement>().isDead()) {
            GetHit(5);
        }
        else if(other.gameObject.CompareTag("Boss")/* && !other.gameObject.GetComponent<SlimeStats>().isDead()*/) {
            GetHit(7);
        }
    }

    public float GetLife() {
        return life;
    }

    public void GetHit(float damage) {
        if (!invulnerable && !hit) {
            life = life - damage;
            if (life < 0) life = 0;
            StartCoroutine(Hit());
            if (hitSound)
				AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }
    }

    IEnumerator Hit()
    {
        hit = true;
        yield return new WaitForSeconds(0.5f);
        hit = false;
    }

    private void CheckLife() {
        // Check if dead
        if(life <= 0) Dead();

        // Scale sword length to life
        Vector3 scale = sword.transform.localScale;
        sword.transform.localScale = new Vector3(scale.x, 0.5f + life/initialLife, scale.z);
    }

    private void Dead() {
        SceneManager.LoadScene("GameOver");
    }

    public void ObtainCoin() {
        coins = coins + 1;
    }

    public void ObtainSpecialObject() {
        hasSpecialObject = true;
    }

    public void ObtainHealth()
    {
        if (life < initialLife) ++life;
    }
    
    public int GetCoins(){
        return coins;
    }

    public bool isInvulnerable(){
        return invulnerable;
    }
}