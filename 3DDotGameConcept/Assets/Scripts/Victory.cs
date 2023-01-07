using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
    private int BossesDefeated;
    // Start is called before the first frame update
    void Start()
    {
        BossesDefeated = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(BossesDefeated == 1)
        {
            SceneManager.LoadScene("Victory");
        }
    }


    public void BossDefeat()
    {
        ++BossesDefeated;
    }
}
