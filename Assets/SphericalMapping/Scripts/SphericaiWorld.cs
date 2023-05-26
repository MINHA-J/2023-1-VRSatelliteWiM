using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SphericaiWorld : MonoBehaviour
{
    [Header("Basic")] 
    public List<GameObject> satellite = new List<GameObject>();
    
    // Create Proxy
    OneEuroFilter<Vector3> markPosFilter = new OneEuroFilter<Vector3>(30.0f, 0.3f);
    OneEuroFilter<Vector3> proxyPosFilter = new OneEuroFilter<Vector3>(30.0f, 0.3f);
    OneEuroFilter markScaleFilter = new OneEuroFilter(30.0f);
    OneEuroFilter proxyScaleFilter = new OneEuroFilter(30.0f);
    
    void Start()
    {
        
    }

    /// <summary>
    /// Spherical world에서 Pin으로 선택된 ROI를 Set
    /// </summary>
    /// <param name="pin">Detect Collider</param>
    public void SetSatellite(GameObject pin)
    {

    }
    
    public void CreateProxies(Vector3 M_pos, float size, Vector3 P_pos)
    {
        GameObject EntryWarp = Resources.Load("Prefabs/ProxyNode", typeof(GameObject)) as GameObject;
        GameObject ExitWarp = Resources.Load("Prefabs/MarkNode", typeof(GameObject)) as GameObject;
        
        GameObject markedSpace = Instantiate(ExitWarp);
        GameObject proxySpace = Instantiate(EntryWarp);
        
        ProxyNode proxyNode = proxySpace.GetComponent<ProxyNode>();
        MarkNode markNode = markedSpace.GetComponent<MarkNode>();
        
        proxyNode.Marks.Add(markNode);
        proxyNode.SetCreationMode(true);

        float proxyFilteredSize = 0, markFilteredScale = 0;


        markPosFilter.Filter(M_pos);
        proxyPosFilter.Filter(P_pos);

        proxyFilteredSize = proxyScaleFilter.Filter(1.0f);
        markFilteredScale = markScaleFilter.Filter(3.0f);


        markedSpace.transform.position = M_pos;
        markedSpace.transform.localScale = new Vector3(markFilteredScale, markFilteredScale, markFilteredScale);

        proxySpace.transform.position = P_pos;
        proxySpace.transform.localScale = new Vector3(proxyFilteredSize, proxyFilteredSize, proxyFilteredSize);
        
        proxyNode.SetCreationMode(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
