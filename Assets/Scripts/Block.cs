using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    protected Vector3 upPosition;
    protected Vector3 downPosition;
    protected bool moveUp = false;
    protected bool moveDown = false;
    protected float speed = 5;
    protected Animator anim;

    protected virtual void Start ()
    {
        anim = transform.parent.GetComponent<Animator>();
        upPosition = transform.parent.position;
        upPosition.y = upPosition.y + 0.5f;
        downPosition = transform.parent.position;
    }

    protected IEnumerator MoveUpAndDown()
    {
        float step = Time.deltaTime * speed;
        while (transform.parent.position != upPosition) {
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, upPosition, step);
            yield return null;
        }
        while (transform.parent.position != downPosition) {
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, downPosition, step);
            yield return null;
        }
    }

    protected virtual void HitByPlayer(GameObject player)
    {
        if (player.name == "Super Mario")
        {
            //Play animation for breaking here.
            transform.parent.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine("MoveUpAndDown");
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.tag == "Player") {
            HitByPlayer(coll.collider.gameObject);
        }
    }

}
