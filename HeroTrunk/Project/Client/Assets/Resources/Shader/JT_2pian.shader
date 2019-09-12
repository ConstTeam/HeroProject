// Shader created with Shader Forge v1.35 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.35;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:2,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:9361,x:33209,y:32712,varname:node_9361,prsc:2|emission-8267-OUT;n:type:ShaderForge.SFN_Tex2d,id:6769,x:32584,y:32630,ptovrint:False,ptlb:node_4858,ptin:_node_4858,varname:_node_4858,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-465-UVOUT;n:type:ShaderForge.SFN_ValueProperty,id:7049,x:32590,y:33220,ptovrint:False,ptlb:node_1653,ptin:_node_1653,varname:_node_1653,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Color,id:2487,x:32495,y:32992,ptovrint:False,ptlb:node_6937,ptin:_node_6937,varname:_node_6937,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:8267,x:32940,y:32842,varname:node_8267,prsc:2|A-3229-OUT,B-2487-RGB,C-7049-OUT;n:type:ShaderForge.SFN_TexCoord,id:7893,x:31973,y:32675,varname:node_7893,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Vector1,id:9536,x:31991,y:32906,varname:node_9536,prsc:2,v1:10;n:type:ShaderForge.SFN_Tex2d,id:7484,x:32340,y:32267,ptovrint:False,ptlb:node_4858_copy,ptin:_node_4858_copy,varname:_node_4858_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:3229,x:32777,y:32526,varname:node_3229,prsc:2|A-1864-OUT,B-6769-RGB;n:type:ShaderForge.SFN_Multiply,id:1864,x:32584,y:32252,varname:node_1864,prsc:2|A-7484-RGB,B-8621-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8621,x:32340,y:32492,ptovrint:False,ptlb:node_8621,ptin:_node_8621,varname:node_8621,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Panner,id:465,x:32249,y:32651,varname:node_465,prsc:2,spu:0,spv:1|UVIN-7893-UVOUT,DIST-9536-OUT;proporder:6769-7049-2487-7484-8621;pass:END;sub:END;*/

Shader "Shader Forge/JT_2pian" {
    Properties {
        _node_4858 ("node_4858", 2D) = "white" {}
        _node_1653 ("node_1653", Float ) = 0
        _node_6937 ("node_6937", Color) = (0.5,0.5,0.5,1)
        _node_4858_copy ("node_4858_copy", 2D) = "white" {}
        _node_8621 ("node_8621", Float ) = 0
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
            uniform sampler2D _node_4858; uniform float4 _node_4858_ST;
            uniform float _node_1653;
            uniform float4 _node_6937;
            uniform sampler2D _node_4858_copy; uniform float4 _node_4858_copy_ST;
            uniform float _node_8621;
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
                float4 _node_4858_copy_var = tex2D(_node_4858_copy,TRANSFORM_TEX(i.uv0, _node_4858_copy));
                float node_9536 = 10.0;
                float2 node_465 = (i.uv0+node_9536*float2(0,1));
                float4 _node_4858_var = tex2D(_node_4858,TRANSFORM_TEX(node_465, _node_4858));
                float3 emissive = (((_node_4858_copy_var.rgb*_node_8621)*_node_4858_var.rgb)*_node_6937.rgb*_node_1653);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
