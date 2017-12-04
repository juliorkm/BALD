using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {

    [SerializeField]
    private Transform[] blackScreen;
    [SerializeField]
    private Transform[] titleScreenUI;
    [SerializeField]
    private EnemySpawner enemySpawner;
    
    void Start () {
        StartCoroutine(OpenCurtain());
	}

    IEnumerator OpenCurtain() {
        while (blackScreen[0].transform.localScale.y > .1f) {
            foreach (Transform b in blackScreen) {
                b.localScale = new Vector3(1, Mathf.Lerp(b.localScale.y, 0, .2f),1);
            }
            yield return new WaitForSeconds(.02f);
        }
        foreach (Transform b in blackScreen) {
            b.localScale = new Vector3(1, 0, 1);
        }
    }

    public IEnumerator StartGame() {
        while (titleScreenUI[0].localScale.y > .1f) {
            foreach (Transform t in titleScreenUI) {
                if (t != null)
                    t.localScale = new Vector3(Mathf.Lerp(t.localScale.x, 2f, .2f), Mathf.Lerp(t.localScale.y, 0, .2f), 1);
            }
            yield return new WaitForSeconds(.02f);
        }
        foreach (Transform t in titleScreenUI) {
            if (t != null)
                t.localScale = new Vector3(2, 0, 1);
        }
        enemySpawner.gameObject.SetActive(true);
    }

    public IEnumerator ToGameOver() {
        while (blackScreen[0].transform.localScale.y < .99f) {
            foreach (Transform b in blackScreen) {
                b.localScale = new Vector3(1, Mathf.Lerp(b.localScale.y, 1, .2f), 1);
            }
            yield return new WaitForSeconds(.02f);
        }
        foreach (Transform b in blackScreen) {
            b.localScale = new Vector3(1, 1, 1);
        }
        SceneManager.LoadScene(0);
    }
}
