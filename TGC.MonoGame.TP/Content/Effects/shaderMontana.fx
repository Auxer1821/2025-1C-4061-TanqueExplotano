#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// Custom Effects - https://docs.monogame.net/articles/content/custom_effects.html
// High-level shader language (HLSL) - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl
// Programming guide for HLSL - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-pguide
// Reference for HLSL - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-reference
// HLSL Semantics - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-semantics
// Parámetros del efecto

#include "utilities/PhongShader.fx"
#include "utilities/ShadowShader.fx"
#include "utilities/DepthShader.fx"

texture2D Texture;
sampler TextureSampler = sampler_state
{
    Texture = (Texture);
    MagFilter = Linear;
    MinFilter = Anisotropic;
    MipFilter = Linear;
	Maxanisotropy = 16;
    AddressU = Wrap;
    AddressV = Wrap;
};
float4x4 World;
float4x4 View;
float4x4 Projection;


struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Normal : NORMAL0;
	float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
 	float2 TexCoord : TEXCOORD0;
    float4 WorldPosition : TEXCOORD1;
	float4 LightPosition : TEXCOORD2;
	float4 Normal : TEXCOORD3;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    // Clear the output
	VertexShaderOutput output = (VertexShaderOutput)0;
    // Model space to World space
    float4 worldPosition = mul(input.Position, World);
	output.WorldPosition = worldPosition;
    // World space to View space
    float4 viewPosition = mul(worldPosition, View);	
	// View space to Projection space
    output.Position = mul(viewPosition, Projection);
	output.LightPosition = mul(output.WorldPosition, LightViewProjection);

    //output.Normal = mul(input.Normal, InverseTransposeWorld);
	output.Normal = input.Normal; // Normalización de la normal
	//output.Normal = normalize(input.Normal); // Si la normal ya está en el espacio correcto

	output.TexCoord = input.TexCoord;

    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 color = tex2D(TextureSampler, input.TexCoord); // Textura base (gran escala)
	//Cargamos y corremos el phongshader 
	PhongShaderInput phongInput = CargarPhoneShaderInput(input.Normal.xyz, input.WorldPosition);
	color = PhongShader(color, phongInput);
	color = ShadowShader(color, input.LightPosition, input.WorldPosition, input.Normal, lightPosition);
	return color;
}

technique TextureDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
