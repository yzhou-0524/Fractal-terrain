Shader "Note/Chapter6-DiffuseVertex"
{
	//逐顶点的漫反射
	Properties{
		_Diffuse("Diffuse",Color) = (1,1,1,1)
	}

		SubShader{
			Pass{
				Tags {"LightMode" = "ForwardBase"}

				CGPROGRAM
	            #pragma vertex vert
	            #pragma fragment frag
				//包含头文件
	            #include "Lighting.cginc"
			    fixed4 _Diffuse;

	              struct a2v
	             {
		           //取到模型顶点坐标
				   float4 vertex : POSITION;
				   //取到法线向量
		           float3 normal: NORMAL;

	              };

	              struct v2f {
					  //输出到裁剪空间坐标的值
				   float4 pos : SV_POSITION;
				   //作为储存的变量
		           fixed3 color : COLOR;
	               };

	              v2f vert(a2v i)
	               {
	            	//rgba对应的是xwyz
					v2f o;
					//UnityObjectToClipPos把顶点坐标转化为裁剪空间坐标
	            	o.pos = UnityObjectToClipPos(i.vertex);

					//取到场景环境光
					fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT;
					//坐标变换 把模型空间坐标下的法线向量变换为世界空间坐标下的向量
					//unity_WorldToObject float4*4 的矩阵，用于空间坐标向世界空间坐标的变化 即：MVP变换中的M矩阵
					fixed3 worldNormal = normalize ( mul(i.normal, (float3x3)unity_WorldToObject));
					//得到世界空间坐标下的光照方向
					fixed3 worldLight = normalize(_WorldSpaceLightPos0.xyz); 

					fixed3 diffuse = _LightColor0.rgb * _Diffuse.rgb * saturate(dot(worldNormal, worldLight));
		            o.color = diffuse + ambient; //normalize(i.normal);
					return o;
	                }

				  fixed4 frag(v2f i) :SV_TARGET
				  {
				   return fixed4(i.color,1);
					   
					   //return _Diffuse;
	                }

		            ENDCG
             }
	     }
}