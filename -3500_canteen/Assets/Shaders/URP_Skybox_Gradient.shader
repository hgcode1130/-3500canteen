Shader "Custom/URP/SkyboxGradient"
{
    Properties
    {
        _TopColor ("������ɫ", Color) = (0.2, 0.5, 0.8, 1.0)
        _MiddleColor ("�м���ɫ", Color) = (0.4, 0.6, 0.9, 1.0)
        _BottomColor ("�ײ���ɫ", Color) = (0.6, 0.4, 0.3, 1.0)
        
        _TopHeight ("�����߶�", Range(-1, 1)) = 0.8
        _BottomHeight ("�ײ��߶�", Range(-1, 1)) = -0.2
        
        _Exponent ("����ָ��", Range(0.1, 10)) = 1.0
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
            
            // ����
            half4 _TopColor;
            half4 _MiddleColor;
            half4 _BottomColor;
            half _TopHeight;
            half _BottomHeight;
            half _Exponent;
            
            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.direction = input.texcoord;
                return output;
            }
            
            half4 frag(Varyings input) : SV_Target
            {
                // ��һ����������
                float3 dir = normalize(input.direction);
                
                // ��ȡY������Ϊ�߶�ֵ (-1 �� 1)
                float height = dir.y;
                
                // ���㶥�����м���ɫ�Ļ������
                float topBlend = saturate(pow((height - _BottomHeight) / (_TopHeight - _BottomHeight), _Exponent));
                
                // ����������ɫ
                half4 finalColor;
                
                if (height > _TopHeight)
                {
                    // ���ڶ����߶ȣ�ʹ�ö�����ɫ
                    finalColor = _TopColor;
                }
                else if (height < _BottomHeight)
                {
                    // ���ڵײ��߶ȣ�ʹ�õײ���ɫ
                    finalColor = _BottomColor;
                }
                else if (height > 0)
                {
                    // ���ڵ�ƽ�ߣ���϶������м���ɫ
                    finalColor = lerp(_MiddleColor, _TopColor, topBlend);
                }
                else
                {
                    // ���ڵ�ƽ�ߣ���ϵײ����м���ɫ
                    finalColor = lerp(_BottomColor, _MiddleColor, topBlend);
                }
                
                return finalColor;
            }
            ENDHLSL
        }
    }
    
    FallBack "Skybox/Procedural"
}
