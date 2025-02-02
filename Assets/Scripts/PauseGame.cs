using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using TMPro;

public class PauseGame : MonoBehaviour
{
    public bool isPaused = false;
    public bool teleportMode = false;
    private bool teleportModeWas;

    private Transform locomotionSystem;
    private Transform cameraOffset;
    private Transform rightHandController;
    private GameObject rightHandControllerGameObject;
    private TeleportationProvider teleProvider;

    private ContinuousMoveProviderBase contMotion;
    private SnapTurnProviderBase snapTurn;
    private ContinuousTurnProviderBase contTurn;

    public InputActionReference pauseGameActionReference;
    XRDirectInteractor dirInteractor;
    XRRayInteractor rayInteractor;

    public GameObject movementModeText;
    private TextMeshProUGUI movementModeTextMesh;

    //exmaple code to turn an object off on a timer
    /*
     object.SetActive(false);
     yield WaitForSeconds(10);
     object.SetActive(true);
    */

    //pasting this in update gives me a red squiggle for `yield` - doesn't exist in this namespace
    //additionally WaitForSeconds is reading as a local function...am i missing a namespace
    /*
     movementModeText.SetActive(false);
     yield WaitForSeconds(10);
     movementModeText.SetActive(true);
    */


    // Start is called before the first frame update
    void Start()
    {
        
        movementModeTextMesh = movementModeText.GetComponent<TextMeshProUGUI>();

        pauseGameActionReference.action.performed += pauseGameAction;

        locomotionSystem = transform.Find("Locomotion System");
        
        teleProvider = locomotionSystem.GetComponent<TeleportationProvider>();
        contMotion = locomotionSystem.GetComponent<ContinuousMoveProviderBase>();
        snapTurn = locomotionSystem.GetComponent<SnapTurnProviderBase>();
        contTurn = locomotionSystem.GetComponent<ContinuousTurnProviderBase>();
    
        cameraOffset = transform.Find("Camera Offset");
        rightHandController = cameraOffset.Find("RightHand Controller");
        rightHandControllerGameObject = rightHandController.gameObject;
        dirInteractor = rightHandControllerGameObject.GetComponentInChildren<XRDirectInteractor>();
        rayInteractor = rightHandControllerGameObject.GetComponentInChildren<XRRayInteractor>();

        rayInteractor.gameObject.SetActive(false);
        dirInteractor.gameObject.SetActive(false);
        
        if(teleportMode){
            teleProvider.enabled = true;
            contMotion.enabled = false;
            snapTurn.enabled = true;
            contTurn.enabled = false;
            //rayInteractor = rightHandControllerGameObject.AddComponent(typeof(XRRayInteractor)) as XRRayInteractor;
            rayInteractor.gameObject.SetActive(true);

        } else {
            teleProvider.enabled = false;
            contMotion.enabled = true;
            snapTurn.enabled = false;
            contTurn.enabled = true;
            //dirInteractor = rightHandControllerGameObject.AddComponent(typeof(XRDirectInteractor)) as XRDirectInteractor;
            dirInteractor.gameObject.SetActive(true);
        }
        teleportModeWas = teleportMode;
    }

    private void pauseGameAction(InputAction.CallbackContext obj)
    {
        if(teleportMode){
            teleportMode = false;
            movementModeTextMesh.text = "free movement mode";
            StartCoroutine(displayMovementModeText());
        }
        else if (!teleportMode){
            teleportMode = true;
            movementModeTextMesh.text = "teleport mode";
            StartCoroutine(displayMovementModeText());
        }

        //toggleTeleportMode();

    }

    private void toggleTeleportMode(){

        //i reversed the code below, but i'm still seeing "can't add x, can only have one XRbase"
        if(teleportMode){
            teleProvider.enabled = false;
            contMotion.enabled = true;
            snapTurn.enabled = false;
            contTurn.enabled = true;
            if(rayInteractor){
	            Destroy(rayInteractor);
            }
            dirInteractor = rightHandControllerGameObject.AddComponent(typeof(XRDirectInteractor)) as XRDirectInteractor;
        } else {
            teleProvider.enabled = true;
            contMotion.enabled = false;
            snapTurn.enabled = true;
            contTurn.enabled = false;
            if(dirInteractor){
	            Destroy(dirInteractor);
            }
            rayInteractor = rightHandControllerGameObject.AddComponent(typeof(XRRayInteractor)) as XRRayInteractor;

        }  
        teleportMode = !teleportMode;
    }

    IEnumerator displayMovementModeText(){
        movementModeText.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        movementModeText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(teleportMode != teleportModeWas){
            teleportModeWas = teleportMode;
            if(teleportMode){
                teleProvider.enabled = true;
                contMotion.enabled = false;
                snapTurn.enabled = true;
                contTurn.enabled = false;
                dirInteractor.gameObject.SetActive(false);
                rayInteractor.gameObject.SetActive(true);
                /*
                if(rightHandControllerGameObject.GetComponent<XRDirectInteractor>()){
                    DestroyImmediate(rightHandControllerGameObject.GetComponent<XRDirectInteractor>());
                    //destroy will destroy at the end of frame
                    //as opposed to DestroyImmediate() <- use selectively
                }
                rayInteractor = rightHandControllerGameObject.AddComponent(typeof(XRRayInteractor)) as XRRayInteractor;
                */

            } else {
                teleProvider.enabled = false;
                contMotion.enabled = true;
                snapTurn.enabled = false;
                contTurn.enabled = true;
                dirInteractor.gameObject.SetActive(true);
                rayInteractor.gameObject.SetActive(false);
                /*
                if(rightHandControllerGameObject.GetComponent<XRRayInteractor>()){
                    DestroyImmediate(rightHandControllerGameObject.GetComponent<XRRayInteractor>());
                }
                dirInteractor = rightHandControllerGameObject.AddComponent(typeof(XRDirectInteractor)) as XRDirectInteractor;
                */
            }   
        }
   }
}