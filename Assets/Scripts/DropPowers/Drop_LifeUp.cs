using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Drop_LifeUp : Drop_PowerBase
{

    public override void ApplyPower ()
    {
        GameScript.Instance.Lives++;
    }
}
