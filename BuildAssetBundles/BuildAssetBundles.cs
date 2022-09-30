using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildAssetBundles : MonoBehaviour
{
#if UNITY_ANDROID
	static BuildTarget buildTarget=BuildTarget.Android;
#elif UNITY_IPHONE
	static BuildTarget buildTarget=BuildTarget.iOS;
#else
	static BuildTarget buildTarget = BuildTarget.StandaloneWindows;
#endif


	static string buildRootPath = GameConfigs.GameResPath;//��Ҫ�������Դ�ļ���
	static string exportPath = GameConfigs.GameResExporPath;//assetbundle���·��

	[@MenuItem("AssetBuild/����AB��")]
	public static void BuildWinAsset()
	{
		Debug.Log("��ʼ��������·��:" + exportPath);
		AssetDatabase.SaveAssets();
		AssetBundleBuild[] buildMap = GetBuildFileList(buildRootPath);
		PathUtils.CreateFolder(exportPath);
		BuildPipeline.BuildAssetBundles(exportPath, buildMap, BuildAssetBundleOptions.DeterministicAssetBundle, buildTarget);
		Debug.Log("������");
	}

	private static AssetBundleBuild[] GetBuildFileList(string buildRootPath)
	{
		//��ȡ���й̶�������ļ�
		FileInfo[] files = PathUtils.GetFiles(buildRootPath);
		//List<string> fixedPaths = new List<string>();
		List<AssetBundleBuild> buildMap = new List<AssetBundleBuild>();
		for (int i = 0; i < files.Length; i++)
		{
			//��ȡ�����assetĿ¼�����·��
			string path = PathUtils.GetRelativePath(files[i].FullName, Application.dataPath);

			AssetBundleBuild build = new AssetBundleBuild();
			build.assetBundleName = GetAssetBundleNameWithPath(path);
			build.assetNames = new string[1] { path };
			buildMap.Add(build);
		}
		Debug.Log("��ȡ���д����Դ���:" + buildMap.Count);
		return buildMap.ToArray();
	}

	/// <summary>
	/// ��ȡ������Ҫ�������Դ ����������Դ �˷�������й�ͬ������Դ��ȡ�������д�� ��ʹ�����Դ���ڴ�����ļ�����
	/// </summary>
	/// <param name="buildRoot"></param>
	/// <returns></returns>
	static AssetBundleBuild[] GetBuildFileListWithDepend(string buildRoot)
	{
		//��ȡ���й̶�������ļ�
		FileInfo[] files = PathUtils.GetFiles(buildRoot);
		List<string> fixedPaths = new List<string>();//�̶������Դ���ض����������Դ��

		for (int i = 0; i < files.Length; i++)
		{
			//��ȡ�����assetĿ¼�����·��
			fixedPaths.Add(PathUtils.GetRelativePath(files[i].FullName, Application.dataPath));

		}
		//�ҳ��̶�������ļ��Լ������������ļ� ����
		string[] allDependencies = AssetDatabase.GetDependencies(fixedPaths.ToArray(), true);//���е�������ϵ�������Լ���������

		//�ҳ��̶�����ļ��������ļ��ĵ�һ��������ϵ��������������
		Dictionary<string, int> dependenciesCount = new Dictionary<string, int>();//�����������ֵ�
		foreach (string path in allDependencies)
		{
			if (Path.GetExtension(path) == ".spriteatlas")
			{
				continue;
			}
			string[] dependencie = AssetDatabase.GetDependencies(path, false);
			foreach (string item in dependencie)
			{
				if (!dependenciesCount.ContainsKey(item))
				{
					dependenciesCount.Add(item, 1);
				}
				else
				{
					dependenciesCount[item]++;
				}
			}
		}
		//������������1����Դ��ȡ�������޳����̶��������Դ
		List<AssetBundleBuild> buildMap = new List<AssetBundleBuild>();
		List<string> dependenciesPaths = new List<string>();//�����ô�������1��������Դ ��Ҫ���д��
		foreach (string Key in dependenciesCount.Keys)
		{
			int count = dependenciesCount[Key];
			if (count > 1 && !isFixedBuildAsset(Key))
			{
				dependenciesPaths.Add(Key);
			}
		}
		//�ϲ��̶������Դ�����������Դ
		List<string> allBuildPaths = new List<string>(fixedPaths);
		allBuildPaths.AddRange(dependenciesPaths);

		foreach (string path in allBuildPaths)
		{
			//ȥ���ű���Դ
			if (Path.GetExtension(path) == ".cs")
			{
				continue;
			}

			AssetBundleBuild build = new AssetBundleBuild();
			build.assetBundleName = GetAssetBundleNameWithPath(path);
			build.assetNames = new string[1] { path };
			buildMap.Add(build);
			Debug.Log(build.assetBundleName + "|" + build.assetNames[0]);
		}

		return buildMap.ToArray();
	}

	/// <summary>
	/// �ж��ǲ��ǹ̶������Դ
	/// </summary>
	/// <param name="path">���·��</param>
	/// <returns></returns>
	private static bool isFixedBuildAsset(string path)
	{
		if (path.IndexOf(PathUtils.GetRelativePath(buildRootPath,Application.dataPath))==-1)
		{
			return false;
		}
		return true;
	}

	/// <summary>
	/// ͨ����Ե�ַ��ȡassetbundle������
	/// </summary>
	/// <param name="path">��Ե�ַ</param>
	/// <returns></returns>
	private static string GetAssetBundleNameWithPath(string path)
	{
		string paths = Path.GetDirectoryName(path) + "/" + Path.GetFileNameWithoutExtension(path);
		//�ж���������Դ���ǹ̶���Դ
		if (!isFixedBuildAsset(paths))
		{
			paths = PathUtils.ReplaceFirst(paths, "Assets", "Dependencie");
			//paths=paths.Replace("Assets","Dependencie");
		}
		else
		{
			paths = PathUtils.ReplaceFirst(paths, PathUtils.GetRelativePath(buildRootPath, Application.dataPath) + "/", "");
			//paths=paths.Replace(buildRoot+"/","");
		}
		return paths;
	}
}
