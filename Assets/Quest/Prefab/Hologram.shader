Shader "Custom/Hologram"
{
    Properties
    {
        _MainTex ("MainTexture (RGB)", 2D) = "white" {}
		_SubTex  ("SubTexture (RGB)", 2D) = "white" {}
        _MaskTex ("MaskTexture (RGB)", 2D) = "white" {}
		_AlphaValue("AlphaValue", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        LOD 200

        CGPROGRAM

        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _MaskTex;

        struct Input
        {
            float2 uv_MainTex;
        };

		float _AlphaValue;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            
        }
        ENDCG
    }
    FallBack "Diffuse"
}
