using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSensor : MonoBehaviour {

    public PlayerController m_root;
    public BoxCollider2D collider2D;

    // Use this for initialization
    void Start()
    {
        m_root = this.transform.root.GetComponent<PlayerController>();
       
    }

 

    ContactPoint2D[] contacts = new ContactPoint2D[1];

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            if (m_root.m_rigidbody.velocity.y <= 0)
            {
                m_root.isGrounded = true;
            }     
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        m_root.isGrounded = false;
    }



}
