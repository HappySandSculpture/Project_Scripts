using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��¼Hierarchy���ڵĲ㼶��ϵ
/// </summary>
public class DFSTest : MonoBehaviour
{
    public Transform parent;
    private Dictionary<Transform, Transform> parentDic = new Dictionary<Transform, Transform>();
	private void Start()
	{
		DetachAllChildren(parent);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			DetachAllChildren(parent, true);
		}

		if (Input.GetKeyDown(KeyCode.B))
		{
			GetBack();
		}
	}

	/// <summary>
	/// ���游����״�ṹ��Ϣ
	/// </summary>
	/// <param name="transform">������</param>
	/// <param name="isDetach">�ָ���״�ṹ</param>
	void DetachAllChildren(Transform transform,bool isDetach=false)
	{
		if (transform.childCount==0)
		{
			if (!isDetach)
			{
				parentDic[transform] = transform.transform.parent;
			}
			return;
		}

		for (int i = 0; i < transform.childCount; i++)
		{
			if (!isDetach)
			{
				parentDic[transform] = transform.parent;
			}
			DetachAllChildren(transform.GetChild(i), isDetach);
		}

		if (isDetach)
		{
			transform.DetachChildren();
		}
	}

	void GetBack()
	{
		foreach (Transform item in parentDic.Keys)
		{
			item.SetParent(parentDic[item]);
		}
	}
}
