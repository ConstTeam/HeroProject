// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.35 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.35;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:9361,x:33209,y:32712,varname:node_9361,prsc:2|emission-6656-OUT,custl-6656-OUT;n:type:ShaderForge.SFN_Tex2d,id:2571,x:32648,y:32787,ptovrint:False,ptlb:node_7491,ptin:_node_7491,varname:node_7491,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-8296-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:1446,x:32648,y:32997,ptovrint:False,ptlb:node_7528,ptin:_node_7528,varname:node_7528,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-4734-UVOUT;n:type:ShaderForge.SFN_Multiply,id:6656,x:32864,y:32873,varname:node_6656,prsc:2|A-2571-RGB,B-1446-RGB,C-5538-OUT;n:type:ShaderForge.SFN_TexCoord,id:6544,x:32072,y:32731,varname:node_6544,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Panner,id:8296,x:32272,y:32731,varname:node_8296,prsc:2,spu:0.01,spv:0|UVIN-6544-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:4349,x:32079,y:33021,varname:node_4349,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Panner,id:4734,x:32279,y:33021,varname:node_4734,prsc:2,spu:0.5,spv:0|UVIN-4349-UVOUT;n:type:ShaderForge.SFN_Color,id:7909,x:32386,y:33320,ptovrint:False,ptlb:node_9461,ptin:_node_9461,varname:node_9461,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Add,id:5538,x:32611,y:33343,varname:node_5538,prsc:2|A-7909-RGB,B-6024-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6024,x:32413,y:33529,ptovrint:False,ptlb:node_9659,ptin:_node_9659,varname:node_9659,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;proporder:2571-1446-7909-6024;pass:END;sub:END;*/

Shader "Shader Forge/pvpsu" {
    Properties {
        _node_7491 ("node_7491", 2D) = "white" {}
        _node_7528 ("node_7528", 2D) = "white" {}
        _node_9461 ("node_9461", Color) = (0.5,0.5,0.5,1)
        _node_9659 ("node_9659", Float ) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _node_7491; uniform float4 _node_7491_ST;
            uniform sampler2D _node_7528; uniform float4 _node_7528_ST;
            uniform float4 _node_9461;
            uniform float _node_9659;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float4 node_9356 = _Time + _TimeEditor;
                float2 node_8296 = (i.uv0+node_9356.g*float2(0.01,0));
                float4 _node_7491_var = tex2D(_node_7491,TRANSFORM_TEX(node_8296, _node_7491));
                float2 node_4734 = (i.uv0+node_9356.g*float2(0.5,0));
                float4 _node_7528_var = tex2D(_node_7528,TRANSFORM_TEX(node_4734, _node_7528));
                float3 node_6656 = (_node_7491_var.rgb*_node_7528_var.rgb*(_node_9461.rgb+_node_9659));
                float3 emissive = node_6656;
                float3 finalColor = emissive + node_6656;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x 
            #pragma target 3.0
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos(v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
