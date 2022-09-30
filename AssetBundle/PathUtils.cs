using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PathUtils
{
	/// <summary>
	/// ����һ������·����������Դ��assetbundle name
	/// </summary>
	/// <param name="path"></param>
	/// <param name="root"></param>
	/// <returns></returns>
	public static string GetAssetBundleNameWithPath(string path, string root)
	{
		string str = NormalizePath(path);
		str = ReplaceFirst(str, root + "/", "");
		return str;
	}

	/// <summary>
	/// ��ȡ�ļ��е������ļ����������ļ��� ������.meta�ļ�
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static FileInfo[] GetFiles(string path)
	{
		DirectoryInfo folder = new DirectoryInfo(path);

		DirectoryInfo[] subFolders = folder.GetDirectories();
		List<FileInfo> filesList = new List<FileInfo>();

		foreach (DirectoryInfo subFolder in subFolders)
		{
			filesList.AddRange(GetFiles(subFolder.FullName));
		}

		FileInfo[] files = folder.GetFiles();
		foreach (FileInfo file in files)
		{
			if (file.Extension != ".meta")
			{
				filesList.Add(file);
			}
		}
		return filesList.ToArray();
	}

	/// <summary>
	/// ��ȡ�ļ��е������ļ�·�����������ļ��� ������.meta�ļ�
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static string[] GetFilesPath(string path)
	{
		DirectoryInfo folder = new DirectoryInfo(path);
		DirectoryInfo[] subFolders = folder.GetDirectories();
		List<string> filesList = new List<string>();

		foreach (DirectoryInfo subFolder in subFolders)
		{
			filesList.AddRange(GetFilesPath(subFolder.FullName));
		}

		FileInfo[] files = folder.GetFiles();
		foreach (FileInfo file in files)
		{
			if (file.Extension != ".meta")
			{
				filesList.Add(NormalizePath(file.FullName));
			}
		}
		return filesList.ToArray();
	}

	/// <summary>
	/// �����ļ�Ŀ¼ǰ���ļ��У���֤�����ļ���ʱ�򲻻�����ļ��в����ڵ����
	/// </summary>
	/// <param name="path"></param>
	public static void CreateFolderByFildePath(string path)
	{
		FileInfo file = new FileInfo(path);
		DirectoryInfo dir = file.Directory;
		if (!dir.Exists)
		{
			dir.Create();
		}
	}

	/// <summary>
	/// �����ļ���
	/// </summary>
	/// <param name="path"></param>
	public static void CreateFolder(string path)
	{
		DirectoryInfo dir = new DirectoryInfo(path);
		if (!dir.Exists)
		{
			dir.Create();
		}
	}

	/// <summary>
	/// ������·��ת�ɹ����ռ��ڵ����·��
	/// </summary>
	/// <param name="fullPath"></param>
	/// <param name="root"></param>
	/// <returns></returns>
	public static string GetRelativePath(string fullPath, string root)
	{
		string path = NormalizePath(fullPath);
		//path = path.Replace(Application.dataPath,"Assets");
		path = ReplaceFirst(path, root, "Assets");
		return path;
	}

	public static string GetAbsolutePath(string relatvePath, string root)
	{
		string path = NormalizePath(relatvePath);
		//path = Application.dataPath.Replace("Assets","") + path;
		path = ReplaceFirst(root, "Assets", "") + path;
		return path;
	}

	/// <summary>
	/// �滻����һ��������ָ���ַ���
	/// </summary>
	/// <param name="str"></param>
	/// <param name="oldValue"></param>
	/// <param name="newValue"></param>
	/// <returns></returns>
	public static string ReplaceFirst(string str, string oldValue, string newValue)
	{
		int i = str.IndexOf(oldValue);
		str = str.Remove(i, oldValue.Length);
		str = str.Insert(i, newValue);
		return str;
	}

	/// <summary>
	/// �淶��·������ ����·���е�����б��
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static string NormalizePath(string path)
	{
		return path.Replace(@"\", "/");
	}
}
