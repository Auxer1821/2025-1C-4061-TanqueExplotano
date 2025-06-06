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

texture2D Texture;
sampler TextureSampler = sampler_state
{
    Texture = (Texture);
    MagFilter = Linear;
    MinFilter = Linear;
    MipFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};
float4x4 WorldViewProjection;

float2 Coordenadas;

float3 DiffuseColor;
float AlphaThreshold = 0.5f; // Valor de umbral de transparencia
float PorcentaClaridad = 1.0f; // Porcentaje de recarga del HUD


float Time = 0;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
	float4 Color : COLOR0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float2 TexCoord : TEXCOORD0;
	float4 Color : COLOR0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    // Clear the output
	VertexShaderOutput output = (VertexShaderOutput)0;

    float4 posMundo = input.Position;
	posMundo.xy += Coordenadas; // Aplico desplazamiento del carácter
    output.Position = mul(posMundo, WorldViewProjection);
	output.TexCoord = input.TexCoord;
	output.Color = input.Color;

    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 color = tex2D(TextureSampler, input.TexCoord);

    if (color.a < AlphaThreshold)
        discard;
    return color;
}

float4 RecargaPS(VertexShaderOutput input) : COLOR
{
	float4 color = tex2D(TextureSampler, input.TexCoord);
	//aplicar tiempo para animación
	color.xyz *= PorcentaClaridad;

    if (color.a < AlphaThreshold)
        discard;
    return color;
}

technique Default
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
		AlphaBlendEnable = true;
        SrcBlend = SrcAlpha;
        DestBlend = InvSrcAlpha;
	}
}

technique Recarga
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL RecargaPS();
		AlphaBlendEnable = true;
        SrcBlend = SrcAlpha;
        DestBlend = InvSrcAlpha;
	}
}

technique Fondo
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
}
