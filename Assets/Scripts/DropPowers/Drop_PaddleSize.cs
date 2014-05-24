using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class Drop_PaddleSize : Drop_PowerBase
{

    public float sizeModifyer = 0;


    public override void ApplyPower ()
    {
        GameScript.Instance.paddleState.Size += sizeModifyer;
    }
}
