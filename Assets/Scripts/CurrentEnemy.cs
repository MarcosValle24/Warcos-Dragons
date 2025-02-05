using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CurrentEnemy : MonoBehaviour
{
    [SerializeField]
    private Image sprite;

    [SerializeField]
    private Slider lifeSlider;

    [SerializeField]
    private TMP_Text turnText;

    [SerializeField]
    private GameManager _GM;

   public int maxTurns;

    public string name;
    public int value;

    private int currentTurns;

    private static int totalLife;

    private int currentLife;

    private static int attackPower;


    public void Init(Sprite NewSprite, int TotalLife, int MaxTurns, int atack, string Name)
    {
        sprite.sprite = NewSprite;
        totalLife = TotalLife;
        currentLife = totalLife;
        maxTurns = MaxTurns;
        currentTurns = MaxTurns;
        attackPower = atack;
        name = Name;

        lifeSlider.maxValue = TotalLife;
        lifeSlider.value = TotalLife;
        turnText.text = currentTurns.ToString();
        _GM = GameObject.Find("Game Manager").GetComponent<GameManager>();

    }

    private void Update()
    {
        if(currentTurns == 0)
        {
            currentTurns = maxTurns;
            Hit();   
        }
    }

    void Hit()
    {

        StartCoroutine(resetTurns());

    }
    public void takeTurn()
    {
        currentTurns-=1;
        turnText.text = currentTurns.ToString();
        value -= 1;
        
    }

    public void GetHit(int value)
    {
        currentLife -= value;
        lifeSlider.value = currentLife;

    }

    public int GetLife()
    {
        return currentLife;
    }

   public IEnumerator Death()
    {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }

        IEnumerator resetTurns()
    {
            _GM.GetHit(attackPower);
        
        yield return new WaitForSeconds(1);
        
        turnText.text = currentTurns.ToString();
    }
}
