using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Type B attributes: primary (inverse)
/// Heavy (light)
/// Smooth (rough)
/// Durable (weak)
/// Mutative (non-mutative)
/// Stiff (soft)
/// </summary>
public class StatB : MonoBehaviour
{
    const float defMax = 3;

    public float heavy;
    public float smooth;
    public float durable;
    public float mutative;
    public float stiff;

    public float maxSmooth = defMax;
    public float maxHeavy = defMax;
    public float maxDurable = defMax;
    public float maxMutative = defMax;
    public float maxStiff = defMax;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
