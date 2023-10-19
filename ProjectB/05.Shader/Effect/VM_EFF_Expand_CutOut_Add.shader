// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Shader/Effect/Expand/VM_EFF_Expand_CutOut_Add" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _MainTexIntensity ("-MainTex Intensity", Float ) = 1
        [MaterialToggle] _UseMainTexAlphaChannel ("-Use MainTex AlphaChannel", Float ) = 0
		_StartCutoffValue ("[Start Cutoff Value]", Range(0.01, 1)) = 0.1
		
		[Header(Medium)]
        [MaterialToggle] _UseSubTexIntepolatebyAlpha ("[Use SubTex Intepolate by Alpha]", Float ) = 0
        _SubTex ("SubTex", 2D) = "white" {}
        _SubTexIntensity ("-SubTex Intensity", Float ) = 1
        [MaterialToggle] _UseSubTexAlphaChannel ("-Use SubTex AlphaChannel", Float ) = 0
        [Space]
        [MaterialToggle] _UseSoftOpacity ("[Use Soft Opacity]", Float ) = 0
        _OpacitybyHeight ("-Opacity by Height", Float ) = -1.7
		
		[Header(High)]
        [MaterialToggle] _UseMainTexRotation ("[Use MainTex Rotation]", Float ) = 0
        _TextureRotation ("-Texture Rotation", Range(-1, 1)) = 0
		
		[MaterialToggle] _UseOutLine ("[Use OutLine]", Float ) = 0
        _OutLineWidth ("-OutLine Width", Range(0, 1)) = 0.05
        _OutLineColor ("-OutLine Color", Color) = (0.5,0.5,0.5,1)
		
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 600
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
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex; uniform fixed4 _MainTex_ST;
            uniform fixed _StartCutoffValue;
            uniform fixed4 _OutLineColor;
            uniform fixed _MainTexIntensity;
            uniform fixed _OpacitybyHeight;
            uniform fixed _OutLineWidth;
            uniform fixed _UseOutLine;
            uniform fixed _UseMainTexAlphaChannel;
            uniform sampler2D _SubTex; uniform fixed4 _SubTex_ST;
            uniform fixed _SubTexIntensity;
            uniform fixed _UseSubTexIntepolatebyAlpha;
            uniform fixed _UseSubTexAlphaChannel;
            uniform fixed _UseSoftOpacity;
            uniform fixed _TextureRotation;
            uniform fixed _UseMainTexRotation;
            struct VertexInput {
                fixed4 vertex : POSITION;
                fixed2 texcoord0 : TEXCOORD0;
                fixed4 vertexColor : COLOR;
            };
            struct VertexOutput {
                fixed4 pos : SV_POSITION;
                fixed2 uv0 : TEXCOORD0;
                fixed4 posWorld : TEXCOORD1;
                fixed4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {

////// Emissive:
                fixed node_3420_ang = (_TextureRotation*3.141592654);
                fixed node_3420_cos = cos(node_3420_ang);
                fixed node_3420_sin = sin(node_3420_ang);
                fixed2 node_3420_piv = fixed2(0.5,0.5);
                fixed2 node_3420 = (mul(i.uv0-node_3420_piv,fixed2x2( node_3420_cos, -node_3420_sin, node_3420_sin, node_3420_cos))+node_3420_piv);
                fixed2 node_9458 = ((i.uv0*(1.0 - _UseMainTexRotation))+(node_3420*_UseMainTexRotation));
                fixed4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_9458, _MainTex));
				
                fixed4 node_1121 = (_MainTexIntensity*fixed4(_MainTex_var.rgb,((dot(_MainTex_var.rgb,fixed3(0.3,0.59,0.11))*(1.0 - _UseMainTexAlphaChannel))+(_MainTex_var.a*_UseMainTexAlphaChannel))));
                fixed4 _SubTex_var = tex2D(_SubTex,TRANSFORM_TEX(i.uv0, _SubTex));
				fixed4 subAlpha = (fixed4(_SubTex_var.rgb,((dot(_SubTex_var.rgb,fixed3(0.3,0.59,0.11))*(1.0 - _UseSubTexAlphaChannel))+(_SubTex_var.a*_UseSubTexAlphaChannel)))*_SubTexIntensity);
                fixed4 node_3745 = ((node_1121*(1.0 - _UseSubTexIntepolatebyAlpha))+(_UseSubTexIntepolatebyAlpha*((node_1121*i.vertexColor.a)+((1.0 - i.vertexColor.a)*subAlpha))));
                fixed OutlineColorccccc = (node_3745.a*i.vertexColor.a);
                fixed node_2542 = saturate(ceil((OutlineColorccccc-_StartCutoffValue)));
                fixed3 node_5542 = ((node_3745.rgb*i.vertexColor.rgb)*node_2542);
				fixed3 outLine= ((node_5542*(1.0 - _UseOutLine))+(_UseOutLine*(node_5542+((node_2542-saturate(ceil((OutlineColorccccc-(_StartCutoffValue+_OutLineWidth*i.vertexColor.rgb)))))*(((_OutLineColor.rgb+0.01)*2.0)+(-1.0))))));
                fixed3 finalColor = ((1.0*(1.0 - _UseSoftOpacity))+(_UseSoftOpacity*saturate(((0.0 - _OpacitybyHeight)+i.posWorld.g)*4.0+0.0)))* outLine ;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }


        SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 400
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
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex; uniform fixed4 _MainTex_ST;
            uniform fixed _StartCutoffValue;
            uniform fixed _MainTexIntensity;
            uniform fixed _UseMainTexAlphaChannel;
            uniform sampler2D _SubTex; uniform fixed4 _SubTex_ST;
            uniform fixed _SubTexIntensity;
            uniform fixed _UseSubTexIntepolatebyAlpha;
            uniform fixed _UseSubTexAlphaChannel;
			uniform fixed _OpacitybyHeight;
			uniform fixed _UseSoftOpacity;

            struct VertexInput {
                fixed4 vertex : POSITION;
                fixed2 texcoord0 : TEXCOORD0;
                fixed4 vertexColor : COLOR;
            };
            struct VertexOutput {
                fixed4 pos : SV_POSITION;
                fixed2 uv0 : TEXCOORD0;
				fixed4 posWorld : TEXCOORD1;
                fixed4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {

////// Emissive:
                fixed4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
				
                fixed4 node_1121 = (_MainTexIntensity*fixed4(_MainTex_var.rgb,((dot(_MainTex_var.rgb,fixed3(0.3,0.59,0.11))*(1.0 - _UseMainTexAlphaChannel))+(_MainTex_var.a*_UseMainTexAlphaChannel))));
                fixed4 _SubTex_var = tex2D(_SubTex,TRANSFORM_TEX(i.uv0, _SubTex));
				fixed4 subAlpha = (fixed4(_SubTex_var.rgb,((dot(_SubTex_var.rgb,fixed3(0.3,0.59,0.11))*(1.0 - _UseSubTexAlphaChannel))+(_SubTex_var.a*_UseSubTexAlphaChannel)))*_SubTexIntensity);
                fixed4 lerpSubTex = ((node_1121*(1.0 - _UseSubTexIntepolatebyAlpha))+(_UseSubTexIntepolatebyAlpha*((node_1121*i.vertexColor.a)+((1.0 - i.vertexColor.a)*subAlpha))));
                fixed clipping = saturate(ceil(((lerpSubTex.a*i.vertexColor.a)-_StartCutoffValue)));
                fixed3 finalColor = (lerpSubTex.rgb*i.vertexColor.rgb)*clipping*((1.0*(1.0 - _UseSoftOpacity))+(_UseSoftOpacity*saturate(((0.0 - _OpacitybyHeight)+i.posWorld.g)*4.0+0.0)));

                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }


        SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
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
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex; uniform fixed4 _MainTex_ST;
            uniform fixed _StartCutoffValue;
            uniform fixed _MainTexIntensity;
            uniform fixed _UseMainTexAlphaChannel;
            struct VertexInput {
                fixed4 vertex : POSITION;
                fixed2 texcoord0 : TEXCOORD0;
                fixed4 vertexColor : COLOR;
            };
            struct VertexOutput {
                fixed4 pos : SV_POSITION;
                fixed2 uv0 : TEXCOORD0;
                fixed4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
////// Emissive:
                fixed4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                fixed4 alpha = ((dot(_MainTex_var.rgb,fixed3(0.3,0.59,0.11))*(1.0 - _UseMainTexAlphaChannel))+(_MainTex_var.a*_UseMainTexAlphaChannel))*i.vertexColor.a;
                fixed clipping = saturate(ceil((alpha-_StartCutoffValue)));
                fixed3 finalColor = (_MainTex_var.rgb*_MainTexIntensity*i.vertexColor.rgb*clipping);
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    Fallback "Diffuse"

}
