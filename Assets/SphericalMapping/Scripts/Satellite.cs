using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Satellite : MonoBehaviour
{
    public Vector3 initPos;
    [SerializeField] private uint index;
    [SerializeField] private MarkNode mark;
    [SerializeField] private ProxyNode proxy;
    [SerializeField] private Color color;

    private Vector3 _curPos, _lastPos;

    private void Awake()
    {
        _curPos = transform.localPosition;
        _lastPos = transform.localPosition;
    }

    public void SetSatelliteIndex(uint i)
    {
        index = i;
    }
    
    public void SetProxies(MarkNode m, ProxyNode p)
    {
        mark = m;
        proxy = p;
        color = p.Color;
        GetComponent<MeshRenderer>().materials[0].SetColor("_Color", p.Color);
    }

    private void ScaleMarkedSpace(float delta) 
    {
        //var grabVec = (grabHand.GetGraspPoint() - transform.position) - initialGrabOffset;
        //var baseVec = grabSphere.transform.position - transform.position;

        //var delta = grabVec.magnitude/ baseVec.magnitude - 1;
        //var delta = this.transform.localPosition.magnitude  - 1;

        //Debug.Log(delta);
        //delta = Mathf.Sign(delta) * CoolMath.SmoothStep(0.05f, 0.3f, delta);
        delta = Time.deltaTime * delta * mark.Radius; 
        mark.transform.localScale -= new Vector3(delta, delta, delta);
        mark.transform.localScale.Clamp(SphericaiWorld.MinMarkSize, SphericaiWorld.MaxMarkSize);
    }
    
    private void TranslateMarkedSpace(Vector3 delta) 
    {
        //delta = grabPos - grabSphere.transform.position;
        delta /= mark.Radius;
        //delta.x = Mathf.Sign(delta.x) * CoolMath.SmoothStep(0.1f, 0.2f, Mathf.Abs(delta.x));
        delta.z = delta.y;
        delta.y = 0.0f;
        delta = Time.deltaTime * delta * mark.Radius * (1 / _curPos.y);
        mark.transform.position += delta;
    }
    
    private void Update()
    {
        // 언제나 Spherical WorldMap을 향함
        this.transform.LookAt(SphericaiWorld.Instance.transform);
        
        _curPos = transform.localPosition;
        //Debug.Log(Math.Abs(_curPos.z - _lastPos.z));
        if (Math.Abs(_curPos.z - _lastPos.z) > 0.005f)
            ScaleMarkedSpace((_curPos.z - _lastPos.z) * 150);

        if ((Math.Abs(_curPos.x - _lastPos.x) > 0.005f) || (Math.Abs(_curPos.y - _lastPos.y) > 0.005f))
            //TranslateMarkedSpace((_curPos - _lastPos) * 300);
        _lastPos = _curPos;

        //Debug.Log(this.transform.localPosition.magnitude * 10000);
        if (this.transform.localPosition.magnitude * 10000 > 15000)
        {
            SphericaiWorld.Instance.RemoveSatellite(index);
        }
    }
}
