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
	float3 Normal : NORMAL0;
	float3 Tangent : TANGENT0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float2 TexCoord : TEXCOORD0;
	float3 TangentLightDir : TEXCOORD1;
    float3 TangentViewDir : TEXCOORD2;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    // Clear the output
	VertexShaderOutput output = (VertexShaderOutput)0;
    // Model space to World space
    //float4 worldPosition = mul(input.Position, World);
    // World space to View space
    //float4 viewPosition = mul(worldPosition, View);	
	// View space to Projection space
	
	 output.Position = mul(mul(mul(input.Position, World), View), Projection);
    //output.Position = mul(viewPosition, Projection);
	output.TexCoord = input.TexCoord;

	// Cálculo de TBN (Tangent-Binormal-Normal) Matrix
    float3 normal = normalize(mul(input.Normal, (float3x3)World));
    float3 tangent = normalize(mul(input.Tangent, (float3x3)World));
    float3 binormal = cross(normal, tangent);

	float3x3 TBN = float3x3(tangent, binormal, normal);
    
    // Direcciones en espacio tangente
    float3 lightDir = normalize(float3(1, -1, 1)); // Luz direccional
	//float3 ViewTBN = mul((float3x3)View, TBN);
    //float3 viewDir = normalize(ViewTBN);
    
	float4 worldPosition = mul(input.Position, World);

// Asumimos que la cámara está en el origen del espacio de vista (0, 0, 0),
// así que su posición en espacio mundo es la inversa de la matriz View (normalmente pasada como una variable separada).
// Si tienes la posición de la cámara en una variable, úsala directamente.

	float3 cameraWorldPos = CameraPosition; // Este valor deberías pasarlo desde el CPU

	float3 viewDirWorld = normalize(cameraWorldPos - worldPosition.xyz);

// Transformar dirección de vista al espacio tangente
output.TangentViewDir = mul(TBN, viewDirWorld);
    output.TangentLightDir = mul(TBN, lightDir);
    return output;
}


float4 MainPS(VertexShaderOutput input) : COLOR
{
   // Leer color base (difuso)
    float4 diffuseColor = tex2D(TextureSampler, input.TexCoord);
	diffuseColor.xyz *= Opaco;

    // Leer la normal del normal map (en espacio tangente)
    float3 normalMap = tex2D(NormalSampler, input.TexCoord).xyz;
    float3 N = normalize(normalMap); // Normal perturbada

    float3 L = normalize(input.TangentLightDir); // Dirección de luz en espacio tangente
    float3 V = normalize(input.TangentViewDir);  // Dirección de vista en espacio tangente
    float3 H = normalize(L + V);

	// Componente difusa
    float NdotL = saturate(dot(N, L));
    float3 diffuse = diffuseColor.rgb * NdotL;
    
    // Componente especular
    float NdotH = saturate(dot(N, H));
    float specular = pow(NdotH, 32) * 0.9; // Reducir intensidad
    
    // Componente ambiental
    float3 ambient = diffuseColor.rgb * 0.65; //iluminación ambiente
    
    // 4. Combinación final con gamma correction
    float3 finalColor = saturate(ambient + diffuse + specular);
    finalColor = pow(finalColor, 1/1.05); // Gamma correction
    
    return float4(finalColor, diffuseColor.a);
}

VertexShaderOutput RuedasVS(in VertexShaderInput input)
{
    // Clear the output
	VertexShaderOutput output = (VertexShaderOutput)0;
    // Model space to World space
    float4 worldPosition = mul(input.Position, World);
    // World space to View space
    float4 viewPosition = mul(worldPosition, View);	
	// View space to Projection space
    output.Position = mul(viewPosition, Projection);
	output.TexCoord = input.TexCoord + UVOffset;
	// Cálculo de TBN (Tangent-Binormal-Normal) Matrix
    float3 normal = normalize(mul(input.Normal, (float3x3)World));
    float3 tangent = normalize(mul(input.Tangent, (float3x3)World));
    float3 binormal = cross(normal, tangent);

	float3x3 TBN = float3x3(tangent, binormal, normal);
    
    // Direcciones en espacio tangente
    float3 lightDir = normalize(float3(1, -1, 1)); // Luz direccional

	float3 cameraWorldPos = CameraPosition; // Este valor deberías pasarlo desde el CPU

	float3 viewDirWorld = normalize(cameraWorldPos - worldPosition.xyz);

	// Transformar dirección de vista al espacio tangente
	output.TangentViewDir = mul(TBN, viewDirWorld);
    output.TangentLightDir = mul(TBN, lightDir);
    return output;
}

float4 RuedasPS(VertexShaderOutput input) : COLOR
{
   // Leer color base (difuso)
    float4 diffuseColor = tex2D(TextureSampler2, input.TexCoord);
	diffuseColor.xyz *= Opaco;

    // Leer la normal del normal map (en espacio tangente)
    float3 normalMap = tex2D(NormalSampler2, input.TexCoord).xyz;
    float3 N = normalize(normalMap); // Normal perturbada

    float3 L = normalize(input.TangentLightDir); // Dirección de luz en espacio tangente
    float3 V = normalize(input.TangentViewDir);  // Dirección de vista en espacio tangente
    float3 H = normalize(L + V);

	// Componente difusa
    float NdotL = saturate(dot(N, L));
    float3 diffuse = diffuseColor.rgb * NdotL;
    
    // Componente especular
    float NdotH = saturate(dot(N, H));
    float specular = pow(NdotH, 32) * 0.9; // Reducir intensidad
    
    // Componente ambiental
    float3 ambient = diffuseColor.rgb * 0.55; //iluminación ambiente
    
    // 4. Combinación final con gamma correction
    float3 finalColor = saturate(ambient + diffuse + specular);
    finalColor = pow(finalColor, 1/1.09); // Gamma correction
    
    return float4(finalColor, diffuseColor.a);
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
