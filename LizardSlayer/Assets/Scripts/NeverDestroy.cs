using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeverDestroy : MonoBehaviour
{
    public static NeverDestroy instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

}
