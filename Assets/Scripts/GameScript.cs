using UnityEngine;
using System.Collections;

using Darkhexxa.SimplePool;

public class GameScript : MonoBehaviour {

#region GameScript Singleton
    static GameScript _instance;
    static bool applicationIsQuitting = false;
    private static object _lock = new object ();

    public static GameScript Instance
    {
        get
        {
            if ( applicationIsQuitting )
            {
                Debug.LogWarning ( "[Singleton] Instance '" + typeof ( GameScript ) +
                    "' already destroyed on application quit." +
                    " Won't create again - returning null." );
                return null;
            }

            lock ( _lock )
            {
                if ( _instance == null )
                {
                    _instance = (GameScript)FindObjectOfType ( typeof ( GameScript ) );

                    if ( FindObjectsOfType ( typeof ( GameScript ) ).Length > 1 )
                    {
                        Debug.LogError ( "[Singleton] Something went really wrong " +
                            " - there should never be more than 1 singleton!" +
                            " Reopenning the scene might fix it." );
                        return _instance;
                    }

                    if ( _instance == null )
                    {
                        GameObject singleton = new GameObject ();
                        _instance = singleton.AddComponent<GameScript> ();
                        singleton.name = "(singleton) " + typeof ( GameScript ).ToString ();

                        DontDestroyOnLoad ( singleton );

                        Debug.Log ( "[Singleton] An instance of " + typeof ( GameScript ) +
                            " is needed in the scene, so '" + singleton +
                            "' was created with DontDestroyOnLoad." );
                    }
                    else
                    {
                        Debug.Log ( "[Singleton] Using instance already created: " +
                            _instance.gameObject.name );
                    }
                }

                return _instance;
            }
        }
    }
#endregion

    SimplePool _ballPool;

    public Texture lifeImagae;
    public int StartingLives = 3;

    public int Lives { get; set;}
    public int Score { get; set; }


    public void OnDestroy ()
    {
        applicationIsQuitting = true;
    }

	// Use this for initialization
	void Start () {
        applicationIsQuitting = false;
        Transform t = transform.FindChild ( "BallPool" );
        _ballPool = t.GetComponent<SimplePool> ();
        Ball_Controller.ActiveBalls = 0;
        Lives = StartingLives;
	}
	
    void OnGUI()
    {
        GUILayout.BeginArea ( new Rect ( 10, 10, 100, 100 ) );
        {
            GUILayout.Box ( "Score: " + Score );
        }
        GUILayout.EndArea ();


        GUILayout.BeginArea ( new Rect ( 10, Screen.height-40, 100, 30 ) );
        {
            GUILayout.BeginHorizontal ();
            {
                GUILayout.Box ( lifeImagae );
                GUILayout.Label ( " x" + Lives );
            }
            GUILayout.EndHorizontal ();
        }
        GUILayout.EndArea ();
    }

	// Update is called once per frame
	void Update () {
	
        if( Ball_Controller.ActiveBalls == 0 )
        {
            _ballPool.Spawn ( Vector3.zero, Quaternion.identity );
            Lives--;
        }

        if( Lives <= 0)
        {
            Debug.Log ( "GameOver!" );
        }
	}


}
