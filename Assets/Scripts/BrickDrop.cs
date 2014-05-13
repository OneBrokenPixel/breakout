using UnityEngine;
using System.Collections;

using Darkhexxa.SimplePool.Components;

[RequireComponent(typeof(Rigidbody2D),typeof(CircleCollider2D))]
public class BrickDrop : BasePoolComponent {

    public   float DropGravity = 2;
    private  Camera _mainCamera;
    private  CircleCollider2D _circleCollider;

    void Awake()
    {
        rigidbody2D.gravityScale = DropGravity;
        _mainCamera = Camera.main;

        _circleCollider = collider2D as CircleCollider2D;
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate ()
    {
        Vector3 cameraWorldMin = _mainCamera.ScreenToWorldPoint ( Vector3.zero );

        float rad = _circleCollider.radius;

        if ( ( transform.position.y + rad ) < cameraWorldMin.y )
        {
            pool.Despawn ( gameObject );
        }
    }

    void OnTriggerEnter2D ( Collider2D col )
    {
        Debug.Log ( "Triggered" );
    }

    public override void OnSpawn ()
    {
    }

    public override void OnDespawn ()
    {
    }


}
