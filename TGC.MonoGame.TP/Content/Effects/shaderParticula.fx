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
// Parameters.fx

float4x4 WorldViewProjection;
float3 ParticlePosition;  // Posición en espacio mundial
float4 ParticleColor;    // Color RGB + Alpha
float ParticleSize;       // Tamaño relativo
float Time;        // Para animaciones opcionales

texture2D Texture;
sampler TextureSampler = sampler_state
{
    Texture = <Texture>;
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

// Estructuras
struct VertexInput
{
    float3 Position : POSITION0;  // Posición en el cuadrado de la partícula (-1 a 1)
    float2 TexCoord : TEXCOORD0;
};

struct VertexOutput
{
    float4 Position : SV_POSITION;
    float2 TexCoord : TEXCOORD0;
    float4 Color : COLOR0;
};

// Vertex Shader
VertexOutput VS(VertexInput input)
{
    VertexOutput output;
    
    // Billboarding: Transformamos un cuadrado 2D para que siempre mire a cámara
    float3 cameraRight = float3(WorldViewProjection._11, WorldViewProjection._21, WorldViewProjection._31);
    float3 cameraUp = float3(WorldViewProjection._12, WorldViewProjection._22, WorldViewProjection._32);
    
    // Aplicamos tamaño y posición
    float3 worldPosition = ParticlePosition + 
                          (input.Position.x * cameraRight * ParticleSize) + 
                          (input.Position.y * cameraUp * ParticleSize);
    
    // Transformación final
    output.Position = mul(float4(worldPosition, 1.0), WorldViewProjection);
    output.TexCoord = input.TexCoord;
    output.Color = ParticleColor;
    
    // Efecto opcional: Parpadeo suave basado en tiempo
    //output.Color.a *= 0.8 + 0.2 * sin(Time * 5.0);
    
    return output;
}

// Pixel Shader
float4 PS(VertexOutput input) : COLOR
{
    // Muestra la textura (debería ser negra con alpha)
    float4 texColor = tex2D(TextureSampler, input.TexCoord);

    // Aplica el color dinámico manteniendo la intensidad original
    float3 finalColor = ParticleColor.rgb * (1.0 - texColor.r); // Invertimos porque la textura es negra
    
    // Mantenemos el alpha de la textura y aplicamos el alpha del color
    float finalAlpha = texColor.a * ParticleColor.a;

    //if (finalAlpha < 0.5) // Umbral para evitar partículas invisibles
    {
        //discard; // Descartamos el pixel si es casi transparente
    }
    
    return float4(finalColor, finalAlpha);
}

// Técnica
technique Particle
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL VS();
        PixelShader = compile PS_SHADERMODEL PS();
        
		AlphaBlendEnable = true;
        SrcBlend = SrcAlpha;
        DestBlend = InvSrcAlpha;
    }
}