using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public abstract class Drop_PowerBase : MonoBehaviour
{

    private static List<Drop_PowerBase> _powers = new List<Drop_PowerBase>();
    public static List<Drop_PowerBase> Powers
    {
        get
        {
            return _powers;
        }
    }

    public static float DropChance = 0.25f;

    public Sprite Sprite;

    public static void CreateDrop( Transform t)
    {
        if ( _powers.Count != 0 && Random.Range ( 0f, 1f ) < DropChance )
        {
            Drop_Container drop = GameScript.Instance.DropPool.Spawn ( t.position, Quaternion.identity ).GetComponent<Drop_Container> ();
            int droptype = Random.Range ( 0, _powers.Count );
            drop.Effect = _powers [droptype];
        }
    }

    protected void Awake()
    {
        print ( "adding Effect" );
        _powers.Add ( this );
    }

    public virtual void InitaliseContainer ( Drop_Container contriner )
    {
        if ( Sprite != null )
        {
            SpriteRenderer sr = contriner.GetComponent<SpriteRenderer> ();
            if ( sr != null )
            {
                sr.sprite = Sprite;
            }
        }
    }

    public abstract void ApplyPower ();
}
