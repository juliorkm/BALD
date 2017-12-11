using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public static int score = 0;

    [SerializeField]
    private Text scoreText;
    private static int hiscore;
    
	void Start () {
        score = 0;
		if (PlayerPrefs.HasKey("hiscore")) {
            hiscore = PlayerPrefs.GetInt("hiscore");
        } else {
            PlayerPrefs.SetInt("hiscore", 0);
            hiscore = 0;
        }
        scoreText.text = "0\n" + hiscore;
    }
	
	void Update () {
        scoreText.text = score + "\n" + hiscore;
	}

    public static int PointConversion(int point) {
        if (PlayerManager.difficulty == CellState.BIG) return point;
        else if (PlayerManager.difficulty == CellState.MEDIUM) return (int)(point * 1.5f);
        else return 0;
    }

    public static void SaveHiScore() {
        if (score > hiscore) {
            PlayerPrefs.SetInt("hiscore", score);
            PlayerPrefs.Save();
            hiscore = score;
        }
    }
}
