using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlay : MonoBehaviour
{
    private static SoundPlay instance;

    private AudioSource player;
    private AudioSource background;

    public AudioClip[] monsterClip;
    public AudioClip[] playerClip;
    public AudioClip[] backgroundClip;
    // Start is called before the first frame update

    public static SoundPlay MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundPlay>();
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    void Start()
    {
        player = this.gameObject.AddComponent<AudioSource>();
        background = this.gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AudioClip EnemyHit
    {
        get { return monsterClip[0]; }
    }

    public AudioClip EnemyDie
    {
        get { return monsterClip[1]; }
    }

    public void PlayerAttack()
    {
        player.clip = playerClip[0];
        player.Play();
    }

    public void PlayerHit()
    {
        player.clip = playerClip[1];
        player.Play();
    }
}
