using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoad : MonoBehaviour
{
    public GameObject[] enemiesToUnLoad;
    [SerializeField] private List<GameObject> enemiesLoaded = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject enemy in enemiesToUnLoad) {
            enemy.SetActive(false);
        }
        enemiesLoaded.Add(Instantiate(enemiesToUnLoad[0], transform.position, Quaternion.identity));
        enemiesLoaded[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadAndUnloadEnemies(int enemiesToLoadIndex) {
        if(enemiesLoaded.Count != 0) {
            enemiesLoaded[0].SetActive(false);
            enemiesLoaded.RemoveAt(0);
        }
        enemiesLoaded.Add(Instantiate(enemiesToUnLoad[enemiesToLoadIndex-1], enemiesToUnLoad[enemiesToLoadIndex-1].transform.position, Quaternion.identity));
        enemiesLoaded[0].SetActive(true);
    }
}
