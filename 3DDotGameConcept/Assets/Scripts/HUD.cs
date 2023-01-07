using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public GameObject player;
    public GameObject heartGodMode;
    public GameObject healthCount;
    public GameObject coinsCount;
    public GameObject normalKey;
    public GameObject bossKey;

    // Start is called before the first frame update
    void Start()
    {
        heartGodMode.SetActive(false);
        healthCount.GetComponent<Text>().text = player.GetComponent<Player>().GetLife().ToString();
        coinsCount.GetComponent<Text>().text = player.GetComponent<Player>().GetCoins().ToString();
        normalKey.SetActive(player.GetComponent<Player>().hasNormalKey);
        bossKey.SetActive(player.GetComponent<Player>().hasBossKey);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<Player>().isInvulnerable()) heartGodMode.SetActive(true);
        else heartGodMode.SetActive(false);
        healthCount.GetComponent<Text>().text = player.GetComponent<Player>().GetLife().ToString();
        coinsCount.GetComponent<Text>().text = player.GetComponent<Player>().GetCoins().ToString();
        normalKey.SetActive(player.GetComponent<Player>().hasNormalKey);
        bossKey.SetActive(player.GetComponent<Player>().hasBossKey);
    }
}
