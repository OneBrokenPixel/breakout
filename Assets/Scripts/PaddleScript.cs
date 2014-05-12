using UnityEngine;
using System.Collections;

public class PaddleScript : MonoBehaviour {

    public GameObject ball;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        rigidbody2D.velocity = Input.GetAxis("Horizontal") * Vector2.right * 3;
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        ball.rigidbody2D.velocity *= -1;
    }
}
