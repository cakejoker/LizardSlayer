using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusDataScript : MonoBehaviour
{
    private static StatusDataScript instance;

    private Player player;

    public static StatusDataScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StatusDataScript>();
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HpUpgrade()
    {
        player.HpUpgrade++;

        if (player.HpUpgrade % 10 == 0)
            player.MaxHp *= 2;
        else if (player.HpUpgrade < 20)
            player.MaxHp = (int)(player.MaxHp * 1.1f);
        else if (player.HpUpgrade < 30)
            player.MaxHp = (int)(player.MaxHp * 1.05f);
        else if (player.HpUpgrade < 40)
            player.MaxHp = (int)(player.MaxHp * 1.03f);
        else if (player.HpUpgrade < 50)
            player.MaxHp = (int)(player.MaxHp * 1.01f);
        else
            player.MaxHp += 5000;

        player.NowHp = player.MaxHp;
        player.abilityPoint--;
    }

    public void AttackUpgrade()
    {
        player.AttackUpgrade++;

        if (player.AttackUpgrade % 10 == 0)
            player.AttackPower *= 2;
        else
            player.AttackPower += (player.AttackUpgrade / 10) + 1;

        player.abilityPoint--;
    }
}
