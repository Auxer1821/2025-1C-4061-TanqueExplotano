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
texture2D TextureTronco;
sampler TextureSampler = sampler_state
{
    Texture = (TextureTronco);
    MagFilter = Linear;
    MinFilter = Linear;
    MipFilter = Anisotropic;
    AddressU = Wrap;
    AddressV = Wrap;
};

texture2D TextureHojas;
sampler TextureSampler2 = sampler_state
{
    Texture = (TextureHojas);
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
float WindStrength = 0.1f;
float WindSpeed = 2.0f;
float LeafFlexibility = 0.3f;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
	float4 Normal : NORMAL0; // Normal para el efecto de viento
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float2 TexCoord : TEXCOORD0;
    float4 WorldPosition : TEXCOORD1;
	float3 Normal : TEXCOORD3;
};

VertexShaderOutput TroncoVS(in VertexShaderInput input)
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

    output.Normal = mul(input.Normal, InverseTransposeWorld);
	output.TexCoord = input.TexCoord;

    return output;
}

VertexShaderOutput HojasVS(in VertexShaderInput input)
{
    // Clear the output
	VertexShaderOutput output = (VertexShaderOutput)0;

	// 1. Calculamos la influencia del viento en base a:
    //    - Coordenada Y (las hojas superiores se mueven más)
    //    - Tiempo para animación
    //    - Normal de la hoja (para dirección)
    float windEffect = sin(Time * WindSpeed + input.Position.x * 10.0) * WindStrength;
    
    // 2. Aplicamos movimiento a los vértices
    float3 displacement = float3(
        windEffect * LeafFlexibility * input.Normal.x,
        windEffect * 0.2 * input.Normal.y, // Menor movimiento en Y
        0
    );
    
    // 3. Deformamos la posición
    input.Position.xyz += displacement;


    // Model space to World space
    float4 worldPosition = mul(input.Position, World);
	output.WorldPosition = worldPosition;
    // World space to View space
    float4 viewPosition = mul(worldPosition, View);	
	// View space to Projection space
    output.Position = mul(viewPosition, Projection);

    output.Normal = mul(input.Normal, InverseTransposeWorld);
	output.TexCoord = input.TexCoord;

    return output;
}

float4 TroncoPS(VertexShaderOutput input) : COLOR
{
    float4 color = tex2D(TextureSampler, input.TexCoord);
    
    PhongShaderInput phongInput = CargarPhoneShaderInput(input.Normal, input.WorldPosition);
	color = PhongShader(color, phongInput);
	return color;
}

float4 HojasPS(VertexShaderOutput input) : COLOR
{
    float4 color = tex2D(TextureSampler2, input.TexCoord);

	PhongShaderInput phongInput = CargarPhoneShaderInput(input.Normal, input.WorldPosition);
	color = PhongShader(color, phongInput);
	return color;
}

technique Tronco
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL TroncoVS();
		PixelShader = compile PS_SHADERMODEL TroncoPS();
	}
};


technique Hojas
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL HojasVS();
		PixelShader = compile PS_SHADERMODEL HojasPS();
	}
};