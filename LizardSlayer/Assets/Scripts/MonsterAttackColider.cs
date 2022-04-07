using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackColider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            Debug.Log("Attack");
            GameObject.Find("Player").GetComponent<Player>().Damaged(this.transform.parent.GetComponent<MonsterController>().AttackPower);
        }
    }
}
