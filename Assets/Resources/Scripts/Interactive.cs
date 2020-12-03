using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace WasaaMP {
public class Interactive : MonoBehaviourPun {
    // Start is called before the first frame update

    MonoBehaviourPun support = null ;

    private int liftHelp = 0;

    // to get the script Information
    public Information info;

    void Start () {
    }

    // Update is called once per frame
    void Update () {

    }

    void OnTriggerEnter (Collider other) {
        print (name + " : OnCollisionEnter") ;
		var hit = other.gameObject ;
		var cursor = hit.GetComponent<CursorTool> () ;
		if (cursor != null) {
			Renderer renderer = GetComponentInChildren <Renderer> () ;
		    renderer.material.color = Color.blue ;
		}
	}
    
    void OnTriggerExit (Collider other) {
        print (name + " : OnCollisionExit") ;
		var hit = other.gameObject ;
		var cursor = hit.GetComponent<CursorTool> () ;
		if (cursor != null) {
			Renderer renderer = GetComponentInChildren <Renderer> () ;
		    renderer.material.color = Color.white ;
		}
	}

    public void SetSupport (MonoBehaviourPun support) {
        this.support = support ;
        if (support != null) {
            transform.SetParent (support.transform) ;
        } else {
            transform.SetParent (null) ;
        }
    }

    public void RemoveSupport () {
        transform.SetParent (null) ;
        support = null ;
    }

    public MonoBehaviourPun GetSupport () {
        return support ;
    }

    // Function to rotate the plane with the variable deltaY
    public void Rotate(float deltaY, bool rotationX, bool rotationZ){
        Debug.Log("Lift : " + liftHelp);

        switch(liftHelp){
            case 2 :
                // Need 2 persons/forces to move the plane
                info.setHeavy(false);
                if (rotationX){
                    // Update the transform of the plane 
                    this.transform.Rotate(deltaY,0f,0f);    
                    Debug.Log("rotationX " + deltaY);
                }
                if (rotationZ){
                    // Update the transform of the plane
                    this.transform.Rotate(0f, 0f, deltaY);
                    Debug.Log("rotationZ " + deltaY);
                }
                break;
            case 1 :
                // display the panel heavyInfo
                info.setHeavy(true);
                break;
            default :
                // At the beginning : no displaying the panel heavyInfo
                info.setHeavy(false);
                break;
        }
    }

    // mutator to modify the variable liftHelp
    public void setLift(bool l, bool active){
        Debug.Log("setLift : " + l);
        if (l & active){
            this.liftHelp += 1;
        }
        if (!l & !active)
        {
            this.liftHelp -= 1;
        }
    }

}
}
