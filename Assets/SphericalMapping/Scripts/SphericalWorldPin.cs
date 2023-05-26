using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalWorldPin : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Collider _collider;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<MeshCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO: Hand 인색해서 Pin을 뗄 수 있도록 하기
        Debug.Log("<-------- Hand가 Pin에게 접근!");
        if (other.tag == "BaseHand" && 
            _rigidbody.constraints == RigidbodyConstraints.FreezeAll)
        {
            // A의 Collider에서 벗어난 물체를 A의 자식 개체에서 해제
            this.transform.SetParent(null);
            _rigidbody.constraints = RigidbodyConstraints.None;
        }
    }
}
