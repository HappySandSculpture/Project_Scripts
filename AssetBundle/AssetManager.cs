using FoxGame.Asset;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace FoxGame.Asset
{
	//��Դ������(��Դ������ �������Դ�������ڴ�)
	public class AssetManager : MonoSingleton<AssetManager>
	{
		[Header("��ǰ��Դ����ģʽ��")]
		public AssetLoadMode LoadMode = AssetLoadMode.Editor;
		[Header("��ʱ��������(��)��")]
		public float ClearCacheDuration;
		[Header("��������פ���¼�(��)")]
		public float CacheDataStayTime;

		private IAssetLoader editorLoader;
		private IAssetLoader abLoader;
		private float cacheTimeTemp;

		//������[key Ϊ����·��]
		private Dictionary<string, CacheDataInfo> cacheDataDic = new Dictionary<string, CacheDataInfo>();

		public void InitMode(AssetLoadMode mode, float duration = 10f, float cacheStayTime = 9f)
		{
			Debug.LogFormat("[AssetManager]��ʼ�� ��ǰ����ģʽ��{0} ��ʱ����������{1}s", mode, duration);
			LoadMode = mode;
			ClearCacheDuration = duration;
			editorLoader = new EditorAssetLoader();
			abLoader = new AssetBundleLoader(GameConfigs.StreamingAssetABRootPath, GameConfigs.StreamingAssetManifestPath);
		}

		//ͬ������
		public T LoadAsset<T>(string path) where T : Object
		{
			CacheDataInfo info = queryCache(path);
			if (info != null)
			{
				info.UpdateTick();
				return info.CacheObj as T;
			}
			else
			{
				switch (LoadMode)
				{
					case AssetLoadMode.Editor:
						return editorLoader.LoadAsset<T>(path);
					case AssetLoadMode.AssetBundler:
						return abLoader.LoadAsset<T>(path);
				}
				return null;
			}
		}

		//�첽����
		public void LoadAssetAsync<T>(string path, UnityAction<T> onLoadComplate) where T : Object
		{
			CacheDataInfo info = queryCache(path);
			if (info != null)
			{
				info.UpdateTick();
				if (onLoadComplate != null)
				{
					onLoadComplate(info.CacheObj as T);
				}
			}
			else
			{
				switch (LoadMode)
				{
					case AssetLoadMode.Editor:
						StartCoroutine(editorLoader.LoadAssetAsync<T>(path, onLoadComplate));
						break;
					case AssetLoadMode.AssetBundler:
						StartCoroutine(abLoader.LoadAssetAsync<T>(path, onLoadComplate));
						break;
				}

			}
		}


		//��⻺����
		private CacheDataInfo queryCache(string path)
		{
			if (cacheDataDic.ContainsKey(path))
			{
				return cacheDataDic[path];
			}
			return null;
		}

		//���뻺����
		public void pushCache(string path, Object obj)
		{
			Debug.Log("[AssetManager]���뻺��:" + path);

			lock (cacheDataDic)
			{
				if (cacheDataDic.ContainsKey(path))
				{
					cacheDataDic[path].UpdateTick();
				}
				else
				{
					CacheDataInfo info = new CacheDataInfo(path, obj);
					cacheDataDic.Add(path, info);
					info.UpdateTick();
				}
			}
		}


		//��ջ�����
		public void RemoveCache()
		{
			cacheDataDic.Clear();
		}

		//��������
		private void updateCache()
		{
			Debug.Log("[AssetManager]������");
			foreach (var item in cacheDataDic.ToList())
			{
				if (item.Value.StartTick+CacheDataStayTime>=Time.realtimeSinceStartup)
				{
					Debug.Log("��������" + item.Value.CacheName);
					cacheDataDic.Remove(item.Key);
				}
			}
		}


		private void Update()
		{
			if (ClearCacheDuration < 0) return;

			cacheTimeTemp += Time.deltaTime;

			if (cacheTimeTemp >= ClearCacheDuration)
			{
				updateCache();
				cacheTimeTemp -= ClearCacheDuration;
			}
		}
	}
}