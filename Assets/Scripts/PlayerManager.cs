using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CellState {
    BIG,
    MEDIUM,
    SMALL
}

public class PlayerManager : MonoBehaviour {

    public const float distanceBetweenCells = .75f;

    [SerializeField]
    private float timeUntilMerge;
    private float mergeCounter = 0;
    private bool shouldMerge = false;
    private bool canSplit = true;

    [SerializeField]
    private GameObject bigCellPrefab,
                    mediumCellPrefab,
                    smallCellPrefab;

    [SerializeField]
    private Cell startingCell;

    private List<Cell> activeCells;

    // Use this for initialization
    void Start () {
        activeCells = new List<Cell>();
        activeCells.Add(startingCell);
	}
	
	// Update is called once per frame
	void Update () {
        Movement();
        Split();
        CountdownToMerge();

        //----DEBUG----

        if (Input.GetKeyDown(KeyCode.A))
            StartCoroutine(Merge());
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(0);
    }

    void Movement() {
        float x, y;

        if (Input.mousePosition.y < Screen.height) {
            if (Input.mousePosition.y > 0)
                y = Input.mousePosition.y;
            else
                y = 0;
        } else y = Screen.height;

        if (Input.mousePosition.x < Screen.width) {
            if (Input.mousePosition.x > 0)
                x = Input.mousePosition.x;
            else
                x = 0;
        } else x = Screen.width;

        Vector3 mousePosition = new Vector3(x, y, Input.mousePosition.z);

        Vector3 aux = Camera.main.ScreenToWorldPoint(mousePosition);

        transform.position = Vector3.Lerp(transform.position, new Vector3(aux.x, aux.y, transform.position.z), .2f);
    }

    void Split() {
        if (Input.GetMouseButtonDown(1) && canSplit) {

            int activeCellsSize = activeCells.Count;

            for (int i = 0; i < activeCellsSize; i++) {
                var c = activeCells[0];

                if (c == null) {
                    activeCells.RemoveAt(0);
                    continue;
                }

                if (c.health == 1) {
                    activeCells.RemoveAt(0);
                    activeCells.Add(c);
                    continue;
                }

                if (c.health > 1) {
                    SetCountdownToMerge(true);

                    int h2 = c.health / 2;
                    int h1 = c.health - h2;

                    if (c.cellstate == CellState.BIG) {
                        activeCells.RemoveAt(0);

                        var v1 = Instantiate(mediumCellPrefab, c.transform.position, Quaternion.identity, c.transform.parent);
                        Cell c1 = v1.GetComponent<Cell>();
                        c1.health = h1;

                        var v2 = Instantiate(mediumCellPrefab, c.transform.position, Quaternion.identity, c.transform.parent);
                        Cell c2 = v2.GetComponent<Cell>();
                        c2.health = h2;

                        c1.SetPosition(distanceBetweenCells, c.transform.localPosition.y);
                        c2.SetPosition(-distanceBetweenCells, c.transform.localPosition.y);

                        activeCells.Add(c1);
                        activeCells.Add(c2);
                    }
                    else if (c.cellstate == CellState.MEDIUM) {
                        activeCells.RemoveAt(0);

                        var v1 = Instantiate(smallCellPrefab, c.transform.position, Quaternion.identity, c.transform.parent);
                        Cell c1 = v1.GetComponent<Cell>();
                        c1.health = h1;

                        var v2 = Instantiate(smallCellPrefab, c.transform.position, Quaternion.identity, c.transform.parent);
                        Cell c2 = v2.GetComponent<Cell>();
                        c2.health = h2;

                        c1.SetPosition(distanceBetweenCells/2, c.transform.localPosition.y);
                        c2.SetPosition(-distanceBetweenCells/2, c.transform.localPosition.y);

                        activeCells.Add(c1);
                        activeCells.Add(c2);
                    }
                    else {
                        activeCells.RemoveAt(0);
                        activeCells.Add(c);
                    }

                    if (c != null) Destroy(c.gameObject);
                }
            }
        }
    }

    IEnumerator Merge() {
        int totalHealth = 0;
        int numberOfCells = 0;
        bool thereIsSmall = false;

        foreach(Cell c in activeCells) {
            if (c != null) {
                totalHealth += c.health;
                numberOfCells++;
                if (c.cellstate == CellState.SMALL)
                    thereIsSmall = true;
            }
        }

        if (numberOfCells < 2) yield break;

        if (totalHealth > 2 || !thereIsSmall) {
            foreach (Cell c in activeCells) {
                if (c != null) {
                    c.SetPosition(0, 0);
                }
            }
            canSplit = false;
            while (true) {
                int centeredCells = 0;
                foreach (Cell c in activeCells) {
                    if (c != null) {
                        if (Mathf.Abs(c.transform.localPosition.y) > .1f) {
                            yield return new WaitForSeconds(.05f);
                            break;
                        } else {
                            centeredCells++;
                            yield return new WaitForEndOfFrame();
                        }
                    }
                }
                if (centeredCells == numberOfCells) break;
            }
            canSplit = true;

            var v1 = Instantiate(bigCellPrefab, transform.position, Quaternion.identity, transform);
            Cell c1 = v1.GetComponent<Cell>();
            c1.health = totalHealth;

            foreach(Cell c in activeCells) {
                if (c != null) Destroy(c.gameObject);
            }
            activeCells.Clear();
            activeCells.Add(c1);
        }

            
        else if (totalHealth > 1 && thereIsSmall) {
            foreach (Cell c in activeCells) {
                if (c != null) {
                    c.SetPosition(0, 0);
                }
            }
            canSplit = false;
            while (true) {
                int centeredCells = 0;
                foreach (Cell c in activeCells) {
                    if (c != null) {
                        if (Mathf.Abs(c.transform.localPosition.y) > .1f) {
                            yield return new WaitForSeconds(.05f);
                            break;
                        } else {
                            centeredCells++;
                            yield return new WaitForEndOfFrame();
                        }
                    }
                }
                if (centeredCells == numberOfCells) break;
            }
            canSplit = true;

            var v1 = Instantiate(mediumCellPrefab, transform.position, Quaternion.identity, transform);
            Cell c1 = v1.GetComponent<Cell>();
            c1.health = totalHealth;

            foreach (Cell c in activeCells) {
                if (c != null) Destroy(c.gameObject);
            }
            activeCells.Clear();
            activeCells.Add(c1);
        }

        /*
        int activeCellsSize = activeCells.Count;
        if (activeCellsSize < 2) return;

        for (int i = 0; i < activeCellsSize; i++) {
            var c = activeCells[0];
            var d = activeCells[1];

            if (c != null && d != null && c.cellstate == d.cellstate) {
                if (c.cellstate == CellState.MEDIUM) {
                    activeCells.RemoveAt(0);
                    activeCells.RemoveAt(0);

                    var h = c.health + d.health;
                }
            }
        }
        */
    }

    public void SetCountdownToMerge(bool shouldMerge) {
        if (shouldMerge) this.shouldMerge = true;
        mergeCounter = timeUntilMerge;
    }

    void CountdownToMerge() {
        if (shouldMerge && mergeCounter <= 0) {
            shouldMerge = false;
            mergeCounter = 0;
            StartCoroutine(Merge());
        }
        else if (shouldMerge) {
            mergeCounter -= Time.deltaTime;
        }
    }
}
