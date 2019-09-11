// Shader created with Shader Forge v1.35 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.35;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:2,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.4173335,fgcg:0.4346335,fgcb:0.6102941,fgca:1,fgde:0.01,fgrn:21.1,fgrf:153,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:9361,x:33389,y:32734,varname:node_9361,prsc:2|emission-7411-OUT;n:type:ShaderForge.SFN_Tex2d,id:3058,x:32520,y:32566,ptovrint:False,ptlb:node_4858,ptin:_node_4858,varname:_node_4858,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-1383-UVOUT;n:type:ShaderForge.SFN_ValueProperty,id:2158,x:32526,y:33156,ptovrint:False,ptlb:node_1653,ptin:_node_1653,varname:_node_1653,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Color,id:942,x:32431,y:32928,ptovrint:False,ptlb:node_6937,ptin:_node_6937,varname:_node_6937,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:7411,x:32876,y:32778,varname:node_7411,prsc:2|A-3058-RGB,B-942-RGB,C-2158-OUT;n:type:ShaderForge.SFN_Rotator,id:1383,x:32127,y:32611,varname:node_1383,prsc:2|UVIN-2639-UVOUT,SPD-7125-OUT;n:type:ShaderForge.SFN_TexCoord,id:2639,x:31909,y:32611,varname:node_2639,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Vector1,id:7125,x:31927,y:32842,varname:node_7125,prsc:2,v1:10;proporder:3058-2158-942;pass:END;sub:END;*/

Shader "Shader Forge/Effect_Light001_JT" {
    Properties {
        _node_4858 ("node_4858", 2D) = "white" {}
        _node_1653 ("node_1653", Float ) = 0
        _node_6937 ("node_6937", Color) = (0.5,0.5,0.5,1)
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
            uniform sampler2D _node_4858; uniform float4 _node_4858_ST;
            uniform float _node_1653;
            uniform float4 _node_6937;
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
                float4x4 bbmv = UNITY_MATRIX_MV;
                bbmv._m00 = -1.0/length(unity_WorldToObject[0].xyz);
                bbmv._m10 = 0.0f;
                bbmv._m20 = 0.0f;
                bbmv._m01 = 0.0f;
                bbmv._m11 = -1.0/length(unity_WorldToObject[1].xyz);
                bbmv._m21 = 0.0f;
                bbmv._m02 = 0.0f;
                bbmv._m12 = 0.0f;
                bbmv._m22 = -1.0/length(unity_WorldToObject[2].xyz);
                o.pos = mul( UNITY_MATRIX_P, mul( bbmv, v.vertex ));
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_9574 = _Time + _TimeEditor;
                float node_1383_ang = node_9574.g;
                float node_1383_spd = 10.0;
                float node_1383_cos = cos(node_1383_spd*node_1383_ang);
                float node_1383_sin = sin(node_1383_spd*node_1383_ang);
                float2 node_1383_piv = float2(0.5,0.5);
                float2 node_1383 = (mul(i.uv0-node_1383_piv,float2x2( node_1383_cos, -node_1383_sin, node_1383_sin, node_1383_cos))+node_1383_piv);
                float4 _node_4858_var = tex2D(_node_4858,TRANSFORM_TEX(node_1383, _node_4858));
                float3 emissive = (_node_4858_var.rgb*_node_6937.rgb*_node_1653);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
