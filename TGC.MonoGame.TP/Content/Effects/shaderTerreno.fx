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
    MagFilter = Linear;
    MinFilter = Anisotropic;
    MipFilter = Anisotropic;
	Maxanisotropy = 16;
    AddressU = Wrap;
    AddressV = Wrap;
};
float4x4 World;
float4x4 View;
float4x4 Projection;


float Time = 0;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Normal : NORMAL0;
	float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	//float3 Normal : TEXCOORD0;
 	float2 TexCoord : TEXCOORD1;
	//float3 LightDirection : TEXCOORD2;
	float4 WorldPosition : TEXCOORD2;
	float3 Normal : TEXCOORD3;
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

	output.Normal =  mul(input.Normal, InverseTransposeWorld);
	output.TexCoord = input.TexCoord;
	//output.LightDirection = LightDirection;

    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    /*float4 textureColor = tex2D(TextureSampler, input.TexCoord);
    float lightIntensity = saturate(dot(normalize(input.Normal), -normalize(input.LightDirection)));
    float4 lighting = saturate(LightColor * lightIntensity + AmbientColor);
	*/
	//float4 baseColor = tex2D(TextureSampler, input.TexCoord * 0.1); // Textura base (gran escala)
    //float4 detailColor = tex2D(TextureSampler, input.TexCoord * 2.0); // Textura detalle
    
    // Mezcla según distancia (implementa tu lógica)
    //float4 color = lerp(baseColor, detailColor, 0.5);

    float4 color = tex2D(TextureSampler, input.TexCoord);
	PhongShaderInput phongInput = CargarPhoneShaderInput(input.Normal, input.WorldPosition);
	color = PhongShader(color, phongInput);
	return color;


    //return float4(baseColor.rgb, 1);
    //return textureColor * lighting;
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
