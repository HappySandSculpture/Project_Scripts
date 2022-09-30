using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IAssetLoader
{
    //异步加载
    IEnumerator LoadAssetAsync<T>(string path,UnityAction<T> callback) where T : class;
    //同步加载
    T LoadAsset<T>(string path)where T : class;
}
