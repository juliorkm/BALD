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
    private GameObject hurtParticle;
    [SerializeField]
    private float shootCooldown;

    [HideInInspector]
    public float silenceDuration = 0;

    // Use this for initialization
    void Start () {
        sr = GetComponent<SpriteRenderer>();
        pm = GetComponentInParent<PlayerManager>();

        StartCoroutine(Shoot());
	}
	
	// Update is called once per frame
	void Update () {
        LerpVerticalPosition();
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
        else {
            pm.CenterLastCell(this);
            pm.CheckIfAllDead(this);
            Destroy(gameObject);
        }
    }

    IEnumerator Shoot() {
        while (true) {
            if (silenceDuration > 0) {
                yield return new WaitForSeconds(silenceDuration);
                silenceDuration = 0;
            }
            if (Input.GetMouseButton(0)) {
                pm.SetCountdownToMerge(false);
                Instantiate(bullet, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(shootCooldown);
            } else yield return new WaitForEndOfFrame();
        }
    }

    public bool CanBeHealed() {
        return (cellstate == CellState.BIG && health < 4) || (cellstate == CellState.MEDIUM && health < 2);
    }

    public void GetHealed(int healAmount) {
        if (!regroup) {
            if (cellstate == CellState.BIG && health <= 4 - healAmount) health += healAmount;
            else if (cellstate == CellState.MEDIUM && health <= 2 - healAmount) health += healAmount;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("EnemyBullet") && !regroup) {
            if (collision != null) {
                var p = Instantiate(hurtParticle, collision.transform.position, Quaternion.identity);
                Destroy(p, p.GetComponent<ParticleSystem>().main.duration);
                Destroy(collision.gameObject);
            }
            this.health--;
        }
        else if (collision.CompareTag("Enemy") && !regroup) {
            if (collision != null) {
                var p = Instantiate(hurtParticle, collision.transform.position, Quaternion.identity);
                Destroy(p, p.GetComponent<ParticleSystem>().main.duration);
            }
            this.health--;
        }
    }
}
