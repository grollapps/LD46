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
    public Image bgFillImg;
    public Image charImg;

    public Color selectColor;
    public Color hoverColor;
    public Color idleColor;

    //mite associated with the current tile
    public MiteAttr mite;

    private Color origFillColor;

    private Color whiter = new Vector4(1.1f, 1.1f, 1.1f, 1f);

    private bool isSelected = false;
    private bool isHovering = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        //toggle border selection
        isSelected = !isSelected;
        setBorderColor(isSelected, isHovering);
        //toggle global stat selection indicator
        bool isStatSelected = SceneManager.instance.toggleStatSelect(this);
        setBgFillColor(isStatSelected);
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

    //set the inner fill color based on being selected or not
    private void setBgFillColor(bool bgSelected)
    {
        if (bgSelected)
        {
            bgFillImg.color = 0.6f * origFillColor + (0.4f * hoverColor * whiter);
        }
        else
        {
            bgFillImg.color = origFillColor;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(borderImg == null)
        {
            Debug.LogError("missing image for border");
        }
        if (charImg == null)
        {
            Debug.LogError("missing char image slot");
        }
        origFillColor = bgFillImg.color;

        if(mite != null)
        {
            Debug.Log("Setting mite image using " + mite.gameObject.name);
            charImg.sprite = mite.getUiImageSprite();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Set bg fill as unselected (stat)
    /// </summary>
    public void unselectStat()
    {
        setBgFillColor(false);
    }

    public StatA getStatA()
    {
        return mite.statA;
    }

    public StatB getStatB()
    {
        return mite.statB;
    }

}
