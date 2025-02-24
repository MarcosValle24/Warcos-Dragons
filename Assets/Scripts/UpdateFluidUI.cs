using System;
using UnityEngine;
using UnityEngine.UI;

public class UpdateFluidUI : MonoBehaviour
{
    [SerializeField] private int colorVal; //0 red, 1 blue, 2 green, 3 white
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void FixedUpdate()
    {
        float val = GameManager.Instance.GetScore(colorVal);
        val = val / 100;
        val = Mathf.Lerp( -0.5f, 0.5f,val);
        GetComponent<Image>().material.SetFloat("_value",val);
    }
}
