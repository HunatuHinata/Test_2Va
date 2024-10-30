using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
	[SerializeField] QuestSO m_questSO;
	[SerializeField] GameObject m_searchObject;
	[SerializeField] GameObject m_itemPrefab;
	[SerializeField] GameObject m_parent;
	[SerializeField] ItemCreat m_itemCreat;
	[SerializeField] EventSystem m_eventSystem;

	List<GameObject> m_items = new List<GameObject>();
	GameObject m_selectObject;
	GameObject m_targetObject;
	string m_search;
	bool m_bAddMode;

    void Start()
    {
		m_bAddMode = true;
		m_selectObject = null;
		m_itemCreat.SetQuestListRef(ref m_questSO);
		m_search = string.Empty;
		//m_searchObject.GetComponent<TextMeshProUGUI>().text = string.Empty;
		m_searchObject.GetComponent<TMP_InputField>().text = string.Empty;
		foreach (Quest quest in m_questSO.quests)
			GeneratItem(quest);
	}

	//Itemを生成
	void GeneratItem(Quest quest)
	{
        GameObject itemObject = GameObject.Instantiate(m_itemPrefab, m_parent.transform);
        itemObject.GetComponent<ItemView>().Creat(quest);
        m_items.Add(itemObject);
    }

	void Update()
    {
		ItemSearch();
		if (Input.GetMouseButtonDown(0))
			ItemSelect();
	}

	//Itemを検索
	void ItemSearch()
	{
#if false //TextMeshProUGUIでの作成方法（こっちめんどくさい）
		TextMeshProUGUI tmp_Input = m_searchObject.GetComponent<TextMeshProUGUI>();
		string work = string.Empty;
		for (int i = 0; i < tmp_Input.text.Length - 1; i++) work += tmp_Input.text[i];

#else	//TMP_InputFieldを使用する方法（こっちの方が簡単）
		TMP_InputField tmp_Input = m_searchObject.GetComponent<TMP_InputField>();
		string work = tmp_Input.text;
#endif
		//同じワードの場合
		if (m_search == work) return;
		m_search = work;

		//検索ワードに一文字でもヒットしなければ非表示
		foreach (GameObject item in m_items) item.SetActive(true);

		if (m_search == string.Empty) return;
		List<GameObject> items = m_items.Where(o => o.name.IndexOf(m_search) < 0).ToList();
		foreach (GameObject item in items) item.SetActive(false);
	}

	//Itemを選択
	void ItemSelect()
	{
		PointerEventData ped = new PointerEventData(EventSystem.current);
		ped.position = Input.mousePosition;
		List<RaycastResult> result = new List<RaycastResult>();
		EventSystem.current.RaycastAll(ped, result);

        //ItemUIのタグがない場合
        //前回の選択したItemのハイライトを消す
		m_selectObject?.transform.GetComponent<ItemView>().SetHighlightAnimation(false);
        if (!result.Any(o => o.gameObject.CompareTag("ItemUI")))
		{
			m_selectObject = null;
			return;
		}

		RaycastResult itemObject = result.Find(o => o.gameObject.CompareTag("ItemUI"));
		m_selectObject = itemObject.gameObject;

		m_selectObject?.transform.GetComponent<ItemView>().SetHighlightAnimation(true);

        //FoldoutUIのタグがある場合
        //クエストの詳細を表示する
        if (result.Any(o => o.gameObject.CompareTag("FoldoutUI"))) 
			m_selectObject.transform.GetComponent<ItemView>().ItemDisplaySwitching();
	}

	//クエストボタン
	public void OnQuestButton()
	{
		if (m_bAddMode)
		{
			m_itemCreat.AddQuest();
			GeneratItem();
		}
	}

	public void EditButton()
	{
		Debug.Log("Coming Soon...");
	}

	public void DeleteButton()
	{
		//int result1 = list.FindIndex(n => n == "C");

		Debug.Log("Coming Soon...");
	}
}
