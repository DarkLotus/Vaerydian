float screenWidth;
float screenHeight;
float lightStrength;
float lightRadius;
float3 lightPosition;
float4 lightColor;
float specularStrength;
float3 viewCenter;
float3 viewOrigin;
float4 specularColor;

Texture NormalMap;
sampler NormalMapSampler = sampler_state {
	texture = <NormalMap>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = mirror;
	AddressV = mirror;
};

Texture ColorMap;
sampler ColorMapSampler = sampler_state {
	texture = <ColorMap>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = mirror;
	AddressV = mirror;
};

struct VertexToPixel
{
	float4 Position : POSITION;
	float2 TexCoord : TEXCOORD0;
	float4 Color : COLOR0;
};

struct PixelToFrame
{
	float4 Color : COLOR0;
};

VertexToPixel MyVertexShader(float4 inPos: POSITION0, float2 texCoord: TEXCOORD0, float4 color: COLOR0)
{
	VertexToPixel Output = (VertexToPixel)0;
	
	Output.Position = inPos;
	Output.TexCoord = texCoord;
	Output.Color = color;
	
	return Output;
}

PixelToFrame PointLightShader(VertexToPixel PSIn) : COLOR0
{	
	PixelToFrame Output = (PixelToFrame)0;
	
	float4 colorMap = tex2D(ColorMapSampler, PSIn.TexCoord);
	float3 normal = (2.0f * (tex2D(NormalMapSampler, PSIn.TexCoord))) - 1.0f;
		
	float3 pixelPosition;
	pixelPosition.x = screenWidth * PSIn.TexCoord.x;
	pixelPosition.y = screenHeight * PSIn.TexCoord.y;
	pixelPosition.z = 0;

	float3 view = viewOrigin - viewCenter;
	float3 realLightPos = lightPosition - view;
	float3 lightDir = realLightPos - pixelPosition;
	float3 lightDirNorm = normalize(lightDir);

	float3 eye = float3(viewCenter.x, viewCenter.y,1000) - pixelPosition;
	float3 eyeNorm = normalize(eye);
	
	float amount = max(dot(normal, lightDirNorm), 0);
	float coneAttenuation = saturate(1.0f - length(lightDir) / (lightRadius)); 
				
	float3 ref = normalize(reflect(-lightDirNorm,normal));

	float specular = min(pow(saturate(dot(ref,eyeNorm)),10),amount) * specularStrength;
				
	Output.Color = colorMap * coneAttenuation * lightColor * lightStrength + (specular * coneAttenuation) * specularColor;

	return Output;
}

technique PointLight
{
    pass P0
    {
		VertexShader = compile vs_2_0 MyVertexShader();
        PixelShader = compile ps_2_0 PointLightShader();
    }
}
