using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Satellite : MonoBehaviour
{
    public Vector3 pos;
    [SerializeField] private uint index;
    [SerializeField] private MarkNode markRoi;
    [SerializeField] private ProxyNode proxy;

    public void SetSatelliteIndex(uint i)
    {
        index = i;
    }
    
    public void SetProxies(MarkNode m, ProxyNode p)
    {
        markRoi = m;
        proxy = p;
    }
}
