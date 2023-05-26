using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Satellite : MonoBehaviour
{
    public Vector3 pos;
    private MarkNode markRoi;
    private ProxyNode proxy;

    public void SetProxies(MarkNode m, ProxyNode p)
    {
        markRoi = m;
        proxy = p;
    }
}
