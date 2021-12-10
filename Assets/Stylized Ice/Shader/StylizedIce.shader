Shader "StylizedIce/StylizedIce"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_MainColor("Main Color", Color) = (1,1,1,1)
		[NoScaleOffset] _Bump("Bump", 2D) = "bump" {}
		_BumpMapScaleX("Bump Map Scale X", Range(0, 50)) = 1
		_BumpMapScaleY("Bump Map Scale Y", Range(0, 50)) = 1
		_BumpOffset("Bump Offset", Vector) = (0,0,0,0)
		[NoScaleOffset] _ThicknessMap("Thickness Map", 2D) = "white" {}
		_ThicknessMapScaleX("Thickness Map Scale X", Range(0, 50)) = 1
		_ThicknessMapScaleY("Thickness Map Scale Y", Range(0, 50)) = 1
		_SpecularColor("Spec Color", Color) = (1,1,1,1)
		_SpecularRange("Spec Range", Range(-1, 1)) = 0
		_Lighting("Lighting", Range(0,5)) = 1.5
		[NoScaleOffset] _Noise("Noise Map", 2D) = "white" {}
		_NoiseScaleX("Glitter Scale X", Range(0, 3)) = 1
		_NoiseScaleY("Glitter Scale Y", Range(0, 3)) = 1
		[NoScaleOffset] _LUT("LUT", 2D) = "white" {}
		[NoScaleOffset] _EnvMap ("Env Map", Cube) = "defaulttexture" {}
		_LightingEnv("Lighting Env", Range(0,2)) = 0.4
		_LightingEnvSigma("Lighting Env Sigma", Range(-5, 0)) = -1.78
		_Luminousness("Luminousness", Range(0.1, 10)) = 8

		[HideInInspector] [KeywordEnum(OFF, ON, ON_MASK)] SNOW("Snow", Float) = 0
		[HideInInspector] _SnowColor("Snow Color", Color) = (1,1,1,1)
		[HideInInspector] _SnowDir("Snow Dir", Vector) = (0, 1, 0, 1)
		[HideInInspector] _SnowMult("Snow Multiplier", Range(0, 10)) = 3
		[HideInInspector] _SnowPow("Snow Power", Range(0, 200)) = 10
		[HideInInspector] _SnowIntensity("Snow Intensity", Range(1, 5)) = 1
		[HideInInspector] _SnowMask("Snow Mask", 2D) = "white" {}

		[HideInInspector] [KeywordEnum(LOCAL, WORLD, WORLD_TRIPLANAR)] SPACE("Local Space or World Space", Float) = 0
		[HideInInspector] _WorldSpaceScale("World Space Scale", Range(1, 100)) = 25
		[HideInInspector] _WorldSpaceRotation("World Space Rotation", Range(0, 6.283)) = 0
		[HideInInspector] _WorldSpaceTriplanarScale("World Space Triplanar Scale", Range(0.0001, 0.5)) = 0.025
		[HideInInspector] _WorldSpaceTriplanarBlend("World Space Triplanar Blend", Range(1, 10)) = 5
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue"="Geometry" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma multi_compile_fwdbase
			#pragma shader_feature _ SNOW_ON SNOW_ON_MASK
			#pragma shader_feature SPACE_LOCAL SPACE_WORLD SPACE_WORLD_TRIPLANAR
			
			#include "UnityCG.cginc"

			half4 _SnowColor;
			float4 _SnowDir;
			half _SnowMult;
			half _SnowPow;
			half _SnowIntensity;
			sampler2D _SnowMask;
			float4 _SnowMask_ST;

			#define SNOW_UV_SURFACE float2 uv_SnowMask
			#define SNOW_UV_VP(idx) float2 uv_SnowMask:TEXCOORD##idx
			#define SNOW_UV_TRANSFER(i, uv) i.uv_SnowMask = TRANSFORM_TEX(uv, _SnowMask)
			#define SNOW_UV_GET(i) i.uv_SnowMask

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 uv : TEXCOORD0;
				float4 uv2 : TEXCOORD1;
				float4 tangent : TANGENT;
			};

			struct v2f
			{
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 TtoW0 : TEXCOORD2;
				float4 TtoW1 : TEXCOORD3;
				float4 TtoW2 : TEXCOORD4;
				SNOW_UV_VP(5);
				float3 wNormal : TEXCOORD6;
				float3 ccc : TEXCOORD7;
			};

			sampler2D _MainTex;
			sampler2D _Bump;
			sampler2D _LUT;
			sampler2D _ThicknessMap;
			sampler2D _Noise;
			samplerCUBE _EnvMap;

			half4 _MainColor;
			float4 _MainTex_ST;
			float4 _Bump_ST;
			half4 _SpecularColor;
			float _Lighting;
			float _LightingEnv;
			float _SpecularRange;
			float _LightingEnvSigma;
			float _ThicknessMapScaleX;
			float _ThicknessMapScaleY;
			float _BumpMapScaleX;
			float _BumpMapScaleY;
			float3 _BumpOffset;
			float _NoiseScaleX;
			float _NoiseScaleY;
			float _Luminousness;
			
			uniform float _TangentCorrection;

			float _WorldSpaceScale;
			float _WorldSpaceRotation;
			float _WorldSpaceTriplanarScale;
			float _WorldSpaceTriplanarBlend;

			#define Quaternion float4

			inline Quaternion SetAxisAngle(float3 axis, float radian)
			{
				float sinValue = 0;
				float cosValue = 0;
				sincos(radian * 0.5, sinValue, cosValue);
				Quaternion q = Quaternion(sinValue * axis.xyz, cosValue);
				return q;
			}

			inline float3 MultiplyQP(Quaternion rotation, float3 p)
			{
				float3 xyz = rotation.xyz * 2;
				float3 xx_yy_zz = rotation.xyz * xyz.xyz;
				float3 xy_xz_yz = rotation.xxy * xyz.yzz;
				float3 wx_wy_wz = rotation.www * xyz.xyz;

				float3 res;
				res.x = (1 - (xx_yy_zz.y + xx_yy_zz.z)) * p.x + (xy_xz_yz.x - wx_wy_wz.z) * p.y + (xy_xz_yz.y + wx_wy_wz.y) * p.z;
				res.y = (xy_xz_yz.x + wx_wy_wz.z) * p.x + (1 - (xx_yy_zz.x + xx_yy_zz.z)) * p.y + (xy_xz_yz.z - wx_wy_wz.x) * p.z;
				res.z = (xy_xz_yz.y - wx_wy_wz.y) * p.x + (xy_xz_yz.z + wx_wy_wz.x) * p.y + (1 - (xx_yy_zz.x + xx_yy_zz.y)) * p.z;
				return res;
			}

			inline half4 tex2DTriplanar(sampler2D tex, float3 pos, float3 normal, float2 uvScale)
			{
				float3 bf = normalize(pow(abs(normal), _WorldSpaceTriplanarBlend));
				bf /= dot(bf, (float3)1);

				float _MapScale = _WorldSpaceTriplanarScale;
				float2 tx = pos.yz * _MapScale;
				float2 ty = pos.zx * _MapScale;
				float2 tz = pos.xy * _MapScale;

				half4 cx = tex2D(tex, tx * uvScale) * bf.x;
				half4 cy = tex2D(tex, ty * uvScale) * bf.y;
				half4 cz = tex2D(tex, tz * uvScale) * bf.z;

				return cx + cy + cz;
			}

			inline half3 snow(float3 worldNormal, float3 wNormal, float3 wPos, float3 viewDir, half3 albedo, float2 uv_SnowMask)
			{
#if defined(SNOW_ON) || defined(SNOW_ON_MASK)
				half mask = 1;
	#if defined(SNOW_ON_MASK)
		#if defined(SPACE_WORLD_TRIPLANAR)
				mask = tex2DTriplanar(_SnowMask, wPos, wNormal, float2(1,1));
		#else
				mask = tex2D(_SnowMask, uv_SnowMask.xy).r;
		#endif
	#endif

				float diff = 0;
				float snowMask = 1 - saturate(pow(dot(worldNormal, normalize(_SnowDir.xyz)) * _SnowMult, -abs(_SnowPow)));
				diff = saturate(diff + snowMask * worldNormal.g);
				
				float NdotV = pow((1 - saturate(dot(worldNormal, viewDir))), 10) + 1;

				return lerp(albedo, _SnowColor.rgb * (_SnowIntensity * NdotV), diff * mask);
#else
				return albedo;
#endif
			}

			v2f vert (appdata v)
			{
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

#if defined(SPACE_WORLD) || defined(SPACE_WORLD_TRIPLANAR)
				Quaternion q = SetAxisAngle(v.normal, _TangentCorrection);
				float3 tan = MultiplyQP(q, v.tangent.xyz);
#else
				float3 tan = v.tangent.xyz;
#endif
o.ccc = tan;

				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				float3 binormal = cross( normalize(v.normal), normalize(tan) ) * v.tangent.w;
    			float3x3 rotation = float3x3( tan, binormal, v.normal );

				float3x3 WtoT = mul(rotation, (float3x3)unity_WorldToObject);
				o.TtoW0 = float4(WtoT[0].xyz, worldPos.x);
				o.TtoW1 = float4(WtoT[1].xyz, worldPos.y);
				o.TtoW2 = float4(WtoT[2].xyz, worldPos.z);

				o.wNormal = mul(v.normal, (float3x3)unity_WorldToObject);
				SNOW_UV_TRANSFER(o, v.uv);
				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}
			
			half4 frag (v2f i) : SV_Target
			{
				float3 worldPos = float3(i.TtoW0.w, i.TtoW1.w, i.TtoW2.w);
				float3 wNormal = normalize(i.wNormal); 
				
#if defined(SPACE_WORLD) || defined(SPACE_WORLD_TRIPLANAR)
				float3 wPosRot = worldPos;
				Quaternion q = SetAxisAngle(float3(0,1,0), _WorldSpaceRotation);
				wPosRot = MultiplyQP(q, wPosRot);
				float3 wPosRot2 = wPosRot;
				wPosRot.xz *= 0.001;
				wPosRot.xz *= _WorldSpaceScale;
				float2 uv = wPosRot.xz;
#else
				float2 uv = i.uv;
#endif

#if defined(SPACE_WORLD_TRIPLANAR)
				half4 thicknessMap = tex2DTriplanar(_ThicknessMap, wPosRot2, wNormal, float2(_ThicknessMapScaleX,_ThicknessMapScaleY));
#else
				half4 thicknessMap = tex2D(_ThicknessMap, uv * float2(_ThicknessMapScaleX,_ThicknessMapScaleY));
#endif
#if defined(SPACE_WORLD_TRIPLANAR)
				half4 diffuseColor = tex2DTriplanar(_MainTex, wPosRot2, wNormal, float2(1,1)) * _MainColor * _Lighting;
#else
				half4 diffuseColor = tex2D(_MainTex, uv) * _MainColor * _Lighting;
#endif
#if defined(SPACE_WORLD_TRIPLANAR)
				half3 norm = UnpackNormal(tex2DTriplanar(_Bump, wPosRot2, wNormal, float2(_BumpMapScaleX, _BumpMapScaleY)));
#else
				half3 norm = UnpackNormal(tex2D(_Bump, uv * float2(_BumpMapScaleX, _BumpMapScaleY)));
#endif
				half3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
				half3 lightDir = (-worldViewDir + float3(thicknessMap.xyz * 2 - 1));
				float3 H = normalize(lightDir + worldViewDir);
				half3 worldNormal = normalize(mul(norm, float3x3(i.TtoW0.xyz, i.TtoW1.xyz, i.TtoW2.xyz)));
				worldNormal += _BumpOffset;
				worldNormal = normalize(worldNormal);
				// control normal to get more better effect

				thicknessMap *= pow(dot(worldNormal, worldViewDir), _Luminousness);
				thicknessMap = saturate(thicknessMap);

				half2 s;
				s.x = dot(worldNormal, lightDir);
				s.y = dot(worldNormal, H);
				float thickness = exp(thicknessMap.r * _LightingEnvSigma);
				half4 light = tex2D(_LUT, s * 0.5 + 0.5);
				half4 c = diffuseColor * half4(light.rgb * thickness,1) + _SpecularColor * saturate(light.a + _SpecularRange) + (1-saturate(thickness)) * texCUBE(_EnvMap, -worldViewDir-worldNormal)*_LightingEnv;
				
#if defined(SPACE_WORLD) || defined(SPACE_WORLD_TRIPLANAR)
				float2 snowUV = uv;
#else
				float2 snowUV = SNOW_UV_GET(i);
#endif

#if defined(SPACE_WORLD_TRIPLANAR)
				c.rgb = snow(worldNormal, wNormal, wPosRot2, worldViewDir, c.rgb, snowUV);
#else
				c.rgb = snow(worldNormal, wNormal, worldPos, worldViewDir, c.rgb, snowUV);
#endif

#if defined(SPACE_WORLD_TRIPLANAR)
				half3 noise = UnpackNormal(tex2DTriplanar(_Noise, wPosRot2, wNormal, float2(_NoiseScaleX,_NoiseScaleY)));
#else
				half3 noise = UnpackNormal(tex2D(_Noise, uv * float2(_NoiseScaleX,_NoiseScaleY)));
#endif
				half3 glitter = frac(0.6 * worldNormal + 9 * noise.xyz + worldViewDir * lightDir);
				glitter = saturate(1 - 2.5 * (glitter.x + glitter.y + glitter.z)) * _SpecularColor * 1.25;
				c.rgb += glitter;
				c.a = 1;        

				UNITY_APPLY_FOG(i.fogCoord, c);
				return c;
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "StylizedIceShaderGUI"
}
