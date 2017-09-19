using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatMario : Mario {

	public FatMario(PlayerController controller, GameObject myGameObject, Mario prevMario) : base(controller, myGameObject, prevMario)
	{
		this.canDuck = true;
	}    

}