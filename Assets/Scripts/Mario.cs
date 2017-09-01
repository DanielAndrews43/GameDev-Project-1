using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario
{

    protected PlayerController controller;
    public GameObject gameObject;
    public Mario prevMario = null;
    protected bool canDuck = false;

    public Mario(PlayerController controller, GameObject gameObject, Mario prevMario = null)
    {
        this.controller = controller;
        this.gameObject = gameObject;
        //Is it best to do these things in the constructor? 
        //Or just add an Enter() function and call it each time.
        this.prevMario = prevMario;
    }

    public void Enter()
    {
        controller.anim = gameObject.GetComponent<Animator>();
        gameObject.SetActive(true);
        gameObject.transform.position = 
            new Vector3(controller.transform.position.x, gameObject.transform.position.y);
        Vector3 scale = gameObject.transform.localScale;
        scale.x = scale.x * Mathf.Sign(controller.transform.localScale.x);
    }

    public Mario Exit(Mario newState) {
        gameObject.SetActive(false);
        return newState;
    }

    public bool CanDuck()
    {
        return canDuck;
    }

}
