// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Shader/Effect/Expand/VM_EFF_Expand_Add(Use_Control)" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _MainTexIntensity ("-MainTex Intensity", Float ) = 1
        [MaterialToggle] _UseAlphaChannel ("-Use AlphaChannel", Float ) = 0
		
		_SubTex ("SubTex", 2D) = "white" {}
        _SubTexIntensity ("-SubTex Intensity", Float ) = 1
		
		[Space]

		[MaterialToggle] _UseMainTexScroll ("[Use MainTex Scroll]",Float ) = 1
		_ScrollSpeedbyTime ("-Scroll Speed by Time",Float ) = 0
        _ScrollSpeedbyAlpha ("-Scroll Speed by Alpha", Float ) = 0
        _ScrollVector0U1V ("-Scroll Vector [0=U/1=V]", Float ) = 1

		[Space(20)]
		[Header(Medium)]
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
            //Blend SrcColor One
            Blend One One
			Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex; uniform fixed4 _MainTex_ST;
            uniform fixed _MainTexIntensity;
            uniform fixed _OpacitybyHeight;
            uniform fixed _UseSoftOpacity;
            uniform fixed _ScrollVector0U1V;
            uniform fixed _ScrollSpeedbyTime;
            uniform fixed _ScrollSpeedbyAlpha;
			uniform fixed _MainTexRotationValue;
            uniform fixed _UseMainTexRotation;
            uniform sampler2D _SubTex; uniform fixed4 _SubTex_ST;
            uniform fixed _SubTexIntensity;
            uniform fixed _UseAlphaChannel;
            uniform fixed _UseMainTexScroll;
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
                o.uv0 = v.texcoord0;
                o.uv1 = TRANSFORM_TEX(v.texcoord0, _SubTex);

                o.vertexColor = v.vertexColor;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
				
				fixed scrollAlpha = lerp(0.0,(_ScrollSpeedbyTime*_Time.g)+(o.vertexColor.a*_ScrollSpeedbyAlpha),_UseMainTexScroll);
				
				float node_394_ang = (3.141592654*_MainTexRotationValue);
                float node_394_spd = 1.0;
                float node_394_cos = cos(node_394_spd*node_394_ang);
                float node_394_sin = sin(node_394_spd*node_394_ang);
                float2 node_394_piv = float2(0.5,0.5);
                float2 node_394 = (mul(o.uv0-node_394_piv,float2x2( node_394_cos, -node_394_sin, node_394_sin, node_394_cos))+node_394_piv);
                float2 mtRot = ((o.uv0*(1.0 - _UseMainTexRotation))+(node_394*_UseMainTexRotation));
                fixed2 uvVector = lerp((mtRot+scrollAlpha*fixed2(1,0)),(mtRot+scrollAlpha*fixed2(0,1)),_ScrollVector0U1V);
				
				o.uv0 = TRANSFORM_TEX(uvVector, _MainTex);
				
				
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {

////// Emissive:

                fixed4 _MainTex_var = tex2D(_MainTex,i.uv0);
				fixed3 mtAlpha = (_MainTex_var.rgb*_MainTex_var.a);

                fixed4 _SubTex_var = tex2D(_SubTex,i.uv1);
				fixed softOpacity = (1.0*(1.0 - _UseSoftOpacity))+(_UseSoftOpacity*saturate(((0.0 - _OpacitybyHeight)+i.posWorld.g)*4.0+0.0));

                fixed3 emissive = (((_MainTexIntensity*((_MainTex_var.rgb*(1.0 - _UseAlphaChannel))+(mtAlpha*_UseAlphaChannel)))*_SubTex_var.rgb*_SubTexIntensity)*i.vertexColor.rgb*softOpacity);
                fixed3 finalColor = emissive;
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
            uniform fixed _MainTexIntensity;
            uniform fixed _OpacitybyHeight;
            uniform fixed _UseSoftOpacity;
            uniform fixed _ScrollVector0U1V;
            uniform fixed _ScrollSpeedbyAlpha;
            uniform sampler2D _SubTex; uniform fixed4 _SubTex_ST;
            uniform fixed _SubTexIntensity;
            uniform fixed _UseAlphaChannel;
            uniform fixed _UseMainTexScroll;
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
                o.uv0 = v.texcoord0;
                o.uv1 = TRANSFORM_TEX(v.texcoord0, _SubTex);
                o.vertexColor = v.vertexColor;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
				
				fixed scrollAlpha = o.vertexColor.a*lerp(0.0,_ScrollSpeedbyAlpha,_UseMainTexScroll);
                fixed2 uvVector = lerp((o.uv0+scrollAlpha*fixed2(1,0)),(o.uv0+scrollAlpha*fixed2(0,1)),_ScrollVector0U1V);
				o.uv0 = TRANSFORM_TEX(uvVector, _MainTex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {

////// Emissive:
                
				
                fixed4 _MainTex_var = tex2D(_MainTex,i.uv0);
                fixed3 mtAlpha = (_MainTex_var.rgb*_MainTex_var.a);
                fixed4 _SubTex_var = tex2D(_SubTex,i.uv1);
                fixed softOpacity = (1.0*(1.0 - _UseSoftOpacity))+(_UseSoftOpacity*saturate(((0.0 - _OpacitybyHeight)+i.posWorld.g)*4.0+0.0));
                fixed3 finalColor = (((_MainTexIntensity*((_MainTex_var.rgb*(1.0 - _UseAlphaChannel))+(mtAlpha*_UseAlphaChannel)))*_SubTex_var.rgb*_SubTexIntensity)*i.vertexColor.rgb*softOpacity);
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
            uniform fixed _MainTexIntensity;

            uniform fixed _ScrollVector0U1V;
            uniform fixed _ScrollSpeedbyAlpha;
            uniform sampler2D _SubTex; uniform fixed4 _SubTex_ST;
            uniform fixed _SubTexIntensity;
            uniform fixed _UseAlphaChannel;
            uniform fixed _UseMainTexScroll;
			
			
            struct VertexInput {
                fixed4 vertex : POSITION;
                fixed2 texcoord0 : TEXCOORD0;

                fixed4 vertexColor : COLOR;
            };
            struct VertexOutput {
                fixed4 pos : SV_POSITION;
                fixed2 uv0 : TEXCOORD0;
                fixed2 uv1 : TEXCOORD1;
                fixed4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = TRANSFORM_TEX(v.texcoord0, _SubTex);
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
				
				fixed scrollAlpha = o.vertexColor.a*lerp(0.0,_ScrollSpeedbyAlpha,_UseMainTexScroll);
                fixed2 uvVector = lerp((o.uv0+scrollAlpha*fixed2(1,0)),(o.uv0+scrollAlpha*fixed2(0,1)),_ScrollVector0U1V);
				o.uv0 = TRANSFORM_TEX(uvVector, _MainTex);
				
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {

////// Emissive:

                fixed4 _MainTex_var = tex2D(_MainTex,i.uv0);
                fixed3 mtAlpha = (_MainTex_var.rgb*_MainTex_var.a);
                fixed4 _SubTex_var = tex2D(_SubTex,i.uv1);

                fixed3 emissive = (((_MainTexIntensity*((_MainTex_var.rgb*(1.0 - _UseAlphaChannel))+(mtAlpha*_UseAlphaChannel)))*_SubTex_var.rgb*_SubTexIntensity)*i.vertexColor.rgb);
                fixed3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }

   Fallback "Diffuse"
}
