using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{

    //constants for all moving mites
    public const float baseSpeed = 0.1f;
    public const float baseDrag = 0.09f;

    public const float F = 0.025f; //speed factor
    public const float D = 0.013f; //drag factor
    public const float H = 0.0125f; //Heavy factor
    public const float SM = 0.02f; //Min smooth factor
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

        //TODO remove hardcode
        prepareToMove(GetComponent<MiteAttr>());
        startMove();
    }

    void prepareToMove(MiteAttr attr)
    {
        statA = attr.statA;
        statB = attr.statB;
        curStamina = attr.curStamina;
        mySpeedDelta = calcFlatSpeedDelta(0);
        myMaxSpeed = calcMaxSpeed();

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

        mySpeedDelta = calcFlatSpeedDelta(lastSpeed);
        lastSpeed += mySpeedDelta;
        lastSpeed = Mathf.Min(lastSpeed, myMaxSpeed);
        //Debug.Log("speed: " + lastSpeed);
        //TODO stamina update (from damage, e.g.)

        //do the move
        float newX = transform.position.x + lastSpeed * Time.deltaTime;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

    }

    /// <summary>
    /// Get max speed based on intelligence and current stamina
    /// </summary>
    /// <returns></returns>
    public float calcMaxSpeed()
    {
        float maxS = MI * statA.smart
            - MS * (statB.maxDurable - curStamina);
        //Debug.Log(gameObject.name + " maxSpeed:" + maxS);

        return maxS;
    }

    /// <summary>
    /// Calc speed in linear direction of movement. Positive values go forward.
    /// </summary>
    /// <returns></returns>
    public float calcFlatSpeedDelta(float curSpeed)
    {
        float s = baseSpeed + (F * statA.fast);
        //heavy smooth => little drag
        float drag = baseDrag 
            + D * H * statB.heavy * Mathf.Max(MS, (statB.maxSmooth - statB.smooth));

        float speedDelta = s - (curSpeed * drag);
        //Debug.Log(gameObject.name + " speed:" + s + ", drag:" + drag + ", speedDelta:" + speedDelta);

        return speedDelta;
    }
}
