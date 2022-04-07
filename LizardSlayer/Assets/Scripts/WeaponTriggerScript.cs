using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTriggerScript : MonoBehaviour
{
    public ParticleSystem hitEffect;
    public Animator playerAnim;
    public PlayerController playerData;

    private void Awake()
    {
        playerAnim = GameObject.Find("model").GetComponent<Animator>();
        playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Monster" && playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !playerData.isAttack)
        {
            playerData.isAttack = true;
            collision.gameObject.GetComponent<MonsterController>().Damaged(UnityEngine.Random.Range((int)(playerData.AttackPower * 0.5f), (int)(playerData.AttackPower * 1.5f)));
            StartCoroutine("HitEffect", collision);
        }
    }

    IEnumerator HitEffect(Collider2D collision)
    {
        Instantiate(hitEffect, collision.transform.localPosition, collision.transform.rotation);
        yield return new WaitForSeconds(1.0f);   
       
        Destroy(GameObject.Find("Hit_2(Clone)"));
    }
}
