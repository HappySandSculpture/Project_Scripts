using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingManager : MonoBehaviour
{
	[Header("��Ƶ������")]
	public AudioMixer mixer;//��Ƶ������

	public void SetBGMbolume(float Value)
	{
		//����BGM������
		mixer.SetFloat("BGM", Value);
	}
}
