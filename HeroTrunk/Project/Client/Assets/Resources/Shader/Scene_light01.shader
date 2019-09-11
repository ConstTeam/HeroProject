// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.35 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.35;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.4173335,fgcg:0.4346335,fgcb:0.6102941,fgca:1,fgde:0.01,fgrn:21.1,fgrf:153,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:-500,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:9361,x:33209,y:32712,varname:node_9361,prsc:2|custl-9718-OUT;n:type:ShaderForge.SFN_Tex2d,id:460,x:32461,y:32693,ptovrint:False,ptlb:node_460,ptin:_node_460,varname:_node_460,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-263-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:3162,x:32461,y:32924,ptovrint:False,ptlb:node_3162,ptin:_node_3162,varname:_node_3162,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-263-UVOUT;n:type:ShaderForge.SFN_ValueProperty,id:6243,x:32363,y:32462,ptovrint:False,ptlb:node_6243,ptin:_node_6243,varname:_node_6243,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Tex2d,id:2580,x:32448,y:33165,ptovrint:False,ptlb:mask,ptin:_mask,varname:_mask,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:9718,x:32881,y:32951,varname:node_9718,prsc:2|A-8232-OUT,B-2580-RGB,C-7955-RGB;n:type:ShaderForge.SFN_Add,id:8232,x:32648,y:32693,varname:node_8232,prsc:2|A-460-RGB,B-3162-RGB,C-6243-OUT;n:type:ShaderForge.SFN_Color,id:7955,x:32162,y:33221,ptovrint:False,ptlb:node_7955,ptin:_node_7955,varname:_node_7955,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_TexCoord,id:7849,x:31886,y:32940,varname:node_7849,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Panner,id:263,x:32130,y:32925,varname:node_263,prsc:2,spu:0.2,spv:0|UVIN-7849-UVOUT;proporder:460-3162-6243-2580-7955;pass:END;sub:END;*/

Shader "Shader Forge/Scene_light01" {
    Properties {
        _node_460 ("node_460", 2D) = "white" {}
        _node_3162 ("node_3162", 2D) = "white" {}
        _node_6243 ("node_6243", Float ) = 0
        _mask ("mask", 2D) = "white" {}
        _node_7955 ("node_7955", Color) = (0.5,0.5,0.5,1)
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
            Offset 0, -500
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _node_460; uniform float4 _node_460_ST;
            uniform sampler2D _node_3162; uniform float4 _node_3162_ST;
            uniform float _node_6243;
            uniform sampler2D _mask; uniform float4 _mask_ST;
            uniform float4 _node_7955;
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
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
                float4 node_1051 = _Time + _TimeEditor;
                float2 node_263 = (i.uv0+node_1051.g*float2(0.2,0));
                float4 _node_460_var = tex2D(_node_460,TRANSFORM_TEX(node_263, _node_460));
                float4 _node_3162_var = tex2D(_node_3162,TRANSFORM_TEX(node_263, _node_3162));
                float4 _mask_var = tex2D(_mask,TRANSFORM_TEX(i.uv0, _mask));
                float3 finalColor = ((_node_460_var.rgb+_node_3162_var.rgb+_node_6243)*_mask_var.rgb*_node_7955.rgb);
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
