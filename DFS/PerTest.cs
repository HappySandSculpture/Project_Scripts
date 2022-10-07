using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerTest : MonoBehaviour
{
    public Transform parentTrans;
    public List<ListData> listDatas;
    public Dictionary<Transform,Transform> perTran = new Dictionary<Transform,Transform>();
    // Start is called before the first frame update
    void Start()
    {
        ConParent();
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Q))
		{
            Test();

        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Test2();
        }
    }
    void ConParent()
    {
		foreach (var item in listDatas)
		{
            perTran[item.listDataObj.transform] = item.listDataObj.transform.parent;

        }
    }
    void Test()
    {
		foreach (var item in listDatas)
		{
            item.listDataObj.transform.parent = parentTrans;
		}
    }
    void Test2()
    {
		foreach (var item in listDatas)
		{
            item.listDataObj.transform.parent = perTran[item.listDataObj.transform];

        }
    }
}
