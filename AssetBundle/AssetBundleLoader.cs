using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace FoxGame.Asset
{
	//ab��Դ������
	public class AssetBundleLoader : IAssetLoader
	{
		private string assetRootPath;
		private string mainfastPath;
		private static AssetBundleManifest manifest;

		public AssetBundleLoader(string assetPath, string mainfast)
		{
			assetRootPath = assetPath;
			mainfastPath = mainfast;
		}

		public T LoadAsset<T>(string path) where T : class
		{
			string absolutepath = path;

			path = PathUtils.NormalizePath(path);

			Debug.Log("[LoadAsset]:"+path);
			//���ab������Դ���ƺ��ļ�������Сд
			string assetBundleName = PathUtils.GetAssetBundleNameWithPath(path, assetRootPath);

			//����Manifest�ļ�
			LoadManifest();

			//��ȡ�ļ������б�
			string[] dependencies = manifest.GetAllDependencies(assetBundleName);

			//����������Դ
			List<AssetBundle> assetbundlesList = new List<AssetBundle>();
			foreach (string fileName in dependencies)
			{
				string dependencyPath = assetRootPath + "/" + fileName;

				Debug.Log("[AssetBundle]����������Դ��" + dependencyPath);
				assetbundlesList.Add(AssetBundle.LoadFromFile(dependencyPath));
			}

			//����Ŀ����Դ
			AssetBundle assetBundle = null;
			Debug.Log("[AssetBundle]����Ŀ����Դ��" + path);
			assetBundle = AssetBundle.LoadFromFile(path);
			assetbundlesList.Insert(0, assetBundle);

			Object obj = assetBundle.LoadAsset(Path.GetFileNameWithoutExtension(path),typeof(T));

			//�ͷ�������Դ
			UnloadAssetbundle(assetbundlesList);

			//���뻺��
			AssetManager.Instance.pushCache(absolutepath, obj);
			return obj as T;
		}

		public IEnumerator LoadAssetAsync<T>(string path, UnityAction<T> callback) where T : class
		{
			string absolutepath = path;
			path = PathUtils.NormalizePath(path);

			Debug.Log("[LoadAssetAsync]:" + path);
			//���ab������Դ���ƺ��ļ�������Сд��
			string assetBundleName = PathUtils.GetAssetBundleNameWithPath(path, assetRootPath);
			//TODO:����Manifest
			LoadManifest();
			//��ȡ�ļ������б�
			string[] dependencies=manifest.GetAllDependencies(assetBundleName);
			//����������Դ
			AssetBundleCreateRequest createRequest;
			List<AssetBundle> assetbundleList = new List<AssetBundle>();
			foreach (string fileName in dependencies)
			{
				string dependencyPath = assetRootPath + "/" + fileName;

				Debug.Log("[AssetBundle]����������Դ��" + dependencyPath);
				createRequest = AssetBundle.LoadFromFileAsync(dependencyPath);
				yield return createRequest;
				if (createRequest.isDone)
				{
					assetbundleList.Add(createRequest.assetBundle);
				}
				else
				{
					Debug.LogError("[AssetBundle]����������Դ����");
				}
			}
			//����Ŀ����Դ
			AssetBundle assetBundle = null;
			Debug.Log("[AssetBundle]����Ŀ����Դ��" + path);
			createRequest = AssetBundle.LoadFromFileAsync(path);
			yield return createRequest;
			if (createRequest.isDone)
			{
				assetBundle = createRequest.assetBundle;
				assetbundleList.Insert(0, assetBundle);
			}
			AssetBundleRequest abr = assetBundle.LoadAssetAsync(Path.GetFileNameWithoutExtension(path), typeof(T));
			yield return abr;
			Object obj = abr.asset;

			//���뻺��
			AssetManager.Instance.pushCache(absolutepath, obj);

			callback(obj as T);

			//�ͷ�������Դ
			UnloadAssetbundle(assetbundleList);
		}

		//����manifest
		private void LoadManifest()
		{
			if (manifest== null)
			{
				 string path = mainfastPath;
				Debug.Log("[AssetBundle]����manifest��" + path);

				AssetBundle manifestAB = AssetBundle.LoadFromFile(path);
				manifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
				manifestAB.Unload(false);
			}
		}

		private void UnloadAssetbundle(List<AssetBundle> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				list[i].Unload(false);
			}
			list.Clear();
		}
	}
}