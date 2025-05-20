Shader "Custom/OutlinedSprite"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineSize ("Outline Size", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Name "OUTLINE"
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _OutlineColor;
            float _OutlineSize;

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 offset = float2(_OutlineSize / _ScreenParams.x, _OutlineSize / _ScreenParams.y);
                float alpha = 0;
                for(int x = -1; x <= 1; x++)
                {
                    for(int y = -1; y <= 1; y++)
                    {
                        float2 uv = i.texcoord + float2(x, y) * offset;
                        alpha = max(alpha, tex2D(_MainTex, uv).a);
                    }
                }

                fixed4 col = tex2D(_MainTex, i.texcoord);
                return (col.a > 0.1) ? col : fixed4(_OutlineColor.rgb, alpha);
            }
            ENDCG
        }
    }
}
