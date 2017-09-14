using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMonster3D : Enemy {

	Vector3 currentSpeed;

	public override void Start() {
		base.Start();
		currentSpeed = new Vector3(walkingSpeed, 0);
		rb.velocity = currentSpeed;
		scoreValue = 0;
		rb.freezeRotation = false;
	}

	// Update is called once per frame
	public override void FixedUpdate () {
		if (rb.velocity.magnitude <= 0.1f) {
			currentSpeed.x *= -1;
			rb.velocity = currentSpeed;
		}
		else {
			rb.velocity = currentSpeed;
		}
	}
}
