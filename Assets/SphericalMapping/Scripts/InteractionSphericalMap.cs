using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Interaction;
using UnityEngine;

[RequireComponent(typeof(InteractionBehaviour))]
public class InteractionSphericalMap : MonoBehaviour
{
    [Header("Setting")]
    public GameObject cam;
    
    private InteractionBehaviour interactionBehaviour;

    public void MoveSphericalCamera()
    {
        //interactionBehaviour.GetGraspPoint(interactionBehaviour.controller);
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
