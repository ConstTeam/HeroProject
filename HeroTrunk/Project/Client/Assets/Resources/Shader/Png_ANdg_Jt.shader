// Shader created with Shader Forge v1.35 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.35;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:2,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:9361,x:32561,y:32500,varname:node_9361,prsc:2|custl-8878-OUT,alpha-8768-OUT;n:type:ShaderForge.SFN_Tex2d,id:7892,x:31682,y:32446,ptovrint:False,ptlb:node_4858,ptin:_node_4858,varname:_node_4858,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:8878,x:32147,y:32888,varname:node_8878,prsc:2|A-7892-RGB,B-4124-OUT,C-7805-RGB,D-4206-RGB;n:type:ShaderForge.SFN_ValueProperty,id:4124,x:31724,y:33036,ptovrint:False,ptlb:node_1653,ptin:_node_1653,varname:_node_1653,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Color,id:7805,x:31629,y:32808,ptovrint:False,ptlb:node_6937,ptin:_node_6937,varname:_node_6937,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Lerp,id:222,x:32118,y:32666,varname:node_222,prsc:2|A-7892-A,B-7805-A,T-4124-OUT;n:type:ShaderForge.SFN_Multiply,id:3530,x:32317,y:32747,varname:node_3530,prsc:2|A-222-OUT,B-7805-A,C-7892-A;n:type:ShaderForge.SFN_Add,id:2167,x:32010,y:32378,varname:node_2167,prsc:2|A-7892-R,B-7892-B,C-7892-G;n:type:ShaderForge.SFN_Multiply,id:8768,x:32217,y:32341,varname:node_8768,prsc:2|A-2167-OUT,B-4124-OUT,C-4206-A;n:type:ShaderForge.SFN_VertexColor,id:4206,x:31656,y:32655,varname:node_4206,prsc:2;proporder:7892-4124-7805;pass:END;sub:END;*/

Shader "Shader Forge/Png_ANdg_Jt" {
    Properties {
        _node_4858 ("node_4858", 2D) = "white" {}
        _node_1653 ("node_1653", Float ) = 0
        _node_6937 ("node_6937", Color) = (0.5,0.5,0.5,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
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
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x 
            #pragma target 3.0
            uniform sampler2D _node_4858; uniform float4 _node_4858_ST;
            uniform float _node_1653;
            uniform float4 _node_6937;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
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
                float4 _node_4858_var = tex2D(_node_4858,TRANSFORM_TEX(i.uv0, _node_4858));
                float3 finalColor = (_node_4858_var.rgb*_node_1653*_node_6937.rgb*i.vertexColor.rgb);
                return fixed4(finalColor,((_node_4858_var.r+_node_4858_var.b+_node_4858_var.g)*_node_1653*i.vertexColor.a));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
