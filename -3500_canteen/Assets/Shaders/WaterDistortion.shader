Shader "Hidden/URP/WaterDistortion"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTexture ("Noise Texture", 2D) = "white" {}
        _CausticsTexture ("Caustics Texture", 2D) = "white" {}
        _DistortionStrength ("Distortion Strength", Range(0, 0.1)) = 0.02
        _WaveSpeed ("Wave Speed", Range(0, 5)) = 1.0
        _CausticsIntensity ("Caustics Intensity", Range(0, 1)) = 0.5
        _CausticsScale ("Caustics Scale", Range(0, 10)) = 2.0
        _TimeFactor ("Time Factor", Float) = 0
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalRenderPipeline" }
        LOD 100
        
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            
            sampler2D _MainTex;
            sampler2D _NoiseTexture;
            sampler2D _CausticsTexture;
            float _DistortionStrength;
            float _WaveSpeed;
            float _CausticsIntensity;
            float _CausticsScale;
            float _TimeFactor;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                return o;
            }
            
            half4 frag (v2f i) : SV_Target
            {
                // �������������UV���꣬���ʱ������ʹ�����ƶ�
                float2 noiseUV = i.uv * 2.0 + float2(_TimeFactor * _WaveSpeed, _TimeFactor * _WaveSpeed * 0.7);
                
                // �������������ȡ�Ŷ�ֵ
                half4 noise = tex2D(_NoiseTexture, noiseUV);
                half4 noise2 = tex2D(_NoiseTexture, noiseUV * 0.7 + float2(0.3, 0.5));
                
                // ��������������Ի�ø���Ȼ��Ч��
                float2 distortion = (noise.rg + noise2.rg - 1.0) * _DistortionStrength;
                
                // Ӧ���Ŷ���������UV
                float2 distortedUV = i.uv + distortion;
                
                // ����������
                half4 mainColor = tex2D(_MainTex, distortedUV);
                
                // ���㽹ɢЧ��
                float2 causticsUV = i.uv * _CausticsScale + float2(_TimeFactor * _WaveSpeed * 0.5, _TimeFactor * _WaveSpeed * 0.3);
                half4 caustics = tex2D(_CausticsTexture, causticsUV);
                half4 caustics2 = tex2D(_CausticsTexture, causticsUV * 0.6 + float2(0.2, 0.8));
                
                // ��Ͻ�ɢЧ��
                half causticsEffect = (caustics.r + caustics2.g) * 0.5 * _CausticsIntensity;
                
                // ������ɫ = �Ŷ��������ɫ + ��ɢЧ��
                mainColor.rgb += mainColor.rgb * causticsEffect;
                
                return mainColor;
            }
            ENDHLSL
        }
    }
}    