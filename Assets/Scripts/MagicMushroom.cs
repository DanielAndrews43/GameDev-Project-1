using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMushroom : Item {

    Vector3 activatedPosition;
    Vector3 currentSpeed = new Vector3(5, 0);
    float timeToHide = 0.2f;

    public override void Start()
    {
        base.Start();
        activatedPosition = transform.position;
        activatedPosition.y = activatedPosition.y + 1;
    }

    public override void ItemBehavior()
    {
        //Use coroutine for rising up;
        if (activated)
        {
            if (rb.velocity.magnitude <= 0.1f)
            {
                currentSpeed.x *= -1;
                rb.velocity = new Vector3(currentSpeed.x, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector3(currentSpeed.x, rb.velocity.y);
            }
        }
    }

    IEnumerator Activate()
    {
        while (transform.position.y < (activatedPosition.y - 0.01f))
        {
            rb.velocity = new Vector3(0, 2f);
            yield return null;
        }
        rb.velocity = Vector3.zero;
        myCollider.isTrigger = false;
        rb.isKinematic = false;
        rb.velocity = currentSpeed;
        activated = true;
        yield break;
    }

    IEnumerator ShowAndHide()
    {
        mySprite.enabled = false;
        yield return new WaitForSeconds(timeToHide);
        mySprite.enabled = true;
        yield break;
    }

    public override void PickUpItem(PlayerController player)
    {
        if (!pickedUp)
        {
            pickedUp = true;
        }
        player.Grow(player.superMario);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine("Activate");
            StartCoroutine("ShowAndHide");
        }
    }

}
