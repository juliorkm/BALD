using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField]
    private GameObject[] enemyCells;
    [SerializeField]
    private float[] chance;
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
            float r = Random.Range(0f, 1f);
            int i = 0;
            for (; i < enemyCells.Length; i++) {
                if (r <= chance[i])
                    break;
                else
                    r -= chance[i];
            }
            Instantiate(enemyCells[i], new Vector3(transform.position.x, Random.Range(-4,4),0), Quaternion.identity);
            yield return new WaitForSeconds(cooldown);
        }
    }
}
