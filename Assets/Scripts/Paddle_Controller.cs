using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Paddle_Controller : MonoBehaviour {

    public float speed = 8.0f;
    public float MaxReboundAngle = 45.0f;

    private BoxCollider2D _box;

    private Vector2 cp = new Vector2 ();
    private Vector2 lp = new Vector2 ();
    private Vector2 rp = new Vector2 ();

    enum Edge
    {
        LEFT, RIGHT, NONE
    }

    Edge _edge = Edge.NONE;
    Vector2 _vel = new Vector2 ();
    void Awake ()
    {
        _box = GetComponent<BoxCollider2D> ();
    }

	// Use this for initialization
	void Start () {
	
	}
	
    void UpdatePaddlePoints()
    {
        cp = transform.position;
        lp = cp;
        rp = cp;
        lp.x -= 0.5f * _box.size.x;
        rp.x += 0.5f * _box.size.x;
    }

	// Update is called once per frame
	void Update () {

        UpdatePaddlePoints ();

        Debug.DrawRay ( cp, Vector2.up, Color.green );
        Debug.DrawRay ( lp, Quaternion.Euler ( 0, 0, MaxReboundAngle ) * Vector2.up, Color.green );
        Debug.DrawRay ( rp, Quaternion.Euler ( 0, 0,-MaxReboundAngle ) * Vector2.up, Color.green );


        _vel = Input.GetAxis ( "Horizontal" ) * Vector2.right * speed;


        switch(_edge)
        {
            case Edge.LEFT:
                _vel.x = Mathf.Clamp ( _vel.x, 0, speed );
                break;
            case Edge.RIGHT:
                _vel.x = Mathf.Clamp ( _vel.x, -speed, 0 );
                break;
            default:
                break;
        }

        rigidbody2D.velocity = _vel;
	}

    void OnCollisionEnter2D ( Collision2D coll )
    {
        if ( coll.gameObject.tag == "Ball" )
        {

        }

    }

    void OnCollisionExit2D ( Collision2D coll )
    {
    }

    void OnTriggerEnter2D ( Collider2D other )
    {
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
