using FoxGame.Asset;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadTest : MonoBehaviour
{
    public Transform modelTransform;
	public Button btn_Synchronous;
	public Button btn_Async;

	private void Start()
	{
		AssetManager.Instance.InitMode(GameConfigs.LoadAssetMode);

		Debug.Log("资源管理器加载模式:" + GameConfigs.LoadAssetMode);

		btn_Synchronous.onClick.AddListener(OnClickedBtnSynchronous);
		btn_Async.onClick.AddListener(OnClickedBtnAsync);
	}

	void OnClickedBtnSynchronous()
	{
		GameObject HC_SB = AssetManager.Instance.LoadAsset<GameObject>(GameConfigs.GetModelAtlasPath("hc_sb"));
		GameObject HC_SBs = GameObject.Instantiate(HC_SB);
		HC_SBs.transform.parent = modelTransform.transform;
	}

	void OnClickedBtnAsync()
	{
		AssetManager.Instance.LoadAssetAsync<GameObject>(GameConfigs.GetModelAtlasPath("hc_sb"),(GameObject HC_SB)=>
		{
			GameObject.Instantiate(HC_SB);
		}	
		);
	}
}
