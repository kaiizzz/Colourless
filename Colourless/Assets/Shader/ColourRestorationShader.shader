Shader "Custom/ColorRestorationFilter"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}  // The original sprite texture
        _PlayerPos ("Player Position", Vector) = (0,0,0,0)  // Player's world position
        _EffectRadius ("Effect Radius", Float) = 2.0  // Radius of color restoration
        _Phase ("Phase", Float) = 0.0  // Color restoration phase
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha  // Ensures proper blending
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 worldPos : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _PlayerPos;
            float _EffectRadius;
            float _Phase;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Sample the original sprite color
                fixed4 texColor = tex2D(_MainTex, i.uv);

                // Convert color to greyscale for the default state
                float grayscale = dot(texColor.rgb, float3(0.3, 0.59, 0.11));
                fixed4 greyTex = fixed4(grayscale, grayscale, grayscale, texColor.a);
                fixed4 blueTex = fixed4(0, texColor.g, texColor.b, texColor.a);
                fixed4 greenTex = fixed4(texColor.r, 0, texColor.b, texColor.a);
                fixed4 redTex = fixed4(texColor.r, texColor.g, 0, texColor.a);
                fixed4 fullTex = texColor;

                // Distance from the player
                float dist = length(i.worldPos.xy - _PlayerPos.xy);

                // Calculate color restoration factor based on distance
                float colorFactor = saturate(1.0 - (dist / _EffectRadius));

                // Apply phase-based color unlocking
                float3 restoredColor = greyTex.rgb;
                if (_Phase == 1.0) {
                    restoredColor = blueTex.rgb;
                } else if (_Phase == 2.0) {
                    restoredColor = greenTex.rgb;
                } else if (_Phase == 3.0) {
                    restoredColor = redTex.rgb;
                } else if (_Phase >= 4.0) {
                    restoredColor = fullTex.rgb;
                }
                

                return fixed4(restoredColor, texColor.a);
            }
            ENDCG
        }
    }
}
