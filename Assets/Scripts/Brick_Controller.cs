﻿using UnityEngine;
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


    IEnumerator DeathAnimationCoroutine ( )
    {
        GameScript.Instance.Score += 10;
        ParticleSystem particles = particlePool.Spawn ( transform.position, transform.rotation ).particleSystem;

        collider2D.enabled = false;
        spriteRenderer.enabled = false;

        particles.Play ();
        
        while(particles.isPlaying)
            yield return new WaitForSeconds ( particles.startLifetime );

        particles.Clear ();
        particlePool.Despawn ( particles.gameObject );

        Destroy ( gameObject );
        yield return null;
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
                StartCoroutine ( DeathAnimationCoroutine ( ) );
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
