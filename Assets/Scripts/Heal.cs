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

    [SerializeField]
    private GameObject healParticle;

    private SpriteRenderer sr;
    private Rigidbody2D rb;

    // Use this for initialization
    void Start () {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        Movement();
        Pulse();
	}

    void Movement() {
        rb.velocity = new Vector3(-speed, Mathf.Sin(Time.time * sinWidth) * sinHeight, transform.position.z);
        if (transform.position.x < -10) Destroy(gameObject);
    }

    void Pulse() {
        //transform.localScale = new Vector3(1 + scalePulse / 2 + Mathf.Sin(Time.time) * scalePulse / 2, 1 + scalePulse / 2 + Mathf.Sin(Time.time) * scalePulse / 2, 1);
        sr.color = new Color(1, colorPulse / 2 + Mathf.Sin(Time.time + Mathf.PI/2) * colorPulse / 2, colorPulse / 2 + Mathf.Sin(Time.time + Mathf.PI / 2) * colorPulse / 2, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            if (collision != null) {
                Cell c = collision.gameObject.GetComponent<Cell>();
                if (c.CanBeHealed()) {
                    var p = Instantiate(healParticle, transform.position, Quaternion.identity);
                    Destroy(p, p.GetComponent<ParticleSystem>().main.duration);
                    Destroy(gameObject);
                }
            }
            collision.gameObject.GetComponent<Cell>().GetHealed(healAmount);
        }
    }
}
