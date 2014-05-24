using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Paddle_Controller : MonoBehaviour
{
    [System.Serializable]
    enum Edge
    {
        LEFT, RIGHT, NONE
    }
    [System.Serializable]
    public class Paddle_State
    {
        public float Speed = 8.0f;
        public float Size = 1.0f;
    }

    private BoxCollider2D _box;
    private Vector2 cp = new Vector2 ();
    private Transform _launchPoint;
    public Transform LaunchPoint
    {
        get
        {
            return _launchPoint;
        }
    }

    private Ball_Controller _lanchBall;
    public Ball_Controller LaunchBall {
        get
        {
            return _lanchBall;
        }
        set
        {
            _lanchBall = value;
            if ( _lanchBall != null )
            {
                _lanchBall.isFree = false;
            }
        }
    }

    public Paddle_State state;// = new Paddle_State();
    private Vector2 _currentSize = new Vector3 ( 1.0f, 1.0f );
    public float CurrentSize
    {
        get
        {
            return _currentSize.x;
        }
        set
        {
            _currentSize.x = value;
            Vector2 boxSize = _box.size;
            boxSize.x = value+0.5f;
            _box.size = boxSize;

            Vector2 halfSize = new Vector2(0.5f * value,0);

            _left.transform.localPosition = -halfSize;
            _right.transform.localPosition = halfSize;

            Vector2  scaleFactor = new Vector2 (value / 0.5f,1.0f);

            _center.localScale = scaleFactor;

        }
    }

    private  Transform _left;
    private  Transform _right;
    private  Transform _center;

    Edge _edge = Edge.NONE;
    Vector2 _vel = new Vector2 ();

    float height = 0f;

    void Awake ()
    {
        _box = GetComponent<BoxCollider2D> ();
        _launchPoint = transform.FindChild ( "BallLaunchPoint" );

        _left = transform.FindChild ( "Left" );
        _right = transform.FindChild ( "Right" );
        _center = transform.FindChild ( "Center" );

    }

	// Use this for initialization
	void Start () {
        height = transform.position.y;
        state = GameScript.Instance.paddleState;

        CurrentSize = state.Size;
	}
	
    void UpdatePaddlePoints()
    {
        cp = transform.position;
    }

	// Update is called once per frame
	void FixedUpdate () {

        UpdatePaddlePoints ();

        _vel = Input.GetAxis ( "Horizontal" ) * Vector2.right * state.Speed;


        switch(_edge)
        {
            case Edge.LEFT:
                _vel.x = Mathf.Clamp ( _vel.x, 0, state.Speed );
                break;
            case Edge.RIGHT:
                _vel.x = Mathf.Clamp ( _vel.x, -state.Speed, 0 );
                break;
            default:
                break;
        }

        rigidbody2D.velocity = _vel;

        if( LaunchBall != null )
        {

            LaunchBall.transform.position = _launchPoint.position;
            if( Input.GetButtonDown("Fire1") )
            {
                LaunchBall.Launch ( Vector2.up );
                LaunchBall = null;
            }
        }

        Vector3 pos = transform.position;
        pos.y = height;
        transform.position = pos;

        if( CurrentSize != state.Size )
        {
            CurrentSize = state.Size;
        }
    }

    void OnCollisionEnter2D ( Collision2D coll )
    {
        if ( coll.gameObject.tag == "Ball" )
        {
            UpdatePaddlePoints ();
            Ball_Controller ballScript = coll.gameObject.GetComponent ( typeof ( Ball_Controller ) ) as Ball_Controller;
            foreach( var contact in coll.contacts)
            {
                float ballVel =  coll.rigidbody.velocity.magnitude;
                Vector2 dist = contact.point - cp;
                coll.rigidbody.velocity += dist * coll.rigidbody.mass * 5;
                coll.rigidbody.velocity = coll.rigidbody.velocity.normalized * ballVel;
            }
            _vel = rigidbody2D.velocity;
            _vel.y = 0.0f;
            rigidbody2D.velocity = _vel;
        }

    }

    void OnTriggerEnter2D ( Collider2D other )
    {
        Debug.Log ( "Using: " + other.gameObject );
        if(other.tag == "Paddle")
        {
            Vector2 direction = (other.transform.position - this.transform.position).normalized;

            if( Vector2.Dot(Vector2.right, direction) > 0 )
            {
                _edge = Edge.LEFT;
            }
            else if ( Vector2.Dot ( Vector2.right, direction ) < 0 )
            {
                _edge = Edge.RIGHT;
            }
            else
            {
                _edge = Edge.NONE;
            }
        }
    }


    void OnTriggerExit2D ( Collider2D other )
    {
        if ( other.tag == "Paddle" )
        {
            _edge = Edge.NONE;
        }
    }

}
