using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Handles selection of the UI tiles for the mites - highlighting tiles and
/// wiring together related events
/// </summary>
public class MiteTileSelect : MonoBehaviour, IPointerClickHandler,
    IPointerEnterHandler, IPointerExitHandler
{

    public Image borderImg;
    public Color selectColor;
    public Color hoverColor;
    public Color idleColor;

    //mite associated with the current tile
    public MiteAttr mite;

    private bool isSelected = false;
    private bool isHovering = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        //toggle selection
        clearStatRadars(); //clear existing (none selected), if necessary
        isSelected = !isSelected;
        setBorderColor(isSelected, isHovering);
        updateStatRadars();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        setBorderColor(isSelected, isHovering);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        setBorderColor(isSelected, isHovering);
    }

    private void setBorderColor(bool selected, bool hovering)
    {
        if (isSelected)
        {
            if (hovering)
            { //hover and select
                borderImg.color = 0.8f * selectColor + 0.2f * hoverColor;
            }
            else
            { //select only
                borderImg.color = selectColor;
            }
        }
        else
        {
            if (hovering)
            { //not selected but hovering
                borderImg.color = hoverColor;
            }
            else
            { //not selected, not hovering
                borderImg.color = idleColor;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(borderImg == null)
        {
            Debug.LogError("missing image for border");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void updateStatRadars()
    {
        if (isSelected)
        {
            if (mite != null)
            {
                SceneManager.instance.updateStatA(mite.statA);
                SceneManager.instance.updateStatB(mite.statB);
            }
            else
            {
                clearStatRadars();
            }
        }
    }

    private void clearStatRadars()
    {
        SceneManager.instance.clearStatA();
        SceneManager.instance.clearStatB();
    }
}
