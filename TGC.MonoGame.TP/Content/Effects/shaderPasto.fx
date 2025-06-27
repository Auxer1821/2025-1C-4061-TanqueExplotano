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

#include "utilities/ShadowShader.fx"
float3 lightPosition;

texture2D Texture;
sampler TextureSampler = sampler_state
{
    Texture = (Texture);
    MagFilter = Linear;
    MinFilter = Linear;
    MipFilter = Anisotropic;
    MaxAnisotropy = 4;
    AddressU = Wrap;
    AddressV = Wrap;
};
float4x4 World;
float4x4 View;
float4x4 Projection;

float3 DiffuseColor;
float AlphaThreshold = 0.5f; // Valor de umbral de transparencia
float WindStrength = 0.4f;
float WindSpeed = 1.3f;
float GrassStiffness = 0.3f;
float Time = 0;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
    //float4 Normal : NORMAL0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float2 TexCoord : TEXCOORD0;
    float4 WorldPosition : TEXCOORD1;
    float4 LightPosition : TEXCOORD2;
	float4 Color : COLOR0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    // Clear the output
	VertexShaderOutput output = (VertexShaderOutput)0;
    // Movimiento principal (usa posición en X y Z para variación)
    float wave = sin(Time * WindSpeed + input.Position.x + input.Position.z) * WindStrength;
    
    // Aplica movimiento solo a la parte superior
    float4 modifiedPosition = input.Position;
    modifiedPosition.x += wave * (1.0f - abs(input.Position.y) * GrassStiffness);
    
    // Transformaciones normales
    float4 WorldModificado = mul(modifiedPosition ,World);
    output.WorldPosition = WorldModificado;
    float4 ViewModificado = mul(WorldModificado, View);
    output.Position = mul(ViewModificado, Projection);
    output.LightPosition = mul(output.WorldPosition, LightViewProjection);
    output.TexCoord = input.TexCoord;
    
    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    // Distorsión basada en el tiempo y posición en Y (para que el pasto se mueva desde la base)
    float windEffect = sin(1 - input.TexCoord.y + Time * WindSpeed) * WindStrength * (1- input.TexCoord.y);
    windEffect *= (1.0f - input.TexCoord.y) * GrassStiffness; // Aumenta el efecto en la parte superior del pasto
    
    // Aplicamos la distorsión solo en X para simular el viento
    float2 distortedTexCoord = input.TexCoord + float2(windEffect, 0);
    
    // Muestreamos la textura con las coordenadas modificadas
    float4 color = tex2D(TextureSampler, distortedTexCoord);
    color = ShadowShader(color, input.LightPosition, input.WorldPosition, float4(0.0,1.0,0.0,0.0), lightPosition);
    if (color.a < AlphaThreshold)         
        discard;


    return color;
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
		AlphaBlendEnable = true;
        SrcBlend = SrcAlpha;
        DestBlend = InvSrcAlpha;
	}
};
