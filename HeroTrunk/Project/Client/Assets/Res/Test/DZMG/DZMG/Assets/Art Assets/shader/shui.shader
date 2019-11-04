// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.35 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.35;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5691501,fgcg:0.8897059,fgcb:0.5978896,fgca:1,fgde:0.015,fgrn:19.8,fgrf:29.89,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:9361,x:34341,y:32752,varname:node_9361,prsc:2|diff-2822-OUT,alpha-5567-OUT,refract-7185-OUT,voffset-1916-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4965,x:32189,y:32733,ptovrint:False,ptlb:Uspeed,ptin:_Uspeed,varname:_Uspeed,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.05;n:type:ShaderForge.SFN_Time,id:8916,x:32189,y:32839,varname:node_8916,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:710,x:32189,y:33042,ptovrint:False,ptlb:Vspeed,ptin:_Vspeed,varname:_Vspeed,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:5993,x:32406,y:32853,varname:node_5993,prsc:2|A-4965-OUT,B-8916-T;n:type:ShaderForge.SFN_Multiply,id:3849,x:32406,y:32990,varname:node_3849,prsc:2|A-8916-T,B-710-OUT;n:type:ShaderForge.SFN_Append,id:9839,x:32595,y:32893,varname:node_9839,prsc:2|A-5993-OUT,B-3849-OUT;n:type:ShaderForge.SFN_TexCoord,id:2449,x:32406,y:32689,varname:node_2449,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_ValueProperty,id:9172,x:32406,y:32572,ptovrint:False,ptlb:TEXspeed,ptin:_TEXspeed,varname:_TEXspeed,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:-0.02;n:type:ShaderForge.SFN_Add,id:8713,x:32772,y:32869,varname:node_8713,prsc:2|A-2449-UVOUT,B-9839-OUT;n:type:ShaderForge.SFN_Multiply,id:5959,x:32772,y:32660,varname:node_5959,prsc:2|A-9172-OUT,B-9839-OUT;n:type:ShaderForge.SFN_Tex2d,id:1961,x:32970,y:32869,ptovrint:False,ptlb:shui,ptin:_shui,varname:_shui,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:10d5d9cd1e41c6641bfb9ef8b2b87f62,ntxv:0,isnm:False|UVIN-8713-OUT;n:type:ShaderForge.SFN_ValueProperty,id:9256,x:32970,y:33115,ptovrint:False,ptlb:FlowIntnsity,ptin:_FlowIntnsity,varname:_FlowIntnsity,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:3172,x:33288,y:32870,varname:node_3172,prsc:2|A-520-OUT,B-9256-OUT;n:type:ShaderForge.SFN_Multiply,id:1269,x:33371,y:32577,varname:node_1269,prsc:2|A-8744-OUT,B-1961-R;n:type:ShaderForge.SFN_Slider,id:8744,x:32799,y:32431,ptovrint:False,ptlb:specular,ptin:_specular,varname:_specular,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Add,id:9438,x:33500,y:32792,varname:node_9438,prsc:2|A-5959-OUT,B-2449-UVOUT,C-3172-OUT;n:type:ShaderForge.SFN_Tex2d,id:5315,x:33647,y:32825,ptovrint:False,ptlb:node_5315,ptin:_node_5315,varname:_node_5315,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:6115c8cac54e5d449ad388160a199a49,ntxv:0,isnm:False|UVIN-9438-OUT;n:type:ShaderForge.SFN_Add,id:2822,x:33938,y:32798,varname:node_2822,prsc:2|A-1269-OUT,B-2431-OUT;n:type:ShaderForge.SFN_Multiply,id:5567,x:33940,y:33318,varname:node_5567,prsc:2|A-6590-OUT,B-9176-OUT;n:type:ShaderForge.SFN_Multiply,id:7185,x:33940,y:33437,varname:node_7185,prsc:2|A-9176-OUT,B-3172-OUT;n:type:ShaderForge.SFN_Multiply,id:1916,x:33940,y:33576,varname:node_1916,prsc:2|A-1961-R,B-2589-OUT,C-7084-OUT;n:type:ShaderForge.SFN_Slider,id:6590,x:33474,y:33321,ptovrint:False,ptlb:Alpha,ptin:_Alpha,varname:_Alpha,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5,max:1;n:type:ShaderForge.SFN_DepthBlend,id:9176,x:33611,y:33444,varname:node_9176,prsc:2|DIST-6665-OUT;n:type:ShaderForge.SFN_Slider,id:6665,x:33194,y:33475,ptovrint:False,ptlb:Distance,ptin:_Distance,varname:_Distance,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5,max:20;n:type:ShaderForge.SFN_Vector3,id:2589,x:33611,y:33635,varname:node_2589,prsc:2,v1:0,v2:0,v3:1;n:type:ShaderForge.SFN_Slider,id:7084,x:33492,y:33793,ptovrint:False,ptlb:Amp,ptin:_Amp,varname:_Amp,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5,max:20;n:type:ShaderForge.SFN_Append,id:520,x:33127,y:32886,varname:node_520,prsc:2|A-1961-R,B-1961-R;n:type:ShaderForge.SFN_Multiply,id:2431,x:33827,y:32961,varname:node_2431,prsc:2|A-5315-RGB,B-6938-RGB;n:type:ShaderForge.SFN_Color,id:6938,x:33647,y:33054,ptovrint:False,ptlb:node_6938,ptin:_node_6938,varname:_node_6938,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;proporder:4965-710-9172-1961-9256-8744-5315-7084-6590-6665-6938;pass:END;sub:END;*/

Shader "Shader Forge/shui" {
    Properties {
        _Uspeed ("Uspeed", Float ) = 0.05
        _Vspeed ("Vspeed", Float ) = 0
        _TEXspeed ("TEXspeed", Float ) = -0.02
        _shui ("shui", 2D) = "white" {}
        _FlowIntnsity ("FlowIntnsity", Float ) = 0
        _specular ("specular", Range(0, 1)) = 0
        _node_5315 ("node_5315", 2D) = "white" {}
        _Amp ("Amp", Range(0, 20)) = 0.5
        _Alpha ("Alpha", Range(0, 1)) = 0.5
        _Distance ("Distance", Range(0, 20)) = 0.5
        _node_6938 ("node_6938", Color) = (0.5,0.5,0.5,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
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
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _GrabTexture;
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _TimeEditor;
            uniform float _Uspeed;
            uniform float _Vspeed;
            uniform float _TEXspeed;
            uniform sampler2D _shui; uniform float4 _shui_ST;
            uniform float _FlowIntnsity;
            uniform float _specular;
            uniform sampler2D _node_5315; uniform float4 _node_5315_ST;
            uniform float _Alpha;
            uniform float _Distance;
            uniform float _Amp;
            uniform float4 _node_6938;
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
                float4 screenPos : TEXCOORD3;
                float4 projPos : TEXCOORD4;
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_8916 = _Time + _TimeEditor;
                float2 node_9839 = float2((_Uspeed*node_8916.g),(node_8916.g*_Vspeed));
                float2 node_8713 = (o.uv0+node_9839);
                float4 _shui_var = tex2Dlod(_shui,float4(TRANSFORM_TEX(node_8713, _shui),0.0,0));
                v.vertex.xyz += (_shui_var.r*float3(0,0,1)*_Amp);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float3 normalDirection = i.normalDir;
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float node_9176 = saturate((sceneZ-partZ)/_Distance);
                float4 node_8916 = _Time + _TimeEditor;
                float2 node_9839 = float2((_Uspeed*node_8916.g),(node_8916.g*_Vspeed));
                float2 node_8713 = (i.uv0+node_9839);
                float4 _shui_var = tex2D(_shui,TRANSFORM_TEX(node_8713, _shui));
                float2 node_3172 = (float2(_shui_var.r,_shui_var.r)*_FlowIntnsity);
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + (node_9176*node_3172);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float2 node_9438 = ((_TEXspeed*node_9839)+i.uv0+node_3172);
                float4 _node_5315_var = tex2D(_node_5315,TRANSFORM_TEX(node_9438, _node_5315));
                float3 diffuseColor = ((_specular*_shui_var.r)+(_node_5315_var.rgb*_node_6938.rgb));
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(lerp(sceneColor.rgb, finalColor,(_Alpha*node_9176)),1);
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
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _GrabTexture;
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _TimeEditor;
            uniform float _Uspeed;
            uniform float _Vspeed;
            uniform float _TEXspeed;
            uniform sampler2D _shui; uniform float4 _shui_ST;
            uniform float _FlowIntnsity;
            uniform float _specular;
            uniform sampler2D _node_5315; uniform float4 _node_5315_ST;
            uniform float _Alpha;
            uniform float _Distance;
            uniform float _Amp;
            uniform float4 _node_6938;
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
                float4 screenPos : TEXCOORD3;
                float4 projPos : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_8916 = _Time + _TimeEditor;
                float2 node_9839 = float2((_Uspeed*node_8916.g),(node_8916.g*_Vspeed));
                float2 node_8713 = (o.uv0+node_9839);
                float4 _shui_var = tex2Dlod(_shui,float4(TRANSFORM_TEX(node_8713, _shui),0.0,0));
                v.vertex.xyz += (_shui_var.r*float3(0,0,1)*_Amp);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                o.screenPos = o.pos;
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float3 normalDirection = i.normalDir;
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float node_9176 = saturate((sceneZ-partZ)/_Distance);
                float4 node_8916 = _Time + _TimeEditor;
                float2 node_9839 = float2((_Uspeed*node_8916.g),(node_8916.g*_Vspeed));
                float2 node_8713 = (i.uv0+node_9839);
                float4 _shui_var = tex2D(_shui,TRANSFORM_TEX(node_8713, _shui));
                float2 node_3172 = (float2(_shui_var.r,_shui_var.r)*_FlowIntnsity);
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + (node_9176*node_3172);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float2 node_9438 = ((_TEXspeed*node_9839)+i.uv0+node_3172);
                float4 _node_5315_var = tex2D(_node_5315,TRANSFORM_TEX(node_9438, _node_5315));
                float3 diffuseColor = ((_specular*_shui_var.r)+(_node_5315_var.rgb*_node_6938.rgb));
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor * (_Alpha*node_9176),0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float _Uspeed;
            uniform float _Vspeed;
            uniform sampler2D _shui; uniform float4 _shui_ST;
            uniform float _Amp;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float4 node_8916 = _Time + _TimeEditor;
                float2 node_9839 = float2((_Uspeed*node_8916.g),(node_8916.g*_Vspeed));
                float2 node_8713 = (o.uv0+node_9839);
                float4 _shui_var = tex2Dlod(_shui,float4(TRANSFORM_TEX(node_8713, _shui),0.0,0));
                v.vertex.xyz += (_shui_var.r*float3(0,0,1)*_Amp);
                o.pos = UnityObjectToClipPos(v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
