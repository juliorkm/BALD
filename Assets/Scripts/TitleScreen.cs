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
    private Color dialogueColor;
    [SerializeField]
    private RectTransform score;
    private Vector3 scoreFinalPosition;

    private Vector3 backdropMidScale = new Vector3(.06f, 1f, 1f);

    private SceneState state = SceneState.NO_ESC;
    private bool canceledTheText = false;

    void Start () {
        state = SceneState.TITLE;

        scoreFinalPosition = new Vector3(-70, -25, 0);
	}

    private void Update() {

        #if !UNITY_WEBGL
        if (state == SceneState.TITLE) {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        } else
        #endif 

        if (state == SceneState.INTRO) {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
                canceledTheText = true;
            }
        } else if (state == SceneState.GAMEPLAY) {
            if (Input.GetKeyDown(KeyCode.Escape))
                ToTitleScreen();
        }

    }

    public void StartGame() {
        foreach (Transform t in titleScreenUI) {
            if (t != null)
                Destroy(t.gameObject);
        }

        score.anchoredPosition = scoreFinalPosition;

        state = SceneState.INTRO;
        StartCoroutine(IntroductionText());
    }

    public IEnumerator IntroductionText() {

        if (canceledTheText) {
            yield return CancelText();
            yield break;
        }
        dialoguePosition.color = dialogueColor;
        foreach (string str in introductionTexts) {
            dialoguePosition.text = str;

            for (int i = 0; i < 10; i++) {
                if (canceledTheText) {
                    yield return CancelText();
                    yield break;
                }
                yield return new WaitForSeconds(textDuration/10);
            }
        }

        dialoguePosition.color = new Color(1, 1, 1, 0);

        state = SceneState.GAMEPLAY;

        yield return new WaitForSeconds(1f);

        enemySpawner.gameObject.SetActive(true);
    }

    public IEnumerator CancelText() {
        dialoguePosition.color = new Color(1, 1, 1, 0);

        state = SceneState.GAMEPLAY;

        yield return new WaitForSeconds(1f);

        enemySpawner.gameObject.SetActive(true);
    }

    public void ToTitleScreen() {
        state = SceneState.NO_ESC;
        ScoreManager.SaveHiScore();
        SceneManager.LoadScene(0);
    }

    public IEnumerator ToGameOver() {
        state = SceneState.NO_ESC;

        dialoguePosition.color = new Color(0.5f, 0, 0, 1);
        dialoguePosition.text = "Game Over";
        yield return new WaitForSeconds(textDuration);
        ScoreManager.SaveHiScore();
        SceneManager.LoadScene(0);
    }
}
