using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the radar grid chart rendering
/// </summary>
public class RadarGridChart : MonoBehaviour
{
    //Set endpoints for the material renderer
    //Coordinates are (0,0) to (1,1) (texture uvs)
    public Vector2 MidVert = new Vector2(0.5f,0.5f);
    public Vector2 TopVert = new Vector2(0.5f,0.8f);
    public Vector2 TopRightVert = new Vector2(0.8f,0.75f);
    public Vector2 BottomRightVert = new Vector2(0.75f,0.25f);
    public Vector2 BottomLeftVert = new Vector2(0.2f,0.25f);
    public Vector2 TopLeftVert = new Vector2(0.2f,0.7f);

    public float maxVal = 3;
    public float topVal = 3;
    public float topRightVal = 3;
    public float botRightVal = 3;
    public float botLeftVal = 3;
    public float topLeftVal = 3;

    private Image img;


    // Start is called before the first frame update
    void Start()
    {
        img = gameObject.GetComponent<Image>();
        
        if(img == null)
        {
            Debug.LogError("No image");
        }
        Debug.Log("img: " + img.name);
        Debug.Log("shader: " + img.material.shader.name);

        setupMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        //setupMaterial(); //add in to debug initial settings
        updateChart();   
    }

    /// <summary>
    /// Must be called first to initialize the chart bounds
    /// </summary>
    public void setupMaterial()
    {
        Material curMat = Instantiate(img.material);
        curMat.SetVector("_MidVert", MidVert);
        curMat.SetVector("_TopVert", TopVert);
        curMat.SetVector("_TopRightVert", TopRightVert);
        curMat.SetVector("_BotRightVert", BottomRightVert);
        curMat.SetVector("_BotLeftVert", BottomLeftVert);
        curMat.SetVector("_TopLeftVert", TopLeftVert);
        curMat.SetFloat("_MaxVal", maxVal);
        curMat.SetColor("_MainCol", img.color);
        img.material = curMat;
       
    }

    /// <summary>
    /// Performs the update of the material to render the new chart values
    /// </summary>
    private void updateChart()
    {
        Material curMat = img.material;
        //curMat.SetFloat("_MaxVal", maxVal); //shouldn't change after init
        curMat.SetFloat("_TopVal", topVal);
        curMat.SetFloat("_TopRightVal", topRightVal);
        curMat.SetFloat("_BotRightVal", botRightVal);
        curMat.SetFloat("_BotLeftVal", botLeftVal);
        curMat.SetFloat("_TopLeftVal", topLeftVal);       
    }

    public void setValues(float top, float topRight, float bottomRight,
        float bottomLeft, float topLeft)
    {
        topVal = top;
        topRightVal = topRight;
        botRightVal = bottomRight;
        botLeftVal = bottomLeft;
        topLeftVal = topLeft;
    }
}
