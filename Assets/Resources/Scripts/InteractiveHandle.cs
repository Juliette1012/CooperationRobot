using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace WasaaMP {
public class InteractiveHandle : MonoBehaviourPun {
    // Start is called before the first frame update

    MonoBehaviourPun support = null ;

    [Header("Rotations")]
    public bool xRotation = true;
    public bool zRotation = false;

    [Space(10)]
    public bool selected = false;

    private Transform planeTransform;

    private Transform sphereTransform;
    //private CursorTool sphere;
    private GameObject plane;

    private float positionYWhenSelected = 0.0f;
    private float deltaY;

    private Interactive interactPlane;

    // a boolean to know if the handle is selected on the update before or no
    [Header("Lift")]
    public bool liftHelp = false;
    public bool active = false;

    void Start () {
        if(transform.parent != null)
            planeTransform = transform.parent.transform;
        plane = this.transform.parent.gameObject;

        // get the script Interactive of the plane
        interactPlane = plane.GetComponent<Interactive>();
    }

    // Update is called once per frame
    void Update () {
        if(selected) {
            Debug.Log("is selected");
            
            Renderer renderer = GetComponentInChildren <Renderer> () ;
            if(renderer.material.color != Color.red)
                renderer.material.color = Color.red;

            if(Input.GetKeyDown(KeyCode.Escape)){
                selected = false;
                renderer.material.color = Color.white;

                liftHelp = false;
            }

            deltaY = positionYWhenSelected - sphereTransform.position.y;
            Debug.Log("deltaY : " + deltaY);

            // Call the different functions RPC to move the plane 
            // Update the variable liftHelp if it is necessary
            photonView.RPC("Lift", RpcTarget.All, interactPlane.photonView.ViewID, liftHelp, active);
            // Update the transform of the plane
            photonView.RPC("RotateSupport", RpcTarget.All, interactPlane.photonView.ViewID, deltaY, xRotation, zRotation);
            interactPlane.photonView.TransferOwnership (PhotonNetwork.LocalPlayer);
            PhotonNetwork.SendAllOutgoingCommands();

            // to consider the help on the handle just one time (to have liftHelp +=1 in Interactive script)
            // Need the variable liftHelp = 2 (2 persons/forces) to move the plane
            if (liftHelp & active){
                active = false;
            }
        }
    }

    // Function RPC to modify the variable liftHelp on the Interactive script of the plane 
    [PunRPC] public void Lift(int interactiveID, bool l, bool active){
        Interactive go = PhotonView.Find (interactiveID).gameObject.GetComponent<Interactive> ();
        go.setLift(l, active);
        Debug.Log("Lift");
    }
    
    // Function RPC to modify the transform of the plane on the Interactive script of the plane
    [PunRPC] public void RotateSupport (int interactiveID, float deltaY, bool xRotation, bool zRotation) {
        Interactive go = PhotonView.Find (interactiveID).gameObject.GetComponent<Interactive> ();
        go.Rotate(deltaY, xRotation, zRotation);
        Debug.Log("RotateSupport");
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

    void OnTriggerStay (Collider other) {
        var hit = other.gameObject ;
		var cursor = hit.GetComponent<CursorTool> () ;
        if((name == "XHandle" || name == "ZHandle") && Input.GetKeyDown(KeyCode.Space)) {
            print(name + " is selected");
            selected = true;
            sphereTransform = cursor.GetComponent<Transform>();
            positionYWhenSelected = sphereTransform.position.y;
            liftHelp = true;
            active = true;
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
}

}
