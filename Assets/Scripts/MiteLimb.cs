using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiteLimb : MonoBehaviour
{

    public JointPoint conPt;

    // Start is called before the first frame update
    void Start()
    {
        if(conPt == null)
        {
            Debug.LogError("Missing connection point " + gameObject.name);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
