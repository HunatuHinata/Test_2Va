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
		if (Input.GetMouseButtonDown(0))
			ItemSelect();
	}

	void ItemSearch()
	{
#if false //TextMeshProUGUI�ł̍쐬���@�i�������߂�ǂ������j
		TextMeshProUGUI tmp_Input = m_searchObject.GetComponent<TextMeshProUGUI>();
		string work = string.Empty;
		for (int i = 0; i < tmp_Input.text.Length - 1; i++) work += tmp_Input.text[i];

#else	//TMP_InputField���g�p������@�i�������̕����ȒP�j
		TMP_InputField tmp_Input = m_searchObject.GetComponent<TMP_InputField>();
		string work = tmp_Input.text;
#endif
		//�������[�h�̏ꍇ
		if (m_search == work) return;
		m_search = work;

		//�������[�h�Ɉꕶ���ł��q�b�g���Ȃ���Δ�\��
		foreach (GameObject item in m_items) item.SetActive(true);

		if (m_search == string.Empty) return;
		List<GameObject> items = m_items.Where(o => o.name.IndexOf(m_search) < 0).ToList();
		foreach (GameObject item in items) item.SetActive(false);
	}

	void ItemSelect()
	{
		PointerEventData ped = new PointerEventData(EventSystem.current);
		ped.position = Input.mousePosition;
		List<RaycastResult> result = new List<RaycastResult>();
		EventSystem.current.RaycastAll(ped, result);
		
		if (!result.Any(o => o.gameObject.CompareTag("ItemUI")))
		{
			//m_selectObject.transform.GetComponent<>();
			m_selectObject = null;
			return;
		}

		RaycastResult itemObject = result.Find(o => o.gameObject.CompareTag("ItemUI"));
		m_selectObject = itemObject.gameObject;
		//if (result.Any(o => o.gameObject.CompareTag("FoldoutUI"))) ;

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
