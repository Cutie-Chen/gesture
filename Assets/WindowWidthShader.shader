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

                // CT�����Ч��
                half4 frag(v2f i) : SV_Target
                {
                    half4 texColor = tex2D(_MainTex, i.uv);
                    float ctValue = texColor.r * 255.0f; // �����ɫͨ����ʾCTֵ

                    // ���ݴ���ʹ����ĵ���CTֵ
                    float windowedValue = (ctValue - _WindowCenter) / _WindowWidth;

                    // �������ȺͶԱȶ�
                    windowedValue = saturate(windowedValue); // ��ֵ֤��0��1֮��
                    return half4(windowedValue, windowedValue, windowedValue, 1.0f);
                }
                ENDCG
            }
        }
}
