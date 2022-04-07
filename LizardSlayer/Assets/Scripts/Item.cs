using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject, IMoveable, IDescribable
{
    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private int itemCode;

    [SerializeField]
    private int stackSize;

    [SerializeField]
    private string title;

    public Sprite MyIcon
    {
        get { return icon; }
    }

    public int MyStackSize
    {
        get { return stackSize; }
    }

    public int ItemCode
    {
        get { return itemCode; }
    }

    public virtual string GetDescription()
    {
        return title;
    }
}
