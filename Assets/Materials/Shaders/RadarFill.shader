Shader "Unlit/RadarFill"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainCol("MainCol", Color) = (1, 1, 1, 1)
        _BorderColor("Border Color", Color) = (0, 0, 0, 1.0)
        _FillColor("Fill Color", Color) = (0, 1, 0, 1.0)
        _BorderThresh("Border Thresh", Float) = 0.001
        //endpoints
        _TopVert("Top Vert", Vector) = (0.5, 0.9, 0, 0)
        _TopRightVert("Top Right Vert", Vector) = (0.8, 0.8, 0, 0)
        _BotRightVert("Bottom Right Vert", Vector) = (0.7, 0.2, 0, 0)
        _BotLeftVert("Bottom Left Vert", Vector) = (0.2, 0.2, 0, 0)
        _TopLeftVert("Top Left Vert", Vector) = (0.2, 0.8, 0, 0)
        _MidVert("Middle", Vector) = (0.5, 0.5, 0, 0)

        //max val to set endpoint
        _MaxVal("Max Value", Float) = 3
        //allow small spikes to show
        _MinVal("Min Value", Float) = 0.125 

        //cur values
        _TopVal("Top Value", Float) = 3
        _TopRightVal("Top Right Value", Float) = 3
        _BotRightVal("Bottom Right Value", Float) = 3
        _BotLeftVal("Bottom Left Value", Float) = 3
        _TopLeftVal("Top Left Value", Float) = 3
    }
    SubShader
    {
        //Tags { "RenderType"="Opaque" }
        //LOD 100
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent+1"}
        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            half4 getAABB(half2 a, half2 b, half2 c, half2 d, half2 e);
            bool inAABB(half2 pt, half2 bl, half2 tr);
            fixed intersects(half2 pt0_0, half2 pt0_1, half2 pt1_0, half2 pt1_1, out half distFromIntersect);

            sampler2D _MainTex;
            fixed4 _MainCol;
            float4 _MainTex_ST;
            float4 _BorderColor;
            float4 _FillColor;
            float _BorderThresh;

            //endpoints
            half2 _TopVert;
            half2 _TopRightVert;
            half2 _BotRightVert;
            half2 _BotLeftVert;
            half2 _TopLeftVert;
            half2 _MidVert;

            //max val to set endpoint
            half _MaxVal;
            half _MinVal;

            //cur values
            half _TopVal;
            half _TopRightVal;
            half _BotRightVal;
            half _BotLeftVal;
            half _TopLeftVal;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {

                //determine bounding points in uv text coords (assumes BL:(0,0) to TR:(1,1,))
                //half2 tuv = lerp(_MidVert.xy, _TopVert.xy, 1);
                half2 tuv = lerp(_MidVert.xy, _TopVert.xy, max(_MinVal, (_TopVal / _MaxVal)));
                half2 truv = lerp(_MidVert.xy, _TopRightVert.xy, max(_MinVal, (_TopRightVal / _MaxVal)));
                half2 bruv = lerp(_MidVert.xy, _BotRightVert.xy, max(_MinVal, (_BotRightVal / _MaxVal)));
                half2 bluv = lerp(_MidVert.xy, _BotLeftVert.xy, max(_MinVal, (_BotLeftVal / _MaxVal)));
                half2 tluv = lerp(_MidVert.xy, _TopLeftVert.xy, max(_MinVal, (_TopLeftVal / _MaxVal)));

                half4 aabb = getAABB(tuv, truv, bruv, bluv, tluv);
                half2 aabbBL = aabb.xy;
                half2 aabbTR = aabb.zw;
                
                //2-part test to see if uv is inside our defined polygon
                bool uvIsInside = false;
                bool isInAABB = inAABB(i.uv, aabbBL, aabbTR);
                half minDist = 1.0;
                if (isInAABB) {
                    half2 outerPt = aabbBL - 0.1;  //ensure outside possible polygon
                    int intersectCnt = 0;
                    //test against all 5 line segments
                    half4 lines[5];
                    lines[0] = half4(tuv.xy, truv.xy);  //top -> TR
                    lines[1] = half4(truv.xy, bruv.xy);  //TR -> BR
                    lines[2] = half4(bruv.xy, bluv.xy);  //BR -> BL
                    lines[3] = half4(bluv.xy, tluv.xy);  //BL -> TL
                    lines[4] = half4(tluv.xy, tuv.xy);  //TL -> top
                    
                    for (int idx = 0; idx < 5; idx++) {
                        half dist;
                        intersectCnt += intersects(outerPt, i.uv, lines[idx].xy, lines[idx].zw, dist);
                        minDist = min(minDist, dist);
                    }
                    uvIsInside = intersectCnt & 1; //inside if count is odd
                }

                // sample the texture.  If the uv is inside the polygon then apply our shading too.
                fixed4 texCol = tex2D(_MainTex, i.uv) * _MainCol;
                //fixed4 fillCol = (minDist <= _BorderThresh) ? _BorderColor : _FillColor; //TODO border
                fixed4 fillCol = _FillColor; //TODO add in border color
                fixed4 col;
                if (uvIsInside) {
                    col = texCol * _FillColor;
                }
                else {
                    col = texCol;
                }
                return col;
            }

            //Returns axis-aligned bounding box of the given points as
            // (x1,y1, x2,y2) where (x1,y1) is lower left point, (x2,y2) is upper right point.
            half4 getAABB(half2 a, half2 b, half2 c, half2 d, half2 e) {
                half xmin = min(a.x, min(b.x, min(c.x, min(d.x, e.x))));
                half ymin = min(a.y, min(b.y, min(c.y, min(d.y, e.y))));

                half xmax = max(a.x, max(b.x, max(c.x, max(d.x, e.x))));
                half ymax = max(a.y, max(b.y, max(c.y, max(d.y, e.y))));

                return half4(xmin, ymin, xmax, ymax);
            }

            bool inAABB(half2 pt, half2 bl, half2 tr) {
                return all(pt >= bl && pt <= tr);
            }

            fixed intersects(half2 pt0_0, half2 pt0_1, half2 pt1_0, half2 pt1_1, out half distFromIntersect) {
                //test intersection against the first line
                half3 pt0_abc = half3(pt0_0.y - pt0_1.y,
                                      pt0_1.x - pt0_0.x,
                                    ((pt0_0.x*pt0_1.y) - (pt0_1.x*pt0_0.y)));

                //the distance of each endpoint of pt1 from the line pt0
                half2 d = half2((pt0_abc.x * pt1_0.x) + (pt0_abc.y * pt1_0.y) + pt0_abc.z,
                                (pt0_abc.x * pt1_1.x) + (pt0_abc.y * pt1_1.y) + pt0_abc.z);

                distFromIntersect = min(abs(d.x), abs(d.y));
                //if both points share the same sign for distance then the lines did not intersect
                if (all(d < 0) || all(d > 0)) return 0;

                //test intersection against the second line
                half3 pt1_abc = half3(pt1_0.y - pt1_1.y,
                                      pt1_1.x - pt1_0.x,
                                    ((pt1_0.x * pt1_1.y) - (pt1_1.x * pt1_0.y)));

                //the distance of each endpoint of pt0 from the line pt1
                half2 d2 = half2((pt1_abc.x * pt0_0.x) + (pt1_abc.y * pt0_0.y) + pt1_abc.z,
                                 (pt1_abc.x * pt0_1.x) + (pt1_abc.y * pt0_1.y) + pt1_abc.z);

                distFromIntersect = min(distFromIntersect, min(abs(d2.x), abs(d2.y)));

                //if both points share the same sign for distance then the lines did not intersect
                if (all(d2 < 0) || all(d2 > 0)) return 0;

                if ((pt0_abc.x * pt1_abc.y) - (pt1_abc.x * pt0_abc.y) == 0.0f) return 0; // colinear

                return 1;
            }

            ENDCG
        }
    }
}
