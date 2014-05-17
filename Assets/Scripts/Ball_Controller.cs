using UnityEngine;
using System.Collections;

using Darkhexxa.SimplePool.Components;

[RequireComponent ( typeof ( Rigidbody2D ), typeof ( CircleCollider2D ) )]
public class Ball_Controller : BasePoolComponent
{

    CircleCollider2D _circleCollider;
    Rigidbody2D _rigidbody2D;
    Camera _mainCamera;
    GameScript _gameScript;

    Vector3 _screenWidth = new Vector3 ();
    Vector3 _vel = new Vector3();

    public Vector2 SpeedRange = new Vector2(2,8);

    public static int ActiveBalls = 0;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D> ();
        _circleCollider = collider2D as CircleCollider2D;
        _gameScript = GameScript.Instance;
        _mainCamera = Camera.main;
    }

	// Use this for initialization
	void Start () {

        //rigidbody2D.velocity = Vector3.up * SpeedRange.x;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        _vel = _rigidbody2D.velocity;
        float minSpeed = SpeedRange.x;
        float maxSpeed = SpeedRange.y; ;
        float currentSpeed = _vel.magnitude;

        if ( currentSpeed < minSpeed )
        {
            _rigidbody2D.AddForce ( _vel * ( minSpeed - currentSpeed ) );
        }
        else if ( currentSpeed > maxSpeed )
        {
            _rigidbody2D.AddForce ( _vel * ( maxSpeed - currentSpeed ) );
        }
    }

    void OnCollisionEnter2D ( Collision2D coll )
    {
        foreach( var contact in coll.contacts)
        {
            Debug.DrawRay ( contact.point, -_vel, Color.red, 1f );
            Debug.DrawRay ( contact.point, contact.normal, Color.green, 1f );
            Debug.DrawRay ( contact.point, _rigidbody2D.velocity, Color.red, 1f );
        }
    }

    void OnCollisionExit2D ( Collision2D coll )
    {
    }
        /*
    void OnTriggerEnter2D ( Collider2D other )
    {
        if ( other.GetType () == typeof ( EdgeCollider2D ) )
        {
            Debug.Log ( gameObject + " has entered" );
        }
    }
        */

    void OnTriggerExit2D ( Collider2D other )
    {
        if ( other.tag == "MainCamera" && other.GetType () == typeof ( BoxCollider2D ) )
        {
            pool.Despawn ( gameObject );
        }
    }

    public override void OnSpawn ()
    {
        ActiveBalls++;
        _vel = UnityEngine.Random.insideUnitCircle.normalized * SpeedRange.x;
        rigidbody2D.velocity = _vel;
    }

    public override void OnDespawn ()
    {
        ActiveBalls--;
        _vel = Vector2.zero;
        rigidbody2D.velocity = Vector2.zero;

    }
}
