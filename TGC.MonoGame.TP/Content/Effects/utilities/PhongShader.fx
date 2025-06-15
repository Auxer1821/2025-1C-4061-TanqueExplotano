#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

//float4x4 WorldViewProjection;
//float4x4 World;

float4x4 InverseTransposeWorld;
float3 ambientColor; // Light's Ambient Color
float3 diffuseColor; // Light's Diffuse Color
float3 specularColor; // Light's Specular Color
float KAmbient; 
float KDiffuse; 
float KSpecular;
float shininess; 
float3 lightPosition;
float3 eyePosition; // Camera position

struct PhongShaderInput
{
    float4 WorldPosition;
    float3 Normal;
    float3 lightPosition;
    float3 eyePosition;
    float3 ambientColor;
    float3 diffuseColor;
    float3 specularColor;
    float KAmbient; 
    float KDiffuse; 
    float KSpecular;
    float shininess;
};

float4 PhongShader(float4 color, PhongShaderInput phongInput)
{
   // Base vectors
    float3 lightDirection = normalize(-phongInput.lightPosition + phongInput.WorldPosition.xyz);
    float3 viewDirection = normalize(phongInput.eyePosition - phongInput.WorldPosition.xyz);
    float3 halfVector = normalize(lightDirection + viewDirection);
    
	// Calculate the diffuse light
    float NdotL = saturate(dot(phongInput.Normal.xyz, lightDirection));
    float3 diffuseLight = phongInput.KDiffuse * phongInput.diffuseColor * NdotL;

	// Calculate the specular light
    float NdotH = dot(phongInput.Normal.xyz, halfVector);
    float3 specularLight = sign(NdotL) * phongInput.KSpecular * phongInput.specularColor * pow(saturate(NdotH), phongInput.shininess);
    
    // Final calculation
    float4 finalColor = float4(saturate(phongInput.ambientColor * phongInput.KAmbient + diffuseLight) * color.rgb + specularLight, color.a);
    return finalColor;
}

PhongShaderInput CargarPhoneShaderInput(float3 normal, float4 worldPosition){
    PhongShaderInput phongInput = (PhongShaderInput)0;
	phongInput.ambientColor = ambientColor;
	phongInput.diffuseColor = diffuseColor;
	phongInput.specularColor = specularColor;
	phongInput.eyePosition = eyePosition;
	phongInput.KAmbient = KAmbient;
	phongInput.KDiffuse = KDiffuse;
	phongInput.KSpecular = KSpecular;
	phongInput.lightPosition = lightPosition;
	phongInput.Normal = normal;
	phongInput.shininess = shininess;
	phongInput.WorldPosition = worldPosition;

    return phongInput;
}
