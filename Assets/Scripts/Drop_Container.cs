using UnityEngine;
using System.Collections;

using Darkhexxa.SimplePool.Components;

[RequireComponent(typeof(Rigidbody2D))]
public class Drop_Container : BasePoolComponent {

    public   float DropGravity = 2;

    Rigidbody2D _rigidbody2d;

    private Drop_PowerBase _effect;
    public Drop_PowerBase Effect
    {
        get
        {
            return _effect;
        }
        set
        {
            _effect = value;
            _effect.InitaliseContainer ( this );
        }
    }

    void Awake()
    {
        _rigidbody2d = rigidbody2D;
    }

    void OnTriggerEnter2D ( Collider2D other )
    {
        if ( other.tag == "Paddle" )
        {
            Effect.ApplyPower();
            pool.Despawn ( gameObject );
        }
    }

    void OnTriggerExit2D ( Collider2D other )
    {
        if ( other.tag == "MainCamera" && other.GetType () == typeof ( BoxCollider2D ) )
        {
            pool.Despawn ( gameObject );
        }
    }

    public override void OnSpawn ()
    {
        _rigidbody2d.velocity = Vector2.up * -DropGravity;
    }

    public override void OnDespawn ()
    {
    }


}
