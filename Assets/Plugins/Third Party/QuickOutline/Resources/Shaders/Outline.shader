﻿//
//  Outline.shader
//  QuickOutline
//
//  Created by Chris Nolet on 5/7/18.
//  Copyright © 2018 Chris Nolet. All rights reserved.
//

Shader "Custom/Outline" {
  Properties {
    [Enum(UnityEngine.Rendering.CompareFunction)] _ZTestMask("ZTest Mask", Float) = 0
    [Enum(UnityEngine.Rendering.CompareFunction)] _ZTestFill("ZTest Fill", Float) = 0

    _OutlineColor("Outline Color", Color) = (1, 1, 1, 1)
    _OutlineWidth("Outline Width", Range(0, 10)) = 2
  }

  SubShader {
    
    Tags {
      "Queue" = "Geometry"//"Queue" = "Transparent+100"
      //"RenderType" = "Transparent"
      "DisableBatching" = "True"
    }

    Pass {
      
      Name "Mask"
      Cull Off
      ZTest [_ZTestMask]
      ZWrite Off
      ColorMask 0

      Stencil {
        Ref 1
        Pass Replace
      }
    }

    Pass {
      Name "Fill"
      Cull Off
      ZTest [_ZTestFill]
      ZWrite Off
      Blend SrcAlpha OneMinusSrcAlpha
      ColorMask RGB

      Stencil {
        Ref 1
        Comp NotEqual
      }

      CGPROGRAM

      #pragma multi_compile _ USE_OUTLINE
      
      #include "UnityCG.cginc"

      #pragma vertex vert
      #pragma fragment frag

      struct appdata {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
        float3 smoothNormal : TEXCOORD3;
        UNITY_VERTEX_INPUT_INSTANCE_ID
      };

      struct v2_f {
        float4 position : SV_POSITION;
        fixed4 color : COLOR;
        UNITY_VERTEX_OUTPUT_STEREO
      };

      uniform fixed4 _OutlineColor;
      uniform float _OutlineWidth;

      v2_f vert(appdata input) {
        v2_f output;

        UNITY_SETUP_INSTANCE_ID(input);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

        float3 normal = any(input.smoothNormal) ? input.smoothNormal : input.normal;
        float3 viewPosition = UnityObjectToViewPos(input.vertex);
        float3 viewNormal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, normal));

        output.position = UnityViewToClipPos(viewPosition + viewNormal * -viewPosition.z * _OutlineWidth / 1000.0);
        output.color = _OutlineColor;

        return output;
      }

      fixed4 frag(v2_f input) : SV_Target {
#if defined(USE_OUTLINE) == false
        discard;
#endif
        return input.color;
      }

      ENDCG
    }

    Pass {
      Name "Reset"
      Cull Off
      ZTest [_ZTestMask]
      ZWrite Off
      ColorMask 0

      Stencil {
        Pass Zero
      }
    }
  }
}
