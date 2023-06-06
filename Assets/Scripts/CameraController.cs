using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public float range = 100.0f;

	
	// Use this for initialization
    void Start () 
    {
		
	}

    private void CamRangeCheck()
    {
	    // x, z축의 범위를 벗어난다면
	    Vector2 dis = new Vector2(this.transform.position.x, this.transform.position.z);
	    //Debug.Log(dis.sqrMagnitude);
	    if (range * range < dis.sqrMagnitude)
	    {
		    Vector3 newPos = new Vector3(-this.transform.position.x,
			    this.transform.position.y,
			    -this.transform.position.z);
		    
		    this.transform.position = newPos;
		    Debug.Log("Camera Pos Change");
	    }
    }
    
	// Update is called once per frame
	void Update () 
	{
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 100.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 100.0f;

        transform.Translate(x, 0, 0);
        transform.Translate(0, 0, z);

        CamRangeCheck();
	}
}
