// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.05 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.05;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:0,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:2,dpts:2,wrdp:True,dith:0,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:8609,x:32354,y:33242,varname:node_8609,prsc:2|emission-4812-OUT,custl-5667-RGB;n:type:ShaderForge.SFN_Tex2d,id:5667,x:31825,y:33203,ptovrint:False,ptlb:node_5667,ptin:_node_5667,varname:node_5667,prsc:2,ntxv:1,isnm:False|MIP-8427-OUT;n:type:ShaderForge.SFN_NormalVector,id:6482,x:31332,y:33178,prsc:2,pt:False;n:type:ShaderForge.SFN_LightAttenuation,id:2168,x:31329,y:33323,varname:node_2168,prsc:2;n:type:ShaderForge.SFN_Dot,id:8427,x:31487,y:33189,varname:node_8427,prsc:2,dt:1|A-6482-OUT,B-2168-OUT;n:type:ShaderForge.SFN_Fresnel,id:7892,x:31584,y:33497,varname:node_7892,prsc:2|NRM-7741-XYZ,EXP-2307-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2307,x:31526,y:33954,ptovrint:False,ptlb:node_2307,ptin:_node_2307,varname:node_2307,prsc:2,glob:False,v1:0;n:type:ShaderForge.SFN_Color,id:1656,x:31815,y:33843,ptovrint:False,ptlb:node_1656,ptin:_node_1656,varname:node_1656,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_LightAttenuation,id:1952,x:31171,y:33635,varname:node_1952,prsc:2;n:type:ShaderForge.SFN_Multiply,id:4812,x:31780,y:33497,varname:node_4812,prsc:2|A-7892-OUT,B-1656-RGB,C-8392-OUT,D-8208-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8208,x:31264,y:33836,ptovrint:False,ptlb:node_8208,ptin:_node_8208,varname:node_8208,prsc:2,glob:False,v1:0;n:type:ShaderForge.SFN_LightPosition,id:7741,x:31163,y:33355,varname:node_7741,prsc:2;n:type:ShaderForge.SFN_Dot,id:8392,x:31329,y:33513,varname:node_8392,prsc:2,dt:1|A-9684-OUT,B-1952-OUT;n:type:ShaderForge.SFN_NormalVector,id:9684,x:31163,y:33489,prsc:2,pt:True;proporder:5667-2307-1656-8208;pass:END;sub:END;*/

Shader "Shader Forge/NewShader02" {
    Properties {
        _Diffuse ("node_5667", 2D) = "gray" {}
        _node_2307 ("node_2307", Float ) = 0
        _node_1656 ("node_1656", Color) = (0.5,0.5,0.5,1)
        _node_8208 ("node_8208", Float ) = 0
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #pragma glsl
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _node_2307;
            uniform float4 _node_1656;
            uniform float _node_8208;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                
                float nSign = sign( dot( viewDirection, i.normalDir ) ); // Reverse normal if this is a backface
                i.normalDir *= nSign;
                normalDirection *= nSign;
                
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
////// Emissive:
                float3 emissive = (pow(1.0-max(0,dot(_WorldSpaceLightPos0.rgb, viewDirection)),_node_2307)*_node_1656.rgb*max(0,dot(normalDirection,attenuation))*_node_8208);
                float4 _node_5667_var = tex2Dlod(_Diffuse,float4(TRANSFORM_TEX(i.uv0, _Diffuse),0.0,max(0,dot(i.normalDir,attenuation))));
                float3 finalColor = emissive + _node_5667_var.rgb;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            Cull Off
            
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #pragma glsl
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _node_2307;
            uniform float4 _node_1656;
            uniform float _node_8208;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                
                float nSign = sign( dot( viewDirection, i.normalDir ) ); // Reverse normal if this is a backface
                i.normalDir *= nSign;
                normalDirection *= nSign;
                
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float4 _node_5667_var = tex2Dlod(_Diffuse,float4(TRANSFORM_TEX(i.uv0, _Diffuse),0.0,max(0,dot(i.normalDir,attenuation))));
                float3 finalColor = _node_5667_var.rgb;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
