using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckInteraction : MonoBehaviour
{

    [SerializeField]
    private float minInteractionDistance;
   
    [SerializeField]
    private GameObject rayOrigin;


    private Ray ray;

    private bool canInteract;

    private InteractionReceiver currentReceiver;


    private void Start()
    {

    }

    void Update()
    {
        CheckRaycast();
        if (canInteract)
        {
           
            if (Input.GetKeyDown(KeyCode.E))
            {
                currentReceiver.Activate();
            }
            
        }

    }

    private void CheckRaycast()
    {
        ray = new Ray(rayOrigin.transform.position, rayOrigin.transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
           

            if (hit.distance < minInteractionDistance)
            {


                currentReceiver = hit.transform.gameObject.GetComponent<InteractionReceiver>();

                if (currentReceiver != null)
                {

                    Debug.Log(currentReceiver.GetInteractionMessage());
 
                    canInteract = true;
                   
                }
                else
                {
                    canInteract = false;
                }
            }
        }

      
    }

}