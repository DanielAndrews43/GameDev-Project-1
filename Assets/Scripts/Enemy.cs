using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {

    protected Rigidbody2D rb;
    protected Animator anim;
    protected Collider2D[] myColliders;
    protected float walkingSpeed = 5;
    protected float timeToDeath = 0.5f;
    protected bool dead = false;
    protected int scoreValue;

    // Use this for initialization
    public virtual void Start () {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        anim = GetComponent<Animator>();
        myColliders = GetComponentsInChildren<Collider2D>();
    }

    public virtual void FixedUpdate () {
		
    }

    public int GetScore()
    {
        return scoreValue;
    }

    public virtual void HitByPlayer()
    {
        anim.SetBool("Hit", true);
        rb.velocity = Vector3.zero;
        dead = true;
        foreach (Collider2D collider in myColliders)
        {
            Destroy(collider);
        }
        rb.isKinematic = true;
    }

    public virtual void HitPlayer(PlayerController player)
    {
        player.Shrink();
    }

}
