using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    
    //List of enemies these are scriptableObjects
    [SerializeField] EnemyScript[] listOfMinions;
    //This is an empty where put the enemy as scriptable object
    [SerializeField] GameObject enemyPlace;
    //Canvas Conteiner
    [SerializeField] GameObject enemyCanvas;
    //List of current enemies in screen
    [SerializeField] public List<CurrentEnemy> listOfEnemys;

    [SerializeField] private List<CurrentEnemy> attackList;

    [SerializeField] private int indexSelected = 0;

    private void Awake()
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

    //Create new enemies and shows in screen
    public void CallNewMinions()
    {
        int numberOfMinions = Random.Range(1, 5);
        for(int x = 0; x <numberOfMinions;x++)
        {
            int value = Random.Range(0, listOfMinions.Length);
           // Debug.Log(value);
            GameObject newMinion = Instantiate(enemyPlace, enemyCanvas.transform);
            newMinion.transform.GetComponent<CurrentEnemy>().Init(listOfMinions[value].art,listOfMinions[value].life,listOfMinions[value].turnsToAtack,listOfMinions[value].atack,x);
            listOfEnemys.Add(newMinion.GetComponent<CurrentEnemy>());
        }
        
        listOfEnemys[indexSelected].GetComponent<CurrentEnemy>().SelectEnemy();

    }

    //Hit the enemies
    public void HitEnemys(int value)
    {
        listOfEnemys[0].GetHit(value);
        if (listOfEnemys[0].GetLife() <= 0)
        {
            //Destroy(listOfEnemys[0]);
            //listOfEnemys.Remove(listOfEnemys[0]);
        }

        if(listOfEnemys.Count == 0)
        {
        }
    }
//Decrease a turn to all enemies in list
    public void TakeTurn()
    {
        for (int i = 0; i < listOfEnemys.Count; i++)
        {
            listOfEnemys[i].TakeTurn();
            if (listOfEnemys[i].GetCanAttack())
            {
                attackList.Add(listOfEnemys[i]);
            }
        }
    }

    public void CanAddEnemies()
    {
        foreach (var e in listOfEnemys)
        {
            if(e.GetCanAttack())
                attackList.Add(e);
        }
    }

    public void HitPlayer()
    {
        if (attackList.Count == 0)
            return;

            foreach (var e in attackList)
            {
                GameManager.Instance.GetHit(e.IsAttacking());
                e.ResetTurns();
            }
            attackList.Clear();
    }

//Clear enemies list, for debugging or end game
    public void ClearEnemys()
    {
        foreach (Transform child in enemyCanvas.transform)
        {
            GameObject.Destroy(child.gameObject);
            listOfEnemys.Clear();
        }
    }

    public void UpdateSelectedEnemy(int value)
    {
        indexSelected = value;
        for (int i = 0; i < listOfEnemys.Count; i++)
        {
            listOfEnemys[i].DeselectEnemy();
        }
        listOfEnemys[indexSelected].SelectEnemy();
        
    }

}
