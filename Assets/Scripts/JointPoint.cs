using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Assign this script to an empty game object that represents a connection
/// point for a joint.  The gameobject position will be used as the attach
/// point, plus or minus the x,y range.
/// </summary>
public class JointPoint : MonoBehaviour
{

    public float xRange = 0.06f;  //movement from midpt
    public float yRange = 0.01f;  //movement from midpt

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Get the rotation that would set joint to point toward hostOrigin.
    /// This assumes the current point is shared between the joint and host.
    /// </summary>
    public Quaternion matchDirection(Vector2 hostOrigin, Vector2 jointOrigin)
    {
        Vector2 myPt = gameObject.transform.position; //connection point
//        Vector2 jointDir = myPt - jointOrigin;
//        Vector2 hostDir = hostOrigin - myPt;
        Vector2 jointDir = myPt - jointOrigin;
        Vector2 hostDir = myPt - hostOrigin;
        return Quaternion.FromToRotation(jointDir, hostDir);
    }
}
