using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Ê±¼ä×¢²á
public class EventTest : MonoBehaviour
{
    private UnityAction someListener;
	private void Awake()
	{
		someListener = new UnityAction(SomeFunction);
	}

	private void OnEnable()
	{
		EventManager.GetInstance.StartListening("test", someListener);
		EventManager.GetInstance.StartListening("Spawn", SomeOtherFunction);
		EventManager.GetInstance.StartListening("Destory", SomeThirdFunction);
	}

	private void OnDisable()
	{
		EventManager.GetInstance.StopListening("test", someListener);
		EventManager.GetInstance.StopListening("Spawn", SomeOtherFunction);
		EventManager.GetInstance.StopListening("Destory", SomeThirdFunction);
	}

	private void SomeFunction()
	{
		Debug.Log("Some Function was called!");
	}

	private void SomeOtherFunction()
	{
		Debug.Log("Some Other Function was called!");
	}

	private void SomeThirdFunction()
	{
		Debug.Log("Some Third Function was called");
	}
}
