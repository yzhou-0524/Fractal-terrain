Shader "Note/Chapter6-DiffuseFrag"
{
	//逐片元/像素的漫反射
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
		           fixed3 worldNormal : COLOR;
	               };

	              v2f vert(a2v i)
	               {
	            	//rgba对应的是xwyz
					v2f o;
					//UnityObjectToClipPos把顶点坐标转化为裁剪空间坐标
	            	o.pos = UnityObjectToClipPos(i.vertex);
					//坐标变换 把模型空间坐标下的法线向量变换为世界空间坐标下的向量
					//unity_WorldToObject float4*4 的矩阵，用于空间坐标向世界空间坐标的变化 即：MVP变换中的M矩阵
					o.worldNormal = normalize ( mul(i.normal, (float3x3)unity_WorldToObject));	

					return o;
	                }

				  fixed4 frag(v2f i) :SV_TARGET
				  {
				   fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
				   fixed3 worldLight = normalize (_WorldSpaceLightPos0.xyz);
				   fixed3 diffuse =ambient + _LightColor0.rgb * _Diffuse.rgb * saturate(0.5 * dot(i.worldNormal, worldLight) + 0.5);//半兰伯特模型，这个0.5乘和加，是使得其后面没有那么黑
				   fixed4 color = fixed4(diffuse,1); //normalize(i.normal);
				   return color;
					   
					   //return _Diffuse;
	                }

		            ENDCG
             }
	     }
}