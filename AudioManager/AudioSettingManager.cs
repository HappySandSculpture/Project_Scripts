using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingManager : MonoBehaviour
{
	[Header("ÒôÆµ»ìÏìÆ÷")]
	public AudioMixer mixer;//ÒôÆµ»ìÏìÆ÷

	public void SetBGMbolume(float Value)
	{
		//µ÷ÕûBGMµÄÒôÁ¿
		mixer.SetFloat("BGM", Value);
	}
}
