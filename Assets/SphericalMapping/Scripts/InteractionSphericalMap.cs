using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Interaction;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(InteractionBehaviour))]
public class InteractionSphericalMap : MonoBehaviour
{
    private GameObject cam;
    private GameObject sphericalMap;

    private InteractionBehaviour grabSphericalMapLeap;
    private InteractionHand grabHand;
    private Vector3 initialGrabOffset;
    private Material grabSphereMaterial;
    private Vector3 _standard;
    
    private Color GrabSphereDefaultColor = new Color(0.2f, 0.6f, 0.2f, 0.9f);
    private Color GrabSphereGrabbedColor = new Color(0.8f, 0.1f, 0.1f, 0.9f);

    // Start is called before the first frame update
    void Start()
    {
        cam = SphericaiWorld.Instance.cam;
        sphericalMap = SphericaiWorld.Instance.sphericalMap;

        GameObject s = GameObject.Find("standard");
        Transform P = s.transform.parent;
        s.transform.SetParent(null);
        _standard = s.transform.position;
        s.transform.SetParent(P);

        grabSphericalMapLeap = GetComponent<InteractionBehaviour>();
        grabSphericalMapLeap.OnGraspBegin += GrabSphereGraspBegin;
        grabSphericalMapLeap.OnGraspStay += GrabSphereGraspStay;
        grabSphericalMapLeap.OnGraspEnd += GrabSphereGraspedEnd;

        grabSphereMaterial = sphericalMap.GetComponent<MeshRenderer>().material;
    }

    private void GrabSphereGraspBegin() 
    {
        Debug.Log("GrabSphereGraspBegin");
        grabHand = grabSphericalMapLeap.graspingController.intHand;
        if(grabHand.NoFistPinchStrength() < 0.6f) 
        {
            //initialGrabOffset = grabHand.GetGraspPoint() - grabSphere.transform.position;
            initialGrabOffset = grabHand.GetGraspPoint() - _standard;
            //grabSphereMaterial.DOColor(GrabSphereGrabbedColor, 0.1f);
        }
    }

    private void GrabSphereGraspedEnd() 
    {
        Debug.Log("GrabSphereGraspedEnd");
        grabHand = null;
        //grabSphereMaterial.DOColor(GrabSphereDefaultColor, 0.1f);
    }

    private void GrabSphereGraspStay() 
    {
        //if (grabHand.isLeft)
        {
            TranslateSphericalMap();
            //ScaleMarkedSpace();
        }
        // else if (grabHand == rightHand.Leap)
        // {
        //     TranslateMarkedSpace();
        // }
    }

    public void ScaleSphericalMap()
    {
        // Scale은 Move이후에 진행
    }
    
    public void TranslateSphericalMap()
    {
        var grabPos = grabHand.GetGraspPoint();
        Debug.DrawLine(grabPos, _standard, Color.green, 3.0f);

        var delta = grabPos - _standard;
        var vel = 0.1f; // 너무 빠르게 돌지 않도록
        
        //delta /= mark.Radius;
        //delta.x = Mathf.Sign(delta.x) * CoolMath.SmoothStep(0.1f, 0.2f, delta.x);
        delta.x = Mathf.Sign(delta.x) * CoolMath.SmoothStep(0.1f, 0.2f, Mathf.Abs(delta.x));
        //delta.x = Mathf.Sign(delta.z) * CoolMath.SmoothStep(0.1f, 0.2f, Mathf.Abs(delta.z));
        
        delta.y = 0.0f; //delta.y = Mathf.Sign(delta.y) * CoolMath.SmoothStep(0.1f, 0.2f, Mathf.Abs(delta.y));
        //delta.z = Mathf.Sign(delta.z) * CoolMath.SmoothStep(0.1f, 0.2f, delta.z);
        delta.z = Mathf.Sign(delta.z) * CoolMath.SmoothStep(0.1f, 0.2f, delta.z);
        //delta.z = Mathf.Sign(delta.x) * CoolMath.SmoothStep(0.1f, 0.2f, Mathf.Abs(delta.x));

        Debug.Log("Standard" + _standard + ", grabPos:" + grabPos + ", delta:" + delta);
        //delta = Time.deltaTime * delta * mark.Radius;
        cam.transform.position += delta * delta.sqrMagnitude * vel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
