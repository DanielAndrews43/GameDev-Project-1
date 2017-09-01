using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour{

    protected Rigidbody2D rb;
    protected Collider2D myCollider;
    protected SpriteRenderer mySprite;
    protected int scoreValue = 0;

    public bool activated = false;
    protected bool pickedUp = false;

    /* Use for item movement, sounds, etc. */
    public abstract void ItemBehavior();

    /* Called when item is picked up. */
    public abstract void PickUpItem(PlayerController player);

    public virtual void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        rb.freezeRotation = true;
        myCollider = this.GetComponent<Collider2D>();
        myCollider.isTrigger = true;
        mySprite = transform.parent.GetComponentInChildren<SpriteRenderer>();
        int itemLayer = LayerMask.NameToLayer("Item");
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(itemLayer, enemyLayer, true);
    }

    public virtual void FixedUpdate() {
        ItemBehavior();
    }

    public int GetScore()
    {
        return scoreValue;
    }

    public bool isPickedUp()
    {
        return pickedUp;
    }
}
