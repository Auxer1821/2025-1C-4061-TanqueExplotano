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

texture2D TextureMaskTanque;
sampler2D MaskSampler = sampler_state
{
    Texture = <TextureMaskTanque>;
	    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};
texture2D TextureMaskCinta;
sampler2D MaskSampler2 = sampler_state
{
    Texture = <TextureMaskCinta>;
	MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};
float4x4 World;
float4x4 View;
float4x4 Projection;

float2 UVOffset = {0, 0};
float3 DiffuseColor;
float Opaco = 1.0;

float NormalIntensity = 1.0f;
float3 CameraPosition = {900, 400, -1000}; // Posición de la cámara en espacio mundo
float Time = 0;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
	float4 Normal : NORMAL0;
	//float3 Tangent : TANGENT0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float2 TexCoord : TEXCOORD0;
	//float3 TangentLightDir : TEXCOORD1;
    //float3 TangentViewDir : TEXCOORD2;
    float4 WorldPosition : TEXCOORD1;
    float4 Normal : TEXCOORD3;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    // Clear the output
	VertexShaderOutput output = (VertexShaderOutput)0;

	output.Position = mul(mul(mul(input.Position, World), View), Projection);

	output.WorldPosition = mul(input.Position, World);

    
    output.Normal = mul(input.Normal , InverseTransposeWorld);
	output.TexCoord = input.TexCoord;

    return output;
}


float4 MainPS(VertexShaderOutput input) : COLOR
{
   // Leer color base (difuso)
    float4 color = tex2D(TextureSampler, input.TexCoord);
	color.xyz *= Opaco;

    // Leer la normal del normal map (en espacio tangente)
    float4 normalMap = float4(tex2D(NormalSampler, input.TexCoord).xyz , 0.0);
    //normalMap = normalize(normalMap);
    //normalMap= normalMap * World;
    normalMap  = mul(normalMap , InverseTransposeWorld);

    PhongShaderInput phongInput = CargarPhoneShaderInput(normalMap.xyz, input.WorldPosition);
	color = PhongShader(color, phongInput);
    
    return float4(color);
}

VertexShaderOutput CintaVS(in VertexShaderInput input)
{
    // Clear the output
	VertexShaderOutput output = (VertexShaderOutput)0;

    output.Position = mul(mul(mul(input.Position, World), View), Projection);
	output.WorldPosition = mul(input.Position, World);

	output.TexCoord = input.TexCoord + UVOffset;
    output.Normal = mul(input.Normal, InverseTransposeWorld);

    return output;
}

float4 CintaPS(VertexShaderOutput input) : COLOR
{
    float4 diffuseColor = tex2D(TextureSampler2, input.TexCoord);

    float4 normalMap = float4(tex2D(NormalSampler2, input.TexCoord).xyz , 0.0);
    normalMap  = mul(normalMap , InverseTransposeWorld);
    PhongShaderInput phongInput = CargarPhoneShaderInput(normalMap.xyz, input.WorldPosition);
    diffuseColor = PhongShader(diffuseColor, phongInput);

	diffuseColor.xyz *= Opaco;
    return float4(diffuseColor);
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
		VertexShader = compile VS_SHADERMODEL CintaVS();
		PixelShader = compile PS_SHADERMODEL CintaPS();
	}
};
