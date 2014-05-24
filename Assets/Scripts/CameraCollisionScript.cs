using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EdgeCollider2D),typeof(BoxCollider2D))]
public class CameraCollisionScript : MonoBehaviour {

    EdgeCollider2D _edge;
    BoxCollider2D _box;
    Camera _camera;

    Vector3 _screenWidth = new Vector3 ();
    float lastCameraSize = 0;

    Vector2[] _points = new Vector2 [4];
    BoxCollider2D[] _paddleEdgeBoxes;

    void Awake ()
    {
        _edge = GetComponent<EdgeCollider2D> ();
        _box = GetComponent<BoxCollider2D> ();
        _camera = camera;
        Transform paddleEdge = transform.FindChild ( "PaddelEdge" );
        _paddleEdgeBoxes = paddleEdge.GetComponents<BoxCollider2D> ();
        //lastCameraSize = _camera.orthographicSize;
    }

    void UpdateCameraColliders()
    {
        if(lastCameraSize != _camera.orthographicSize)
        {
            _screenWidth.Set ( Screen.width, Screen.height, 0 );

            Vector2 max = _camera.ScreenToWorldPoint ( _screenWidth );
            Vector2 min = _camera.ScreenToWorldPoint ( Vector3.zero );

            GameScript.Instance.PlayArea.SetMinMax ( min, max );

            _box.size = max - min;

            _points[0].Set( min.x, min.y);
            _points[1].Set( min.x, max.y);
            _points[2].Set( max.x, max.y);
            _points[3].Set( max.x, min.y);

            _edge.points = _points;

            _paddleEdgeBoxes [0].size   = new Vector2( 0.2f, max.y - min.y );
            _paddleEdgeBoxes [0].center = new Vector2 ( min.x, 0f );
            _paddleEdgeBoxes [1].size = _paddleEdgeBoxes [0].size;
            _paddleEdgeBoxes [1].center = new Vector2( max.x, 0f );

            lastCameraSize = _camera.orthographicSize;
        }
    }

	// Use this for initialization
	void Start () {
        UpdateCameraColliders ();
	}
	
	// Update is called once per frame
	void Update () {
        UpdateCameraColliders ();
	}
}
