Shader "Dithered Transparent/Dithered Unlit Double-Sided" {
    
    Properties  {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Main Texture", 2D) = "white" {}
    }

    SubShader {
        
        Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }
        
        Cull Off // this enables double-sided rendering 

        Pass {

            CGPROGRAM
            
            #include "UnityCG.cginc"
            #include "Dither Functions.cginc"
            
            #pragma vertex vert
            #pragma fragment frag

            uniform fixed4 _LightColor0;
            float4 _Color;
            float4 _MainTex_ST;         // For the Main Tex UV transform
            sampler2D _MainTex;         // Texture used for the line
            
            struct v2_f {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
                float4 screen_position : TEXCOORD1;
            };

            v2_f vert(appdata_base v) {
                v2_f o;
                
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.screen_position = ComputeScreenPos(o.position);

                return o;
            }

            float4 frag(v2_f i) : COLOR {
                float4 color = _Color * tex2D(_MainTex, i.uv);
                ditherClip(i.screen_position.xy / i.screen_position.w, color.a);

                return color;
            }

            ENDCG
        }
    }
}
