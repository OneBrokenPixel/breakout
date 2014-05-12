using UnityEngine;
using System.Collections;


[RequireComponent ( typeof ( Rigidbody2D ), typeof ( CircleCollider2D ) )]
public class Ball_Controller : MonoBehaviour {

    CircleCollider2D _circleCollider;
    Rigidbody2D _rigidbody2D;
    Camera _mainCamera;
    Vector3 _screenWidth = new Vector3 ();
    Vector2 _vel = new Vector2();

    public float speed = 2;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D> ();
        _circleCollider = collider2D as CircleCollider2D;
        _mainCamera = Camera.main;
    }

	// Use this for initialization
	void Start () {
        //rigidbody2D.velocity = UnityEngine.Random.insideUnitCircle.normalized * speed;
        rigidbody2D.velocity = Vector3.up * speed;
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void FixedUpdate()
    {
        _screenWidth.Set ( Screen.width, Screen.height, 0 );

        Vector3 cameraWorldMax = _mainCamera.ScreenToWorldPoint ( _screenWidth );
        Vector3 cameraWorldMin = _mainCamera.ScreenToWorldPoint ( Vector3.zero );

        float minSpeed = speed*speed;

        float rad = _circleCollider.radius;
        _vel = _rigidbody2D.velocity;
        if( (transform.position.x - rad) < cameraWorldMin.x || (transform.position.x + rad) > cameraWorldMax.x )
        {
            _vel.x *= -1;
        }

        if ( ( transform.position.y - rad ) < cameraWorldMin.y || (transform.position.y + rad ) > cameraWorldMax.y )
        {
            _vel.y *= -1;
        }

        if( _vel.sqrMagnitude < minSpeed )
        {
            _vel += _vel * ( minSpeed - _vel.sqrMagnitude );
        }

        _rigidbody2D.velocity = _vel;
    }
}
