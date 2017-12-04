using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneState {
    TITLE,
    INTRO,
    GAMEPLAY,
    GAMEOVER
}

public class TitleScreen : MonoBehaviour {

    [SerializeField]
    private Transform[] blackScreen;
    [SerializeField]
    private Transform[] titleScreenUI;
    [SerializeField]
    private EnemySpawner enemySpawner;

    [SerializeField]
    private List<string> introductionTexts;
    [SerializeField]
    private float textDuration = 1.5f;
    [SerializeField]
    private Text dialoguePosition;

    private SceneState state = SceneState.TITLE;
    private bool canceledTheText = false;

    void Start () {
        StartCoroutine(OpenCurtain());
	}

    private void Update() {
        if (state == SceneState.TITLE) {
            if (Input.GetKeyDown(KeyCode.Escape))
                StartCoroutine(ExitGame());
        } else if (state == SceneState.INTRO) {
                canceledTheText = true;
            }
        } else if (state == SceneState.GAMEPLAY) {
            if (Input.GetKeyDown(KeyCode.Escape))
                StartCoroutine(ToGameOver());
        }

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

    IEnumerator ExitGame() {
        while (blackScreen[0].transform.localScale.y < .99f) {
            foreach (Transform b in blackScreen) {
                b.localScale = new Vector3(1, Mathf.Lerp(b.localScale.y, 1, .2f), 1);
            }
            yield return new WaitForSeconds(.02f);
        }
        Application.Quit();
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
                Destroy(t.gameObject);
        }

        state = SceneState.INTRO;
        yield return IntroductionText();

        yield return new WaitForSeconds(1f);

        enemySpawner.gameObject.SetActive(true);
    }

    public IEnumerator IntroductionText() {
        foreach (string str in introductionTexts) {
            dialoguePosition.text = str;
            dialoguePosition.color = new Color(0, 0, 0, 0);
            while (dialoguePosition.color.a < .99f) {
                if (canceledTheText) {
                    yield return CancelText();
                    yield break;
                }
                dialoguePosition.color = new Color(0, 0, 0, Mathf.Lerp(dialoguePosition.color.a, 1, .2f));
                yield return new WaitForSeconds(.02f);
            }

            dialoguePosition.color = Color.black;

            for (int i = 0; i < 10; i++) {
                if (canceledTheText) {
                    yield return CancelText();
                    yield break;
                }
                yield return new WaitForSeconds(textDuration/10);
            }

            while (dialoguePosition.color.a > .01f) {
                if (canceledTheText) {
                    yield return CancelText();
                    yield break;
                }
                dialoguePosition.color = new Color(0, 0, 0, Mathf.Lerp(dialoguePosition.color.a, 0, .2f));
                yield return new WaitForSeconds(.02f);
            }
        }
        state = SceneState.GAMEPLAY;
    }

    public IEnumerator CancelText() {
        while (dialoguePosition.color.a > .01f) {
            dialoguePosition.color = new Color(0, 0, 0, Mathf.Lerp(dialoguePosition.color.a, 0, .2f));
            yield return new WaitForSeconds(.02f);
        }
        state = SceneState.GAMEPLAY;
    }

    public IEnumerator ToGameOver() {
        state = SceneState.GAMEOVER;
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

    public void CoroutineStarter(IEnumerator ie) {
        StartCoroutine(ie);
    }
}
