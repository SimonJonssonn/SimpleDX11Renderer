Texture2D ShaderTexture[9];
SamplerState Sampler;

struct LightSource
{
    float4 LightColor;
    float3 LightPositionDirection;
    float LightBrightnes; //(in LUX @ 1 m^2)
    float LightType;
    float3 padding1;
};

cbuffer MatrixBuffer
{
	matrix worldMatrix;
	matrix viewMatrix;
	matrix projectionMatrix;
};

cbuffer CameraBuffer
{
	float3 cameraPosition;
	float padding;
};

cbuffer LightBuffer
{     
    float useTex1;
    float useTex2;
    float useNorm1;
    float useNorm2;
    float useSpec1;
    float useSpec2;
    float useRef1;
    float useRef2;
    float useTransp;
    
    float AffectedByLight;
    float ActiveLights;
    float Exposure;
    
    float4 ambientColor;
    
    LightSource lightSource1;
    LightSource lightSource2;
    LightSource lightSource3;
    LightSource lightSource4;
    LightSource lightSource5;
    LightSource lightSource6;
    LightSource lightSource7;
    LightSource lightSource8;
    LightSource lightSource9;
    LightSource lightSource10;
    LightSource lightSource11;
    LightSource lightSource12;
    LightSource lightSource13;
    LightSource lightSource14;
    LightSource lightSource15;
    LightSource lightSource16;
 //   float3 LightPositionDirections[1];
 //   float LightBrightness[1];
 //   float LightTypes[1];
    
	//float padding1;
    
    float4 materialColor;
};

struct VS_IN
{
	float4 pos : POSITION;
	float2 texUV : TEXCORD0;
	float3 normal : NORMAL;
	float3 tangent : TANGENT;
	float3 binormal : BINORMAL;
};

struct PS_IN
{
	float4 pos : SV_POSITION;
	float2 texUV : TEXCORD0;
	float3 normal : NORMAL;
	float3 tangent : TANGENT;
	float3 binormal : BINORMAL;
	float3 viewDir : TEXCOORD1;
	float3 Light1Pos : TEXCOORD2;
	float3 Light2Pos : TEXCOORD3;
	float3 Light3Pos : TEXCOORD4;
	float3 Light4Pos : TEXCOORD5;
	float3 Light5Pos : TEXCOORD6;
	float3 Light6Pos : TEXCOORD7;
	float3 Light7Pos : TEXCOORD9;
	float3 Light8Pos : TEXCOORD10;
	float3 Light9Pos : TEXCOORD11;
	float3 Light10Pos : TEXCOORD12;
	float3 Light11Pos : TEXCOORD13;
	float3 Light12Pos : TEXCOORD14;
	float3 Light13Pos : TEXCOORD15;
	float3 Light14Pos : TEXCOORD16;
	float3 Light15Pos : TEXCOORD17;
	float3 Light16Pos : TEXCOORD18;
};

float4x4 worldViewProj;

float4 DirectonalColor(float4 LightColor, float LightBrightnes, float3 Normal, float3 LightPositionDirection)
{
    return LightColor * LightBrightnes * saturate(dot(Normal, LightPositionDirection));
}

float DirectonalSpecular(float3 Normal, float3 LightPositionDirection, float3 viewDir, float LightBrightnes)
{
    return saturate(dot(normalize(2 * saturate(dot(Normal, LightPositionDirection) * Normal - LightPositionDirection)), normalize(viewDir))) * LightBrightnes;
}

float4 PointColor(float4 LightColor, float LightBrightnes, float3 Normal, float3 LightPositionDirection, float Area)
{
    return LightColor * LightBrightnes * dot(Normal, LightPositionDirection) / Area;
}

float PointSpecular(float3 Normal, float3 LightDirection, float3 viewDir, float LightBrightnes, float Area)
{
    float f = abs(dot(Normal, LightDirection)) / abs(dot(Normal, normalize(viewDir)));
    
    if(f > 1)
    {
        f = 1 / f;
    }
       
    return LightBrightnes / Area * pow(f, 2);
    //dot(normalize(2 * saturate(dot(-Normal, normalize(LightPositionDirection - PixelPosition)) * -Normal - normalize(LightPositionDirection - PixelPosition))), viewDir) * LightBrightnes / Area;
}

PS_IN VS( VS_IN input )
{
	PS_IN output = (PS_IN)0;
	float4 worldPos;

	input.pos.w = 1.0f;
	output.pos= mul(mul(mul(input.pos, worldMatrix), viewMatrix), projectionMatrix);

	output.texUV = input.texUV;
	
	worldPos = mul(input.pos, worldMatrix);

    output.viewDir = cameraPosition.xyz - worldPos.xyz;
    
	output.normal = normalize(mul(input.normal, (float3x3)worldMatrix));
	output.tangent = normalize(mul(input.tangent, (float3x3)worldMatrix));
	output.binormal = normalize(mul(input.binormal, (float3x3)worldMatrix));
	
    output.Light1Pos = lightSource1.LightPositionDirection;
    output.Light2Pos = lightSource2.LightPositionDirection;
    output.Light3Pos = lightSource3.LightPositionDirection;
    output.Light4Pos = lightSource4.LightPositionDirection;
    output.Light5Pos = lightSource5.LightPositionDirection;
    output.Light6Pos = lightSource6.LightPositionDirection;
    output.Light7Pos = lightSource7.LightPositionDirection;
    output.Light8Pos = lightSource8.LightPositionDirection;
    output.Light9Pos = lightSource9.LightPositionDirection;
    output.Light10Pos = lightSource10.LightPositionDirection;
    output.Light11Pos = lightSource11.LightPositionDirection;
    output.Light12Pos = lightSource12.LightPositionDirection;
    output.Light13Pos = lightSource13.LightPositionDirection;
    output.Light14Pos = lightSource14.LightPositionDirection;
    output.Light15Pos = lightSource15.LightPositionDirection;
    output.Light16Pos = lightSource16.LightPositionDirection;
    
    if (lightSource1.LightType == 1)
    {
        output.Light1Pos -= worldPos.xyz;
    }
    
    if (lightSource2.LightType == 1)
    {
        output.Light2Pos -= worldPos.xyz;
    }
    else
    {
        output.Light1Pos -= worldPos.xyz;
    }
    
    if (lightSource3.LightType == 1)
    {
        output.Light3Pos -= worldPos.xyz;
    }
	
    if (lightSource4.LightType == 1)
    {
        output.Light4Pos -= worldPos.xyz;
    }
	
    if (lightSource5.LightType == 1)
    {
        output.Light5Pos -= worldPos.xyz;
    }
	
    if (lightSource6.LightType == 1)
    {
        output.Light6Pos -= worldPos.xyz;
    }
	
    if (lightSource7.LightType == 1)
    {
        output.Light7Pos -= worldPos.xyz;
    }
	
    if (lightSource8.LightType == 1)
    {
        output.Light8Pos -= worldPos.xyz;
    }
	
    if (lightSource9.LightType == 1)
    {
        output.Light9Pos -= worldPos.xyz;
    }
	
    if (lightSource10.LightType == 1)
    {
        output.Light10Pos -= worldPos.xyz;
    }
	
    if (lightSource11.LightType == 1)
    {
        output.Light11Pos -= worldPos.xyz;
    }
	
    if (lightSource12.LightType == 1)
    {
        output.Light12Pos -= worldPos.xyz;
    }
	
    if (lightSource13.LightType == 1)
    {
        output.Light13Pos -= worldPos.xyz;
    }
	
    if (lightSource14.LightType == 1)
    {
        output.Light14Pos -= worldPos.xyz;
    }
	
    if (lightSource15.LightType == 1)
    {
        output.Light15Pos -= worldPos.xyz;
    }
	
    if (lightSource16.LightType == 1)
    {
        output.Light16Pos -= worldPos.xyz;
    }
	
	return output;
}

float4 PS(PS_IN input) : SV_Target
{	
	float4 texColor0 = ShaderTexture[0].Sample(Sampler, input.texUV);
	float4 texColor1 = ShaderTexture[1].Sample(Sampler, input.texUV);
	float4 normal0 = ShaderTexture[2].Sample(Sampler, input.texUV);
	float4 normal1 = ShaderTexture[3].Sample(Sampler, input.texUV);
	float4 spec0 = ShaderTexture[4].Sample(Sampler, input.texUV);
	float4 spec1 = ShaderTexture[5].Sample(Sampler, input.texUV);
	float4 reflec0 = ShaderTexture[6].Sample(Sampler, input.texUV);
	float4 reflec1 = ShaderTexture[7].Sample(Sampler, input.texUV);
	float4 transparancy = ShaderTexture[8].Sample(Sampler, input.texUV);

	float4 Texture;
	float3 Normal;
	float4 specular;
	
	float4 AllLightColor = float4(0, 0, 0, 0);
	float AllSpecular = 0;
	
    float specularIntensity = 0.5f;
    
	//Color
	if (true)
	{
		if (useTex1 == 1 && useTex2 == 1)
		{
            Texture = saturate(texColor0 * texColor1 * 2);

        }
        else if (useTex1 == 1 && useTex2 != 1)
		{
            Texture = texColor0;
        }
		else if (useTex1 != 1 && useTex2 == 1)
		{
            Texture = texColor1;
        }
        else
        {
            Texture = materialColor;
        }
    }

	//Normal
    if (AffectedByLight)
    {
        normal0 = (normal0 * 2) - 1;
        normal1 = (normal1 * 2) - 1;

        normal0.xyz = normalize((normal0.x * input.tangent) + (normal0.y * input.binormal) + (normal0.z * input.normal));
        normal1.xyz = normalize((normal1.x * input.tangent) + (normal1.y * input.binormal) + (normal1.z * input.normal));

        if (useNorm1 == 1 && useNorm2 == 1)
        {
            Normal = saturate(normal0.xyz * normal1.xyz * 2);
        }
        else if (useNorm1 == 1 && useNorm2 != 1)
        {
            Normal = normal0.xyz;
        }
        else if (useNorm1 != 1 && useNorm2 == 1)
        {
            Normal = normal1.xyz;
        }
        else
        {
            Normal = input.normal;
        }
    }

	//Specular
    if (AffectedByLight)
    {
        if (useSpec1 == 1 && useSpec2 == 1)
        {
            specular = saturate(spec0 * spec1 * 2);
        }
        else if (useSpec1 == 1 && useSpec2 != 1)
        {
            specular = spec0;
        }
        else if (useSpec1 != 1 && useSpec2 == 1)
        {
            specular = spec1;
        }
        else
        {
            specular = float4(1, 1, 1, 1);
        }
    }

	//Transparancy
    if (useTransp == 1)
    {
        Texture.w *= (transparancy.x + transparancy.y + transparancy.z) / 3;
    }
	
    if (AffectedByLight)
    {
        AllLightColor = 0; //ambientColor / 0.01667f;
        
        //Normal = float3(1, 1, 1);
        
        if(true)
        {           
         //Light 1
            if (true)
            {
            //Directional Light
                if (lightSource1.LightType == 0)
                {
                    AllLightColor += DirectonalColor(lightSource1.LightColor, lightSource1.LightBrightnes, Normal, lightSource1.LightPositionDirection);
                    AllSpecular += DirectonalSpecular(Normal, lightSource1.LightPositionDirection, input.viewDir, lightSource1.LightBrightnes);
                }
        
            //Point Light
                else if (lightSource1.LightType == 1)
                {
                    float Area = 12.56637 * (abs(input.Light1Pos.x) + abs(input.Light1Pos.y) + abs(input.Light1Pos.z));
                    
                    AllLightColor += PointColor(lightSource1.LightColor, lightSource1.LightBrightnes, -Normal, lightSource1.LightPositionDirection, Area);
                    AllSpecular += PointSpecular(Normal, input.Light1Pos, input.viewDir, lightSource1.LightBrightnes, Area);
                }
            }
        
        //Light 2
            if (true)
            {
            //Directional Light
                if (lightSource2.LightType == 0)
                {
                    AllLightColor += DirectonalColor(lightSource2.LightColor, lightSource2.LightBrightnes, Normal, lightSource2.LightPositionDirection);
                    AllSpecular += DirectonalSpecular(Normal, lightSource2.LightPositionDirection, input.viewDir, lightSource2.LightBrightnes);
                }
        
            //Point Light
                else if (lightSource2.LightType == 1)
                {
                    float Area = 12.56637 * (abs(input.Light2Pos.x) + abs(input.Light2Pos.y) + abs(input.Light2Pos.z));
                
                    AllLightColor += PointColor(lightSource2.LightColor, lightSource2.LightBrightnes, -Normal, lightSource2.LightPositionDirection, Area);
                    AllSpecular += PointSpecular(Normal, input.Light2Pos, input.viewDir, lightSource2.LightBrightnes, Area);
                }
            }
        
        //Light 3
            if (true)
            {
            //Directional Light
                if (lightSource3.LightType == 0)
                {
                    AllLightColor += DirectonalColor(lightSource3.LightColor, lightSource3.LightBrightnes, Normal, lightSource3.LightPositionDirection);
                    AllSpecular += DirectonalSpecular(Normal, lightSource3.LightPositionDirection, input.viewDir, lightSource3.LightBrightnes);
                }
        
            //Point Light
                else if (lightSource3.LightType == 1)
                {
                    float Area = 12.56637 * (abs(input.Light3Pos.x) + abs(input.Light3Pos.y) + abs(input.Light3Pos.z));
            
                    AllLightColor += PointColor(lightSource3.LightColor, lightSource3.LightBrightnes, -Normal, lightSource3.LightPositionDirection, Area);
                    AllSpecular += PointSpecular(Normal, input.Light3Pos, input.viewDir, lightSource3.LightBrightnes, Area);
                }
            }
        
        //Light 4
            if (true)
            {
            //Directional Light
                if (lightSource4.LightType == 0)
                {
                    AllLightColor += DirectonalColor(lightSource4.LightColor, lightSource4.LightBrightnes, Normal, lightSource4.LightPositionDirection);
                    AllSpecular += DirectonalSpecular(Normal, lightSource4.LightPositionDirection, input.viewDir, lightSource4.LightBrightnes);
                }
        
            //Point Light
                else if (lightSource4.LightType == 1)
                {
                    float Area = 12.56637 * (abs(input.Light4Pos.x) + abs(input.Light4Pos.y) + abs(input.Light4Pos.z));
            
                    AllLightColor += PointColor(lightSource4.LightColor, lightSource4.LightBrightnes, -Normal, lightSource4.LightPositionDirection, Area);
                    AllSpecular += PointSpecular(Normal, input.Light4Pos, input.viewDir, lightSource4.LightBrightnes, Area);
                }
            }
        
        //Light 5
            if (true)
            {
            //Directional Light
                if (lightSource5.LightType == 0)
                {
                    AllLightColor += DirectonalColor(lightSource5.LightColor, lightSource5.LightBrightnes, Normal, lightSource5.LightPositionDirection);
                    AllSpecular += DirectonalSpecular(Normal, lightSource5.LightPositionDirection, input.viewDir, lightSource5.LightBrightnes);
                }
        
            //Point Light
                else if (lightSource5.LightType == 1)
                {
                    float Area = 12.56637 * (abs(input.Light5Pos.x) + abs(input.Light5Pos.y) + abs(input.Light5Pos.z));
            
                    AllLightColor += PointColor(lightSource5.LightColor, lightSource5.LightBrightnes, -Normal, lightSource5.LightPositionDirection, Area);
                    AllSpecular += PointSpecular(Normal, input.Light5Pos, input.viewDir, lightSource5.LightBrightnes, Area);
                }
            }
        
        //Light 6
            if (true)
            {
            //Directional Light
                if (lightSource6.LightType == 0)
                {
                    AllLightColor += DirectonalColor(lightSource6.LightColor, lightSource6.LightBrightnes, Normal, lightSource6.LightPositionDirection);
                    AllSpecular += DirectonalSpecular(Normal, lightSource6.LightPositionDirection, input.viewDir, lightSource6.LightBrightnes);
                }
        
            //Point Light
                else if (lightSource6.LightType == 1)
                {
                    float Area = 12.56637 * (abs(input.Light6Pos.x) + abs(input.Light6Pos.y) + abs(input.Light6Pos.z));
            
                    AllLightColor += PointColor(lightSource6.LightColor, lightSource6.LightBrightnes, -Normal, lightSource6.LightPositionDirection, Area);
                    AllSpecular += PointSpecular(Normal, input.Light6Pos, input.viewDir, lightSource6.LightBrightnes, Area);
                }
            }
        
        //Light 7
            if (true)
            {
            //Directional Light
                if (lightSource7.LightType == 0)
                {
                    AllLightColor += DirectonalColor(lightSource7.LightColor, lightSource7.LightBrightnes, Normal, lightSource7.LightPositionDirection);
                    AllSpecular += DirectonalSpecular(Normal, lightSource7.LightPositionDirection, input.viewDir, lightSource7.LightBrightnes);
                }
        
            //Point Light
                else if (lightSource7.LightType == 1)
                {
                    float Area = 12.56637 * (abs(input.Light7Pos.x) + abs(input.Light7Pos.y) + abs(input.Light7Pos.z));
            
                    AllLightColor += PointColor(lightSource7.LightColor, lightSource7.LightBrightnes, -Normal, lightSource7.LightPositionDirection, Area);
                    AllSpecular += PointSpecular(Normal, input.Light7Pos, input.viewDir, lightSource7.LightBrightnes, Area);
                }
            }
        
        //Light 8
            if (true)
            {
            //Directional Light
                if (lightSource8.LightType == 0)
                {
                    AllLightColor += DirectonalColor(lightSource8.LightColor, lightSource8.LightBrightnes, Normal, lightSource8.LightPositionDirection);
                    AllSpecular += DirectonalSpecular(Normal, lightSource8.LightPositionDirection, input.viewDir, lightSource8.LightBrightnes);
                }
        
            //Point Light
                else if (lightSource8.LightType == 1)
                {
                    float Area = 12.56637 * (abs(input.Light8Pos.x) + abs(input.Light8Pos.y) + abs(input.Light8Pos.z));
            
                    AllLightColor += PointColor(lightSource8.LightColor, lightSource8.LightBrightnes, -Normal, lightSource8.LightPositionDirection, Area);
                    AllSpecular += PointSpecular(Normal, input.Light8Pos, input.viewDir, lightSource8.LightBrightnes, Area);
                }
            }
        
        //Light 9
            if (true)
            {
            //Directional Light
                if (lightSource9.LightType == 0)
                {
                    AllLightColor += DirectonalColor(lightSource9.LightColor, lightSource9.LightBrightnes, Normal, lightSource9.LightPositionDirection);
                    AllSpecular += DirectonalSpecular(Normal, lightSource9.LightPositionDirection, input.viewDir, lightSource9.LightBrightnes);
                }
        
            //Point Light
                else if (lightSource9.LightType == 1)
                {
                    float Area = 12.56637 * (abs(input.Light9Pos.x) + abs(input.Light9Pos.y) + abs(input.Light9Pos.z));
            
                    AllLightColor += PointColor(lightSource9.LightColor, lightSource9.LightBrightnes, -Normal, lightSource9.LightPositionDirection, Area);
                    AllSpecular += PointSpecular(Normal, input.Light9Pos, input.viewDir, lightSource9.LightBrightnes, Area);
                }
            }
        
        //Light 10
            if (true)
            {
            //Directional Light
                if (lightSource10.LightType == 0)
                {
                    AllLightColor += DirectonalColor(lightSource10.LightColor, lightSource10.LightBrightnes, Normal, lightSource10.LightPositionDirection);
                    AllSpecular += DirectonalSpecular(Normal, lightSource10.LightPositionDirection, input.viewDir, lightSource10.LightBrightnes);
                }
        
            //Point Light
                else if (lightSource10.LightType == 1)
                {
                    float Area = 12.56637 * (abs(input.Light10Pos.x) + abs(input.Light10Pos.y) + abs(input.Light10Pos.z));
            
                    AllLightColor += PointColor(lightSource10.LightColor, lightSource10.LightBrightnes, -Normal, lightSource10.LightPositionDirection, Area);
                    AllSpecular += PointSpecular(Normal, input.Light10Pos, input.viewDir, lightSource10.LightBrightnes, Area);
                }
            }
        
        //Light 11
            if (true)
            {
            //Directional Light
                if (lightSource11.LightType == 0)
                {
                    AllLightColor += DirectonalColor(lightSource11.LightColor, lightSource11.LightBrightnes, Normal, lightSource11.LightPositionDirection);
                    AllSpecular += DirectonalSpecular(Normal, lightSource11.LightPositionDirection, input.viewDir, lightSource11.LightBrightnes);
                }
        
            //Point Light
                else if (lightSource11.LightType == 1)
                {
                    float Area = 12.56637 * (abs(input.Light11Pos.x) + abs(input.Light11Pos.y) + abs(input.Light11Pos.z));
            
                    AllLightColor += PointColor(lightSource11.LightColor, lightSource11.LightBrightnes, -Normal, lightSource11.LightPositionDirection, Area);
                    AllSpecular += PointSpecular(Normal, input.Light11Pos, input.viewDir, lightSource11.LightBrightnes, Area);
                }
            }
        
        //Light 12
            if (true)
            {
            //Directional Light
                if (lightSource12.LightType == 0)
                {
                    AllLightColor += DirectonalColor(lightSource12.LightColor, lightSource12.LightBrightnes, Normal, lightSource12.LightPositionDirection);
                    AllSpecular += DirectonalSpecular(Normal, lightSource12.LightPositionDirection, input.viewDir, lightSource12.LightBrightnes);
                }
        
            //Point Light
                else if (lightSource12.LightType == 1)
                {
                    float Area = 12.56637 * (abs(input.Light12Pos.x) + abs(input.Light12Pos.y) + abs(input.Light12Pos.z));
            
                    AllLightColor += PointColor(lightSource12.LightColor, lightSource12.LightBrightnes, -Normal, lightSource12.LightPositionDirection, Area);
                    AllSpecular += PointSpecular(Normal, input.Light12Pos, input.viewDir, lightSource12.LightBrightnes, Area);
                }
            }
        
        //Light 13
            if (true)
            {
            //Directional Light
                if (lightSource13.LightType == 0)
                {
                    AllLightColor += DirectonalColor(lightSource13.LightColor, lightSource13.LightBrightnes, Normal, lightSource13.LightPositionDirection);
                    AllSpecular += DirectonalSpecular(Normal, lightSource13.LightPositionDirection, input.viewDir, lightSource13.LightBrightnes);
                }
        
            //Point Light
                else if (lightSource13.LightType == 1)
                {
                    float Area = 12.56637 * (abs(input.Light13Pos.x) + abs(input.Light13Pos.y) + abs(input.Light13Pos.z));
            
                    AllLightColor += PointColor(lightSource13.LightColor, lightSource13.LightBrightnes, -Normal, lightSource13.LightPositionDirection, Area);
                    AllSpecular += PointSpecular(Normal, input.Light13Pos, input.viewDir, lightSource13.LightBrightnes, Area);
                }
            }
        
        //Light 14
            if (true)
            {
            //Directional Light
                if (lightSource14.LightType == 0)
                {
                    AllLightColor += DirectonalColor(lightSource14.LightColor, lightSource14.LightBrightnes, Normal, lightSource14.LightPositionDirection);
                    AllSpecular += DirectonalSpecular(Normal, lightSource14.LightPositionDirection, input.viewDir, lightSource14.LightBrightnes);
                }
        
            //Point Light
                else if (lightSource14.LightType == 1)
                {
                    float Area = 12.56637 * (abs(input.Light14Pos.x) + abs(input.Light14Pos.y) + abs(input.Light14Pos.z));
            
                    AllLightColor += PointColor(lightSource14.LightColor, lightSource14.LightBrightnes, -Normal, lightSource14.LightPositionDirection, Area);
                    AllSpecular += PointSpecular(Normal, input.Light14Pos, input.viewDir, lightSource14.LightBrightnes, Area);
                }
            }
        
        //Light 15
            if (true)
            {
            //Directional Light
                if (lightSource15.LightType == 0)
                {
                    AllLightColor += DirectonalColor(lightSource15.LightColor, lightSource15.LightBrightnes, Normal, lightSource15.LightPositionDirection);
                    AllSpecular += DirectonalSpecular(Normal, lightSource15.LightPositionDirection, input.viewDir, lightSource15.LightBrightnes);
                }
        
            //Point Light
                else if (lightSource15.LightType == 1)
                {
                    float Area = 12.56637 * (abs(input.Light15Pos.x) + abs(input.Light15Pos.y) + abs(input.Light15Pos.z));
            
                    AllLightColor += PointColor(lightSource15.LightColor, lightSource15.LightBrightnes, -Normal, lightSource15.LightPositionDirection, Area);
                    AllSpecular += PointSpecular(Normal, input.Light15Pos, input.viewDir, lightSource15.LightBrightnes, Area);
                }
            }
        
        //Light 16
            if (true)
            {
            //Directional Light
                if (lightSource16.LightType == 0)
                {
                    AllLightColor += DirectonalColor(lightSource16.LightColor, lightSource16.LightBrightnes, Normal, lightSource16.LightPositionDirection);
                    AllSpecular += DirectonalSpecular(Normal, lightSource16.LightPositionDirection, input.viewDir, lightSource16.LightBrightnes);
                }
        
            //Point Light
                else if (lightSource16.LightType == 1)
                {
                    float Area = 12.56637 * (abs(input.Light16Pos.x) + abs(input.Light16Pos.y) + abs(input.Light16Pos.z));
            
                    AllLightColor += PointColor(lightSource16.LightColor, lightSource16.LightBrightnes, -Normal, lightSource16.LightPositionDirection, Area);
                    AllSpecular += PointSpecular(Normal, input.Light16Pos, input.viewDir, lightSource16.LightBrightnes, Area);
                }
            }
        }
        
        Texture = Texture * AllLightColor / 255 * 0.01667f * Exposure;
        specular *= AllLightColor * pow(AllSpecular / 255, 5) * 0.01667f * Exposure;
        Texture += specular;
    }
	
    if (Texture.x > 255)
    {
        Texture.x = 255;
    }
    
    if (Texture.y > 255)
    {
        Texture.y = 255;
    }
    
    if (Texture.z > 255)
    {
        Texture.z = 255;
    }
    
    Texture.w = 255;
    
	return Texture;
}