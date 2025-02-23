using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CurrentEnemy : MonoBehaviour
{
    [SerializeField] private Image sprite;

    [SerializeField] private Slider lifeSlider;

    [SerializeField] private TMP_Text turnText;

    [SerializeField] private Image enemySelected;

   private int maxTurns;

    private int currentTurns;

    private static int totalLife;

    private int currentLife;

    private static int attackPower;

    private int indexValue;

//Init Enemy obj and set values
    public void Init(Sprite NewSprite, int TotalLife, int MaxTurns, int attack, int indexvalue)
    {
        sprite.sprite = NewSprite;
        totalLife = TotalLife;
        currentLife = totalLife;
        maxTurns = MaxTurns;
        currentTurns = MaxTurns;
        attackPower = attack;
        indexValue = indexvalue;

        lifeSlider.maxValue = TotalLife;
        lifeSlider.value = TotalLife;
        turnText.text = currentTurns.ToString();
        enemySelected.gameObject.SetActive(false);
    }

    private void Update()
    {
        
    } 
    //decrease the turn of enemy
    public void takeTurn()
    {
        currentTurns-=1;
        turnText.text = currentTurns.ToString();
    }
//Get hit to enemy
    public void GetHit(int value)
    {
        currentLife -= value;
        lifeSlider.value = currentLife;
    }
//Return life of the enemy
    public int GetLife()
    {
        return currentLife;
    }
//disable current enemy
    public void IsDead()
    {
        gameObject.SetActive(false);
    }
   

    public void EnemySelection()
    {
            EnemyManager.Instance.UpdateSelectedEnemy(indexValue);
    }

    public void SelectEnemy()
    {
        enemySelected.gameObject.SetActive(true);
    }

    public void DeselectEnemy()
    {
        enemySelected.gameObject.SetActive(false);
    }
    
}
