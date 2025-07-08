#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif


//static const float modulatedEpsilon = 0.000041200182749889791011810302734375;
//static const float modulatedEpsilon = 0.000041200182749889791011810302734375;
//static const float maxEpsilon = 0.000023200045689009130001068115234375;
//static const float maxEpsilon = 0.000023200045689009130001068115234375;
static const float modulatedEpsilon = 0.0002;
static const float maxEpsilon = 0.0005;


//----------Valores a recibir en codigo----------//

float2 shadowMapSize;
float4x4 LightViewProjection;

texture shadowMap;
sampler2D shadowMapSampler =
sampler_state
{
	Texture = <shadowMap>;
	MinFilter = Point;
	MagFilter = Point;
	MipFilter = Point;
	AddressU = Clamp;
	AddressV = Clamp;
};


//---------------Funciones Finales----------------//

float4 ShadowShader(float4 color, float4 LightSpacePosition , float4 WorldSpacePosition, float4 Normal, float3 lightPosition){
	float3 lightSpacePosition = LightSpacePosition.xyz / LightSpacePosition.w;
    float2 shadowMapTextureCoordinates = 0.5 * lightSpacePosition.xy + float2(0.5, 0.5);
    shadowMapTextureCoordinates.y = 1.0f - shadowMapTextureCoordinates.y;
	
    float3 normal = normalize(Normal.rgb);
    float3 lightDirection = normalize(lightPosition - WorldSpacePosition.xyz);
    //float inclinationBias = max(modulatedEpsilon  * (1.0 - dot(normal, lightDirection)), maxEpsilon );
    float inclinationBias = max(modulatedEpsilon  * (1.0 - dot(normal, lightDirection)), maxEpsilon );
	
	// Sample and smooth the shadowmap
	// Also perform the comparison inside the loop and average the result
    float notInShadow = 0.0;
    float2 texelSize = 1.0 / shadowMapSize;
    for (int x = -1; x <= 1; x++){
        for (int y = -1; y <= 1; y++)
        {
            float pcfDepth = tex2D(shadowMapSampler, shadowMapTextureCoordinates + float2(x, y) * texelSize).r + inclinationBias;
            notInShadow += step(lightSpacePosition.z, pcfDepth) / 9.0;
        }
	}
    float porcentaje_de_sombra = 0.6;
	
	return float4(color.xyz * ( (1.0-porcentaje_de_sombra) + porcentaje_de_sombra * notInShadow * 1.5), color.a);
    //return color;
}