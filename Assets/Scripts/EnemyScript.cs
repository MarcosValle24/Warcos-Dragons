using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy",menuName ="Enemys")]
public class EnemyScript : ScriptableObject {
    public string name;
    public Sprite art;
    public int atack;
    public int life;
    public int turnsToAtack;

}
