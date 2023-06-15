#ifndef SOBELOUTLINES_INCLUDED
#define SOBELOUTLINES_INCLUDED

#include "DecodeDepthNormals.hlsl"

TEXTURE2D(_DepthNormalsTexture); SAMPLER(sampler_DepthNormalsTexture);

static float2 sobelSamplePoints[9] = {
	float2(-1, 1), float2(0, 1), float2(1, 1),
	float2(-1, 0), float2(0, 0), float2(1, 1),
	float2(-1, -1), float2(0, -1), float2(1, -1),
};

static float sobelXMatrix[9] = {
	1, 0, -1,
	2, 0, -2,
	1, 0, -1
};

static float sobelYMatrix[9] = {
	1, 2, 1,
	0, 0, 0,
	-1, -2, -1
};

void DepthSobel_float(float2 UV, float Thickness, out float Out) {
	float2 sobel = 0;

	[unroll] for (int i = 0; i < 9; i++) {
		float depth = SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV + sobelSamplePoints[i] * Thickness);
		sobel += depth * float2(sobelXMatrix[i], sobelYMatrix[i]);
	}

	Out = length(sobel);
}

void GetDepthAndNormal(float2 uv, out float depth, out float3 normal){
	float4 coded = SAMPLE_TEXTURE2D(_DepthNormalsTexture, sampler_DepthNormalsTexture, uv);
	DecodeDepthNormal(coded, depth, normal);
}

void CalculateDepthNormal_float(float2 UV, out float Depth, out float3 Normal){
	GetDepthAndNormal(UV, Depth, Normal);
	Normal = Normal * 2 - 1;
}

void NormalsSobel_float(float2 UV, float Thickness, out float Out){
	float2 sobelX = 0;
	float2 sobelY = 0;
	float2 sobelZ = 0;

	[unroll] for (int i = 0; i < 9; i++){
		float depth;
		float3 normal;
		GetDepthAndNormal(UV + sobelSamplePoints[i] * Thickness, depth, normal);
		float2 kernel = float2(sobelXMatrix[i], sobelXMatrix[i]);
		sobelX += normal.x * kernel;
		sobelY += normal.y * kernel;
		sobelZ += normal.z * kernel;
	}

	Out = max(length(sobelX), max(length(sobelY), length(sobelZ)));
}

void ViewDirectionFromScreenUV_float(float2 In, out float3 Out)
{
	float2 p11_22 = float2(unity_CameraProjection._11, unity_CameraProjection._22);
	Out = -normalize(float3((In * 2 - 1) / p11_22, -1));
}

#endif