using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    PuzzleController PuzzleC;
    EnemyManager _EM;

    [SerializeField]
    int[] totalAtacks = new int[4];

    [SerializeField]
    Slider healthBar;

    [SerializeField]
    GameObject changeLevelText;

    private int life;
    private static int turn;
    private static int level;

    private int atackValue;
    private bool stillAllive;



    int GetTurn()
    {
        return turn;
    }

    // Start is called before the first frame update
    void Start()
    {
        atackValue = 1;
        level = 1;
        stillAllive = true;
        PuzzleC = GetComponent<PuzzleController>();
        _EM = GetComponent<EnemyManager>();
        PuzzleC.StartPuzzle();
        _EM.CallNewMinions();

        life = 250;
        healthBar.maxValue = life;
        healthBar.value = healthBar.maxValue;

        turn = -1;

    }

    private void Update()
    {
        if (life <= 0 && stillAllive == true)
        {
            stillAllive = false;
            StartCoroutine(DeathPlayer());
        }

        if(_EM.EndLevel == true)
        {
            _EM.EndLevel = false;
            StartCoroutine(ChangeLevel());
        }
    }

    IEnumerator DeathPlayer()
    {
        changeLevelText.GetComponent<TMP_Text>().text = "Game Over";
        changeLevelText.SetActive(true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(0);

    }

    IEnumerator ChangeLevel()
    {
        level++;
        changeLevelText.GetComponent<TMP_Text>().text = "Level " + level;
        changeLevelText.SetActive(true);
        yield return new WaitForSeconds(3);
        changeLevelText.SetActive(false);
        _EM.CallNewMinions();
    }

    public void getTotals(int[] totalsArray)
    {
        if (turn < 0)
            return;
        totalAtacks = totalsArray;

        if (healthBar.value < healthBar.maxValue)
        {
            int addHealth = 0;
            addHealth = totalsArray[0] * atackValue;
            life += addHealth;
            healthBar.value = life;
        }

        int atack1;
        atack1 = totalsArray[1] * atackValue;
        Debug.Log("Atack 1: " + atack1);

        int atack2;
        atack2 = totalsArray[2] * atackValue;
        Debug.Log("Atack 2: " + atack2);

        int atack3;
        atack3 = totalsArray[3] * atackValue;
        Debug.Log("Atack 3: " + atack3);

        int TotalHit;
        TotalHit = atack1 + atack2 + atack3;

        _EM.HitEnemys(TotalHit);

    }


   public void GetHit(int value)
    {
        life -= value;
        healthBar.value = life;
    }


    public void AddTurn()
    {
        turn++;
        if (turn > 0)
        {
            _EM.TakeTurn();
        }
    }
    
}
