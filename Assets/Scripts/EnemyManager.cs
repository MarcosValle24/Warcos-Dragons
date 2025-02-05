using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    EnemyScript[] listOfMinions;
    [SerializeField]
    EnemyScript[] listOfBosses;

    [SerializeField]
    GameObject enemyPlace;

    [SerializeField]
    GameObject enemyCanvas;

    [SerializeField]
     public List<GameObject> listOfEnemys = new List<GameObject>();

    public bool EndLevel;

    public void CallNewMinions()
    {
        EndLevel = false;

        int numberOfMinions = Random.Range(1, 5);
        for(int x = 0; x <numberOfMinions;x++)
        {
            int value = Random.Range(0, listOfMinions.Length);
           // Debug.Log(value);
            GameObject newMinion = Instantiate(enemyPlace, enemyCanvas.transform);
            newMinion.transform.GetComponent<CurrentEnemy>().Init(listOfMinions[value].art,listOfMinions[value].life,listOfMinions[value].turnsToAtack,listOfMinions[value].atack, listOfMinions[value].name);
            listOfEnemys.Add(newMinion);
        }

    }

    public void HitEnemys(int value)
    {
        listOfEnemys[0].GetComponent<CurrentEnemy>().GetHit(value);
        if (listOfEnemys[0].GetComponent<CurrentEnemy>().GetLife() <= 0)
        {
            Destroy(listOfEnemys[0]);
            listOfEnemys.Remove(listOfEnemys[0]);
        }

        if(listOfEnemys.Count == 0)
        {
            EndLevel = true;
        }
    }

    public void TakeTurn()
    {
        for(int i =0;i<listOfEnemys.Count;i++)
        {
            listOfEnemys[i].GetComponent<CurrentEnemy>().takeTurn();
        }
    }


    public void ClearEnemys()
    {
        foreach (Transform child in enemyCanvas.transform)
        {
            GameObject.Destroy(child.gameObject);
            listOfEnemys.Clear();
        }
    }

}
