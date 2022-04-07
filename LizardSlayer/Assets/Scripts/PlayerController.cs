using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerController :MonoBehaviour
{
    public bool IsSit = false; 
    public bool isGrounded = false;
    public bool OnceJumpRayCheck = false;
    public bool isAttack = false;
    public bool isHit = false;
    public   bool isDead = false;

    public GameObject weaponColider;
    public RectTransform HpBar;
    public RectTransform MpBar;
    public RectTransform ExpBar;
    public GameObject canvas;
    public Image nowHpBar;
    public Image nowMpBar;
    public Image nowExpBar;

    public bool Is_DownJump_GroundCheck = false;   // 다운 점프를 하는데 아래 블록인지 그라운드인지 알려주는 불값
    protected float m_MoveX;
    public Rigidbody2D m_rigidbody;
    protected CapsuleCollider2D m_CapsulleCollider;
    protected Animator m_Anim;

    [Header("[Setting]")]
    public int Level = 1;
    public int MaxHp = 100;
    public int NowHp = 100;
    public int MaxMp = 50;
    public int NowMp = 50;
    public int NowExp = 0;
    public int MaxExp = 100;
    public int AttackPower = 10;
    public float MoveSpeed = 6;
    public int JumpCount = 2;
    public float jumpForce = 15f;
    public int abilityPoint = 0;
    public int HpUpgrade = 0;
    public int AttackUpgrade = 0;

    protected void AnimUpdate()
    {
        if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
               m_Anim.Play("Attack");
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
    }

    protected void Filp(bool bLeft)
    {
        transform.localScale = new Vector3(bLeft ? 1 : -1, 1, 1);
    }

    protected void prefromJump()
    {
        m_Anim.Play("Jump");
        m_rigidbody.velocity = new Vector2(0, 0);
        m_rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        OnceJumpRayCheck = true;
        isGrounded = false;
    }

    protected void DownJump()
    {
        Debug.Log("하향점프1");
        if (!isGrounded)
            return;

        Debug.Log("하향점프");
        m_Anim.Play("Jump");
        m_rigidbody.AddForce(Vector2.down * 10);
        isGrounded = false;
        m_CapsulleCollider.enabled = false;
        StartCoroutine(GroundCapsulleColliderTimmerFuc());

    }

    IEnumerator GroundCapsulleColliderTimmerFuc()
    {
        yield return new WaitForSeconds(0.3f);
        m_CapsulleCollider.enabled = true;
    }

    //////바닥 체크 레이케스트 
    Vector2 RayDir = Vector2.down;

    float PretmpY;
    float GroundCheckUpdateTic = 0;
    float GroundCheckUpdateTime = 0.01f;
    protected void GroundCheckUpdate()
    {
        if (!OnceJumpRayCheck)
            return;

        GroundCheckUpdateTic += Time.deltaTime;

        if (GroundCheckUpdateTic > GroundCheckUpdateTime)
        {
            GroundCheckUpdateTic = 0;

            if (PretmpY == 0)
            {
                PretmpY = transform.position.y;
                return;
            }

            float reY = transform.position.y - PretmpY;  //    -1  - 0 = -1 ,  -2 -   -1 = -3

            if (reY <= 0)
            {
                if (isGrounded)
                {
                    LandingEvent();
                    OnceJumpRayCheck = false;
                }
                else
                {
                    //Debug.Log("안부딪힘");
                }
            }
            PretmpY = transform.position.y;
        }
    }
    protected abstract void LandingEvent();

    
}
