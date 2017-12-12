using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour {

    public int health;
    [SerializeField]
    private float speed;
    [SerializeField]
    private int pointsYielded;

    private SpriteRenderer sr;
    private Rigidbody2D rb;

    [SerializeField]
    private Sprite[] healthSprites;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float shootCooldown;

    private float position;

    // Use this for initialization
    void Start () {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        position = Random.Range(2f, 6f);
    }
	
	// Update is called once per frame
	void Update () {
        Movement();
        UpdateHealthSprite();
    }

    void UpdateHealthSprite() {
        if (health > 0)
            sr.sprite = healthSprites[health - 1];
        else {
            EnemySpawner.enemyList.Remove(gameObject);
            ScoreManager.score += ScoreManager.PointConversion(pointsYielded);
            Destroy(gameObject);
        }
    }

    IEnumerator Shoot() {
        while (true) {
            Instantiate(bullet, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(shootCooldown);
        }
    }

    void Movement() {
        if (transform.position.x > position)
            rb.velocity = new Vector3(-speed, 0, 0);
        else if (transform.position.x <= position && rb.velocity.x < 0) {
            rb.velocity = new Vector3(0, (Random.Range(0, 2) * 2 - 1) * speed, 0);  // random between -1 and 1
            StartCoroutine(Shoot()); // starts shooting once it stops moving forwards
        }
        else if (transform.position.y > 4.3f)
            rb.velocity = new Vector3(0, -speed, 0);
        else if (transform.position.y < -4.3f)
            rb.velocity = new Vector3(0, speed, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("PlayerBullet")) {
            if (collision != null) {
                Destroy(collision.gameObject);
            }
            this.health--;
        }
    }
}
