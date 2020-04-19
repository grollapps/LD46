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

    private bool isPlaying = true; //is running a trial

    private Vector3 origCameraPos;
    private GameObject trackGo = null;

    private MiteTileSelect selectedMiteTile = null;

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
        origCameraPos = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying) {
            if (trackGo != null)
            {
                Vector3 trackPos = trackGo.transform.position;
                Camera.main.transform.position = new Vector3(trackPos.x, origCameraPos.y, origCameraPos.z);
            }
        }
        
    }

    public void resetCameraPos()
    {
        Camera.main.transform.position = origCameraPos;
    }


    /// <summary>
    /// Toggle stat selection on the given tile.  Returns true if the tile is now selected
    /// for showing stats.  
    /// Updates stat view accordingly.
    /// Targets selected mite when playing.
    /// </summary>
    /// <param name="tile"></param>
    /// <returns></returns>
    public bool toggleStatSelect(MiteTileSelect tile)
    {
        bool result = false;
        if(tile == null)
        {
            //unselect all current
            selectedMiteTile.unselectStat();
            selectedMiteTile = null;
            result = false;
        }else if(selectedMiteTile == null)
        {
            //nothing selected yet
            selectedMiteTile = tile;
            result = true;
        }else if(GameObject.ReferenceEquals(selectedMiteTile.gameObject, tile.gameObject))
        {
            //tile was already selected, stay selected.
            result = true;
            //selectedMiteTile.unselectStat();
            //selectedMiteTile = null;
        }
        else
        {
            //different tile was selected. unselect existing, select new
            selectedMiteTile.unselectStat();
            selectedMiteTile = tile;
            result = true;
        }

        if(selectedMiteTile != null)
        {
            updateStatA(selectedMiteTile.getStatA());
            updateStatB(selectedMiteTile.getStatB());
        }
        else
        {
            clearStatA();
            clearStatB();
        }

        targetCameraToSelectedMite();

        return result;
    }

    public void targetCameraToSelectedMite()
    {
        if(selectedMiteTile != null)
        {
            trackGo = selectedMiteTile.mite.gameObject;
        }
        else
        {
            trackGo = null;
        }
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
