using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SlimeStats : MonoBehaviour
{
    public int Life = 5;
    public GameObject obj;
    public GameObject coin;
    public bool hit;
    public AudioClip LevelUp;
    public AudioClip Groan;
    private bool dead;
    // Start is called before the first frame update
    void Start()
    {
        hit = false;
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Life <= 0 && !dead)
        {
            StartCoroutine(Dead());
        }   
    }

    public void getHit()
    {
        StartCoroutine(Hit());
        --Life;
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
        yield return new WaitForSeconds(2.0f);
        AudioSource.PlayClipAtPoint(LevelUp, transform.position);
        obj.GetComponent<Player>().ObtainHealth();
        Vector3 coinPosition = new Vector3(transform.position.x, 0.5f, transform.position.z);
        var coinInstance = Instantiate (coin, coinPosition, Quaternion.identity) as GameObject;
        Destroy(gameObject);
    }

    public bool isDead() {
        return dead;
    }

}
