using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiteAttr : MonoBehaviour
{
    //this object is for display only, not an actual sprite
    private bool isUiOnly = true;

    public StatA statA;
    public StatB statB;
    public float curStamina;

    public JointPoint midPt; //place at center of mass

    public JointPoint[] connectPts;

    // Start is called before the first frame update
    void Start()
    {
        //if there is a sprite renderer then this is attached to a sprite
        if(GetComponent<SpriteRenderer>() != null)
        {
            isUiOnly = false;
        }

        

        if (!isUiOnly)
        {
            if (connectPts == null || connectPts.Length == 0)
            {
                Debug.LogError("No connection points for " + gameObject.name);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Sprite getUiImageSprite()
    {
        return gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    private Vector2 getCenter()
    {
        return midPt.transform.position;
    }

    //Moves and rotates the given limb to attach to this mite
    //Attaches the limb to this gameobject
    public void connectLimb(MiteLimb limb, JointPoint myConPt)
    {
        JointPoint limbPt = limb.conPt;
        Vector3 posDelta = myConPt.transform.position - limbPt.transform.position;

        //move limb into place (note connection point is usually offset from obj origin)
        limb.gameObject.transform.position = limb.gameObject.transform.position + posDelta;

        //rotate to match orientation
        Quaternion rot = limbPt.matchDirection(getCenter(), limb.gameObject.transform.position);
        limb.gameObject.transform.rotation = rot;

        //parent the limb
        limb.gameObject.transform.parent = gameObject.transform;
    }
}
