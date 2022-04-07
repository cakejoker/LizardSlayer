using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public int score = 0;
    public bool isGameOver = false;
    public static GameManager Instance;

    public static GameManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
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
        StartCoroutine(this.RemoveDummyParticle());
    }

    // Update is called once per frame
    void Update()
    {
              
    }

    IEnumerator RemoveDummyParticle()
    {
        while (!isGameOver)
        {
            GameObject[] particle = GameObject.FindGameObjectsWithTag("AttackParticle");

            if (particle != null)
            {
                foreach (GameObject buf in particle)
                    Destroy(buf.gameObject);
            }

            yield return new WaitForSeconds(30.0f);
        }
    }
}
