using UnityEngine;
using System.Collections;

using Darkhexxa.SimplePool.Components;

[RequireComponent ( typeof ( Rigidbody2D ) )]
public class Ball_Controller : BasePoolComponent
{
    [System.Serializable]
    public class Ball_State
    {
        public float MinSpeed = 4.0f;
        public float MaxSpeed = 8.0f;
        public float RandomReboundTollerance = 0.999f;
    }

    BoxCollider2D _boxCollider;
    Rigidbody2D _rigidbody2D;
    //Camera _mainCamera;
    GameScript _gameScript;

    Vector3 _vel = new Vector3();
    public Vector3 Vel
    {
        get
        {
            return _vel;
        }
    }
    //public Vector2 SpeedRange = new Vector2(2,8);

    public static int ActiveBalls = 0;

    //public float reboundTollerance = 0.9f;

    public Ball_State state;// = new Ball_State();

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D> ();
        _gameScript = GameScript.Instance;
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
        if ( transform.parent == null )
        {
            _vel = _rigidbody2D.velocity;
            float currentSpeed = _vel.magnitude;

            if ( currentSpeed < state.MinSpeed)
            {
                _rigidbody2D.AddForce ( _vel * ( state.MinSpeed - currentSpeed ) );
            }
            else if ( currentSpeed > state.MaxSpeed )
            {
                _rigidbody2D.AddForce ( _vel * ( state.MaxSpeed - currentSpeed ) );
            }
        }
    }

    void OnCollisionEnter2D ( Collision2D coll )
    {
        if ( isFree == true )
        {
            _vel = Vector2.zero;
            int count = 0;
            foreach ( var contact in coll.contacts )
            {
                float dot = Vector2.Dot ( _rigidbody2D.velocity.normalized, Vector2.right );
                if ( dot <= -state.RandomReboundTollerance || dot >= state.RandomReboundTollerance )
                {
                    count++;
                    _vel += Quaternion.Euler ( 0, 0, Random.Range ( -15, 15f ) ) * _rigidbody2D.velocity;
                }
            }
            if ( count != 0 )
            {
                _rigidbody2D.velocity = _vel * 1f / count;
            }
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

    public void Launch(Vector2 direction)
    {
        _vel = direction * state.MinSpeed;
        rigidbody2D.velocity = _vel;
        isFree = true;
    }

    public override void OnSpawn ()
    {
        ActiveBalls++;
        //_vel = UnityEngine.Random.insideUnitCircle.normalized * SpeedRange.x;
        //rigidbody2D.velocity = _vel;
    }

    public override void OnDespawn ()
    {
        ActiveBalls--;
        _vel = Vector2.zero;
        rigidbody2D.velocity = Vector2.zero;

    }

    public bool isFree { get; set; }
}
