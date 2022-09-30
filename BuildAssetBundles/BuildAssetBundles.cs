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


	static string buildRootPath = GameConfigs.GameResPath;//需要打包的资源文件夹
	static string exportPath = GameConfigs.GameResExporPath;//assetbundle输出路径

	[@MenuItem("AssetBuild/生成AB包")]
	public static void BuildWinAsset()
	{
		Debug.Log("开始打包，输出路径:" + exportPath);
		AssetDatabase.SaveAssets();
		AssetBundleBuild[] buildMap = GetBuildFileList(buildRootPath);
		PathUtils.CreateFolder(exportPath);
		BuildPipeline.BuildAssetBundles(exportPath, buildMap, BuildAssetBundleOptions.DeterministicAssetBundle, buildTarget);
		Debug.Log("打包完毕");
	}

	private static AssetBundleBuild[] GetBuildFileList(string buildRootPath)
	{
		//获取所有固定打包的文件
		FileInfo[] files = PathUtils.GetFiles(buildRootPath);
		//List<string> fixedPaths = new List<string>();
		List<AssetBundleBuild> buildMap = new List<AssetBundleBuild>();
		for (int i = 0; i < files.Length; i++)
		{
			//获取相对于asset目录的相对路径
			string path = PathUtils.GetRelativePath(files[i].FullName, Application.dataPath);

			AssetBundleBuild build = new AssetBundleBuild();
			build.assetBundleName = GetAssetBundleNameWithPath(path);
			build.assetNames = new string[1] { path };
			buildMap.Add(build);
		}
		Debug.Log("获取所有打包资源完毕:" + buildMap.Count);
		return buildMap.ToArray();
	}

	/// <summary>
	/// 获取所有需要打包的资源 包括依赖资源 此方法会把有共同依赖资源提取出来进行打包 即使这个资源不在打包的文件夹内
	/// </summary>
	/// <param name="buildRoot"></param>
	/// <returns></returns>
	static AssetBundleBuild[] GetBuildFileListWithDepend(string buildRoot)
	{
		//获取所有固定打包的文件
		FileInfo[] files = PathUtils.GetFiles(buildRoot);
		List<string> fixedPaths = new List<string>();//固定打包资源（必定被打包的资源）

		for (int i = 0; i < files.Length; i++)
		{
			//获取相对于asset目录的相对路径
			fixedPaths.Add(PathUtils.GetRelativePath(files[i].FullName, Application.dataPath));

		}
		//找出固定打包的文件以及其所有依赖文件 排重
		string[] allDependencies = AssetDatabase.GetDependencies(fixedPaths.ToArray(), true);//所有的依赖关系，包括自己并已排重

		//找出固定打包文件和依赖文件的第一层依赖关系并进行依赖计数
		Dictionary<string, int> dependenciesCount = new Dictionary<string, int>();//依赖计数用字典
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
		//将计数器大于1的资源提取出来，剔除掉固定打包的资源
		List<AssetBundleBuild> buildMap = new List<AssetBundleBuild>();
		List<string> dependenciesPaths = new List<string>();//被引用次数大于1的依赖资源 需要进行打包
		foreach (string Key in dependenciesCount.Keys)
		{
			int count = dependenciesCount[Key];
			if (count > 1 && !isFixedBuildAsset(Key))
			{
				dependenciesPaths.Add(Key);
			}
		}
		//合并固定打包资源和依赖打包资源
		List<string> allBuildPaths = new List<string>(fixedPaths);
		allBuildPaths.AddRange(dependenciesPaths);

		foreach (string path in allBuildPaths)
		{
			//去掉脚本资源
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
	/// 判断是不是固定打包资源
	/// </summary>
	/// <param name="path">相对路径</param>
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
	/// 通过相对地址获取assetbundle的名字
	/// </summary>
	/// <param name="path">相对地址</param>
	/// <returns></returns>
	private static string GetAssetBundleNameWithPath(string path)
	{
		string paths = Path.GetDirectoryName(path) + "/" + Path.GetFileNameWithoutExtension(path);
		//判断是依赖资源还是固定资源
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
