#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D Texture;
sampler SamplerTextura = sampler_state
{
    Texture = (Texture);
    MagFilter = Linear;
    MinFilter = Linear;
    MipFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};
// Constantes que vienen del código C#
float2 Coordenadas;    // Posición donde dibujar el carácter (como si fuera mundo)
float Minimo;          // Coordenada mínima en X (pixel) del carácter en la textura
float Maximo;          // Coordenada máxima en X (pixel) del carácter en la textura

float TamanioTextura;  // Ancho total de la textura en píxeles (se setea desde el código)
float PixelSize;       // 1.0 / TamanioTextura (para convertir píxeles a UV)


float AlphaThreshold = 0.5;
float4x4 WorldViewProjection;

// Textura del atlas de caracteres
//Texture2D Textura : register(t0);
//SamplerState SamplerTextura : register(s0);

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float2 TexCoord : TEXCOORD0;
};

VertexShaderOutput VS(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 posMundo = input.Position;
    posMundo.xy += Coordenadas; // Aplico desplazamiento del carácter
    output.Position = mul(posMundo, WorldViewProjection);

    // Remapeo las coordenadas UV solo en X
    float u = lerp(Minimo * PixelSize, Maximo * PixelSize, input.TexCoord.x);
    output.TexCoord = float2(u, input.TexCoord.y);
	// Calcular coordenadas UV correctas para el carácter
    //output.TexCoord = CharUV + (input.TexCoord * CharSize / AtlasSize);

    return output;
}

float4 PS(VertexShaderOutput input) : SV_TARGET
{
	float4 color = tex2D(SamplerTextura, input.TexCoord);
    if (color.a < AlphaThreshold)
        discard;
    return color;
}

technique Technique1
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL VS();
		PixelShader = compile PS_SHADERMODEL PS();
		AlphaBlendEnable = true;
        SrcBlend = SrcAlpha;
        DestBlend = InvSrcAlpha;
	}
};