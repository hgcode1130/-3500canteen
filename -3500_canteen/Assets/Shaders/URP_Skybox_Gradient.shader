Shader "Custom/URP/SkyboxGradient"
{
    Properties
    {
        _TopColor ("������ɫ", Color) = (0.2, 0.5, 0.8, 1.0)
        _MiddleColor ("�м���ɫ", Color) = (0.4, 0.6, 0.9, 1.0)
        _BottomColor ("�ײ���ɫ", Color) = (0.6, 0.4, 0.3, 1.0)
        
        _TopHeight ("�����߶�", Range(-1, 1)) = 0.8
        _MiddleHeight ("�м�߶�", Range(-1, 1)) = 0.0
        _BottomHeight ("�ײ��߶�", Range(-1, 1)) = -0.2
        
        _TopExponent ("��������ָ��", Range(0.1, 10)) = 1.0
        _BottomExponent ("�ײ�����ָ��", Range(0.1, 10)) = 1.0
    }
    
    SubShader
    {
        Tags { "RenderType"="Background" "RenderPipeline" = "UniversalRenderPipeline" "Queue"="Background" }
        LOD 100
        
        Pass
        {
            Cull Off
            ZWrite Off
            ZTest Always
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 texcoord : TEXCOORD0;
            };
            
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 direction : TEXCOORD0;
            };
            
            half4 _TopColor;
            half4 _MiddleColor;
            half4 _BottomColor;
            half _TopHeight;
            half _MiddleHeight;
            half _BottomHeight;
            half _TopExponent;
            half _BottomExponent;
            
            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.direction = input.texcoord;
                return output;
            }
            
            half4 frag(Varyings input) : SV_Target
            {
                float3 dir = normalize(input.direction);
                float height = dir.y;
                
                half4 finalColor;
                
                // ��ȫ�����ֵ����
                float topFactor = saturate((height - _MiddleHeight) / max(0.001, _TopHeight - _MiddleHeight));
                float bottomFactor = saturate((height - _BottomHeight) / max(0.001, _MiddleHeight - _BottomHeight));
                
                if (height > _MiddleHeight)
                {
                    // ��������
                    float t = pow(topFactor, _TopExponent);
                    finalColor = lerp(_MiddleColor, _TopColor, t);
                }
                else
                {
                    // �ײ�����
                    float t = pow(bottomFactor, _BottomExponent);
                    finalColor = lerp(_BottomColor, _MiddleColor, t);
                }
                
                return finalColor;
            }
            ENDHLSL
        }
    }
    
    FallBack "Skybox/Procedural"
}    