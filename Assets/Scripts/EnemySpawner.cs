using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField]
    private GameObject enemyBullet;
    [SerializeField]
    private float cooldown;

    // Use this for initialization
    void Start () {
        StartCoroutine(SpawnBullets());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator SpawnBullets() {
        while (true) {
            Instantiate(enemyBullet, new Vector3(transform.position.x, Random.Range(-4,4),0), Quaternion.identity);
            yield return new WaitForSeconds(cooldown);
        }
    }
}
