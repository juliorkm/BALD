using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour {

    private float ySize;
    
	void Start () {
        ySize = transform.localScale.y;
	}
	
	void Update () {
        transform.localScale = new Vector3(transform.localScale.x, ySize + (Mathf.Sin(Time.time * 2) + 1) / 5, 1);
    }
}
