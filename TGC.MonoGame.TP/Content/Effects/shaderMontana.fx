﻿#if OPENGL
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
float3 LightDirection = normalize(float3(1, -1, 1));
float4 LightColor = float4(1, 1, 1, 1);
float4 AmbientColor = float4(0.2, 0.2, 0.2, 1);

float Time = 0;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float3 Normal : NORMAL0;
	float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	//float3 Normal : TEXCOORD0;
 	float2 TexCoord : TEXCOORD1;
	//float3 LightDirection : TEXCOORD2;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    // Clear the output
	VertexShaderOutput output = (VertexShaderOutput)0;
    // Model space to World space
    float4 worldPosition = mul(input.Position, World);
    // World space to View space
    float4 viewPosition = mul(worldPosition, View);	
	// View space to Projection space
    output.Position = mul(viewPosition, Projection);

	output.TexCoord = input.TexCoord;
	//output.LightDirection = LightDirection;

    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 baseColor = tex2D(TextureSampler, input.TexCoord * 0.1); // Textura base (gran escala)
    float4 detailColor = tex2D(TextureSampler, input.TexCoord * 2.0); // Textura detalle
    
    // Mezcla según distancia (implementa tu lógica)
    return lerp(baseColor, detailColor, 0.5);
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
