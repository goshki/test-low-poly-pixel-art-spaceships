Shader "Dithered Transparent/Dithered"
{
    Properties 
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Main Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags{ "RenderType" = "Opaque" "Queue" = "Geometry" }

        Pass
        {

            CGPROGRAM
            #include "UnityCG.cginc"
            #include "Dither Functions.cginc"
            #pragma vertex vert
            #pragma fragment frag

            uniform fixed4 _LightColor0;
            float4 _Color;
            float4 _MainTex_ST;         // For the Main Tex UV transform
            sampler2D _MainTex;         // Texture used for the line
            
            struct v2_f
            {
                float4 pos      : POSITION;
                float4 col      : COLOR;
                float2 uv       : TEXCOORD0;
                float4 screen_position     : TEXCOORD1;
            };

            v2_f vert(appdata_base v)
            {
                v2_f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

                float4 norm = mul(unity_ObjectToWorld, v.normal);
                float3 normalDirection = normalize(norm.xyz);
                float4 AmbientLight = UNITY_LIGHTMODEL_AMBIENT;
                float4 LightDirection = normalize(_WorldSpaceLightPos0);
                float4 DiffuseLight = saturate(dot(LightDirection, -normalDirection))*_LightColor0;
                o.col = float4(AmbientLight + DiffuseLight);
                o.screen_position = ComputeScreenPos(o.pos);

                return o;
            }

            float4 frag(v2_f i) : COLOR
            {
                float4 col = _Color * tex2D(_MainTex, i.uv);
                ditherClip(i.screen_position.xy / i.screen_position.w, col.a);

                return col * i.col;
            }

            ENDCG
        }
    }

    SubShader
    {
        Tags { "RenderType" = "ShadowCaster" }
        UsePass "Hidden/Dithered Transparent/Shadow/SHADOW"
    }
}
