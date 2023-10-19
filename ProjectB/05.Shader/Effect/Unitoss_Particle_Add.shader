Shader "Shaders/Effects/Unitoss_Particle_Add"{
Properties	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("MainTex", 2D) = "white" {}
		_Alpha ("Alpha", 2D) = "white" {}
	}

	Category {

	Tags { "Queue" = "Transparent+2" "IgnoreProjector" = "True" "Render Type" = "Transparent" }

	Blend SrcAlpha One
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
				SetTexture [_Alpha] {combine texture * previous}
				SetTexture [_MainTex]{
					constantColor [_TintColor]
					combine constant * primary
				}
				SetTexture [_Alpha] {combine texture * previous}
				SetTexture [_MainTex]{
					combine texture * previous DOUBLE
				}
			}
		}
	}

	Fallback "Diffuse"
}
