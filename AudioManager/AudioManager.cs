using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	[Serializable]
	public class Sound
	{
		[Header("��Ƶ����")]
		public AudioClip clip;

		[Header("��Ƶ����")]
		public AudioMixerGroup outputGroup;

		[Header("��Ƶ����")]
		[Range(0, 1)]
		public float volume;

		[Header("��Ƶ�Ƿ�������")]
		public bool PlayOnAwake;

		[Header("��Ƶ�Ƿ�Ҫѭ������")]
		public bool loop;
	}

	public List<Sound> sounds;//�洢������Ƶ����Ϣ

	private Dictionary<string, AudioSource> audioDic;//û�и���Ƶ���������

	private static AudioManager instance;

	public static AudioManager Instance { get => instance; set => instance = value; }


	private void Awake()
	{
		audioDic = new Dictionary<string, AudioSource>();
		instance = this;
	}

	private void Start()
	{
		foreach (var sound in sounds)
		{
			GameObject obj = new GameObject(sound.clip.name);
			obj.transform.SetParent(transform);

			AudioSource source = obj.AddComponent<AudioSource>();
			source.clip = sound.clip;
			source.volume = sound.volume;
			source.playOnAwake = sound.PlayOnAwake;
			source.loop = sound.loop;
			source.outputAudioMixerGroup = sound.outputGroup;

			if (sound.PlayOnAwake)
			{
				source.Play();
			}
			audioDic.Add(sound.clip.name, source);
		}
	}

	//����ĳ����Ƶ�ķ��� isWaitΪ�Ƿ�ȴ�
	public static void PlayAudio(string name, bool isWait = false)
	{
		if (!instance.audioDic.ContainsKey(name))
		{
			//�����ڴ���Ƶ
			Debug.LogError("������" + name + "��Ƶ");
			return;
		}
		if (isWait)
		{
			if (!instance.audioDic[name].isPlaying)
			{
				//����ǵȴ������ ���ٲ���
				instance.audioDic[name].Play();
			}
		}
		else
		{
			//ֱ�Ӳ���
			instance.audioDic[name].Play();
		}
	}

	//ֹͣ��Ƶ�Ĳ���
	public static void StopMute(string name)
	{
		if (!instance.audioDic.ContainsKey(name))
		{
			//�����ڴ���Ƶ
			Debug.LogError("������" + name + "��Ƶ");
			return;
		}
		else
		{
			instance.audioDic[name].Stop();
		}
	}
}
