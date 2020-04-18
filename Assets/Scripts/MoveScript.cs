using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{

    //constants for all moving mites
    public const float baseSpeed = 0.1f;
    public const float baseDrag = 0.001f;

    public const float F = 0.5f; //speed
    public const float D = 0.01f; //drag
    public const float H = 0.0025f; //Heavy factor
    public const float MI = 2.5f; //max speed intelligence factor
    public const float MS = 0.5f; //max speed stamina factor

    public const float moveScale = 1.0f; //multiplied times transform

    //set attributes before moving
    StatA statA;
    StatB statB;
    float curStamina;
    float mySpeedDelta;
    float myMaxSpeed;

    //updated per frame
    bool started;
    float lastSpeed;
    
    
    // Start is called before the first frame update
    void Start()
    {
        lastSpeed = 0;
        started = false;
        mySpeedDelta = 0;
    }

    void prepareToMove(MiteAttr attr)
    {
        statA = attr.statA;
        statB = attr.statB;
        curStamina = attr.curStamina;
        mySpeedDelta = calcFlatSpeedDelta();

        Debug.Log(gameObject.name + " speedDelta:" + mySpeedDelta);
    }

    void startMove()
    {
        Debug.Log(gameObject.name + " startMove");
        started = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!started) { return; }

        lastSpeed += mySpeedDelta;
        lastSpeed = Mathf.Min(lastSpeed, myMaxSpeed);
    }

    /// <summary>
    /// Get max speed based on intelligence and current stamina
    /// </summary>
    /// <returns></returns>
    public float calcMaxSpeed()
    {
        float maxS = MI * statA.smart
            - MS * (statB.maxDurable - curStamina);
        Debug.Log(gameObject.name + " maxSpeed:" + maxS);

        return maxS;
    }

    /// <summary>
    /// Calc speed in linear direction of movement. Positive values go forward.
    /// </summary>
    /// <returns></returns>
    public float calcFlatSpeedDelta()
    {
        float s = baseSpeed + (F * statA.fast);
        //heavy smooth => little drag
        float drag = baseDrag 
            + D * H * statB.heavy * (statB.maxSmooth - statB.smooth);

        float speedDelta = s + drag;
        Debug.Log(gameObject.name + " speed:" + s + ", drag:" + drag + ", speedDelta:" + speedDelta);

        return speedDelta;
    }
}
