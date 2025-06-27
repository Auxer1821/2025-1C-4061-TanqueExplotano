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
float3 getNormalFromMap(float2 textureCoordinates, float3 worldPosition, float3 worldNormal, float3 tangentNormal)
{
    //float3 tangentNormal = tex2D(normalSampler, textureCoordinates).xyz * 2.0 - 1.0;

    float3 Q1 = ddx(worldPosition);
    float3 Q2 = ddy(worldPosition);
    float2 st1 = ddx(textureCoordinates);
    float2 st2 = ddy(textureCoordinates);

    worldNormal = normalize(worldNormal.xyz);
    float3 T = normalize(Q1 * st2.y - Q2 * st1.y);
    float3 B = -normalize(cross(worldNormal, T));
    float3x3 TBN = float3x3(T, B, worldNormal);

    return normalize(mul(tangentNormal, TBN));
}

float4 PhongShader(float4 color, PhongShaderInput phongInput)
{
   // Base vectors
    float3 lightDirection = normalize(phongInput.lightPosition - phongInput.WorldPosition.xyz);
    float3 viewDirection = normalize(phongInput.eyePosition - phongInput.WorldPosition.xyz);
    float3 halfVector = normalize(lightDirection + viewDirection);

    // Calculate the ambient light
    float3 ambientLight = phongInput.ambientColor * phongInput.KAmbient;
    
	// Calculate the diffuse light
    float NdotL = saturate(dot(phongInput.Normal.xyz, lightDirection));
    float3 diffuseLight = phongInput.KDiffuse * phongInput.diffuseColor * NdotL;

	// Calculate the specular light
    float NdotH = dot(phongInput.Normal.xyz, halfVector);
    float3 specularLight = sign(NdotL) * phongInput.KSpecular * phongInput.specularColor * pow(saturate(NdotH), phongInput.shininess);
    
    // Final calculation
    float4 finalColor = float4(saturate(ambientLight + diffuseLight) * color.rgb + specularLight, color.a);
    //float4 finalColor = float4(specularLight,color.a);
    //float4 finalColor = float4 (phongInput.Normal.xyz, color.a); //Pruebas de colores (normal)
    //float4 finalColor = float4 (phongInput.WorldPosition.xyz, color.a); //Pruebas de colores (POS)
    //float4 finalColor = color;
    return finalColor;
}

float4 PhongShaderNormalMap(float2 texCoord,float4 texelColor, float3 texelNormal,PhongShaderInput phongInput){
    // Base vectors
    float3 lightDirection = normalize(lightPosition - phongInput.WorldPosition.xyz);
    float3 viewDirection = normalize(eyePosition - phongInput.WorldPosition.xyz);
    float3 halfVector = normalize(lightDirection + viewDirection);
    float3 normal =  getNormalFromMap(texCoord, phongInput.WorldPosition.xyz, normalize(phongInput.Normal.xyz), texelNormal);

	// Get the texture texel
    //float4 texelColor = tex2D(textureSampler, input.TextureCoordinates);
    
	// Calculate the diffuse light
    float NdotL = saturate(dot(normal, lightDirection));
    float3 diffuseLight = KDiffuse * diffuseColor * NdotL;

	// Calculate the specular light
    float NdotH = dot(normal, halfVector);
    float3 specularLight = KSpecular * specularColor * pow(NdotH, shininess);
    
    // Final calculation
    float4 finalColor = float4(saturate(ambientColor * KAmbient + diffuseLight) * texelColor.rgb + specularLight, texelColor.a);
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

