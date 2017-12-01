using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField]
    private float speed;
    [SerializeField]
    private float lifetime = 2f;
    private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
	}
	
	// Update is called once per frame
	void Update () {
        rb.velocity = new Vector3(speed, 0, 0);
	}
}
