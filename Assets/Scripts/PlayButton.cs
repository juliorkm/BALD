using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour {

    [SerializeField]
    private Text playText;
    [SerializeField]
    private TitleScreen ts;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private CellState difficulty;

    [SerializeField]
    private AudioClip audioClip;

    private bool startedTheGame = false;

    private void Update() {
        if (playText != null) playText.color = new Color(0, 0, 0, Mathf.Sin(Time.time * 10) / 4 + .75f);
    }

    private void OnMouseDown() {
        if (!startedTheGame) {
            startedTheGame = true;
            ts.GetComponent<AudioSource>().PlayOneShot(audioClip);
            Destroy(playText.gameObject);
            ts.CoroutineStarter(ts.StartGame());
            PlayerManager.difficulty = difficulty;
            player.gameObject.SetActive(true);
            player.position = transform.position;
            transform.position = new Vector3(10000, 10000, 1);
            Destroy(gameObject);
        }
    }

}
