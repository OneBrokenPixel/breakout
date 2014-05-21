using UnityEngine;
using System.Collections;

using Darkhexxa.SimplePool.Components;

[RequireComponent(typeof(Rigidbody2D))]
public class BrickDrop : BasePoolComponent {

    public   float DropGravity = 2;

    void Awake()
    {
        rigidbody2D.gravityScale = DropGravity;

    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate ()
    {
    }

    void OnTriggerEnter2D ( Collider2D col )
    {
        Debug.Log ( "Triggered" );
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
    }

    public override void OnDespawn ()
    {
    }


}
