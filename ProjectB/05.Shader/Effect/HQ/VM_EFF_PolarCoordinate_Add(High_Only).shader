// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Shader/Effect/Expand/VM_EFF_PolarCoordinate_Add(High_Only)" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _MainTexIntensity ("-MainTex Intensity", Float ) = 1
        _SubTex ("SubTex", 2D) = "white" {}
        _LengthScale ("LengthScale", Range(0, 2)) = 0.5
        _RadialScale ("RadialScale", Range(0, 4)) = 2
        _Speed ("Speed", Float ) = 0.5
        [MaterialToggle] _UseSpeedbyAlpha ("[Use Speed by Alpha]", Float ) = 0
		[Space(20)]
        [MaterialToggle] _UseSoftOpacity ("[Use Soft Opacity]", Float ) = 0
        _OpacitybyHeight ("-Opacity by Height", Float ) = -1.7
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
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform fixed4 _TimeEditor;
            uniform fixed _LengthScale;
            uniform fixed _RadialScale;
            uniform sampler2D _MainTex; uniform fixed4 _MainTex_ST;
            uniform sampler2D _SubTex; uniform fixed4 _SubTex_ST;
            uniform fixed _MainTexIntensity;
            uniform fixed _Speed;
            uniform fixed _OpacitybyHeight;
            uniform fixed _UseSpeedbyAlpha;
            uniform fixed _UseSoftOpacity;
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
                //o.uv0 = v.texcoord0;
                o.uv0 = TRANSFORM_TEX(v.texcoord0, _MainTex);
                o.uv1 = TRANSFORM_TEX(v.texcoord0, _SubTex);

                o.vertexColor = v.vertexColor;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );

                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {

////// Emissive:
                fixed4 node_7466 = _Time + _TimeEditor;
                fixed4 SbyA = i.vertexColor.a + _TimeEditor;
                fixed2 node_4684 = (i.uv0 - 0.5);

                fixed node_3422 = (_LengthScale*node_4684.x);

                fixed node_7524 = (_LengthScale*node_4684.y);
                fixed UseSbyA = lerp(node_7466.y*_Speed, SbyA.y * _Speed ,_UseSpeedbyAlpha);
                fixed2 node_2561 = (fixed2(pow(((node_3422*node_3422)+(node_7524*node_7524)),0.5),(atan2(node_4684.x,node_4684.y)/(_RadialScale*3.141592654)))+UseSbyA*fixed2(-1,0));
				
				fixed4 _MainTex_var = tex2D(_MainTex,node_2561);
                fixed4 _SubTex_var = tex2D(_SubTex,i.uv1);
                fixed3 finalColor = (_MainTex_var.rgb*_SubTex_var.r*_MainTexIntensity*i.vertexColor.rgb*lerp(1.0,saturate(((0.0 - _OpacitybyHeight)+i.posWorld.g)*4.0+0.0),_UseSoftOpacity));
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }

	SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        LOD 200
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

            struct VertexInput {
                fixed4 vertex : POSITION;
                fixed2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                fixed4 pos : SV_POSITION;
                fixed2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                return fixed4(0,0,0,0);
            }
            ENDCG
        }
    }

    Fallback "Diffuse"

}
