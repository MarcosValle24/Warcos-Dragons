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
    [SerializeField] public List<GameObject> listOfEnemys = new List<GameObject>();

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
            listOfEnemys.Add(newMinion);
        }
        
        listOfEnemys[indexSelected].GetComponent<CurrentEnemy>().SelectEnemy();

    }

    //Hit the enemies
    public void HitEnemys(int value)
    {
        listOfEnemys[0].GetComponent<CurrentEnemy>().GetHit(value);
        if (listOfEnemys[0].GetComponent<CurrentEnemy>().GetLife() <= 0)
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
        for(int i =0;i<listOfEnemys.Count;i++)
        {
            listOfEnemys[i].GetComponent<CurrentEnemy>().takeTurn();
        }
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
            listOfEnemys[i].GetComponent<CurrentEnemy>().DeselectEnemy();
        }
        listOfEnemys[indexSelected].GetComponent<CurrentEnemy>().SelectEnemy();
        
    }

}
