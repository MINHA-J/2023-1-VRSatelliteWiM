using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SphericaiWorld : MonoBehaviour
{
    [Header("Basic")]
    public Dictionary<uint, GameObject> SatelliteTable = new Dictionary<uint, GameObject>();
    public Dictionary<uint, ProxyNode> ProxiesTable = new Dictionary<uint, ProxyNode>();
    
    [Header("Setting")]
    public GameObject cam;
    public GameObject sphericalMap;
    public GameObject detection;
    public GameObject satellites;
    
    [Header("Prefabs")]
    public GameObject prefabSatellite;
    
    public const float MinMarkSize = 0.1f;
    public const float MaxMarkSize = 10.0f;
    
    // Create Proxy
    OneEuroFilter<Vector3> markPosFilter = new OneEuroFilter<Vector3>(30.0f, 0.3f);
    OneEuroFilter<Vector3> proxyPosFilter = new OneEuroFilter<Vector3>(30.0f, 0.3f);
    OneEuroFilter markScaleFilter = new OneEuroFilter(30.0f);
    OneEuroFilter proxyScaleFilter = new OneEuroFilter(30.0f);

    
    #region SphericaiWorld Singleton 
    private static SphericaiWorld instance;

    public static SphericaiWorld Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }

            return instance;
        }
    }
    #endregion
    void Awake()
    {
        if (null == instance)
        {
            //이 클래스 인스턴스가 탄생했을 때 전역변수 instance에 게임매니저 인스턴스가 담겨있지 않다면, 자신을 넣어준다.
            instance = this;

            //씬 전환이 되더라도 파괴되지 않게 한다.
            //gameObject만으로도 이 스크립트가 컴포넌트로서 붙어있는 Hierarchy상의 게임오브젝트라는 뜻이지만, 
            //나는 헷갈림 방지를 위해 this를 붙여주기도 한다.
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //만약 씬 이동이 되었는데 그 씬에도 Hierarchy에 GameMgr이 존재할 수도 있다.
            //그럴 경우엔 이전 씬에서 사용하던 인스턴스를 계속 사용해주는 경우가 많은 것 같다.
            //그래서 이미 전역변수인 instance에 인스턴스가 존재한다면 자신(새로운 씬의 GameMgr)을 삭제해준다.
            //Destroy(this.gameObject);
        }
    }
    void Start()
    {
        
    }

    /// <summary>
    /// Spherical world에서 Pin으로 선택된 ROI를 Set, Satellite를 생성
    /// </summary>
    /// <param name="pin">Detect Collider</param>
    public void CreateSatellite(uint index, Vector3 pos)
    {
        //.GetComponent<Satellite>();
        GameObject instance = Instantiate(prefabSatellite);
        instance.transform.position = this.transform.position + (pos * 0.7f);
        instance.transform.LookAt(this.transform);
        instance.transform.SetParent(satellites.transform);
        //instance.

        instance.GetComponent<Satellite>().initPos = instance.transform.localPosition;
        instance.GetComponent<Satellite>().SetSatelliteIndex(index);
        instance.GetComponent<Satellite>().SetProxies(ProxiesTable[index].Marks[0], ProxiesTable[index]);
        SatelliteTable.Add(index, instance);
    }
    
    public void CreateProxies(uint index, Vector3 M_pos, float size, Vector3 P_pos)
    {
        //GameObject EntryWarp = Resources.Load("Prefabs/ProxyNode_fix", typeof(GameObject)) as GameObject;
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

        proxyFilteredSize = proxyScaleFilter.Filter(0.5f);
        markFilteredScale = markScaleFilter.Filter(1.0f);


        markedSpace.transform.position = M_pos;
        markedSpace.transform.localScale = new Vector3(markFilteredScale, markFilteredScale, markFilteredScale);

        proxySpace.transform.position = P_pos;
        proxySpace.transform.localScale = new Vector3(proxyFilteredSize, proxyFilteredSize, proxyFilteredSize);
        
        proxyNode.SetCreationMode(false);
        
        ProxiesTable.Add(index, proxyNode);
    }

    public bool CanDeployProxies(Vector3 tempPos, float range)
    {
        // 주변에 이미 배치된 proxy가 존재하는 경우, Pass함
        foreach (var elem in ProxiesTable )
        {
            ProxyNode proxy = elem.Value;
            if ((tempPos - proxy.Marks[0].transform.position).sqrMagnitude < range)
            {
                return false;
            }
        }
        return true;
    }

    public void RemoveSatellite(uint index)
    {
        // Satellite를 Table에서 제거함
        GameObject satellite = SatelliteTable[index];
        SatelliteTable.Remove(index);
        Destroy(satellite);

        // Proxies를 Table에서 제거함
        ProxyNode proxyNode = ProxiesTable[index];
        MarkNode markNode = proxyNode.Marks[0];
        ProxiesTable.Remove(index);
        Destroy(proxyNode.gameObject);
        Destroy(markNode.gameObject);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
