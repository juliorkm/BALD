using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField]
    private GameObject[] enemyCells;
    [SerializeField]
    private GameObject healPickUp;
    [SerializeField]
    private float[] chance;
    [SerializeField]
    private float cooldown;
    [SerializeField]
    private float healCooldown;

    // Use this for initialization
    void Start () {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnHeal());

        //TestChance();   //funciona!
    }
	
	// Update is called once per frame
	void Update () {

    }

    IEnumerator SpawnEnemies() {
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

    IEnumerator SpawnHeal() {
        while (true) {
            yield return new WaitForSeconds(healCooldown);
            Instantiate(healPickUp, new Vector3(transform.position.x, 0, 0), Quaternion.identity);
        }
    }

    void TestChance() {
        int[] a = new int[3];
        a[0] = 0; a[1] = 0; a[2] = 0;
        for (int j = 0; j < 1000000; j++) {
            float r = Random.Range(0f, 1f);
            int i = 0;
            for (; i < enemyCells.Length; i++) {
                if (r <= chance[i])
                    break;
                else
                    r -= chance[i];
            }
            a[i]++;
        }
        print("Primeiro: " + a[0] / 1000000f);
        print("Segundo: " + a[1] / 1000000f);
        print("Terceiro: " + a[2] / 1000000f);
    }
}
