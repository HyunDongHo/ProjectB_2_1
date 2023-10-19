Shader "Shader/Effect/VM_EFF_DepthFirst_Particle_Add_SrcAlpha(Transparent)"{
Properties	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("MainTex", 2D) = "white" {}
		_Alpha ("Alpha", 2D) = "white" {}
	}

	Category {

	Tags { "Queue" = "Transparent+2" "IgnoreProjector" = "True" "Render Type" = "Transparent" }

	Blend SrcAlpha One //SrcColor 소스 색생값에 의해 온전히 곱해집니다.
	Cull Off
	Lighting Off
	ZWrite Off
	fog{mode Off}
	ZTest NotEqual
	ColorMask RGB


	BindChannels{
		Bind "Color" , color
		Bind "Vertex" , vertex
		Bind "Texcoord" , texcoord
	}

	SubShader{
			pass{
				SetTexture [_Alpha] {combine texture * previous}
				SetTexture [_MainTex]{
					constantColor [_TintColor]
					combine constant * primary
				}
				SetTexture [_Alpha] {combine texture * previous}
				SetTexture [_MainTex]{
					combine texture * previous DOUBLE
				}
			}//pass
		}//subshader
	}//category

	Fallback "Diffuse"
}
