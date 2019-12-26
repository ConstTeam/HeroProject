// Shader created with Shader Forge v1.35 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.35;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:2,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:9361,x:33530,y:32738,varname:node_9361,prsc:2|custl-2155-OUT,alpha-2474-OUT;n:type:ShaderForge.SFN_Tex2d,id:7069,x:32487,y:32741,ptovrint:False,ptlb:node_4858,ptin:_node_4858,varname:_node_4858,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:5422,x:32577,y:33245,ptovrint:False,ptlb:node_6937,ptin:_node_6937,varname:_node_6937,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Tex2d,id:2517,x:32349,y:32969,ptovrint:False,ptlb:node_2039,ptin:_node_2039,varname:node_2039,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:a608c6e3404b63a4cadbecef8f02d39e,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Add,id:2155,x:32866,y:32810,varname:node_2155,prsc:2|A-7069-RGB,B-2517-R,C-5422-RGB;n:type:ShaderForge.SFN_Lerp,id:7344,x:32936,y:33026,varname:node_7344,prsc:2|A-7069-R,B-2517-R,T-5422-A;n:type:ShaderForge.SFN_ValueProperty,id:7081,x:33086,y:33193,ptovrint:False,ptlb:node_9096,ptin:_node_9096,varname:node_9096,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:2474,x:33274,y:33033,varname:node_2474,prsc:2|A-7344-OUT,B-7081-OUT;proporder:7069-5422-2517-7081;pass:END;sub:END;*/

Shader "Shader Forge/Dark_JT" {
    Properties {
        _node_4858 ("node_4858", 2D) = "white" {}
        _node_6937 ("node_6937", Color) = (0.5,0.5,0.5,1)
        _node_2039 ("node_2039", 2D) = "white" {}
        _node_9096 ("node_9096", Float ) = 0
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
            uniform float4 _node_6937;
            uniform sampler2D _node_2039; uniform float4 _node_2039_ST;
            uniform float _node_9096;
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
                float4 _node_4858_var = tex2D(_node_4858,TRANSFORM_TEX(i.uv0, _node_4858));
                float4 _node_2039_var = tex2D(_node_2039,TRANSFORM_TEX(i.uv0, _node_2039));
                float3 finalColor = (_node_4858_var.rgb+_node_2039_var.r+_node_6937.rgb);
                return fixed4(finalColor,(lerp(_node_4858_var.r,_node_2039_var.r,_node_6937.a)*_node_9096));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
