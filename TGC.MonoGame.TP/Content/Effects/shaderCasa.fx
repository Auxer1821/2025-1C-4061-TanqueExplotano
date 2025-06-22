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
texture2D TexturePared;
sampler TextureSampler = sampler_state
{
    Texture = (TexturePared);
    MagFilter = Linear;
    MinFilter = Linear;
    MipFilter = Anisotropic;
    AddressU = Wrap;
    AddressV = Wrap;
};

texture2D TextureChimenea;
sampler TextureSampler2 = sampler_state
{
    Texture = (TextureChimenea);
    MagFilter = Linear;
    MinFilter = Linear;
    MipFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

texture2D TextureVentana;
sampler TextureSampler3 = sampler_state
{
    Texture = (TextureVentana);
    MagFilter = Linear;
    MinFilter = Linear;
    MipFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

texture2D TextureTecho;
sampler TextureSampler4 = sampler_state
{
    Texture = (TextureTecho);
    MagFilter = Linear;
    MinFilter = Linear;
    MipFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};
float4x4 World;
float4x4 View;
float4x4 Projection;

float3 DiffuseColor;

float Time = 0;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
	float4 Normal : NORMAL0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float2 TexCoord : TEXCOORD0;
    float4 WorldPosition : TEXCOORD1;
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

    output.Normal = mul(input.Normal , InverseTransposeWorld);
	output.TexCoord = input.TexCoord;

    return output;
}

float4 ParedPS(VertexShaderOutput input) : COLOR
{
    float4 color = tex2D(TextureSampler, input.TexCoord);

    PhongShaderInput phongInput = CargarPhoneShaderInput(input.Normal.xyz, input.WorldPosition);
	color = PhongShader(color, phongInput);
	return color;
}

float4 ChimeneaPS(VertexShaderOutput input) : COLOR
{
    float4 color = tex2D(TextureSampler2, input.TexCoord);
    PhongShaderInput phongInput = CargarPhoneShaderInput(input.Normal.xyz, input.WorldPosition);
	color = PhongShader(color, phongInput);
	return color;
}

float4 VentanaPS(VertexShaderOutput input) : COLOR
{
    float4 color = tex2D(TextureSampler3, input.TexCoord);
    PhongShaderInput phongInput = CargarPhoneShaderInput(input.Normal.xyz, input.WorldPosition);
	color = PhongShader(color, phongInput);
	return color;
}
float4 TechoPS(VertexShaderOutput input) : COLOR
{
    float4 color = tex2D(TextureSampler4, input.TexCoord);
    PhongShaderInput phongInput = CargarPhoneShaderInput(input.Normal.xyz, input.WorldPosition);
	color = PhongShader(color, phongInput);
	return color;
}
technique Pared
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL ParedPS();
	}
};


technique Chimenea
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL ChimeneaPS();
	}
};
technique Ventana
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL VentanaPS();
	}
};
technique Techo
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL TechoPS();
	}
};