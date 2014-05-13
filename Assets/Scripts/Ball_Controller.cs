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
    Vector2 _vel = new Vector2();

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
        _screenWidth.Set ( Screen.width, Screen.height, 0 );

        Vector3 cameraWorldMax = _mainCamera.ScreenToWorldPoint ( _screenWidth );
        Vector3 cameraWorldMin = _mainCamera.ScreenToWorldPoint ( Vector3.zero );

        float minSpeed = SpeedRange.x * SpeedRange.x;
        float maxSpeed = SpeedRange.y * SpeedRange.y;
        float currentSpeed = _vel.sqrMagnitude;

        float rad = _circleCollider.radius;
        _vel = _rigidbody2D.velocity;

        if ( currentSpeed < minSpeed )
        {
            _rigidbody2D.AddForce ( _vel * ( minSpeed - currentSpeed ) );
        }
        else if ( currentSpeed > maxSpeed )
        {
            _rigidbody2D.AddForce ( _vel * ( maxSpeed - currentSpeed ) );
        }
        
        if( (transform.position.x - rad) < cameraWorldMin.x || (transform.position.x + rad) > cameraWorldMax.x )
        {
            _vel.x *= -1;
            if( _vel.y <= 0.1f)
            {
                float fVel = _vel.magnitude;
                _vel.y += UnityEngine.Random.Range ( -0.5f, 0.5f );
                _vel = _vel.normalized * fVel;
            }
        }

        // remove ( transform.position.y - rad ) < cameraWorldMin.y to let the ball fall out the bottom.

        if ( /*( transform.position.y - rad ) < cameraWorldMin.y ||*/ (transform.position.y + rad ) > cameraWorldMax.y )
        {
            _vel.y *= -1;
            if ( _vel.x <= 0.1f )
            {
                float fVel = _vel.magnitude;
                _vel.x += UnityEngine.Random.Range ( -0.5f, 0.5f );
                _vel = _vel.normalized * fVel;
            }
        }

        if( (transform.position.y + rad ) < cameraWorldMin.y )
        {
            pool.Despawn ( gameObject );
        }

        _rigidbody2D.velocity = _vel;
    }

    public override void OnSpawn ()
    {
        ActiveBalls++;
        rigidbody2D.velocity = UnityEngine.Random.insideUnitCircle.normalized * SpeedRange.x;
    }

    public override void OnDespawn ()
    {
        ActiveBalls--;
        rigidbody2D.velocity = Vector2.zero;

    }
}
