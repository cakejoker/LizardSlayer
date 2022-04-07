using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LizardTail", menuName ="Items/LizardTail",order = 2)]
public class LizardTail : Item
{
    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n리자드맨의 잘려나간 꼬리이다.");
    }
}
