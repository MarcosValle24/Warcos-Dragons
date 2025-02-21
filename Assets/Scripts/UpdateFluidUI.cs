using UnityEngine;
using UnityEngine.UI;

public class UpdateFluidUI : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       float val = GameManager.Instance.GetRedScore();
       val = val / 100;
       val = Mathf.Lerp( -1, 1,val);
       print(val);
       GetComponent<Image>().material.SetFloat("_value",val);
    }
}
