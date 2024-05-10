Shader "Hidden/NewImageEffectShaderPixel"
{
    Properties
    {
        _PixelSize("PixelSize", Float) = 20
        _MovingSpeed("MovingSpeed", Float) = 1
        _NoiseScale("NoiseScale", Float) = 1
        _Color("Color", Color) = (0.5660378, 0.4749752, 0.3924884, 0.5607843)
        [NoScaleOffset]_Texture2D("Texture2D", 2D) = "white" {}
        _Alpha("Alpha", Float) = 10
        _Munimum_Alpha("Munimum Alpha", Range(0, 1)) = 0.5
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue"="Transparent"
            // DisableBatching: <None>
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalSpriteUnlitSubTarget"
        }
        Pass
        {
            Name "Sprite Unlit"
            Tags
            {
                "LightMode" = "Universal2D"
            }
        
        // Render State
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITEUNLIT
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float2 NDCPosition;
             float2 PixelPosition;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 color : INTERP1;
             float3 positionWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.color.xyzw = input.color;
            output.positionWS.xyz = input.positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.color = input.color.xyzw;
            output.positionWS = input.positionWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float _PixelSize;
        float _MovingSpeed;
        float _NoiseScale;
        float4 _Color;
        float4 _Texture2D_TexelSize;
        float _Alpha;
        float _Munimum_Alpha;
        CBUFFER_END
        
        
        // Object and Global properties
        TEXTURE2D(_Texture2D);
        SAMPLER(sampler_Texture2D);
        
        // Graph Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Absolute_float(float In, out float Out)
        {
            Out = abs(In);
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Floor_float2(float2 In, out float2 Out)
        {
            Out = floor(In);
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        float2 Unity_GradientNoise_Deterministic_Dir_float(float2 p)
        {
            float x; Hash_Tchou_2_1_float(p, x);
            return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }
        
        void Unity_GradientNoise_Deterministic_float (float2 UV, float3 Scale, out float Out)
        {
            float2 p = UV * Scale.xy;
            float2 ip = floor(p);
            float2 fp = frac(p);
            float d00 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip), fp);
            float d01 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
            float d10 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
            float d11 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
            fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
            Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _Property_2d78280c8ad94f3793485529b56a40db_Out_0_Vector4 = _Color;
            float _Absolute_cc3fb0ef15ee40a5bcc5ffbdc9dceabf_Out_1_Float;
            Unity_Absolute_float(IN.TimeParameters.x, _Absolute_cc3fb0ef15ee40a5bcc5ffbdc9dceabf_Out_1_Float);
            float _Multiply_5d8177cb794e48a3abdd0bd979b4ae6a_Out_2_Float;
            Unity_Multiply_float_float(_Absolute_cc3fb0ef15ee40a5bcc5ffbdc9dceabf_Out_1_Float, unity_DeltaTime.z, _Multiply_5d8177cb794e48a3abdd0bd979b4ae6a_Out_2_Float);
            float _Clamp_915c797f935e4f538105be83f10c635b_Out_3_Float;
            Unity_Clamp_float(_Multiply_5d8177cb794e48a3abdd0bd979b4ae6a_Out_2_Float, 1, 1000, _Clamp_915c797f935e4f538105be83f10c635b_Out_3_Float);
            float4 _ScreenPosition_a3df1410bdda4f8e9f5581ccc9801333_Out_0_Vector4 = float4(IN.NDCPosition.xy, 0, 0);
            float4 _Add_33b411a8c24449e181ffb9b544514632_Out_2_Vector4;
            Unity_Add_float4(_ScreenPosition_a3df1410bdda4f8e9f5581ccc9801333_Out_0_Vector4, float4(0.5, 0.5, 0, 0), _Add_33b411a8c24449e181ffb9b544514632_Out_2_Vector4);
            float2 _Vector2_1f5076ab703548818e0f4c196c43d3c6_Out_0_Vector2 = float2(_ScreenParams.x, _ScreenParams.y);
            float _Property_404b0c3e70d94636ab369482aa60fcf8_Out_0_Float = _PixelSize;
            float _Float_6e386f11fc1d4dacb2a623575e2c63d5_Out_0_Float = _Property_404b0c3e70d94636ab369482aa60fcf8_Out_0_Float;
            float2 _Divide_d2f75c752e384442b24103c62056e2fa_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_1f5076ab703548818e0f4c196c43d3c6_Out_0_Vector2, (_Float_6e386f11fc1d4dacb2a623575e2c63d5_Out_0_Float.xx), _Divide_d2f75c752e384442b24103c62056e2fa_Out_2_Vector2);
            float2 _Multiply_9e27b1bfc6074b82a59e845c5629f87b_Out_2_Vector2;
            Unity_Multiply_float2_float2((_Add_33b411a8c24449e181ffb9b544514632_Out_2_Vector4.xy), _Divide_d2f75c752e384442b24103c62056e2fa_Out_2_Vector2, _Multiply_9e27b1bfc6074b82a59e845c5629f87b_Out_2_Vector2);
            float2 _Floor_8a97fefe39774ec69dcba15e810d6f40_Out_1_Vector2;
            Unity_Floor_float2(_Multiply_9e27b1bfc6074b82a59e845c5629f87b_Out_2_Vector2, _Floor_8a97fefe39774ec69dcba15e810d6f40_Out_1_Vector2);
            float2 _Divide_8218bbf2b13c42f794ecba2960023260_Out_2_Vector2;
            Unity_Divide_float2(_Floor_8a97fefe39774ec69dcba15e810d6f40_Out_1_Vector2, _Divide_d2f75c752e384442b24103c62056e2fa_Out_2_Vector2, _Divide_8218bbf2b13c42f794ecba2960023260_Out_2_Vector2);
            float2 _Subtract_1566bff3ea364cd5af0632277628a3b3_Out_2_Vector2;
            Unity_Subtract_float2(_Divide_8218bbf2b13c42f794ecba2960023260_Out_2_Vector2, float2(0.5, 0.5), _Subtract_1566bff3ea364cd5af0632277628a3b3_Out_2_Vector2);
            float2 _Multiply_e65febcbf49049e58915c814df653b0d_Out_2_Vector2;
            Unity_Multiply_float2_float2((_Clamp_915c797f935e4f538105be83f10c635b_Out_3_Float.xx), _Subtract_1566bff3ea364cd5af0632277628a3b3_Out_2_Vector2, _Multiply_e65febcbf49049e58915c814df653b0d_Out_2_Vector2);
            float _Property_22e689b3c73d465b8ceb9921d72b2bd3_Out_0_Float = _NoiseScale;
            float _GradientNoise_f33aa351bd34451eb2814cafc3992dd4_Out_2_Float;
            Unity_GradientNoise_Deterministic_float(_Multiply_e65febcbf49049e58915c814df653b0d_Out_2_Vector2, _Property_22e689b3c73d465b8ceb9921d72b2bd3_Out_0_Float, _GradientNoise_f33aa351bd34451eb2814cafc3992dd4_Out_2_Float);
            float4 _Multiply_1692971f93f9411294cdb6b6129f7b70_Out_2_Vector4;
            Unity_Multiply_float4_float4(_Property_2d78280c8ad94f3793485529b56a40db_Out_0_Vector4, (_GradientNoise_f33aa351bd34451eb2814cafc3992dd4_Out_2_Float.xxxx), _Multiply_1692971f93f9411294cdb6b6129f7b70_Out_2_Vector4);
            float _Split_ce1c7109aa5c40b090c0309dd7785f8d_R_1_Float = _Multiply_1692971f93f9411294cdb6b6129f7b70_Out_2_Vector4[0];
            float _Split_ce1c7109aa5c40b090c0309dd7785f8d_G_2_Float = _Multiply_1692971f93f9411294cdb6b6129f7b70_Out_2_Vector4[1];
            float _Split_ce1c7109aa5c40b090c0309dd7785f8d_B_3_Float = _Multiply_1692971f93f9411294cdb6b6129f7b70_Out_2_Vector4[2];
            float _Split_ce1c7109aa5c40b090c0309dd7785f8d_A_4_Float = _Multiply_1692971f93f9411294cdb6b6129f7b70_Out_2_Vector4[3];
            float _Property_ae45c1884db848cba268f825327b0dc2_Out_0_Float = _Alpha;
            float _Multiply_69d67b9963274d3eb712e0f55af1c55b_Out_2_Float;
            Unity_Multiply_float_float(_Split_ce1c7109aa5c40b090c0309dd7785f8d_A_4_Float, _Property_ae45c1884db848cba268f825327b0dc2_Out_0_Float, _Multiply_69d67b9963274d3eb712e0f55af1c55b_Out_2_Float);
            float _Property_2e0c1f8f3af94ea095ca32de7579d7ed_Out_0_Float = _Munimum_Alpha;
            float _Clamp_1c1b62464d754e899b22a228c24ae10d_Out_3_Float;
            Unity_Clamp_float(_Multiply_69d67b9963274d3eb712e0f55af1c55b_Out_2_Float, _Property_2e0c1f8f3af94ea095ca32de7579d7ed_Out_0_Float, 1, _Clamp_1c1b62464d754e899b22a228c24ae10d_Out_3_Float);
            surface.BaseColor = (_Multiply_1692971f93f9411294cdb6b6129f7b70_Out_2_Vector4.xyz);
            surface.Alpha = _Clamp_1c1b62464d754e899b22a228c24ae10d_Out_3_Float;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x < 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
            #else
            output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x > 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
            #endif
        
            output.NDCPosition = output.PixelPosition.xy / _ScaledScreenParams.xy;
            output.NDCPosition.y = 1.0f - output.NDCPosition.y;
        
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteUnlitPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    FallBack "Hidden/Shader Graph/FallbackError"
}