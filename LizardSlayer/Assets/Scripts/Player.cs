using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : PlayerController
{
    public bool isPortal = false;
    public string previousMap;

    private SoundPlay sound;
    private float blinkTime;
    private string direction;
    

    public float radius = 0.5f;

    public static Player Instance;

    private void Start()
    {
        m_CapsulleCollider  = this.transform.GetComponent<CapsuleCollider2D>();
        m_Anim = this.transform.Find("model").GetComponent<Animator>();
        m_rigidbody = this.transform.GetComponent<Rigidbody2D>();
        sound = GameObject.Find("SoundManager").GetComponent<SoundPlay>();
        canvas = GameObject.Find("Canvas");
        HpBar = canvas.transform.Find("InfoPanel").Find("BackgGround_HP").GetComponent<RectTransform>();
        MpBar = canvas.transform.Find("InfoPanel").Find("BackgGround_MP").GetComponent<RectTransform>();
        ExpBar = canvas.transform.Find("InfoPanel").Find("BackgGround_Exp").GetComponent<RectTransform>();
        nowHpBar = HpBar.gameObject.transform.GetChild(0).GetComponent<Image>();
        nowMpBar = MpBar.transform.GetChild(0).GetComponent<Image>();
        nowExpBar = ExpBar.transform.GetChild(0).GetComponent<Image>();
    }
    private void Update()
    {
        if (!isDead)
        {
            checkInput();

            if (m_rigidbody.velocity.magnitude > 30)
            {
                m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x - 0.1f, m_rigidbody.velocity.y - 0.1f);

            }

            nowHpBar.fillAmount = (float)NowHp / (float)MaxHp;
            nowMpBar.fillAmount = (float)NowMp / (float)MaxMp;
            nowExpBar.fillAmount = (float)NowExp / (float)MaxExp;
            if (NowExp >= MaxExp)
            {
                NowExp -= MaxExp;
                Level++;
                abilityPoint++;
                MaxExp++;
                GameObject.Find("Level").GetComponent<Text>().text = Level.ToString();
            }

            if (Mathf.Abs(m_rigidbody.velocity.x) < 5 && Mathf.Abs(m_rigidbody.velocity.x) > 4)
                m_rigidbody.velocity = new Vector2(0, 0);
        }
    }

    public void checkInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))  //아래 버튼 눌렀을때. 
        {
            IsSit = true;
            m_Anim.Play("Sit");
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            m_Anim.Play("Idle");
            IsSit = false;
        }

        // sit나 die일때 애니메이션이 돌때는 다른 애니메이션이 되지 않게 한다. 
        if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Sit") || m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                DownJump();
            }
            return;
        }

        m_MoveX = Input.GetAxis("Horizontal");
 
        GroundCheckUpdate();

        if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (Input.GetKey(KeyCode.X))
            {
                m_Anim.Play("Attack");
                sound.PlayerAttack();
            }
            else
            {
                if (m_MoveX == 0)
                {
                    if (!OnceJumpRayCheck)
                        m_Anim.Play("Idle");
                }
                else
                {
                    m_Anim.Play("Run");
                }

            }
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            m_Anim.Play("Die");
        }

        if (m_rigidbody.velocity.x == 0)
        {
            // 기타 이동 인풋.
            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (isGrounded)  // 땅바닥에 있었을때. 
                {
                    //공격시엔 이동불가
                    if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                        return;
                    //2차원 Vector를 이용한 이동
                    transform.transform.Translate(Vector2.right * m_MoveX * MoveSpeed * Time.deltaTime);
                }
                else
                {
                    //3차원 Vector를 이용한 이동
                    transform.transform.Translate(new Vector3(m_MoveX * MoveSpeed * Time.deltaTime, 0, 0));
                }

                if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                    return;

                //캐릭터 Sprite의 좌우 반전 설정
                if (!Input.GetKey(KeyCode.LeftArrow))
                    Filp(false);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (isGrounded)  // 땅바닥에 있었을때. 
                {
                    //공격시엔 이동불가
                    if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                        return;
                    //2차원 Vector를 이용한 이동
                    transform.transform.Translate(Vector2.right * m_MoveX * MoveSpeed * Time.deltaTime);
                }
                else
                {
                    //3차원 Vector를 이용한 이동
                    transform.transform.Translate(new Vector3(m_MoveX * MoveSpeed * Time.deltaTime, 0, 0));
                }


                if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                    return;


                if (!Input.GetKey(KeyCode.RightArrow))
                    Filp(true);
            }
        }


        if (Input.GetKeyDown(KeyCode.C))
        {
            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                return;

            if (isGrounded)
            {
                if (!IsSit)
                {
                    prefromJump();
                }
                else
                {
                    DownJump();
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(isPortal)
            {
                StartCoroutine(LoadScene());
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryScript.MyInstance.OpenClose();
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            if (isGrounded)
            {
                this.m_rigidbody.velocity = new Vector2(Mathf.Sign(this.transform.localScale.x), 0) * 10.0f;
                
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (abilityPoint > 0)
            {
                StatusDataScript.MyInstance.HpUpgrade();
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (abilityPoint > 0)
            {
                StatusDataScript.MyInstance.AttackUpgrade();
            }
        }

    }

    IEnumerator LoadScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        AsyncOperation async = SceneManager.LoadSceneAsync(previousMap);
        while (!async.isDone)
        {
            yield return null;
        }
        GameObject portal = GameObject.Find(sceneName);
        this.transform.position = new Vector3(portal.transform.position.x , portal.transform.position.y,0);
    }

    protected override void LandingEvent()
    {
        if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Run") && !m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            isGrounded = true;
            m_Anim.Play("Idle");
        }
    }

    public void PlayAnimation(string animationName)
    {
        m_Anim.CrossFade(animationName, 0.3f);
    }

    public void Damaged(float damage)
    {
        NowHp -= (int)damage;
        sound.PlayerHit();

        if (NowHp <= 0 && !isDead)
            m_Anim.Play("Die");
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
