using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetResult : MonoBehaviour
{
    public void OpenResult()
    {
        this.gameObject.GetComponent<Animator>().Play("Move");
    }
}
