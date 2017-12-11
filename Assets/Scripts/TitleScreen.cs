using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneState {
    TITLE,
    INTRO,
    GAMEPLAY,
    NO_ESC
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
    [SerializeField]
    private Image dialogueBackdrop;
    [SerializeField]
    private Image gameOverBackdrop;
    [SerializeField]
    private Color dialogueColor;
    [SerializeField]
    private RectTransform score;
    private Vector3 scoreFinalPosition;

    private Vector3 backdropMidScale = new Vector3(.06f, 1f, 1f);

    private SceneState state = SceneState.NO_ESC;
    private bool canceledTheText = false;

    private AudioSource aS;

    void Start () {
        aS = GetComponent<AudioSource>();
        StartCoroutine(OpenCurtain());

        scoreFinalPosition = new Vector3(-70, -25, 0);
	}

    private void Update() {

        #if !UNITY_WEBGL
        if (state == SceneState.TITLE) {
            if (Input.GetKeyDown(KeyCode.Escape))
                StartCoroutine(ExitGame());
        } else
        #endif 

        if (state == SceneState.INTRO) {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
                canceledTheText = true;
            }
        } else if (state == SceneState.GAMEPLAY) {
            if (Input.GetKeyDown(KeyCode.Escape))
                StartCoroutine(ToTitleScreen());
        }

    }

    IEnumerator OpenCurtain() {
        while (blackScreen[0].transform.localScale.y > .1f) {
            foreach (Transform b in blackScreen) {
                b.localScale = new Vector3(1, Mathf.Lerp(b.localScale.y, 0, .2f), 1);
            }
            aS.volume = 1 - blackScreen[0].transform.localScale.y;
            yield return new WaitForSeconds(.02f);
        }
        foreach (Transform b in blackScreen) {
            b.localScale = new Vector3(1, 0, 1);
        }
        state = SceneState.TITLE;
    }

    IEnumerator ExitGame() {
        while (blackScreen[0].transform.localScale.y < .99f) {
            foreach (Transform b in blackScreen) {
                b.localScale = new Vector3(1, Mathf.Lerp(b.localScale.y, 1, .2f), 1);
            }
            aS.volume = 1 - blackScreen[0].transform.localScale.y;
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
            score.anchoredPosition = Vector3.Lerp(score.anchoredPosition, scoreFinalPosition, .2f);
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
        while (dialogueBackdrop.transform.localScale.y < .9f) {
            dialogueBackdrop.transform.localScale = Vector3.Lerp(dialogueBackdrop.transform.localScale, backdropMidScale, .3f);
            yield return new WaitForSeconds(.02f);
        }
        dialogueBackdrop.transform.localScale = backdropMidScale;

        while (dialogueBackdrop.transform.localScale.x < .9f) {
            dialogueBackdrop.transform.localScale = Vector3.Lerp(dialogueBackdrop.transform.localScale, Vector3.one, .3f);
            yield return new WaitForSeconds(.02f);
        }
        dialogueBackdrop.transform.localScale = Vector3.one;

        foreach (string str in introductionTexts) {
            dialoguePosition.text = str;
            dialoguePosition.color = new Color(dialogueColor.r, dialogueColor.g, dialogueColor.b, 0);
            while (dialoguePosition.color.a < .99f) {
                if (canceledTheText) {
                    yield return CancelText();
                    yield break;
                }
                dialoguePosition.color = new Color(dialogueColor.r, dialogueColor.g, dialogueColor.b, Mathf.Lerp(dialoguePosition.color.a, 1, .2f));
                yield return new WaitForSeconds(.02f);
            }

            dialoguePosition.color = dialogueColor;

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
                dialoguePosition.color = new Color(dialogueColor.r, dialogueColor.g, dialogueColor.b, Mathf.Lerp(dialoguePosition.color.a, 0, .2f));
                yield return new WaitForSeconds(.02f);
            }
        }

        while (dialogueBackdrop.transform.localScale.x > backdropMidScale.x + .1f) {
            dialogueBackdrop.transform.localScale = Vector3.Lerp(dialogueBackdrop.transform.localScale, backdropMidScale, .3f);
            yield return new WaitForSeconds(.02f);
        }
        dialogueBackdrop.transform.localScale = backdropMidScale;

        while (dialogueBackdrop.transform.localScale.y > .1f) {
            dialogueBackdrop.transform.localScale = Vector3.Lerp(dialogueBackdrop.transform.localScale, Vector3.zero, .3f);
            yield return new WaitForSeconds(.02f);
        }
        dialogueBackdrop.transform.localScale = Vector3.zero;

        state = SceneState.GAMEPLAY;
    }

    public IEnumerator CancelText() {
        while (dialoguePosition.color.a > .01f) {
            dialoguePosition.color = new Color(dialogueColor.r, dialogueColor.g, dialogueColor.b, Mathf.Lerp(dialoguePosition.color.a, 0, .2f));
            yield return new WaitForSeconds(.02f);
        }

        while (dialogueBackdrop.transform.localScale.x > backdropMidScale.x + .1f) {
            dialogueBackdrop.transform.localScale = Vector3.Lerp(dialogueBackdrop.transform.localScale, backdropMidScale, .3f);
            yield return new WaitForSeconds(.02f);
        }
        dialogueBackdrop.transform.localScale = backdropMidScale;

        while (dialogueBackdrop.transform.localScale.y > .1f) {
            dialogueBackdrop.transform.localScale = Vector3.Lerp(dialogueBackdrop.transform.localScale, Vector3.zero, .3f);
            yield return new WaitForSeconds(.02f);
        }
        dialogueBackdrop.transform.localScale = Vector3.zero;

        state = SceneState.GAMEPLAY;
    }

    public IEnumerator ToTitleScreen() {
        state = SceneState.NO_ESC;
        
        while (blackScreen[0].transform.localScale.y < .99f) {
            foreach (Transform b in blackScreen) {
                b.localScale = new Vector3(1, Mathf.Lerp(b.localScale.y, 1, .2f), 1);
            }
            aS.volume = 1 - blackScreen[0].transform.localScale.y;
            yield return new WaitForSeconds(.02f);
        }
        foreach (Transform b in blackScreen) {
            b.localScale = new Vector3(1, 1, 1);
        }
        ScoreManager.SaveHiScore();
        SceneManager.LoadScene(0);
    }

    public IEnumerator ToGameOver() {
        state = SceneState.NO_ESC;

        dialoguePosition.text = "Game Over";
        while (dialoguePosition.color.a < .99f) {
            dialoguePosition.color = new Color(0.5f, 0, 0, Mathf.Lerp(dialoguePosition.color.a, 1, .2f));
            gameOverBackdrop.color = new Color(0f, 0f, 0f, dialoguePosition.color.a * .7f);
            aS.volume = 1 - dialoguePosition.color.a;
            yield return new WaitForSeconds(.02f);
        }

        dialoguePosition.color = new Color(0.5f, 0, 0, 1);
        gameOverBackdrop.color = new Color(0f, 0f, 0f, .7f);
        aS.volume = 0f;
        yield return new WaitForSeconds(textDuration);

        while (blackScreen[0].transform.localScale.y < .99f) {
            foreach (Transform b in blackScreen) {
                b.localScale = new Vector3(1, Mathf.Lerp(b.localScale.y, 1, .2f), 1);
            }
            yield return new WaitForSeconds(.02f);
        }
        foreach (Transform b in blackScreen) {
            b.localScale = new Vector3(1, 1, 1);
        }
        ScoreManager.SaveHiScore();
        SceneManager.LoadScene(0);
    }

    public void CoroutineStarter(IEnumerator ie) {
        StartCoroutine(ie);
    }
}
