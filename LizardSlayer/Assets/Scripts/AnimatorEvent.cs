using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnimatorEvent : MonoBehaviour
{
    public PlayerController playerData;

    private MonsterController monsterData;
    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttackFinished()
    {  
        playerData.isAttack = false;
    }

    public void AttackColiderOnOff()
    {
        playerData.weaponColider.GetComponent<BoxCollider2D>().enabled = !playerData.weaponColider.GetComponent<BoxCollider2D>().enabled;
    }

    void Dead()
    {
        monsterData = this.GetComponent<MonsterController>();
        monsterData.itemPosition = Instantiate(monsterData.dropItem[0]).GetComponent<RectTransform>();
        monsterData.itemPosition.position = new Vector3(transform.position.x, transform.position.y + monsterData.itemHeight, 0);
        Destroy(monsterData.hpBar.gameObject);
        Destroy(this.gameObject);
    }

    public void GameEnd()
    {
        GameObject panel = GameObject.Find("ResultPanel");

        panel.GetComponent<SetResult>().OpenResult();
        panel.transform.GetChild(1).GetComponent<Text>().text = "최종 점수는\n" + GameManager.MyInstance.score.ToString() + "점 입니다!"; 
        playerData.isDead = true;

    }

}
