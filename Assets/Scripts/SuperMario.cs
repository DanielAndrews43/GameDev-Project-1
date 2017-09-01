using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperMario : Mario {

    public SuperMario(PlayerController controller, GameObject myGameObject, Mario prevMario) : base(controller, myGameObject, prevMario)
    {
        this.canDuck = true;
    }    

}
