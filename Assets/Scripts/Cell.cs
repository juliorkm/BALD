using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    public int health;
    public CellState cellstate;
    private float targetPosition = 0, centerPosition = 0;

    [HideInInspector]
    public bool regroup = false;

    private SpriteRenderer sr;
    private PlayerManager pm;

    [SerializeField]
    private Sprite[] healthSprites;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float shootCooldown;

    // Use this for initialization
    void Start () {
        sr = GetComponent<SpriteRenderer>();
        pm = GetComponentInParent<PlayerManager>();

        StartCoroutine(Shoot());
	}
	
	// Update is called once per frame
	void Update () {
        if (!regroup) LerpVerticalPosition();
        UpdateHealthSprite();
	}

    public void SetPosition(float target, float center) {
        targetPosition = target;
        centerPosition = center;
    }

    void LerpVerticalPosition() {
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, centerPosition + targetPosition, transform.localPosition.z), .3f);
    }

    void UpdateHealthSprite() {
        if (health > 0)
            sr.sprite = healthSprites[health - 1];
        else
            Destroy(gameObject);
    }

    IEnumerator Shoot() {
        while (true) {
            if (Input.GetMouseButton(0)) {
                pm.SetCountdownToMerge(false);
                Instantiate(bullet, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(shootCooldown);
            } else yield return new WaitForEndOfFrame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("EnemyBullet")) {
            if (collision != null) Destroy(collision.gameObject);
            this.health--;
        }
    }
}
