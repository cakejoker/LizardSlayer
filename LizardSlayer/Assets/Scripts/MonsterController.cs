using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour
{
    public GameObject[] dropItem;

    public GameObject player;
    public GameObject canvas;
    public GameObject attackRange;
    public Animator animator;
    public GameObject hpBar;
    public RectTransform itemPosition;

    public GameObject damageObject;
    public Transform damagePosition;

    private AudioSource sound;

    private Vector3 moveVelocity;
    private Vector3 randomPosition;

    private bool isIdle;
    private bool isAttack = false;


    private float nowHp;
    private float radius = 0.5f;

    private const float Hpheight = 0.8f;
    private const float Hpwidth = 0.15f;

    public float itemHeight = -0.3f;

    [Header("[Status]")]
    public float MaxHp;
    public float AttackPower;
    public int Exp;

    [Header("[Property]")]
    public float agroRich = 3.5f;
    public float moveSpeed = 1.0f;
    public float attackSpeed = 1.0f;

    public enum State
    {
        INITIALIZE, // 최초 상태
        IDLE,       // 대기 상태
        MOVE,       // 움직임 상태
        DETECT,     // 플레이어 추적 상태
        ATTACK,     // 공격 상태
        DEAD,       // 사망 상태
    };

    public State currentState = State.INITIALIZE;

    IEnumerator ChangeToIdle(float waitSecond)
    {
        yield return new WaitForSeconds(waitSecond);

        isAttack = false;
        SetState(State.IDLE);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        nowHp = MaxHp;
        canvas = GameObject.Find("Canvas");
        hpBar = this.transform.GetChild(1).gameObject;
        player = GameObject.Find("Player");
        sound = GetComponent<AudioSource>();
        SetState(State.IDLE);
    }

    // Update is called once per frame
    void Update()
    {
        //테스트용 코드
        if (Input.GetKeyDown(KeyCode.K))
        {
            GameObject item = Instantiate(dropItem[0]) as GameObject;
            item.transform.position = this.transform.position;
        }

  
        hpBar.transform.localScale = new Vector3((nowHp*4/MaxHp) < 0 ? 0 : (nowHp * 4 / MaxHp), 0.5f,1);

        if (!isAttack)
            DetectPlayer(3.5f);

        Vector2 frontVec = new Vector2(transform.position.x + (moveVelocity == Vector3.right ? (transform.forward.x + 0.3f) : -(transform.forward.x + 0.3f))
            , transform.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0)); //초록색
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            //Debug.Log("Change");
            if (moveVelocity == Vector3.left)
            {
                moveVelocity = Vector3.right;
                this.transform.localScale = new Vector3(2, 2, 2);
            }
            else if (moveVelocity == Vector3.right)
            {
                moveVelocity = Vector3.left;
                this.transform.localScale = new Vector3(-2, 2, 2);
            }
        }

        if (ProcessDeadState())
            return;

        switch (currentState)
        {
            case State.IDLE:
                Idle();
                break;
            case State.MOVE:
                Move();
                break;
            case State.DETECT:
                Detect();
                break;
            case State.ATTACK:
                Attack();
                break;
            case State.DEAD:
                Dead();
                break;
        }
    }
    bool ProcessDeadState()
    {
        if (nowHp <= 0)
        {
            StopAllCoroutines();
            SetState(State.DEAD);
            return true;
        }
        return false;
    }

    public void SetState(State newState)
    {
        if (newState != State.IDLE && newState == currentState)
            return;

        if (currentState == State.DEAD)
            return;

        currentState = newState;

        switch (newState)
        {
            case State.IDLE:
                EnterIdleState();
                break;
            case State.MOVE:
                EnterMoveState();
                break;
            case State.DETECT:
                EnterDetectState();
                break;
            case State.ATTACK:
                EnterAttackState();
                break;
            case State.DEAD:
                EnterDeadState();
                break;
        }
    }

    void EnterIdleState()
    {
        if (ProcessDeadState())
            return;

        //Debug.Log("Enter");
        isIdle = true;
        PlayAnimation("Idle");
    }

    void Idle()
    {
        if (ProcessDeadState())
            return;

        int num = Random.Range(0, 3);
        if (isIdle && num == 0)
        {
            //Debug.Log("Move");
            SetState(State.MOVE);
            isIdle = false;
        }
    }

    void EnterMoveState()
    {
        if (ProcessDeadState())
            return;

        //Debug.Log("Move");
        PlayAnimation("Walk");
        int range = SetRandomRange(2, 3);
        randomPosition = new Vector3(transform.position.x + (Random.Range(0, 3) == 0 ? -range : range), transform.position.y, 0);


        float distance = Vector3.Distance(randomPosition, transform.position);
        float needSeconds = distance / moveSpeed;
        LookAt2D(randomPosition);
        StartCoroutine("ChangeToIdle", needSeconds);
    }

    void Move()
    {
        if (ProcessDeadState())
            return;

        //Debug.Log("Move2");

        transform.position += (moveVelocity * moveSpeed * Time.deltaTime);
    }

    void EnterDetectState()
    {
        if (ProcessDeadState())
            return;

        PlayAnimation("Walk");
    }

    void Detect()
    {
        if (ProcessDeadState())
            return;

        LookAt2D(player.transform.position);

        Vector2 frontVec = new Vector2(transform.position.x + (moveVelocity == Vector3.right ? (transform.forward.x + 0.3f) : -(transform.forward.x + 0.3f))
            , transform.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0)); //초록색
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            //Debug.Log("Change");
            transform.Translate(new Vector2(-Mathf.Sign(this.transform.localScale.x) * 0.05f, 0));
        }
        transform.position += (moveVelocity * moveSpeed * Time.deltaTime);
    }

    void EnterAttackState()
    {
        if (ProcessDeadState())
            return;

        if (!isAttack)
        {
            isAttack = true;
            PlayAnimation("Attack");
            StartCoroutine("ChangeToIdle", attackSpeed);     
            return;
        }
    }

    void Attack()
    {

    }

    public void Damaged(int damage)
    {
        if (ProcessDeadState())
            return;

        GameObject damageText = Instantiate(damageObject);
        damageText.transform.position = damagePosition.position;
        damageText.GetComponent<FontScript>().damage = damage;
        sound.clip = SoundPlay.MyInstance.EnemyHit;
        nowHp -= player.GetComponent<Player>().AttackPower;
        sound.Play();
        return;
    }

    public void EnterDeadState()
    {
        GameObject.Find("MonsterGenerator").GetComponent<MonsterGenerator>().MonsterUpgrade();
        player.GetComponent<PlayerController>().NowExp++;
        sound.clip = SoundPlay.MyInstance.EnemyDie;
        sound.Play();
        PlayAnimation("Die");
    }

    void Dead()
    {

    }

    public void PlayAnimation(string animationName)
    {
        animator.CrossFade(animationName, 0.1f);
    }

    public void DetectPlayer(float distance)
    {
        Vector2 playerPostion = player.transform.position;
        //Debug.Log(Vector2.Distance(playerPostion, transform.position).ToString());
        if (Vector2.Distance(playerPostion, transform.position) < distance)
        {
            if (Vector2.Distance(player.transform.position, transform.position) < radius + player.GetComponent<Player>().radius)
            {
                //Debug.Log("SetAttack");
                SetState(State.ATTACK);
                return;
            }
            SetState(State.DETECT);
        }
        else if (currentState == State.DETECT && Vector2.Distance(playerPostion, transform.position) > distance)
        {
            SetState(State.IDLE);
        }
    }

    public void LookAt2D(Vector3 playerPos)
    {
        if (playerPos.x < transform.position.x)
        {
            moveVelocity = Vector3.left;
            this.transform.localScale = new Vector3(-2, 2, 2);
        }
        else if (playerPos.x > transform.position.x)
        {
            moveVelocity = Vector3.right;
            this.transform.localScale = new Vector3(2, 2, 2);
        }
    }

    public int SetRandomRange(int min, int max)
    {
        return Random.Range(min, max + 1);
    }

    public void AttackColiderOnOff()
    {
        attackRange.SetActive(!attackRange.activeInHierarchy);
    }
}
