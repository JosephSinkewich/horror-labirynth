Shader "Game/Gem/Crystal"
{
    Properties
    {
        [HDR] _BaseColor ("Base Color", Color) = (0.05, 0.35, 0.85, 0.25)
        [HDR] _RimColor ("Rim Color", Color) = (0.45, 0.95, 1.25, 1)
        [HDR] _EmissionColor ("Emission Color", Color) = (0, 1.5, 2.5, 1)
        _RimPower ("Rim Power", Range(1, 10)) = 4
        _EmissionStrength ("Emission Strength", Range(0, 5)) = 2
        _PulseSpeed ("Pulse Speed", Range(0, 5)) = 1.2
        _PulseAmount ("Pulse Amount", Range(0, 1)) = 0.35
        _SparkleScale ("Sparkle Scale", Range(1, 30)) = 10
        _SparkleStrength ("Sparkle Strength", Range(0, 2)) = 0.75
        _Alpha ("Alpha", Range(0, 1)) = 0.55
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        Pass
        {
            Name "CrystalForward"
            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _RimColor;
                float4 _EmissionColor;
                float _RimPower;
                float _EmissionStrength;
                float _PulseSpeed;
                float _PulseAmount;
                float _SparkleScale;
                float _SparkleStrength;
                float _Alpha;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float fogFactor : TEXCOORD2;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                VertexPositionInputs positionInputs = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInputs = GetVertexNormalInputs(input.normalOS);

                output.positionCS = positionInputs.positionCS;
                output.positionWS = positionInputs.positionWS;
                output.normalWS = normalInputs.normalWS;
                output.fogFactor = ComputeFogFactor(positionInputs.positionCS.z);
                return output;
            }

            float Hash(float3 position)
            {
                return frac(sin(dot(position, float3(127.1, 311.7, 74.7))) * 43758.5453);
            }

            half4 frag(Varyings input) : SV_Target
            {
                float3 normalWS = normalize(input.normalWS);
                float3 viewDirWS = normalize(GetWorldSpaceViewDir(input.positionWS));
                float fresnel = pow(1.0 - saturate(dot(normalWS, viewDirWS)), _RimPower);

                float pulse = 1.0 + sin(_Time.y * _PulseSpeed) * _PulseAmount;
                float sparkleCell = Hash(floor(input.positionWS * _SparkleScale + _Time.y * 1.5));
                float sparkle = smoothstep(0.985, 1.0, sparkleCell) * _SparkleStrength;

                float3 body = _BaseColor.rgb * (0.35 + fresnel * 0.65);
                float3 rim = _RimColor.rgb * fresnel;
                float3 emission = _EmissionColor.rgb * _EmissionStrength * pulse;
                float3 color = body + rim + emission + sparkle;

                half alpha = saturate(_Alpha + fresnel * 0.35 + sparkle * 0.15);
                color = MixFog(color, input.fogFactor);
                return half4(color, alpha);
            }
            ENDHLSL
        }
    }

    FallBack Off
}
