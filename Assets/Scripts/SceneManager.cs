using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds global references
/// </summary>
public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;

    [SerializeField]
    private RadarGridChart statADisp;

    [SerializeField]
    private RadarGridChart statBDisp;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this) { 
                Destroy(gameObject);
            }
        }
    }  //might need to set don't destroy on load?

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clearStatA()
    {
        if (statADisp != null)
        {
            statADisp.setValues(0, 0, 0, 0, 0);
        }
    }

    public void clearStatB()
    {
        if (statBDisp != null)
        {
            statBDisp.setValues(0, 0, 0, 0, 0);
        }
    }

    public void updateStatA(StatA stats)
    {
        if (statADisp == null)
        {
            Debug.LogError("StatA display not set");
        }
        else
        {
            statADisp.setValues(stats.smart, stats.fertile,
                stats.unstable, stats.fast, stats.valuable);
        }
    }

    public void updateStatB(StatB stats)
    {
        if (statBDisp == null)
        {
            Debug.LogError("StatB display not set");
        }
        else
        {
            statBDisp.setValues(stats.heavy, stats.smooth,
                stats.durable, stats.mutative, stats.stiff);
        }
    }
}
