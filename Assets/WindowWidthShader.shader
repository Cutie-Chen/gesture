Shader "Custom/WindowWidthShader"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" { }
        _WindowWidth("Window Width", Range(1, 2000)) = 300
        _WindowCenter("Window Center", Range(-1000, 1000)) = 0
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float _WindowWidth;
                float _WindowCenter;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                // CT窗宽的效果
                half4 frag(v2f i) : SV_Target
                {
                    half4 texColor = tex2D(_MainTex, i.uv);
                    float ctValue = texColor.r * 255.0f; // 假设红色通道表示CT值

                    // 根据窗宽和窗中心调整CT值
                    float windowedValue = (ctValue - _WindowCenter) / _WindowWidth;

                    // 调整亮度和对比度
                    windowedValue = saturate(windowedValue); // 保证值在0和1之间
                    return half4(windowedValue, windowedValue, windowedValue, 1.0f);
                }
                ENDCG
            }
        }
}
