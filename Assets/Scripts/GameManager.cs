using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    
    EnemyManager _EM;

    [SerializeField] Slider healthBar;
    [SerializeField] private TMP_Text[] pointsTexts;
    [SerializeField] GameObject changeLevelText;
    

    private int life;
    private static int level;
    private bool playerTurn = true;

    private int atackValue;
    private bool stillAllive;
    
    private int redScore = 0;
    private int blueScore = 0;
    private int greenScore = 0;
    private int whiteScore = 0;

    //[serializeField] private TMPro_Text[] fluidsScore;

    void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        atackValue = 1;
        level = 1;
        stillAllive = true;
        _EM = GetComponent<EnemyManager>();
        PuzzleController.Instance.StartPuzzle();
        _EM.CallNewMinions();
        life = 250;
        healthBar.maxValue = life;
        healthBar.value = healthBar.maxValue;
    }

    private void Update()
    {
        if (life <= 0 && stillAllive == true)
        {
            stillAllive = false;
            StartCoroutine(DeathPlayer());
        }
        else
        {
            GameLoop();
        }
    }

    void GameLoop()
    {
        if(!stillAllive)
            return;
        else
        {
            if (playerTurn)
            {
                print("player turn");
            }
            else
            {
                EnemyManager.Instance.TakeTurn();
                EnemyManager.Instance.CanAddEnemies();
                EnemyManager.Instance.HitPlayer();
                playerTurn = !playerTurn;
                print("enemy turn");
            }
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

   public void GetHit(int value)
    {
        life -= value;
        healthBar.value = life;
    }
   
//update ColorScore
    public void AddPoints(int redvalue, int bluevalue, int greenvalue, int whitevalue)
    {
        redScore+=redvalue;
        pointsTexts[0].text = redScore.ToString();
        blueScore+=bluevalue;
        pointsTexts[1].text = blueScore.ToString();
        greenScore+=greenvalue;
        pointsTexts[2].text = greenScore.ToString();
        whiteScore += whitevalue;
        pointsTexts[3].text = whiteScore.ToString();
    }
//Get color score from index 0=red,1=blue,2=green,3=whirte
    public float GetScore(int value)
    {
        if(value == 0)
            return (float)redScore;
        else if(value == 1)
            return (float)blueScore;
        else if(value == 2)
            return (float)greenScore;
        else  
            return (float)whiteScore;
    }

    public void SetPlayerTurn(bool value)
    {
        playerTurn = value;
    }
}
