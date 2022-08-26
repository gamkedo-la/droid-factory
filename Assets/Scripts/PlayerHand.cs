using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    private GameObject touchedObject = null;
    private Rigidbody touchedRB = null;
    private bool isGrabbing = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrabbing)
        {
            touchedObject.transform.position = transform.position;
            touchedObject.transform.rotation = transform.rotation;
        }
    }

    public void Grab()
    {
        if (isGrabbing)
        {
            LetGo();
            return;
        }

        if (touchedObject)
        {
            if (touchedObject.tag == "DroidPart" || touchedObject.tag =="Interactable")
            {
                isGrabbing = true;
                touchedRB = touchedObject.GetComponent<Rigidbody>();
                touchedRB.isKinematic = true;
                Collider [] colliders = touchedObject.GetComponentsInChildren<Collider>();
                foreach (Collider collider in colliders)
                {
                    collider.enabled = false;
                }    
                OpenBook OBscript = touchedObject.GetComponent<OpenBook>();
                if (OBscript){
                    OBscript.OpenSesame();
                }else{
                    Debug.Log("no openbookscript found");
                }
                
            }
        }
    }

    public void LetGo()
    {
        isGrabbing = false;
        touchedRB.isKinematic = false; //check position after this
        Collider[] colliders = touchedObject.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
        if (touchedObject.tag == "DroidPart")
        {
            touchedObject.GetComponent<DroidPart>()?.checkPosition();

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (isGrabbing)
        {
            return;
        }

        touchedObject = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (isGrabbing)
        {
            return;
        }
        touchedObject = null;
    }
}
