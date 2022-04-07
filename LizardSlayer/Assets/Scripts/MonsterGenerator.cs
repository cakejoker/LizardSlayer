using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour
{
    public Transform[] points;
    public GameObject monsterPrefab;
    public Player player;

   
    public int round = 1;

    public float createTime;
    public int maxMonster;

    public bool isGameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        points = GameObject.Find("MonsterGenerator").GetComponentsInChildren<Transform>();
        player = GameObject.Find("Player").GetComponent<Player>();

        if(points.Length > 0)
        {
            StartCoroutine(this.CreateMonster());
        }
    }

    private void Update()
    {

        
    }

    IEnumerator CreateMonster()
    {
        while(!isGameOver)
        {
            //현재 맵내의 몬스터수 파악
            int monsterCount = (int)GameObject.FindGameObjectsWithTag("Monster").Length;

            //맵 내의 최대 몬스터수보다 현재 몬스터수가 적다면 몬스터 생성
            if(monsterCount < maxMonster)
            {
                yield return new WaitForSeconds(createTime);

                int index = Random.Range(1, points.Length);

                Instantiate(monsterPrefab, points[index].position, points[index].rotation);
            }
            else
            {
                yield return null;
            }
        }
    }

    public void MonsterUpgrade()
    {
        GameManager.MyInstance.score++;
        MonsterController controller = monsterPrefab.GetComponent<MonsterController>();
        //플레이어의 레벨이 10배수가 될때마다 몬스터의 스텟이 증가한다.
        if (player.Level % 10 == 0)
        {
            //100레벨 미만에선 합연산을 통한 증가를, 100레벨 이후에는 곱연산을 통해 증가시킨다.
            if (player.Level <= 100)
            {
                monsterPrefab.GetComponent<MonsterController>().MaxHp += 600;
                monsterPrefab.GetComponent<MonsterController>().AttackPower *= 2;
                round++;
            }
            else
            {
                monsterPrefab.GetComponent<MonsterController>().MaxHp *= 2;
                monsterPrefab.GetComponent<MonsterController>().AttackPower *= 2;
                round++;
            }
        }
    }
}
