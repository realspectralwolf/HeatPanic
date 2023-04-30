Shader "Custom/ScreenBurn" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _NoiseTex("Noise Texture", 2D) = "gray" {}
        _BurnSpeed("Burn Speed", Range(0, 1)) = 0.5
        _BurnAmount("Burn Amount", Range(0, 1)) = 1.0
    }

        SubShader{
            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                sampler2D _MainTex;
                sampler2D _NoiseTex;
                float _BurnSpeed;
                float _BurnAmount;

                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                float4 frag(v2f i) : SV_Target {
                    float2 noise = tex2D(_NoiseTex, i.uv).xy * 2.0 - 1.0;
                    float4 col = tex2D(_MainTex, i.uv);
                    float burn = pow(noise.x * _BurnAmount, 2.0) * _BurnSpeed;
                    col.rgb += burn;
                    return col;
                }
                ENDCG
            }
        }
}