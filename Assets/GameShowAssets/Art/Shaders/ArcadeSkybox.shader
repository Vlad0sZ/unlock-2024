Shader "Unlit/ArcadeSkybox"
{
    Properties
    {
        _BaseMap ("Texture", 2D) = "black" {}
        _Color ("Color",Color) = (0,0,0,1)
        _SkyColor ("Sky Color",Color) = (0.3,0.2,0.8,1)
        _GroundColor ("Ground Color",Color) = (1,1,1,1)
        _ColorMid ("Mid Color",Color) = (0.5,0.5,0.5,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 position : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID 
            };

            struct Varyings
            {
                half3 positionWS : POSITION2;
                float4 positionCS : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID 
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_BaseMap);    SAMPLER(sampler_BaseMap);
            
            CBUFFER_START(unity_per_material)
            float4 _GroundColor;
            float4 _Color;
            float4 _ColorMid;
            float4 _SkyColor;
            CBUFFER_END
            
            Varyings vert(Attributes i)
            {
                Varyings o;
                UNITY_SETUP_INSTANCE_ID(i);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                UNITY_TRANSFER_INSTANCE_ID(i, o);
                o = (Varyings)0;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(i.position.xyz);
                o.positionCS = vertexInput.positionCS;
                o.positionWS = vertexInput.positionWS;
;
                return o;
            }
        

            float4 frag(Varyings i,float facing : VFACE) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                float4 col = 1;
                float3 normal = normalize(i.positionWS);
                float2 uv = normal.xz / normal.y;
                uv += _Time.x;
                float4 pattern = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv*0.8);
                half mask = abs(dot(normal, half3(0, -1, 0)));
                col.rgb = lerp(_GroundColor, _SkyColor, step(0, normal.y));
                col.rgb = lerp(col.rgb, _ColorMid.rgb,pow(1 - abs(normal.y),8));
                col.rgb = lerp(col.rgb, _Color.rgb, pattern.r * _Color.a * abs(normal.y));
                //MixFog(col.rgb, i.positionWSAndFogFactor.w);
                return col;
            }
            ENDHLSL
        }
    }
}
