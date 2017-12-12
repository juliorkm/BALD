using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour {

    public int healAmount = 1;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float sinHeight;
    [SerializeField]
    private float sinWidth;
    //[SerializeField]
    //private float scalePulse;
    [SerializeField]
    private float colorPulse;
    
    private float initialTime;

    // Use this for initialization
    void Start () {

        initialTime = Time.time;
        transform.position = new Vector3(transform.position.x, transform.position.y - sinHeight / 2, transform.position.z);
    }
	
	// Update is called once per frame
	void Update () {
        Movement();
	}

    float LocalTime() {
        return Time.time - initialTime;
    }

    void Movement() {
        transform.position -= Vector3.right * speed * Time.deltaTime ;
        transform.position = new Vector3(transform.position.x, Mathf.Sin(LocalTime() * sinWidth) * sinHeight, transform.position.z);
        //rb.velocity = new Vector3(-speed, Mathf.Sin(LocalTime() * sinWidth) * sinHeight, 0);
        if (transform.position.x < -10) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            if (collision != null) {
                Cell c = collision.gameObject.GetComponent<Cell>();
                if (c.CanBeHealed()) {
                    Destroy(gameObject);
                }
            }
            collision.gameObject.GetComponent<Cell>().GetHealed(healAmount);
        }
    }
}
