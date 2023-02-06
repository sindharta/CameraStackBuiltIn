Shader "Custom/RenderTextureBlend" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "black" {}
        _BackgroundTex ("Background (RGB)", 2D) = "black" {}
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    sampler2D	_MainTex;
    sampler2D   _BackgroundTex;


    struct Frag_IN {
        float4 pos    : SV_POSITION; 
        float2 uv     : TEXCOORD0; 
    };

    Frag_IN vert(appdata_img v) {
        Frag_IN o;

        o.pos = UnityObjectToClipPos(v.vertex);
        o.uv.xy = v.texcoord.xy; 
        return o;
    }
    

//----------------------------------------------------------------------------------------------------------------------    
    float4 frag(Frag_IN i) : SV_Target {
                
        const float4 backgroundColor = tex2D(_BackgroundTex, i.uv); 
        const float4 inputColor = tex2D(_MainTex, i.uv);

        const float a = inputColor.a;
        const float4 output = inputColor.rgba * a + backgroundColor.rgba * (1-a); 
        return output;
        
    }

    ENDCG

//----------------------------------------------------------------------------------------------------------------------

    Subshader {
        Pass {
            ZTest  Always
            Cull   Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #pragma target   5.0
            ENDCG
        }
    }

    Fallback off
}
