// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.30 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.30;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:2865,x:32719,y:32712,varname:node_2865,prsc:2|diff-6604-OUT,diffpow-6604-OUT,spec-358-OUT,gloss-1813-OUT,normal-2395-OUT,emission-1179-OUT,olwid-8541-OUT,olcol-9579-RGB;n:type:ShaderForge.SFN_Multiply,id:6343,x:31417,y:31569,varname:node_6343,prsc:2|A-7736-RGB,B-6665-RGB;n:type:ShaderForge.SFN_Color,id:6665,x:31189,y:31636,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Tex2d,id:7736,x:31189,y:31447,ptovrint:True,ptlb:Diffuse,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:24ace21d23507744e9ca0125277a57c9,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:5964,x:31367,y:32137,ptovrint:True,ptlb:Normal,ptin:_BumpMap,varname:_BumpMap,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:f7c27487b4792e640b8816a7e0e2f5dd,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Slider,id:358,x:33258,y:32776,ptovrint:False,ptlb:Metallic,ptin:_Metallic,varname:node_358,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Slider,id:1813,x:33258,y:32873,ptovrint:False,ptlb:Gloss,ptin:_Gloss,varname:_Metallic_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.6068376,max:1;n:type:ShaderForge.SFN_Slider,id:8541,x:31939,y:33504,ptovrint:False,ptlb:Outline_Width,ptin:_Outline_Width,varname:node_8541,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:0.05;n:type:ShaderForge.SFN_Color,id:9579,x:32096,y:33655,ptovrint:False,ptlb:Outline_Color,ptin:_Outline_Color,varname:node_9579,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Tex2d,id:5825,x:31130,y:32980,ptovrint:False,ptlb:EmissionMap,ptin:_EmissionMap,varname:node_5825,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:8d39881bd388b244a8abeb9eecd0a0c9,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:5688,x:32705,y:32584,ptovrint:False,ptlb:Damage,ptin:_Damage,varname:node_5688,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Time,id:9034,x:30159,y:32628,varname:node_9034,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:9246,x:30703,y:33004,ptovrint:False,ptlb:DM_Emission,ptin:_DM_Emission,varname:node_9246,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:f882b3d86d3cee6438b8d1b1f61d3b2e,ntxv:0,isnm:False|UVIN-9531-OUT;n:type:ShaderForge.SFN_Set,id:9732,x:33008,y:32584,varname:DamageVal,prsc:2|IN-5688-OUT;n:type:ShaderForge.SFN_Get,id:3542,x:30897,y:33057,varname:node_3542,prsc:2|IN-9732-OUT;n:type:ShaderForge.SFN_Max,id:5315,x:30887,y:32716,varname:node_5315,prsc:2|A-2746-OUT,B-6940-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6940,x:30688,y:32870,ptovrint:False,ptlb:DamageMin,ptin:_DamageMin,varname:node_6940,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.8;n:type:ShaderForge.SFN_Cos,id:5235,x:30542,y:32652,varname:node_5235,prsc:2|IN-9034-TTR;n:type:ShaderForge.SFN_Add,id:2746,x:30718,y:32690,varname:node_2746,prsc:2|A-5235-OUT,B-8774-OUT;n:type:ShaderForge.SFN_Sin,id:8774,x:30542,y:32768,varname:node_8774,prsc:2|IN-9034-TSL;n:type:ShaderForge.SFN_Multiply,id:2460,x:31109,y:32754,varname:node_2460,prsc:2|A-5315-OUT,B-9246-RGB,C-3542-OUT,D-297-OUT;n:type:ShaderForge.SFN_Tex2d,id:2379,x:31264,y:32339,ptovrint:False,ptlb:DM_Normal,ptin:_DM_Normal,varname:node_2379,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:a21c36b7b964e8743a512ee15b96c7be,ntxv:2,isnm:False|UVIN-6805-OUT;n:type:ShaderForge.SFN_Tex2d,id:443,x:31367,y:31895,ptovrint:False,ptlb:DM_Diffuse,ptin:_DM_Diffuse,varname:node_443,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:10c104aa30be2304180e147abc89e79f,ntxv:0,isnm:False|UVIN-9683-OUT;n:type:ShaderForge.SFN_Get,id:1586,x:31920,y:32046,varname:node_1586,prsc:2|IN-9732-OUT;n:type:ShaderForge.SFN_Get,id:1972,x:31449,y:32653,varname:node_1972,prsc:2|IN-9732-OUT;n:type:ShaderForge.SFN_Tex2d,id:1401,x:30751,y:32463,ptovrint:False,ptlb:Noise,ptin:_Noise,varname:node_1401,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-5396-UVOUT;n:type:ShaderForge.SFN_Panner,id:5396,x:30509,y:32463,varname:node_5396,prsc:2,spu:1,spv:1|UVIN-3345-UVOUT,DIST-9034-TSL;n:type:ShaderForge.SFN_TexCoord,id:3345,x:30304,y:32395,varname:node_3345,prsc:2,uv:0;n:type:ShaderForge.SFN_Add,id:297,x:30913,y:32600,varname:node_297,prsc:2|A-1401-RGB,B-6940-OUT;n:type:ShaderForge.SFN_TexCoord,id:9809,x:29939,y:32744,varname:node_9809,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:9531,x:30502,y:32947,varname:node_9531,prsc:2|A-5586-UVOUT,B-8728-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2872,x:32784,y:32455,ptovrint:False,ptlb:DamageSize,ptin:_DamageSize,varname:node_2872,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Set,id:2116,x:32987,y:32455,varname:DamageSize,prsc:2|IN-2872-OUT;n:type:ShaderForge.SFN_Slider,id:4732,x:32705,y:32339,ptovrint:False,ptlb:DamageOffset_X,ptin:_DamageOffset_X,varname:node_4732,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Set,id:875,x:33019,y:32339,varname:DamageOffsetX,prsc:2|IN-4732-OUT;n:type:ShaderForge.SFN_Get,id:8728,x:30258,y:33071,varname:node_8728,prsc:2|IN-2116-OUT;n:type:ShaderForge.SFN_Panner,id:6607,x:30115,y:32837,varname:node_6607,prsc:2,spu:1,spv:0|UVIN-9809-UVOUT,DIST-8185-OUT;n:type:ShaderForge.SFN_Get,id:8185,x:29900,y:32942,varname:node_8185,prsc:2|IN-875-OUT;n:type:ShaderForge.SFN_Get,id:8274,x:30432,y:32140,varname:node_8274,prsc:2|IN-875-OUT;n:type:ShaderForge.SFN_TexCoord,id:6867,x:30453,y:31976,varname:node_6867,prsc:2,uv:0;n:type:ShaderForge.SFN_Panner,id:6625,x:30645,y:32027,varname:node_6625,prsc:2,spu:1,spv:0|UVIN-6867-UVOUT,DIST-8274-OUT;n:type:ShaderForge.SFN_Get,id:4582,x:30791,y:32342,varname:node_4582,prsc:2|IN-2116-OUT;n:type:ShaderForge.SFN_Multiply,id:6805,x:31035,y:32218,varname:node_6805,prsc:2|A-1391-UVOUT,B-4582-OUT;n:type:ShaderForge.SFN_Get,id:5803,x:30483,y:31702,varname:node_5803,prsc:2|IN-875-OUT;n:type:ShaderForge.SFN_TexCoord,id:2811,x:30504,y:31552,varname:node_2811,prsc:2,uv:0;n:type:ShaderForge.SFN_Panner,id:6316,x:30704,y:31645,varname:node_6316,prsc:2,spu:1,spv:0|UVIN-2811-UVOUT,DIST-5803-OUT;n:type:ShaderForge.SFN_Get,id:5735,x:30911,y:31933,varname:node_5735,prsc:2|IN-2116-OUT;n:type:ShaderForge.SFN_Multiply,id:9683,x:31155,y:31809,varname:node_9683,prsc:2|A-7402-UVOUT,B-5735-OUT;n:type:ShaderForge.SFN_Slider,id:1209,x:32695,y:32223,ptovrint:False,ptlb:DamageOffset_Y,ptin:_DamageOffset_Y,varname:_DamageOffset_Y,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Set,id:2401,x:33019,y:32217,varname:DamageOffSetY,prsc:2|IN-1209-OUT;n:type:ShaderForge.SFN_Panner,id:5586,x:30336,y:32837,varname:node_5586,prsc:2,spu:0,spv:1|UVIN-6607-UVOUT,DIST-4812-OUT;n:type:ShaderForge.SFN_Get,id:4812,x:30115,y:33000,varname:node_4812,prsc:2|IN-2401-OUT;n:type:ShaderForge.SFN_Panner,id:1391,x:30861,y:32128,varname:node_1391,prsc:2,spu:0,spv:1|UVIN-6625-UVOUT,DIST-6235-OUT;n:type:ShaderForge.SFN_Get,id:6235,x:30584,y:32214,varname:node_6235,prsc:2|IN-2401-OUT;n:type:ShaderForge.SFN_Panner,id:7402,x:30932,y:31718,varname:node_7402,prsc:2,spu:0,spv:1|UVIN-6316-UVOUT,DIST-1155-OUT;n:type:ShaderForge.SFN_Get,id:1155,x:30683,y:31807,varname:node_1155,prsc:2|IN-2401-OUT;n:type:ShaderForge.SFN_Blend,id:6252,x:31791,y:31992,varname:node_6252,prsc:2,blmd:1,clmp:True|SRC-6343-OUT,DST-443-RGB;n:type:ShaderForge.SFN_Lerp,id:2395,x:31826,y:32382,varname:node_2395,prsc:2|A-5964-RGB,B-5076-OUT,T-1972-OUT;n:type:ShaderForge.SFN_Lerp,id:6604,x:32114,y:31940,varname:node_6604,prsc:2|A-6343-OUT,B-6252-OUT,T-1586-OUT;n:type:ShaderForge.SFN_Blend,id:1179,x:31650,y:32936,varname:node_1179,prsc:2,blmd:6,clmp:True|SRC-2460-OUT,DST-5825-RGB;n:type:ShaderForge.SFN_NormalBlend,id:5076,x:31562,y:32386,varname:node_5076,prsc:2|BSE-5964-RGB,DTL-2379-RGB;proporder:7736-5964-5825-6665-358-1813-8541-9579-5688-6940-443-2379-9246-1401-2872-4732-1209;pass:END;sub:END;*/

Shader "Shader Forge/Ship" {
    Properties {
        _MainTex ("Diffuse", 2D) = "white" {}
        _BumpMap ("Normal", 2D) = "bump" {}
        _EmissionMap ("EmissionMap", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Metallic ("Metallic", Range(0, 1)) = 1
        _Gloss ("Gloss", Range(0, 1)) = 0.6068376
        _Outline_Width ("Outline_Width", Range(0, 0.05)) = 0
        _Outline_Color ("Outline_Color", Color) = (1,1,1,1)
        _Damage ("Damage", Range(0, 1)) = 0
        _DamageMin ("DamageMin", Float ) = 0.8
        _DM_Diffuse ("DM_Diffuse", 2D) = "white" {}
        _DM_Normal ("DM_Normal", 2D) = "black" {}
        _DM_Emission ("DM_Emission", 2D) = "white" {}
        _Noise ("Noise", 2D) = "white" {}
        _DamageSize ("DamageSize", Float ) = 1
        _DamageOffset_X ("DamageOffset_X", Range(0, 1)) = 0
        _DamageOffset_Y ("DamageOffset_Y", Range(0, 1)) = 0
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "Outline"
            Tags {
            }
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float _Outline_Width;
            uniform float4 _Outline_Color;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv1 : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                UNITY_FOG_COORDS(3)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(float4(v.vertex.xyz + v.normal*_Outline_Width,1) );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                return fixed4(_Outline_Color.rgb,0);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
            uniform float _Metallic;
            uniform float _Gloss;
            uniform sampler2D _EmissionMap; uniform float4 _EmissionMap_ST;
            uniform float _Damage;
            uniform sampler2D _DM_Emission; uniform float4 _DM_Emission_ST;
            uniform float _DamageMin;
            uniform sampler2D _DM_Normal; uniform float4 _DM_Normal_ST;
            uniform sampler2D _DM_Diffuse; uniform float4 _DM_Diffuse_ST;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _DamageSize;
            uniform float _DamageOffset_X;
            uniform float _DamageOffset_Y;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD10;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                #ifdef LIGHTMAP_ON
                    o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    o.ambientOrLightmapUV.zw = 0;
                #elif UNITY_SHOULD_SAMPLE_SH
                #endif
                #ifdef DYNAMICLIGHTMAP_ON
                    o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
                #endif
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(i.uv0, _BumpMap)));
                float DamageOffSetY = _DamageOffset_Y;
                float DamageOffsetX = _DamageOffset_X;
                float DamageSize = _DamageSize;
                float2 node_6805 = (((i.uv0+DamageOffsetX*float2(1,0))+DamageOffSetY*float2(0,1))*DamageSize);
                float4 _DM_Normal_var = tex2D(_DM_Normal,TRANSFORM_TEX(node_6805, _DM_Normal));
                float3 node_5076_nrm_base = _BumpMap_var.rgb + float3(0,0,1);
                float3 node_5076_nrm_detail = _DM_Normal_var.rgb * float3(-1,-1,1);
                float3 node_5076_nrm_combined = node_5076_nrm_base*dot(node_5076_nrm_base, node_5076_nrm_detail)/node_5076_nrm_base.z - node_5076_nrm_detail;
                float3 node_5076 = node_5076_nrm_combined;
                float DamageVal = _Damage;
                float3 normalLocal = lerp(_BumpMap_var.rgb,node_5076,DamageVal);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = _Gloss;
                float specPow = exp2( gloss * 10.0+1.0);
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                    d.ambient = 0;
                    d.lightmapUV = i.ambientOrLightmapUV;
                #else
                    d.ambient = i.ambientOrLightmapUV;
                #endif
                d.boxMax[0] = unity_SpecCube0_BoxMax;
                d.boxMin[0] = unity_SpecCube0_BoxMin;
                d.probePosition[0] = unity_SpecCube0_ProbePosition;
                d.probeHDR[0] = unity_SpecCube0_HDR;
                d.boxMax[1] = unity_SpecCube1_BoxMax;
                d.boxMin[1] = unity_SpecCube1_BoxMin;
                d.probePosition[1] = unity_SpecCube1_ProbePosition;
                d.probeHDR[1] = unity_SpecCube1_HDR;
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float LdotH = max(0.0,dot(lightDirection, halfDirection));
                float3 specularColor = _Metallic;
                float specularMonochrome;
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 node_6343 = (_MainTex_var.rgb*_Color.rgb);
                float2 node_9683 = (((i.uv0+DamageOffsetX*float2(1,0))+DamageOffSetY*float2(0,1))*DamageSize);
                float4 _DM_Diffuse_var = tex2D(_DM_Diffuse,TRANSFORM_TEX(node_9683, _DM_Diffuse));
                float3 node_6604 = lerp(node_6343,saturate((node_6343*_DM_Diffuse_var.rgb)),DamageVal);
                float3 diffuseColor = node_6604; // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = max(0.0,dot( normalDirection, viewDirection ));
                float NdotH = max(0.0,dot( normalDirection, halfDirection ));
                float VdotH = max(0.0,dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, 1.0-gloss );
                float normTerm = max(0.0, GGXTerm(NdotH, 1.0-gloss));
                float specularPBL = (NdotL*visTerm*normTerm) * (UNITY_PI / 4);
                if (IsGammaSpace())
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                specularPBL = max(0, specularPBL * NdotL);
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz)*specularPBL*FresnelTerm(specularColor, LdotH);
                half grazingTerm = saturate( gloss + specularMonochrome );
                float3 indirectSpecular = (gi.indirect.specular);
                indirectSpecular *= FresnelLerp (specularColor, grazingTerm, NdotV);
                float3 specular = (directSpecular + indirectSpecular);
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += gi.indirect.diffuse;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float4 node_9034 = _Time + _TimeEditor;
                float2 node_9531 = (((i.uv0+DamageOffsetX*float2(1,0))+DamageOffSetY*float2(0,1))*DamageSize);
                float4 _DM_Emission_var = tex2D(_DM_Emission,TRANSFORM_TEX(node_9531, _DM_Emission));
                float2 node_5396 = (i.uv0+node_9034.r*float2(1,1));
                float4 _Noise_var = tex2D(_Noise,TRANSFORM_TEX(node_5396, _Noise));
                float4 _EmissionMap_var = tex2D(_EmissionMap,TRANSFORM_TEX(i.uv0, _EmissionMap));
                float3 emissive = saturate((1.0-(1.0-(max((cos(node_9034.a)+sin(node_9034.r)),_DamageMin)*_DM_Emission_var.rgb*DamageVal*(_Noise_var.rgb+_DamageMin)))*(1.0-_EmissionMap_var.rgb)));
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
            uniform float _Metallic;
            uniform float _Gloss;
            uniform sampler2D _EmissionMap; uniform float4 _EmissionMap_ST;
            uniform float _Damage;
            uniform sampler2D _DM_Emission; uniform float4 _DM_Emission_ST;
            uniform float _DamageMin;
            uniform sampler2D _DM_Normal; uniform float4 _DM_Normal_ST;
            uniform sampler2D _DM_Diffuse; uniform float4 _DM_Diffuse_ST;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _DamageSize;
            uniform float _DamageOffset_X;
            uniform float _DamageOffset_Y;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(i.uv0, _BumpMap)));
                float DamageOffSetY = _DamageOffset_Y;
                float DamageOffsetX = _DamageOffset_X;
                float DamageSize = _DamageSize;
                float2 node_6805 = (((i.uv0+DamageOffsetX*float2(1,0))+DamageOffSetY*float2(0,1))*DamageSize);
                float4 _DM_Normal_var = tex2D(_DM_Normal,TRANSFORM_TEX(node_6805, _DM_Normal));
                float3 node_5076_nrm_base = _BumpMap_var.rgb + float3(0,0,1);
                float3 node_5076_nrm_detail = _DM_Normal_var.rgb * float3(-1,-1,1);
                float3 node_5076_nrm_combined = node_5076_nrm_base*dot(node_5076_nrm_base, node_5076_nrm_detail)/node_5076_nrm_base.z - node_5076_nrm_detail;
                float3 node_5076 = node_5076_nrm_combined;
                float DamageVal = _Damage;
                float3 normalLocal = lerp(_BumpMap_var.rgb,node_5076,DamageVal);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = _Gloss;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float LdotH = max(0.0,dot(lightDirection, halfDirection));
                float3 specularColor = _Metallic;
                float specularMonochrome;
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 node_6343 = (_MainTex_var.rgb*_Color.rgb);
                float2 node_9683 = (((i.uv0+DamageOffsetX*float2(1,0))+DamageOffSetY*float2(0,1))*DamageSize);
                float4 _DM_Diffuse_var = tex2D(_DM_Diffuse,TRANSFORM_TEX(node_9683, _DM_Diffuse));
                float3 node_6604 = lerp(node_6343,saturate((node_6343*_DM_Diffuse_var.rgb)),DamageVal);
                float3 diffuseColor = node_6604; // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = max(0.0,dot( normalDirection, viewDirection ));
                float NdotH = max(0.0,dot( normalDirection, halfDirection ));
                float VdotH = max(0.0,dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, 1.0-gloss );
                float normTerm = max(0.0, GGXTerm(NdotH, 1.0-gloss));
                float specularPBL = (NdotL*visTerm*normTerm) * (UNITY_PI / 4);
                if (IsGammaSpace())
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                specularPBL = max(0, specularPBL * NdotL);
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _Metallic;
            uniform float _Gloss;
            uniform sampler2D _EmissionMap; uniform float4 _EmissionMap_ST;
            uniform float _Damage;
            uniform sampler2D _DM_Emission; uniform float4 _DM_Emission_ST;
            uniform float _DamageMin;
            uniform sampler2D _DM_Diffuse; uniform float4 _DM_Diffuse_ST;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _DamageSize;
            uniform float _DamageOffset_X;
            uniform float _DamageOffset_Y;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                float4 node_9034 = _Time + _TimeEditor;
                float DamageOffSetY = _DamageOffset_Y;
                float DamageOffsetX = _DamageOffset_X;
                float DamageSize = _DamageSize;
                float2 node_9531 = (((i.uv0+DamageOffsetX*float2(1,0))+DamageOffSetY*float2(0,1))*DamageSize);
                float4 _DM_Emission_var = tex2D(_DM_Emission,TRANSFORM_TEX(node_9531, _DM_Emission));
                float DamageVal = _Damage;
                float2 node_5396 = (i.uv0+node_9034.r*float2(1,1));
                float4 _Noise_var = tex2D(_Noise,TRANSFORM_TEX(node_5396, _Noise));
                float4 _EmissionMap_var = tex2D(_EmissionMap,TRANSFORM_TEX(i.uv0, _EmissionMap));
                o.Emission = saturate((1.0-(1.0-(max((cos(node_9034.a)+sin(node_9034.r)),_DamageMin)*_DM_Emission_var.rgb*DamageVal*(_Noise_var.rgb+_DamageMin)))*(1.0-_EmissionMap_var.rgb)));
                
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 node_6343 = (_MainTex_var.rgb*_Color.rgb);
                float2 node_9683 = (((i.uv0+DamageOffsetX*float2(1,0))+DamageOffSetY*float2(0,1))*DamageSize);
                float4 _DM_Diffuse_var = tex2D(_DM_Diffuse,TRANSFORM_TEX(node_9683, _DM_Diffuse));
                float3 node_6604 = lerp(node_6343,saturate((node_6343*_DM_Diffuse_var.rgb)),DamageVal);
                float3 diffColor = node_6604;
                float specularMonochrome;
                float3 specColor;
                diffColor = DiffuseAndSpecularFromMetallic( diffColor, _Metallic, specColor, specularMonochrome );
                float roughness = 1.0 - _Gloss;
                o.Albedo = diffColor + specColor * roughness * roughness * 0.5;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
