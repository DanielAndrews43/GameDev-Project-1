using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScaler : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        Vector3 myScale = transform.localScale;
        myScale.x = worldScreenWidth / width;
        myScale.y = worldScreenHeight / height;
        transform.localScale = myScale;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
