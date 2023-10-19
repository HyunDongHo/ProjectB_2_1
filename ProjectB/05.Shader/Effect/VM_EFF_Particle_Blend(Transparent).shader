// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Shader/Effect/VM_EFF_Particle_Blend(Transparent)"{
Properties	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("MainTex", 2D) = "white" {}
		_Alpha ("Alpha", 2D) = "white" {}
	}

	SubShader {

        Tags {"IgnoreProjector"="True" "Queue"="Transparent" "RenderType"="Transparent"}
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

	    uniform sampler2D _MainTex; 
	    uniform fixed4 _MainTex_ST;

		uniform sampler2D _Alpha; 
		uniform fixed4 _Alpha_ST;

		uniform fixed4 _TintColor;

	    struct v2f 
	    {
			fixed4 pos : SV_POSITION;
			fixed2 uv : TEXCOORD0;
			fixed2 uv1 : TEXCOORD1;
			fixed4 color : COLOR;
	    };


	    v2f vert (appdata_full v) 
	    {
			v2f o;

			o.color = v.color;
			o.pos = UnityObjectToClipPos(v.vertex);

			o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
			o.uv1 = TRANSFORM_TEX(v.texcoord, _Alpha);

			return o;
	    }

	    fixed4 frag(v2f i) : COLOR {
	     		fixed4 _MainTex_var =  tex2D(_MainTex,i.uv);
	            fixed4 _Alpha_var = tex2D(_Alpha,i.uv1);

              	fixed3 emissive = _MainTex_var.rgb * i.color.rgb;
              	fixed node_9936 = _Alpha_var.r;	 

	            fixed3 finalColor = emissive * (_TintColor * 2);

	            return fixed4(finalColor, (i.color.a * _MainTex_var.a * node_9936));
	        }
	        ENDCG
	    }
	}//SubShader

	Fallback "Diffuse"
}
