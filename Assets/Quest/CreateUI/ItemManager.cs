using System;
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
	string m_search;

    void Start()
    {
		m_selectObject = null;
		m_itemCreat.SetQuestListRef(ref m_questSO);
		m_search = string.Empty;
		//m_searchObject.GetComponent<TextMeshProUGUI>().text = string.Empty;
		m_searchObject.GetComponent<TMP_InputField>().text = string.Empty;
		foreach (Quest quest in m_questSO.quests)
		{
			GameObject itemObject = GameObject.Instantiate(m_itemPrefab, m_parent.transform);
			itemObject.GetComponent<ItemView>().Creat(quest);
			m_items.Add(itemObject);
		}
	}

	void Update()
    {
		ItemSearch();
		ItemSelect();
	}

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

	void ItemSelect()
	{
		if (Input.GetMouseButton(0))
		{
			// TryCatch文でNull回避
			try
			{
				GameObject hitObject = m_eventSystem.currentSelectedGameObject.gameObject;
				Debug.Log(hitObject.name);
			}
			// 例外処理的なやつ
			catch (NullReferenceException ex)
			{
				// なにも選択されない場合に
				m_selectObject = null;
			}
		}
	}

	public void OnQuestButton()
	{
		//if ()
		//{
		//	m_itemCreat.AddQuest();
		//}
	}

	public void Delete()
	{
		//int result1 = list.FindIndex(n => n == "C");
	}
}
