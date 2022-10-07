using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ÊÂ¼þ´¥·¢
public class EventTriggerTest : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetKeyDown("q"))
		{
			EventManager.GetInstance.TriggerEvent("test");
		}
		if (Input.GetKeyDown("o"))
		{
			EventManager.GetInstance.TriggerEvent("Spawn");
		}
		if (Input.GetKeyDown("p"))
		{
			EventManager.GetInstance.TriggerEvent("Destory");
		}
		if (Input.GetKeyDown("x"))
		{
			EventManager.GetInstance.TriggerEvent("Junk");
		}
	}
}
