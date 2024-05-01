Shader "HaihaiShader/CharacterLit" {
    Properties {
        _MainTex ("Albedo", 2D) = "white" {}
        _AmbientColor ("Ambient Color", Color) = (1,1,1,1)
        _ShadowColor ("Shadow Color", Color) = (0.5,0.5,0.5,1)
        _SelfShadowRampTex ("Self Shadow Ramp", 2D) = "white" {}
        _ShadowMapRampTex ("Shadow Map Ramp", 2D) = "white" {}
        [Toggle]_CastShadows ("Cast Shadows", Float) = 1
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
            "Queue"="Geometry"
        }

        Pass {
            Name "ForwardLit"

            Tags {
                "LightMode"="UniversalForwardOnly"
            }

            Cull Back
            ZWrite On

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            // make fog work
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "HaihaiFunctions.hlsl"

            struct Attributes
            {
                float4 posOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS: NORMAL;
                float4 tangentOS: TANGENT;
            };

            struct Varyings
            {
                float4 posHClip : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS: TEXCOORD1;
                float4 tangentWS: TEXCOORD2;
                float3 posWS: TEXCOORD3;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _SelfShadowRampTex_ST;
                float4 _ShadowMapRampTex_ST;
                float4 _AmbientColor;
                float4 _ShadowColor;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_SelfShadowRampTex);
            SAMPLER(sampler_SelfShadowRampTex);

            TEXTURE2D(_ShadowMapRampTex);
            SAMPLER(sampler_ShadowMapRampTex);

            Varyings Vert(Attributes IN)
            {
                Varyings OUT;
                OUT.posHClip = TransformObjectToHClip(IN.posOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(IN.normalOS, IN.tangentOS);
                OUT.normalWS = normalize(TransformObjectToWorldNormal(IN.normalOS));
                OUT.tangentWS = float4(normalInput.tangentWS, IN.tangentOS.w);
                OUT.uv = TRANSFORM_TEX(IN.uv.xy, _MainTex);
                OUT.posWS = TransformObjectToWorld(IN.posOS.xyz).xyz;
                return OUT;
            }

            float4 Frag(Varyings IN) : SV_Target
            {
                float4 result = 0;
                float4 albedo = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                // return albedo;
                float3 normalWS = normalize(IN.normalWS);
                result.rgb = ComputeToonSurface(_AmbientColor.rgb, albedo, _ShadowColor, normalWS, IN.posWS, 1.0, 1.0,
                                                _SelfShadowRampTex, sampler_SelfShadowRampTex, _ShadowMapRampTex, sampler_ShadowMapRampTex);
                return result;
            }
            ENDHLSL
        }

        UsePass "Universal Render Pipeline/Lit/DEPTHONLY"
        
        UsePass "Universal Render Pipeline/Lit/DEPTHNORMALS"
        
        UsePass "Universal Render Pipeline/Lit/SHADOWCASTER"

        //        Pass {
        //            Name "DepthOnly"
        //
        //            Tags {
        //                "LightMode"="DepthOnly"
        //            }
        //
        //            ZWrite On
        //            ColorMask 0
        //            Cull Back
        //
        //            HLSLPROGRAM
        //            #pragma vertex ComputeVertex
        //            #pragma fragment ComputeFragment
        //            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        //
        //            CBUFFER_START(UnityPerMaterial)
        //                float4 _MainTex_ST;
        //                float4 _SelfShadowRampTex_ST;
        //                float4 _ShadowMapRampTex_ST;
        //                float4 _AmbientColor;
        //                float4 _ShadowColor;
        //            CBUFFER_END
        //
        //            TEXTURE2D(_MainTex);
        //            SAMPLER(sampler_MainTex);
        //
        //            struct Attributes
        //            {
        //                float3 positionOS : POSITION;
        //                float4 uv0 : TEXCOORD0;
        //            };
        //
        //            struct Varyings
        //            {
        //                float4 positionHCS : SV_POSITION;
        //                float2 uv0 : TEXCOORD1;
        //            };
        //
        //            Varyings ComputeVertex(Attributes IN)
        //            {
        //                Varyings OUT;
        //                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
        //                // OUT.uv0 = TRANSFORM_TEX(IN.uv0.xy, _MainTex);
        //                OUT.uv0 = IN.uv0.xy;
        //                return OUT;
        //            }
        //
        //            float4 ComputeFragment(Varyings IN) : SV_Target
        //            {
        //                return 0;
        //            }
        //            ENDHLSL
        //        }

//        Pass {
//            Name "ShadowCaster"
//            Tags {
//                "LightMode" = "ShadowCaster"
//            }
//
//            ZWrite On
//            ZTest LEqual
//            ColorMask 0
//            Cull Off
//
//            HLSLPROGRAM
//            #pragma exclude_renderers gles gles3 glcore
//            #pragma target 4.5
//
//            // -------------------------------------
//            // Material Keywords
//            #pragma shader_feature_local_fragment _ALPHATEST_ON
//            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
//            #pragma shader_feature_local_fragment _CASTSHADOWS_ON
//
//            // -------------------------------------
//            // Universal Pipeline keywords
//
//            // This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
//            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
//
//            #pragma vertex ShadowPassVertex
//            #pragma fragment ShadowPassFragment
//
//            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
//            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
//            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
//
//            // Shadow Casting Light geometric parameters. These variables are used when applying the shadow Normal Bias and are set by UnityEngine.Rendering.Universal.ShadowUtils.SetupShadowCasterConstantBuffer in com.unity.render-pipelines.universal/Runtime/ShadowUtils.cs
//            // For Directional lights, _LightDirection is used when applying shadow Normal Bias.
//            // For Spot lights and Point lights, _LightPosition is used to compute the actual light direction because it is different at each shadow caster geometry vertex.
//            float3 _LightDirection;
//            float3 _LightPosition;
//
//            CBUFFER_START(UnityPerMaterial)
//                float4 _MainTex_ST;
//                float4 _SelfShadowRampTex_ST;
//                float4 _ShadowMapRampTex_ST;
//                float4 _AmbientColor;
//                float4 _ShadowColor;
//            CBUFFER_END
//
//            TEXTURE2D(_MainTex);
//            SAMPLER(sampler_MainTex);
//
//            struct Attributes
//            {
//                float4 positionOS : POSITION;
//                float3 normalOS : NORMAL;
//                float2 texcoord : TEXCOORD0;
//                UNITY_VERTEX_INPUT_INSTANCE_ID
//            };
//
//            struct Varyings
//            {
//                float2 uv : TEXCOORD0;
//                float4 positionCS : SV_POSITION;
//            };
//
//            float4 GetShadowPositionHClip(Attributes input)
//            {
//                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
//                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
//
//                #if _CASTING_PUNCTUAL_LIGHT_SHADOW
//                float3 lightDirectionWS = normalize(_LightPosition - positionWS);
//                #else
//                float3 lightDirectionWS = _LightDirection;
//                #endif
//
//                float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));
//
//                #if UNITY_REVERSED_Z
//                positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
//                #else
//                positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
//                #endif
//
//                return positionCS;
//            }
//
//            Varyings ShadowPassVertex(Attributes input)
//            {
//                Varyings output;
//                UNITY_SETUP_INSTANCE_ID(input);
//
//                output.uv = input.texcoord;
//                output.positionCS = GetShadowPositionHClip(input);
//                return output;
//            }
//
//            half4 ShadowPassFragment(Varyings input) : SV_TARGET
//            {
//                #ifndef _CASTSHADOWS_ON
//                discard;
//                #endif
//                return 0;
//            }
//            ENDHLSL
//        }
    }
}