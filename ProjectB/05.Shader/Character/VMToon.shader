//20210827_장민석 수정사항
//_DmgGlow항목추가(Damaged Glow)
// 
//_DmgColor  -> 발광컬러
//_DmgFactor -> 발광수치(0~1고정) (1에 가까워질수록 투명해짐)
//_DmgEffMul -> 발광세기

//그리고 외곽선에서 뷰행렬,투영행렬 영향받는것 제거

Shader "Shaders/Character/VMToon" 
{
Properties
	{
		//TOONY COLORS
		_Color ("Color", Color) = (1.0,1.0,1.0,1.0) 
		_Plus ("Plus", Color) = (0.0,0.0,0.0,1.0) 
		_HColor ("Highlight Color", Color) = (1,1,1,1.0) 
		_SColor ("Shadow Color", Color) = (0.1,0.06,0.06,1.0)

		//DIFFUSE 
		_MainTex ("Main Texture (RGB)", 2D) = "white" {} 
		//_Mask1 ("Mask 1 (Specular)", 2D) = "black" {}

		//[MaterialToggle(_ISCONDITION_ON)] _IsCondition("[_Condition_]", float) = 0
		//_ConditionColor("Condition Color", Color) = (1.0,1.0,1.0,1.0) 
		//_ConditionMask ("Condition Texture", 2D) = "black" {}


		////TOONY COLORS RAMP 
		//_RampThreshold ("#RAMPF# Ramp Threshold", Range(0,1)) = 0.45 
		//_RampSmooth ("#RAMPF# Ramp Smoothing", Range(0.001,1)) = 0.15
		//
		////SPECULAR 
		//_SpecColor (“#SPEC# Specular Color", Color) = (0.5, 0.5, 0.5, 1) 
		//_Shininess ("#SPEC# Shininess", Range(0.0,2)) = 0.8

		//Glow
		//[MaterialToggle(_ISUVSCROLL_ON)] _IsUvScroll("[_UVScroll_]", float) = 0 
		////_GlowTex("Glow UV Texture(RGB) ", 2D) = "white" {} 
		//_GlowMask ("Glow Mask Texture(Rim)", 2D) = "black" {} 
		//_GlowTexIntensity("GlowTexIntensity", Vector) = (1,1,1,0)

		//Scroll
		//_ScrollXSpeed("X Scroll Speed", float) = 0 
		//_ScrollYspeed("Y Scroll Speed", float) = 0

		//Rim Color 
		[MaterialToggle(_ISRIM_ON)] _IsRim("[_Rim_]", float) = 0 
		_RimColor ("Rim Color", Color) = (0,0,0,0) 
		_RimPower ("Rim Power", Range(0,1)) = 1

		//DMGGLOW Color 
		//[MaterialToggle(_ISRIM_ON)] _IsRim("[_DAMAGEDGLOW_]", float) = 0
		_DmgColor("DmgGlow Color", Color) = (1.0,1.0,1.0,1.0)
		_DmgFactor("DmgGlow Factor", Range(0,1)) = 0.0
		_DmgEffMul("DmgGlow Power", Range(0,10)) = 1.0

		//[MaterialToggle(_ISDYETEX_ON)] _IsDyeTex("[_DyeTex_]", float) = 0 
		//_DyeTex ("DyeTex", 2D) = "white" {} 
		//_DyeColor ("DyeColor", Color) = (1.0,1.0,1.0,1.0)


		//_IsDeath ("[Death Color]", Range(0,1)) = 0.7 
		//_Death ("_Death", Range(-0.001,1)) = -0.001


		_HairHighlight("HairHighlight", Color) = (1,1,1,1)
		_SkinColor("SkinColor", Color) = (1,0.73,0.67,1)
		_LightingValue("LightingValue", Range(0,2)) = 1
		_LightInverse("LightInverse", Range(0,1)) = 0
		//_MaskTex("MatMaskTex (RGB)",2D) = "white" {}
		_MatCap("MatCap (RGB)", 2D) = "white" {}

		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Outline("Outline width", Range(0.0, 0.05)) = .002
	}


	//=============================================== 
	SubShader
	{
		
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		//LOD 100
		Lighting On

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite On
			CGPROGRAM
			
#pragma vertex vert
#pragma fragment frag
			// make fog work 
#pragma multi_compile_fog 
#pragma shader_feature _ISRIM_ON 
#pragma shader_feature _ISCONDITION_ON 
#pragma shader_feature _ISUVSCROLL_ON 
#pragma shader_feature _ISDYETEX_ON
			#pragma shader_feature _HITTED
#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				fixed4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
				half2 cap : TEXCOORD1;
				half3 worldPos : TEXCOORD2;
				half3 normal : TEXCOORD3; 
				//half3 viewDir;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			uniform fixed4 _Color;
			uniform fixed3 _Plus;
			uniform fixed _Lightinverse;

			uniform sampler2D _Mask1;
			uniform sampler2D _MatCap;

			uniform sampler2D _GlowTex;
			uniform sampler2D _GlowMask;
			uniform fixed3 _GlowTexIntensity;

			uniform sampler2D _DyeTex;
			uniform fixed3 _DyeColor;

			uniform sampler2D _ConditionMask;
			uniform fixed3 _ConditionColor;

			//uniform fixed4 _Lerp TargetColor;
			uniform fixed4 _HairHighlight;
			uniform fixed4 _SkinColor;
			//uniform fixed _LerpValue;
			uniform fixed _LightingValue;

			uniform fixed3 _RimColor;
			uniform fixed _RimPower;

			uniform fixed4 _DmgColor;
			uniform fixed _DmgFactor;

			uniform fixed _ScrollXSpeed;
			uniform fixed _ScrollYSpeed;

			uniform fixed _Death; 
			uniform fixed _IsDeath;

			uniform fixed4 _SColor;
			uniform fixed4 _HColor;

			uniform fixed _DmgEffMul;

			inline fixed3 GetGrayColor(fixed3 col)
			{
				fixed average = ((col.r + col.g + col.b) * 0.33);
				col.r = average;
				col.g = average;
				col.b = average;

				return col;
			}

			//uniform fixed _IsBns;

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

				float3 worldNorm = normalize(unity_WorldToObject[0].xyz * v.normal.x + unity_WorldToObject[1].xyz * v.normal.y + unity_WorldToObject[2].xyz * v.normal.z);
				worldNorm = mul((float3x3) UNITY_MATRIX_V, worldNorm);
				o.cap.xy = worldNorm.xy * 0.5 + 0.5;
				// MatCap X좌표 Lightinvers 파라미터에 의해 반전.
				//o.cap.x = lerp(o.cap.x, 1 - o.cap.x, _LightInverse);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				half4 tex = tex2D(_MainTex, i.uv);
				half4 mtex = half4(0,0,0,0);
				half4 mc = tex2D(_MatCap, i.cap);

				clip((tex.a * ceil((tex.b - _Death))) - 0.5);

				half4 albedo;

				// skin,Metal Masking은 같은 mc.g채널에 표현되어 있으므로 둘로 나눠주어 각기 변수로 저장. 
				half skin = saturate(mc.g - 0.5) * 2 * mtex.b;
				half noneSkin = saturate(mc.g - 0.5) * 2 * (1 - mtex.b);
				half metal = saturate(0.9 - mc.g - 0.5)*2.5*mtex.r;

				_SColor = lerp(_HColor, _SColor, _SColor.a);

				// 피격 시 색상변경. 
				//tex = Terp(tex, LerpTargetColor + mc.b*0.5, LerpValue); 
				// 피부부분. 사용자 지정 색상으로 피부의 어두운 부분의 색감을 결정. 
				// lerp( 일반 어두운 부분 쉐이딩, 피부분 쉐이딩, 마스킹)
				//					albedo = lerp(tex-saturate(mc.g-0.5)*0.4*_Lighting Value, _SkinColor*tex, skin);
				albedo = lerp(tex - saturate(noneSkin*mc.g - 0.5) * (1 - _SColor)*0.4* _LightingValue, _SkinColor*tex, skin);
				// 헤어부분. "tex mtex.g(마스킹 헤어)* mc.r(헤어 맵켑)* _HairHighlight(헤어 하이라이트 마스킹 프로퍼티) 
				albedo = albedo + (tex*mtex.g*mc.r*_HairHighlight);

				// 금속부분. 색감을 살리기 위해 tex를 2제곱하여 *5로 밝게 만들어 곱해줌.
				albedo = albedo + (metal*tex*tex * 5);

				// 색상의 가장 어두운 부분을 0.93 으로 제한.( 그래야 외각선이 더 잘보임.)
				albedo = (saturate(albedo - 0.07) + 0.07);

				// 밝은 영역 쉐이딩. 
				//					albedo - albedo + mc.b*(saturate(tex-0.35) +0.35)*0.5* Lighting Value ; 
				albedo = albedo + (mc.b - noneSkin*0.25)*(saturate(tex - 0.35) + 0.35) *0.5*_LightingValue* _HColor;

				albedo = albedo * _Color *(unity_LightColor[0] +1)/2;// *_IsBns;
				//albedo *= unity_LightColor[0];

				albedo.rgb = albedo.rgb + _Plus;

#if _ISUVSCROLL_ON

				fixed2 scrolled UV = i.uv;
				fixed xScrollValue = -_ScrollxSpeed * _Time:// * __IsUvScroll;
				fixed yScrollValue = -_ScrollySpeed * _Time:// * __IsUVScroll;
				scrolledUV += fixed2(xScrollValue, yScrollValue);
				//fixed3 glowmaskl = tex2DC GlowMask, i.uv);
				half3 glowTex = tex2D(_GlowTex, i.uv + scrolledUV) * tex2D(_GlowMask, i.uv).r;

				albedo.rgb = albedo.rgb * _Color.rgb + _Color.rgb * (glowTex.rgb * _GlowTexIntensity.rgb);// * __ISUVScroll;

#endif

				fixed3 rimColor = 0;

#if _ISRIM_ON

				half3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));

				fixed rim = (1.0 - saturate(dot(worldViewDir, i.normal)));

				rimColor = smoothstep(1 - _RimPower, 1, rim);

				rimColor *= _RimColor.rgb;

				rimColor = rimColor * pow(rim, _RimPower);

#endif
//				rimColor// * pow(1, _RimPower);

				//장민석추가 - 데미지시 글로우효과
				fixed4 dmgGlowColor = 0;

				half3 worldViewDir2 = normalize(UnityWorldSpaceViewDir(i.worldPos));

				fixed dmgglow = (1.0 - saturate(dot(worldViewDir2, i.normal)));

				dmgGlowColor = smoothstep(1 - _DmgFactor, 1, dmgglow);

				dmgGlowColor *= _DmgColor.rgba;

				dmgGlowColor = dmgGlowColor * pow(dmgglow, _DmgFactor);

#ifdef _HITTED
				dmgGlowColor *= _DmgEffMul;
				if (dmgGlowColor.a >= 1)
					dmgGlowColor.a = 1;
#endif

#if _ISDYETEX_ON
			fixed3 DyeTex_var = tex2D(_DyeTex, i.uv);
			albedo.rgb = lerp(albedo.rgb, GetGrayColor(tex.rgb), DyeTex_var.r);
			half3 DyeColor = lerp(albedo, _DyeColor.rgb, DyeTex_var.r);

			albedo.rgb = DyeColor;

#endif

#if _ISCONDITION_ON
//				fixed3 conditionTex = tex2D Condition Mask, i.uv);
//				albedo.rgb = ((albedo.r + albedo.g + albedo.b) * 0.3 + conditionTex.r) + _ConditionColor;
#endif
				albedo.rgb += rimColor;
				albedo.rgb += dmgGlowColor;
				albedo.rgb = lerp(albedo, (albedo*(_IsDeath)), (_Death * 3));

				//albedo.rgba = lerp(albedo, (albedo* (_IsDeath)), (_Death * 3));
				float AlphaFactor = 1.0f - _DmgFactor;
				albedo.a = _Color.a * AlphaFactor + dmgGlowColor.a * _DmgFactor;

				return albedo;
			}

		ENDCG
		}


		//Tags{ "Queue" = "Geometry" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			Pass{


			Cull Front
			ZWrite On
			ColorMask RGB
			//Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
				#pragma shader_feature _HITTED
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest

			#include "UnityCG.cginc"

			struct appdata {
			  float4 vertex : POSITION;
			  half3 normal : NORMAL;
			};

			struct v2f {
			  float4 pos : POSITION;
			  fixed4 color : Color;
			};

			uniform float _Outline;
			uniform float4 _OutlineColor;

			uniform fixed _DmgFactor;

			v2f vert(appdata v)
			{
			  v2f o;
			  o.pos = UnityObjectToClipPos(v.vertex);

			  float3 norm = UnityObjectToClipPos(v.normal);
			  //float2 offset = TransformViewToProjection(norm.xy);

			  o.pos.xyz += norm * _Outline;
			  o.color = _OutlineColor;
			  return o;
			}

			fixed4 frag(v2f i) :COLOR
			{
#if _HITTED
				discard;
#endif
				i.color.a = 1.0f - _DmgFactor;
				return i.color;
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}