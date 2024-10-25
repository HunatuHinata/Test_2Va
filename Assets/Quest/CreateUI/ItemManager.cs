using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemManager : MonoBehaviour
{
	[SerializeField] QuestSO m_questSO;
	[SerializeField] GameObject m_searchObject;
	[SerializeField] GameObject m_itemPrefab;
	[SerializeField] GameObject m_parent;

	public List<GameObject> HIT_ITEMS { get; private set; } 

	List<GameObject> m_items = new List<GameObject>();
	string m_search;

    void Start()
    {
		m_search = string.Empty;
		m_searchObject.GetComponent<TextMeshProUGUI>().text = string.Empty;
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
	}

	void ItemSearch()
	{
		TextMeshProUGUI tmpUGUI_Input = m_searchObject.GetComponent<TextMeshProUGUI>();

		//同じワードの場合
		string work = string.Empty;
		for (int i = 0; i < tmpUGUI_Input.text.Length - 1; i++) work += tmpUGUI_Input.text[i];
		if (m_search == work) return;
		m_search = work;

		//検索ワードに一文字でもヒットしなければ非表示
		foreach (GameObject item in m_items) item.SetActive(true);

		if (m_search == string.Empty) return;
		List<GameObject> items = m_items.Where(o => o.name.IndexOf(m_search) < 0).ToList();
		foreach (GameObject item in items) item.SetActive(false);
	}

	public void Delete()
	{
		
	}
}
