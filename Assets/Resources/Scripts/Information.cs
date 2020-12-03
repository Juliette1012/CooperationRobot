using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Information : MonoBehaviour
{
    // Start is called before the first frame update


    public GameObject information;
    private bool help = false;

    public GameObject heavyInfo;
    private bool heavy = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // to display or not the panel help
    public void Help(){
        if (help == false) {
            Debug.Log("On pause");
            help = true;
            information.SetActive(true);
        }
        else {
            Debug.Log("On game");
            help = false;
            information.SetActive(false);
        }
    }

    // to display or not the panel heavyInfo
    public void Heavy(){
        if (heavy == false){
            heavyInfo.SetActive(false);
        }
        else {
            heavyInfo.SetActive(true);
        }
    }

    // mutator 
    // to change the boolean heavy (display or not the panel heavyInfo)
    public void setHeavy(bool h){
        heavy = h;
        Debug.Log("heavy : " + heavy);
        Heavy();
    }
}
