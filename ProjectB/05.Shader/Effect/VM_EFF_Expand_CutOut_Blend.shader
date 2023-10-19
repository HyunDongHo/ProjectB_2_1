// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Shader/Effect/Expand/VM_EFF_Expand_CutOut_Blend" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _MainTexIntensity ("-MainTex Intensity", Float ) = 1
        [MaterialToggle] _UseMainAlphaTex ("-Use MainAlphaTex", Float ) = 0
		_Alpha ("MainAlphaTex", 2D) = "white" {}
		_StartCutoffValue ("[Start Cutoff Value]", Range(0.01, 1)) = 0.1
		_DisableAlphaBlend ("[Disable AlphaBlend]", Range(0, 1)) = 1
		
		[Header(Medium)]
		[MaterialToggle] _UseSubTexIntepolatebyAlpha ("[Use SubTex Intepolate by Alpha]", Float ) = 0
        _SubTex ("SubTex", 2D) = "white" {}
        _SubTexIntensity ("-SubTex Intensity", Float ) = 1
        [MaterialToggle] _UseSubAlphaTex ("-Use SubAlphaTex", Float ) = 0
		_SubAlpha ("SubAlphaTex", 2D) = "white" {}
		[Space]
		[MaterialToggle] _UseSoftOpacity ("[Use Soft Opacity]", Float ) = 0
        _OpacitybyHeight ("-Opacity by Height", Float ) = -1.7
		
		[Header(High)]
        [MaterialToggle] _UseMainTexRotation ("[Use MainTex Rotation(High Only)]", Float ) = 0
        _TextureRotation ("-Texture Rotation", Range(-1, 1)) = 0
        
        [MaterialToggle] _UseOutLine ("[Use OutLine]", Float ) = 0
        _OutLineWidth ("-OutLine Width", Range(0, 1)) = 0.05
        _OutLineColor ("-OutLine Color", Color) = (0.5,0.5,0.5,0)
        		
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
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
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex; uniform fixed4 _MainTex_ST;
            uniform sampler2D _Alpha; uniform fixed4 _Alpha_ST;
            uniform fixed _StartCutoffValue;
            uniform fixed _MainTexIntensity;
            uniform fixed4 _OutLineColor;
            uniform fixed _DisableAlphaBlend;
            uniform fixed _UseMainAlphaTex;
            uniform fixed _UseOutLine;
            uniform fixed _OutLineWidth;
            uniform sampler2D _SubTex; uniform fixed4 _SubTex_ST;
            uniform sampler2D _SubAlpha; uniform fixed4 _SubAlpha_ST;
            uniform fixed _SubTexIntensity;
            uniform fixed _UseSubAlphaTex;
            uniform fixed _UseSubTexIntepolatebyAlpha;
            uniform fixed _OpacitybyHeight;
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
                fixed node_3123_ang = (_TextureRotation*3.141592654);
                fixed node_3123_cos = cos(node_3123_ang);
                fixed node_3123_sin = sin(node_3123_ang);
                fixed2 node_3123_piv = fixed2(0.5,0.5);
                fixed2 node_3123 = (mul(i.uv0-node_3123_piv,fixed2x2( node_3123_cos, -node_3123_sin, node_3123_sin, node_3123_cos))+node_3123_piv);
                fixed2 node_4690 = ((i.uv0*(1.0 - _UseMainTexRotation))+(node_3123*_UseMainTexRotation));
                fixed4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_4690, _MainTex));
                fixed4 _Alpha_var = tex2D(_Alpha,TRANSFORM_TEX(node_4690, _Alpha));
				
                fixed4 node_2870 = (_MainTexIntensity*fixed4(_MainTex_var.rgb,((dot(_MainTex_var.rgb,fixed3(0.3,0.59,0.11))*(1.0 - _UseMainAlphaTex))+(dot(_Alpha_var.rgb,fixed3(0.3,0.59,0.11))*_UseMainAlphaTex*_MainTex_var.a))));
                fixed4 _SubTex_var = tex2D(_SubTex,TRANSFORM_TEX(i.uv0, _SubTex));
                fixed4 _SubAlpha_var = tex2D(_SubAlpha,TRANSFORM_TEX(i.uv0, _SubAlpha));
				
                fixed4 node_1618 = (_SubTexIntensity*fixed4(_SubTex_var.rgb,((dot(_SubTex_var.rgb,fixed3(0.3,0.59,0.11))*(1.0 - _UseSubAlphaTex))+(_SubTex_var.a*_UseSubAlphaTex*dot(_SubAlpha_var.rgb,fixed3(0.3,0.59,0.11))))));
                fixed4 node_5884 = ((node_2870*(1.0 - _UseSubTexIntepolatebyAlpha))+(_UseSubTexIntepolatebyAlpha*((node_2870*i.vertexColor.a)+((1.0 - i.vertexColor.a)*node_1618))));
                fixed node_4542 = (i.vertexColor.a*node_5884.a);
                fixed node_710 = ceil((node_4542-_StartCutoffValue));
                clip(node_710 - 0.5);

////// Emissive:
                fixed3 node_2233 = (i.vertexColor.rgb*node_5884.rgb);
                fixed3 finalColor = ((node_2233*(1.0 - _UseOutLine))+(_UseOutLine*(node_2233+((saturate(node_710)-saturate(ceil((node_4542-(_StartCutoffValue+_OutLineWidth)))))*((_OutLineColor.rgb*2.0)+(-1.0))))));
                fixed softOpacity = (1.0*(1.0 - _UseSoftOpacity))+(_UseSoftOpacity*saturate(((0.0 - _OpacitybyHeight)+i.posWorld.g)*4.0+0.0));
                return fixed4(finalColor,(softOpacity*saturate(i.vertexColor.a+_DisableAlphaBlend)));
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
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex; uniform fixed4 _MainTex_ST;
			uniform sampler2D _Alpha; uniform fixed4 _Alpha_ST;
            uniform fixed _StartCutoffValue;
            uniform fixed _MainTexIntensity;
            uniform fixed _DisableAlphaBlend;
            uniform fixed _UseMainAlphaTex;
            uniform sampler2D _SubTex; uniform fixed4 _SubTex_ST;
			uniform sampler2D _SubAlpha; uniform fixed4 _SubAlpha_ST;
            uniform fixed _SubTexIntensity;
            uniform fixed _UseSubAlphaTex;
            uniform fixed _UseSubTexIntepolatebyAlpha;
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

                fixed4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                fixed4 _Alpha_var = tex2D(_Alpha,TRANSFORM_TEX(i.uv0, _Alpha));
				
                fixed4 node_2870 = (_MainTexIntensity*fixed4(_MainTex_var.rgb,((dot(_MainTex_var.rgb,fixed3(0.3,0.59,0.11))*(1.0 - _UseMainAlphaTex))+(dot(_Alpha_var.rgb,fixed3(0.3,0.59,0.11))*_UseMainAlphaTex*_MainTex_var.a))));
                fixed4 _SubTex_var = tex2D(_SubTex,TRANSFORM_TEX(i.uv0, _SubTex));
                fixed4 _SubAlpha_var = tex2D(_SubAlpha,TRANSFORM_TEX(i.uv0, _SubAlpha));				
				
                fixed4 node_1618 = (_SubTexIntensity*fixed4(_SubTex_var.rgb,((dot(_SubTex_var.rgb,fixed3(0.3,0.59,0.11))*(1.0 - _UseSubAlphaTex))+(_SubTex_var.a*_UseSubAlphaTex*dot(_SubAlpha_var.rgb,fixed3(0.3,0.59,0.11))))));
                fixed4 node_5884 = ((node_2870*(1.0 - _UseSubTexIntepolatebyAlpha))+(_UseSubTexIntepolatebyAlpha*((node_2870*i.vertexColor.a)+((1.0 - i.vertexColor.a)*node_1618))));
                fixed node_4542 = (i.vertexColor.a*node_5884.a);
                fixed node_710 = ceil((node_4542-_StartCutoffValue));
                clip(node_710 - 0.5);
////// Emissive:
                fixed3 finalColor = (i.vertexColor.rgb*node_5884.rgb);
                fixed softOpacity = (1.0*(1.0 - _UseSoftOpacity))+(_UseSoftOpacity*saturate(((0.0 - _OpacitybyHeight)+i.posWorld.g)*4.0+0.0));
                return fixed4(finalColor,softOpacity*saturate(i.vertexColor.a+_DisableAlphaBlend));
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
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex; uniform fixed4 _MainTex_ST;
			uniform sampler2D _Alpha; uniform fixed4 _Alpha_ST;
            uniform fixed _StartCutoffValue;
            uniform fixed _MainTexIntensity;
            uniform fixed _DisableAlphaBlend;
            uniform fixed _UseMainAlphaTex;

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

                fixed4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
				fixed4 _Alpha_var = tex2D(_Alpha,TRANSFORM_TEX(i.uv0, _Alpha));
                fixed node_4542 = (i.vertexColor.a*((dot(_MainTex_var.rgb,fixed3(0.3,0.59,0.11))*(1.0 - _UseMainAlphaTex))+(dot(_Alpha_var.rgb,fixed3(0.3,0.59,0.11))*_UseMainAlphaTex*_MainTex_var.a)));
                fixed node_710 = saturate(ceil((node_4542-_StartCutoffValue)));
                clip(node_710 - 0.5);
////// Emissive:
                fixed3 finalColor = (i.vertexColor.rgb*_MainTex_var.rgb*_MainTexIntensity);
                return fixed4(finalColor,saturate(i.vertexColor.a+_DisableAlphaBlend));
            }
            ENDCG
        }        
    }

 	Fallback "Diffuse"
}
