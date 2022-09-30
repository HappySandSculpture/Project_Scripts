using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IAssetLoader
{
    //�첽����
    IEnumerator LoadAssetAsync<T>(string path,UnityAction<T> callback) where T : class;
    //ͬ������
    T LoadAsset<T>(string path)where T : class;
}
