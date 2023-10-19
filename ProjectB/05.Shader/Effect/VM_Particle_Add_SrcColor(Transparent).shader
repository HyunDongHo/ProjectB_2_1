Shader "Shaders/Effect/VM_Particle_Add_SrcColor(Transparent)"{
Properties	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("MainTex", 2D) = "white" {}
	}

	Category {

	Tags { "Queue" = "Transparent+2" "IgnoreProjector" = "True" "Render Type" = "Transparent" }

	Blend SrcColor One // 소스 칼라값에 의해 완전하게 곱해집니다.
	Cull Off
	Lighting Off
	ZWrite Off
	fog{mode Off}
	ColorMask RGB


	BindChannels{
		Bind "Color" , color
		Bind "Vertex" , vertex
		Bind "Texcoord" , texcoord
	}

	SubShader{
			pass{
				SetTexture [_MainTex]{
					constantColor [_TintColor]
					combine constant * primary
				}
				SetTexture [_MainTex]{
					combine texture * previous DOUBLE
				}
			}//pass
		}//subshader
	}//category

	Fallback "Diffuse"
}
