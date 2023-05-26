using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class TransformCoord : MonoBehaviour
{
    [Header("Basic")]
    public GameObject sphericalWorld;
    public GameObject prefab;
    public GameObject cam;
    private SphericaiWorld _sphereWorld;
    
    [Header("Setting")]
    //public float scale_ = 20.0f;
    public GameObject standard;
    public Transform spawnPos;
    [SerializeField] private float _radius = 0.5f;
    [SerializeField] private Vector3 _camPosition;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _sphereWorld = sphericalWorld.GetComponent<SphericaiWorld>();
            
        Collider collider = GetComponent<Collider>();
        
        _camPosition = cam.transform.position;
        _radius = GetComponent<SphereCollider>().radius;

        Vector3 localUp = new Vector3(Mathf.Cos(transform.rotation.z), Mathf.Cos(transform.rotation.y), Mathf.Cos(transform.rotation.x));
        Debug.DrawLine(this.transform.position, this.transform.position + this.transform.up * 50, Color.green, 400.0f);
        Debug.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 50, Color.blue, 400.0f);
        Debug.DrawLine(this.transform.position, this.transform.position + this.transform.right * 50, Color.red, 400.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Collision 발생" + other.gameObject.tag);
        if (other.gameObject.tag == "Roi")
        {
            Vector3 proPos = other.transform.position;
            other.gameObject.transform.SetParent(this.transform);
            //other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            Rigidbody rigidbody = other.gameObject.GetComponent<Rigidbody>();
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;

            Vector3 sphereNormalVec = other.gameObject.transform.localPosition;
            Vector3 sphereVec = other.gameObject.transform.position;

            getSphericalAngle(sphereNormalVec);
            
            //Debug.Log("Sphere Normalized Vector" + sphereNormalVec);
            // Vector3 sphericalCoordinates = getSphericalCoordinates(other.gameObject.transform.localPosition, _radius);
            // sphericalCoordinates = Quaternion.AngleAxis(-90, Vector3.right) * sphericalCoordinates.normalized;
            // Vector3 cartesianCoordinates = getCartesianCoordinates(sphericalCoordinates);

            Vector3 markPos = new Vector3(sphereNormalVec.x, 0.001f, sphereNormalVec.z);
            //markPos = Quaternion.AngleAxis(90, Vector3.up) * markPos.normalized;
            float scale = (sphereNormalVec - standard.transform.localPosition).sqrMagnitude *400;
            
            Vector3 markedPos = makeMarkObject(markPos, scale);
            _sphereWorld.CreateProxies(markedPos, 100.0f, spawnPos.position);
        }
    }
    

    private void getSphericalAngle(Vector3 point)
    {
        // 구의 중심 좌표
        //Vector3 sphereCenter = this.transform.position;
        Vector3 sphereCenter = new Vector3(0, 0, 0);
        
        // 중심과 점 사이의 직선 벡터 계산
        Vector3 between = new Vector3(point.x - sphereCenter.x, point.y - sphereCenter.y, point.z - sphereCenter.z);
        
        // 직선 벡터를 구의 표면과의 교점으로 정규화
        double length = Math.Sqrt(between.x * between.x + between.y * between.y + between.z * between.z);
        Vector3 normalize = new Vector3(between.x / (float)length, between.y / (float)length, between.z / (float)length);
        
        // 각도 계산
        double angle = Math.Acos(-normalize.z) * 180 / Math.PI;
    
        Debug.Log("Sphere Normalized Vector: " + angle);
        Debug.DrawLine(sphereCenter, sphereCenter + point * 3, Color.magenta, 200.0f, false);
    }

    private Vector3 makeMarkObject(Vector3 cartesian, float scale)
    {
        GameObject instance = Instantiate(prefab);
        instance.transform.position = new Vector3(
            _camPosition.x + cartesian.x,
            0.03f,
            (_camPosition.z + cartesian.z)
        );
        
        instance.transform.position *= scale;

        return instance.transform.position;
        //Debug.Log("기준으로부터 "+ scale);
    }

    public void Update()
    {
        //_camPosition = cam.transform.position;
        //Debug.Log(_camPosition);
    }

    private Vector3 getSphericalCoordinates(Vector3 cartesian, float radius)
    {
        float r = radius;
            // Mathf.Sqrt(
            // Mathf.Pow(cartesian.x, 2) + 
            // Mathf.Pow(cartesian.y, 2) + 
            // Mathf.Pow(cartesian.z, 2));

        // use atan2 for built-in checks
        float phi = Mathf.Atan2(cartesian.z, cartesian.x);
        float theta = Mathf.Asin(cartesian.y / r);

        return new Vector3(r, phi, theta);
    }

    private Vector3 getCartesianCoordinates(Vector3 spherical)
    {
        Vector3 ret = new Vector3 ();

        float t = _radius * Mathf.Cos(spherical.z);
        
        ret.x = t * Mathf.Cos (spherical.y);
        ret.y = _radius*Mathf.Sin(spherical.z);
        ret.z = t * Mathf.Sin (spherical.y);
 
        return ret;
    }
    
}
