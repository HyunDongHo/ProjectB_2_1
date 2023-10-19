// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Shader/Effect/Expand/VM_EFF_Expand_Blend(Use_Control)" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _MainTexIntensity ("-MainTex Intensity", Float ) = 1
        [MaterialToggle] _RemoveBlack ("-Remove Black", Float ) = 1
		[Space]
        [MaterialToggle] _UseAlphaTex ("-Use AlphaTex", Float ) = 0
		_Alpha ("-AlphaTex", 2D) = "white" {}
        _AlphaChannelIntensity ("-AlphaTex Intensity", Float ) = 1
		[Space]
		_SubTex ("SubTex", 2D) = "white" {}
        _SubTexIntensity ("-SubTex Intensity", Float ) = 1
		[Space(20)]
		[Header(Medium)]
		[MaterialToggle] _UseMainTexScroll ("[Use MainTex Scroll]",Float ) = 1
        _ScrollSpeedbyAlpha ("-Scroll Speed by Alpha", Float ) = 0
        _ScrollVector0U1V ("-Scroll Vector [0=U/1=V]", Float ) = 1
        [MaterialToggle] _DontAffectColor ("-Don't Affect Color", Float ) = 0
        [Space]
        [MaterialToggle] _UseSoftOpacity ("[Use Soft Opacity]", Float ) = 0
        _OpacitybyHeight ("-Opacity by Height", Float ) = -1.7
		[Space(20)]
		[Header(High)]
		[MaterialToggle] _UseMainTexRotation ("[Use MainTex Rotation]", Float ) = 0
        _MainTexRotationValue ("-MainTex Rotation Value", Range(-1, 1)) = 0
		
        
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
            uniform fixed _MainTexIntensity;
            uniform fixed _RemoveBlack;
            uniform fixed _OpacitybyHeight;
            uniform fixed _UseSoftOpacity;
            uniform sampler2D _SubTex; uniform fixed4 _SubTex_ST;
            uniform fixed _SubTexIntensity;
            uniform fixed _UseAlphaTex;
            uniform fixed _UseMainTexScroll;
            uniform fixed _AlphaChannelIntensity;
			uniform fixed _MainTexRotationValue;
            uniform fixed _UseMainTexRotation;
            uniform fixed _ScrollVector0U1V;
            uniform fixed _ScrollSpeedbyAlpha;
            uniform fixed _DontAffectColor;
            struct VertexInput {
                fixed4 vertex : POSITION;
                fixed2 texcoord0 : TEXCOORD0;
                fixed4 vertexColor : COLOR;
            };
            struct VertexOutput {
                fixed4 pos : SV_POSITION;
                fixed2 uv0 : TEXCOORD0;
                fixed2 uv1 : TEXCOORD1;
                fixed4 posWorld : TEXCOORD2;
                fixed4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = TRANSFORM_TEX(v.texcoord0,_MainTex);
                o.uv1 = TRANSFORM_TEX(v.texcoord0,_SubTex);
                o.vertexColor = v.vertexColor;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {

////// Emissive:
                fixed scrollAlpha = (i.vertexColor.a*lerp(0.0,_ScrollSpeedbyAlpha,_UseMainTexScroll));
				
				float node_394_ang = (3.141592654*_MainTexRotationValue);
                float node_394_spd = 1.0;
                float node_394_cos = cos(node_394_spd*node_394_ang);
                float node_394_sin = sin(node_394_spd*node_394_ang);
                float2 node_394_piv = float2(0.5,0.5);
                float2 node_394 = (mul(i.uv0-node_394_piv,float2x2( node_394_cos, -node_394_sin, node_394_sin, node_394_cos))+node_394_piv);
                float2 mtRot = ((i.uv0*(1.0 - _UseMainTexRotation))+(node_394*_UseMainTexRotation));
				
                fixed2 node_3203 = lerp((mtRot+scrollAlpha*fixed2(1,0)),(mtRot+scrollAlpha*fixed2(0,1)),_ScrollVector0U1V);
                fixed4 _MainTex_var = tex2D(_MainTex,node_3203);
                fixed4 _Alpha_var = tex2D(_Alpha,node_3203);
                fixed3 node_1760 = (_MainTexIntensity*_MainTex_var.rgb);
				fixed3 RmvBlack = (1 * (1-_RemoveBlack)) + (node_1760 * _RemoveBlack);
                fixed3 finalColor = (node_1760*i.vertexColor.rgb);
                fixed node_9268 = dot(node_1760,fixed3(0.3,0.59,0.11));
                fixed node_9936 = dot(_Alpha_var.rgb,fixed3(0.3,0.59,0.11))*_AlphaChannelIntensity*_MainTex_var.a;
                fixed softOpacity = (1.0*(1.0 - _UseSoftOpacity))+(_UseSoftOpacity*saturate(((0.0 - _OpacitybyHeight)+i.posWorld.g)*4.0+0.0));
				fixed4 _SubTex_var = tex2D(_SubTex,i.uv1);
                return fixed4(finalColor,(((node_9268*(1.0 - _UseAlphaTex))+(node_9936*_UseAlphaTex))* softOpacity *dot((RmvBlack*_SubTex_var.rgb*_SubTexIntensity),fixed3(0.3,0.59,0.11))*saturate(i.vertexColor.a+_DontAffectColor)));
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
            uniform fixed _MainTexIntensity;
			uniform sampler2D _Alpha; uniform fixed4 _Alpha_ST;
			uniform fixed _OpacitybyHeight;
            uniform fixed _UseSoftOpacity;
            uniform sampler2D _SubTex; uniform fixed4 _SubTex_ST;
            uniform fixed _SubTexIntensity;
			uniform fixed _RemoveBlack;
            uniform fixed _UseAlphaTex;
            uniform fixed _UseMainTexScroll;
            uniform fixed _AlphaChannelIntensity;
            uniform fixed _ScrollVector0U1V;
            uniform fixed _ScrollSpeedbyAlpha;
            uniform fixed _DontAffectColor;
            struct VertexInput {
                fixed4 vertex : POSITION;
                fixed2 texcoord0 : TEXCOORD0;
                fixed4 vertexColor : COLOR;
            };
            struct VertexOutput {
                fixed4 pos : SV_POSITION;
                fixed2 uv0 : TEXCOORD0;
                fixed2 uv1 : TEXCOORD1;
                fixed4 posWorld : TEXCOORD2;
                fixed4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = TRANSFORM_TEX(v.texcoord0,_MainTex);
                o.uv1 = TRANSFORM_TEX(v.texcoord0,_SubTex);
                o.vertexColor = v.vertexColor;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {

////// Emissive:
                fixed scrollAlpha = (i.vertexColor.a*lerp(0.0,_ScrollSpeedbyAlpha,_UseMainTexScroll));
                fixed2 node_3203 = lerp((i.uv0+scrollAlpha*fixed2(1,0)),(i.uv0+scrollAlpha*fixed2(0,1)),_ScrollVector0U1V);
                fixed4 _MainTex_var = tex2D(_MainTex,node_3203);
                fixed4 _Alpha_var = tex2D(_Alpha,node_3203);
                fixed3 node_1760 = (_MainTexIntensity*_MainTex_var.rgb);
				fixed3 RmvBlack = (1 * (1-_RemoveBlack)) + (node_1760 * _RemoveBlack);
                fixed3 finalColor = (node_1760*i.vertexColor.rgb);
                fixed node_9268 = dot(node_1760,fixed3(0.3,0.59,0.11));
                fixed node_9936 = dot(_Alpha_var.rgb,fixed3(0.3,0.59,0.11))*_AlphaChannelIntensity*_MainTex_var.a;
                fixed softOpacity = (1.0*(1.0 - _UseSoftOpacity))+(_UseSoftOpacity*saturate(((0.0 - _OpacitybyHeight)+i.posWorld.g)*4.0+0.0));
				fixed4 _SubTex_var = tex2D(_SubTex,i.uv1);
                return fixed4(finalColor,(((node_9268*(1.0 - _UseAlphaTex))+(node_9936*_UseAlphaTex))*dot((RmvBlack*_SubTex_var.rgb*_SubTexIntensity),fixed3(0.3,0.59,0.11))*saturate(i.vertexColor.a+_DontAffectColor)*softOpacity));
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
            uniform fixed _MainTexIntensity;
			uniform sampler2D _Alpha; uniform fixed4 _Alpha_ST;

            uniform sampler2D _SubTex; uniform fixed4 _SubTex_ST;
            uniform fixed _SubTexIntensity;
			uniform fixed _RemoveBlack;
            uniform fixed _UseAlphaTex;
            uniform fixed _AlphaChannelIntensity;
			uniform fixed _DontAffectColor;

            struct VertexInput {
                fixed4 vertex : POSITION;
                fixed2 texcoord0 : TEXCOORD0;
                fixed4 vertexColor : COLOR;
            };
            struct VertexOutput {
                fixed4 pos : SV_POSITION;
                fixed2 uv0 : TEXCOORD0;
                fixed2 uv1 : TEXCOORD1;
                fixed4 posWorld : TEXCOORD2;
                fixed4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = TRANSFORM_TEX(v.texcoord0,_MainTex);
                o.uv1 = TRANSFORM_TEX(v.texcoord0,_SubTex);
                o.vertexColor = v.vertexColor;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {

////// Emissive:
                fixed4 _MainTex_var = tex2D(_MainTex,i.uv0);
                fixed4 _Alpha_var = tex2D(_Alpha,i.uv0);
                fixed3 node_1760 = (_MainTexIntensity*_MainTex_var.rgb);
				fixed3 RmvBlack = (1 * (1-_RemoveBlack)) + (node_1760 * _RemoveBlack);
                fixed3 node_7330 = (node_1760*i.vertexColor.rgb);
                fixed3 emissive = node_7330;
                fixed3 finalColor = emissive;
                fixed node_9268 = dot(node_1760,fixed3(0.3,0.59,0.11));
                fixed node_9936 = dot(_Alpha_var.rgb,fixed3(0.3,0.59,0.11))*_AlphaChannelIntensity*_MainTex_var.a;
				fixed4 _SubTex_var = tex2D(_SubTex,i.uv1);
                return fixed4(finalColor,(((node_9268*(1.0 - _UseAlphaTex))+(node_9936*_UseAlphaTex))*dot((RmvBlack*_SubTex_var.rgb*_SubTexIntensity),fixed3(0.3,0.59,0.11))*saturate(i.vertexColor.a+_DontAffectColor)));
            }
            ENDCG
        }
    }

    Fallback "Diffuse"

}
