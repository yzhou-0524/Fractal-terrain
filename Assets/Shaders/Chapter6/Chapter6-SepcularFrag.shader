Shader "Note/Chapter6-SepcularFrag"
{
   //高光片元反射，相对于顶点反射会更加的细腻
   Properties{
       _Diffuse("Diffuse", Color) = (1,1,1,1)
       _Sepcular("Specular",Color) = (1,1,1,1)
       _GLoss("Gloss",Range(8,256)) = 20

   }

   SubShader{
       Pass{

           CGPROGRAM
           #include "Lighting.cginc"
           #pragma vertex vert
           #pragma fragment frag

           fixed4 _Diffuse;
           fixed4 _Sepcular;
           float _GLoss;

           struct a2v{
               float4 vertex: POSITION;
               float3 normal: NORMAL;
           };

           struct v2f{
               float4 pos : SV_POSITION;
               float3 worldPos : TEXCOORD0;
               fixed3 worldNormal:COLOR;
               fixed4 color: COLOR1;
               
           };

           v2f vert(a2v i)
           {
               v2f o;
               //由模型空间坐标变换到裁剪空间坐标
               o.pos = UnityObjectToClipPos(i.vertex);
               o.worldPos = (mul(i.vertex,unity_ObjectToWorld)).xyz;
               o.worldNormal = normalize(mul(i.normal,(float3x3)unity_ObjectToWorld));
               //o.color = fixed4(ambient + diffuse + specular,1);
               return o;
           }

           fixed4 frag(v2f i):SV_TARGET
           {
              //环境光
              fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
               //世界空间的法线向量
               //fixed3 worldNormal = normalize(mul(i.normal,(float3x3)unity_ObjectToWorld));           
               //世界空间的光照向量
               fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
               //半兰伯特光照计算 dot(n,light)*0.5+0.5
               fixed3 diffuse =_LightColor0.rgb * _Diffuse.rgb * saturate(dot(i.worldNormal,worldLightDir)*0.5 + 0.5);             
               //计算反射光方向和向量
               fixed3 relectDir = normalize(reflect(-worldLightDir,i.worldNormal));
               //世界空间坐标系的摄像机坐标
               float3 worldCameraPos = _WorldSpaceCameraPos.xyz;
               //得到世界空间坐标系的顶点坐标
               //float3 worldPos = (mul(i.vertex,unity_ObjectToWorld)).xyz;
               //得到摄像机到顶点的向量
               fixed3 viewDir = normalize (worldCameraPos - i.worldPos);
               //高光计算公式 pow(saturate(dot(v*r)),gloss) 平方
               fixed3 specular = _LightColor0.rgb * _Sepcular.rgb * pow(saturate(dot(relectDir,viewDir)),_GLoss);
              
               return fixed4(ambient + diffuse + specular,1);

           }

           ENDCG
       }
   }
}
