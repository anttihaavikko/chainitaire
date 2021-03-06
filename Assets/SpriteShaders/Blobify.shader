﻿Shader "CustomSprites/SinBlobify"
{
    Properties{
        _Speed ("Speed", Float) = 1
        _Offset ("Offset", Float) = 0
        _Amount ("Amount", Float) = 0.03
        _MainTex ("Texture", 2D) = "white" {}

        _offsetX ("OffsetX",Float) = 0.0
        _offsetY ("OffsetY",Float) = 0.0      
        _octaves ("Octaves",Int) = 7
        _lacunarity("Lacunarity", Range( 1.0 , 5.0)) = 2
        _gain("Gain", Range( 0.0 , 1.0)) = 0.5
        _value("Value", Range( -2.0 , 2.0)) = 0.0
        _amplitude("Amplitude", Range( 0.0 , 5.0)) = 1.5
        _frequency("Frequency", Range( 0.0 , 6.0)) = 2.0
        _power("Power", Range( 0.1 , 5.0)) = 1.0
        _scale("Scale", Float) = 1.0  
    }

    SubShader{
        Tags{ 
            "RenderType"="Transparent" 
            "Queue"="Transparent"
        }

        Blend SrcAlpha OneMinusSrcAlpha

        ZWrite off
        Cull off

        Pass{

            CGPROGRAM

            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _Speed;
            float _RotationSpeed;
            float _Offset;
            float _Amount;
            float _Peaks;

            float _octaves, _lacunarity, _gain, _value, _amplitude, _frequency, _power, _scale;

            float fbm(float2 p)
            {
                p = p * _scale + float2(_Offset + _Time.y * _Speed, _Offset);
                for( int i = 0; i < _octaves; i++ )
                {
                    float2 i = floor( p * _frequency );
                    float2 f = frac( p * _frequency );      
                    float2 t = f * f * f * ( f * ( f * 6.0 - 15.0 ) + 10.0 );
                    float2 a = i + float2( 0.0, 0.0 );
                    float2 b = i + float2( 1.0, 0.0 );
                    float2 c = i + float2( 0.0, 1.0 );
                    float2 d = i + float2( 1.0, 1.0 );
                    a = -1.0 + 2.0 * frac( sin( float2( dot( a, float2( 127.1, 311.7 ) ),dot( a, float2( 269.5,183.3 ) ) ) ) * 43758.5453123 );
                    b = -1.0 + 2.0 * frac( sin( float2( dot( b, float2( 127.1, 311.7 ) ),dot( b, float2( 269.5,183.3 ) ) ) ) * 43758.5453123 );
                    c = -1.0 + 2.0 * frac( sin( float2( dot( c, float2( 127.1, 311.7 ) ),dot( c, float2( 269.5,183.3 ) ) ) ) * 43758.5453123 );
                    d = -1.0 + 2.0 * frac( sin( float2( dot( d, float2( 127.1, 311.7 ) ),dot( d, float2( 269.5,183.3 ) ) ) ) * 43758.5453123 );
                    float A = dot( a, f - float2( 0.0, 0.0 ) );
                    float B = dot( b, f - float2( 1.0, 0.0 ) );
                    float C = dot( c, f - float2( 0.0, 1.0 ) );
                    float D = dot( d, f - float2( 1.0, 1.0 ) );
                    float noise = ( lerp( lerp( A, B, t.x ), lerp( C, D, t.x ), t.y ) );              
                    _value += _amplitude * noise;
                    _frequency *= _lacunarity;
                    _amplitude *= _gain;
                }
                _value = clamp( _value, -1.0, 1.0 );
                return pow(_value * 0.5 + 0.5, _power);
            }

            struct appdata{
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f{
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            v2f vert(appdata v){
                v2f o;
                UNITY_INITIALIZE_OUTPUT(v2f, o);

                float phase = sin((_Time + _Offset) * 100.0 * _Speed);

                float x = v.uv.x - 0.5;
                float y = v.uv.y - 0.5;
                float angle = atan2(y, x);

                float diff = fbm(float2(x, y));

                o.position = UnityObjectToClipPos(v.vertex) + float4(x * _Amount * diff, y * _Amount * diff, 0, 0);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET{
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= i.color;
                return col;
            }

            ENDCG
        }
    }
}
