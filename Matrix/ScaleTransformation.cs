using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTransformation : Transformation
{
	public Vector3 scale;
	//public float scalSpeedx;
	//public float scalSpeedy;
	//public float scalSpeedz;
	//public bool startscalx = true;
	//public bool startscaly = true;
	//public bool startscalz = true;

	public override Vector3 Apply(Vector3 point)
	{
		point.x *= scale.x;
		point.y *= scale.y;
		point.z *= scale.z;
		return point;
	}

	public override Matrix4x4 Matrix
	{
		get
		{
			Matrix4x4 matrix = new Matrix4x4();
			matrix.SetRow(0, new Vector4(scale.x, 0f, 0f, 0f));
			matrix.SetRow(1, new Vector4(0f, scale.y, 0f, 0f));
			matrix.SetRow(2, new Vector4(0f, 0f, scale.z, 0f));
			matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
			return matrix;
		}
	}

	#region 时间缩放控制
	//private void Update()
	//{
	//	if (scale.x >= -1f && startscalx)
	//	{
	//		scale.x -= Time.deltaTime * scalSpeedx;
	//		if (scale.x <= -1f)
	//		{
	//			startscalx = false;
	//		}
	//	}
	//	else if (scale.y <= 1f && !startscalx)
	//	{
	//		scale.x += Time.deltaTime * scalSpeedx;
	//		if (scale.x >= 1f)
	//		{
	//			startscalx = true;
	//		}
	//	}
	//	if (scale.y >= -1f && startscaly)
	//	{
	//		scale.y -= Time.deltaTime * scalSpeedy;
	//		if (scale.y <= -1f)
	//		{
	//			startscaly = false;
	//		}
	//	}
	//	else if (scale.y <= 1f && !startscaly)
	//	{
	//		scale.y += Time.deltaTime * scalSpeedy;
	//		if (scale.y >= 1f)
	//		{
	//			startscaly = true;
	//		}
	//	}
	//	if (scale.z >= -1f && startscalz)//scale.z >=1f||
	//	{
	//		scale.z -= Time.deltaTime * scalSpeedz;
	//		if (scale.z <= -1f)
	//		{
	//			startscalz = false;
	//		}
	//	}
	//	else if (scale.z <= 1f && !startscalz)// scale.z <= -1f ||
	//	{
	//		scale.z += Time.deltaTime * scalSpeedz;
	//		if (scale.z >= 1f)
	//		{
	//			startscalz = true;
	//		}
	//	}
	//}
	#endregion
}
