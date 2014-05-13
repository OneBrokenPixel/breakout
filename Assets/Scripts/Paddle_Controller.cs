using UnityEngine;
using System.Collections;

public class Paddle_Controller : MonoBehaviour {

    public float speed = 4.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        rigidbody2D.velocity = Input.GetAxis ( "Horizontal" ) * Vector2.right * speed;
	}
}
