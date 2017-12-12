using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour {
    
    [SerializeField]
    private TitleScreen ts;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private CellState difficulty;

    public void ButtonClick() {
        ts.StartGame();
        PlayerManager.difficulty = difficulty;
        player.gameObject.SetActive(true);
        Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        player.position = new Vector3(worldMousePosition.x, worldMousePosition.y, player.position.z);
        transform.position = new Vector3(10000, 10000, 1);
        Destroy(gameObject);
        
    }

}
