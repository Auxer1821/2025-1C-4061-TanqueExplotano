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
	    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = None; // Importante para normales
};
texture2D TextureCinta;
sampler TextureSampler2 = sampler_state
{
    Texture = (TextureCinta);
	    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = None; // Importante para normales
};
texture2D TextureNormalTanque;
sampler2D NormalSampler = sampler_state
{
    Texture = <TextureNormalTanque>;
	    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};
texture2D TextureNormalCinta;
sampler2D NormalSampler2 = sampler_state
{
    Texture = <TextureNormalCinta>;
	    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

float4x4 World;
float4x4 View;
float4x4 Projection;

float2 UVOffset = {0, 0};
float Opaco = 1.0;

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
	float4 LightPosition : TEXCOORD2;
    float4 Normal : TEXCOORD3; 
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    // Clear the output
	VertexShaderOutput output = (VertexShaderOutput)0;

    output.Position = mul(mul(mul(input.Position, World), View), Projection);
	output.TexCoord = input.TexCoord;

    output.WorldPosition = mul(input.Position, World);
    output.LightPosition = mul(output.WorldPosition, LightViewProjection);
    output.Normal = mul(input.Normal, InverseTransposeWorld);

    return output;
}


float4 MainPS(VertexShaderOutput input) : COLOR
{
   // Leer color base (difuso)
    float4 color = tex2D(TextureSampler, input.TexCoord);
	color.xyz *= Opaco;
   
    float3 tangentNormal = tex2D(NormalSampler, input.TexCoord).xyz * 2.0 - 1.0;
   	PhongShaderInput phongInput = CargarPhoneShaderInput(input.Normal.xyz, input.WorldPosition);
	color = PhongShaderNormalMap(input.TexCoord, color, tangentNormal, phongInput);
    color = ShadowShader(color, input.LightPosition, input.WorldPosition, input.Normal, lightPosition);
    return color;
}

VertexShaderOutput RuedasVS(in VertexShaderInput input)
{
    // Clear the output
	VertexShaderOutput output = (VertexShaderOutput)0;
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);	
	
    output.Position = mul(viewPosition, Projection);
	output.TexCoord = input.TexCoord + UVOffset;
    output.WorldPosition = mul(input.Position, World);
    output.LightPosition = mul(output.WorldPosition, LightViewProjection);
    output.Normal = mul(input.Normal, InverseTransposeWorld);
    return output;
}

float4 RuedasPS(VertexShaderOutput input) : COLOR
{
   // Leer color base (difuso)
    float4 color = tex2D(TextureSampler2, input.TexCoord);
	color.xyz *= Opaco;

    float3 tangentNormal = tex2D(NormalSampler2, input.TexCoord).xyz * 2.0 - 1.0;
   	PhongShaderInput phongInput = CargarPhoneShaderInput(input.Normal.xyz, input.WorldPosition);
	color = PhongShaderNormalMap(input.TexCoord, color, tangentNormal, phongInput);
    color = ShadowShader(color, input.LightPosition, input.WorldPosition, input.Normal, lightPosition);
    return color;
}

technique Main
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};

technique RuedasDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL RuedasVS();
		PixelShader = compile PS_SHADERMODEL RuedasPS();
	}
};