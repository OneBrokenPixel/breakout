using UnityEngine;
using System.Collections;

using Darkhexxa.SimplePool;

[RequireComponent(typeof(SpriteRenderer))]
public class Brick_Controller : MonoBehaviour {

    public bool Immortal = false;
    public int Life = 1;

    public Sprite[] SpriteSequence;
    public Sprite   SpriteImmortal;

    public AnimationCurve ImpactAnimation;

    private SimplePool particlePool;

    Transform       spriteTransform;
    SpriteRenderer  spriteRenderer;

    public static int ActiveBricks = 0;

    void Awake()
    {
        spriteTransform = transform.FindChild ( "Sprite" );
        spriteRenderer = spriteTransform.GetComponent<SpriteRenderer> ();

        GameObject go = GameObject.Find ( "DeathParticlesPool" );
        //print ( go );
		particlePool = go.GetComponent<SimplePool> ();

        Life = Mathf.Clamp ( Life, 0, (SpriteSequence.Length - 1) );
    }

	// Use this for initialization
	void Start () {

        if ( Immortal )
        {
            spriteRenderer.sprite = SpriteImmortal;
        }
        else
        {
            spriteRenderer.sprite = SpriteSequence [Life];
        }
        ActiveBricks++;
	}
	
	// Update is called once per frame
    void Update ()
    {
        if ( _impactTime < 1.0f )
        {
            float dist = ImpactAnimation.Evaluate ( _impactTime ) * 0.1f;
            Vector3 offset = _impactNormal * dist;

            spriteTransform.position = transform.position + offset;
            _impactTime += Time.deltaTime;
        }
	}


    void KillBrick ( )
    {
        GameScript.Instance.Score += 10;
        ParticleSystem particles = particlePool.Spawn ( transform.position, transform.rotation ).particleSystem;
        particles.Play ();

        GameScript.Instance.BrickDestroyedAt ( transform );

        Destroy ( gameObject );
        ActiveBricks--;
    }

    Vector2 _impactNormal = new Vector2 ();
    float _impactTime = 2.0f;


    void FixedUpdate()
    {

    }

    void OnCollisionExit2D ( Collision2D coll )
    {
        if( !Immortal )
        {
            Life--;
            if ( Life < 0 )
            {
                KillBrick ();
            }
            else
            {
                spriteRenderer.sprite = SpriteSequence [Life];
                _impactNormal = coll.contacts [0].normal;
                _impactTime = Time.deltaTime;
            }
        }
    }
}
