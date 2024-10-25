using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;
using UnityEngine;
using System.Linq;

public class ItemView : MonoBehaviour
{
	[SerializeField] float m_closeSize;
	[SerializeField] float m_openSize;

	[SerializeField] Vector2[] m_openFoldouts = new Vector2[2];

	[SerializeField] List<GameObject> m_lstHideObjects = new List<GameObject>();

	Quest m_quest;
	bool m_bOpen;

	public void Creat(Quest quest)
	{
		m_bOpen = false;

		Initialize(quest);
	}

	public void Initialize(Quest quest)
	{
		m_quest = quest;

		//クエスト名
		string questName = m_quest.GetQuest().name;
		gameObject.name = questName;
		Transform name = transform.GetChild(0);
		name.transform.GetComponent<TextMeshProUGUI>().text = questName;

		//子オブジェクトを取得
		BackScaleChange();
		ActiveChange();
	}

	void Start()
	{
		//RectTransformの座標を変更（なぜかここでは、使える??）
		for (int i = 0; i < transform.childCount; i++)
		{
			GameObject item = transform.GetChild(i).gameObject;
			item.transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		}
		FoldoutChanges();
	}

	void Update()
	{
		//if (IsFrontOpen()) return;

		
	}

	void FoldoutChanges()
	{
		//foldoutの親
		Transform parentFoldout = transform.GetChild(1);

		for (int i = 0; i < parentFoldout.childCount; i++)
		{
			RectTransform rectTransform = parentFoldout.GetChild(i).GetComponent<RectTransform>();
			rectTransform.anchoredPosition = m_bOpen ? m_openFoldouts[i] : Vector2.zero;
		}
	}

	void BackScaleChange()
	{
		//背景の縦サイズ変更
		RectTransform rectTra = GetComponent<RectTransform>();
		Vector2 scale = rectTra.sizeDelta;
		scale.y = (m_bOpen ? m_openSize : m_closeSize);
		rectTra.sizeDelta = scale;
	}

	void ActiveChange()
	{
		foreach (GameObject item in m_lstHideObjects) item.SetActive(m_bOpen);
	}

	bool IsFrontOpen()
	{
		float operatorY = (m_bOpen) ? m_openSize : m_closeSize;
		float objectY = transform.GetComponent<RectTransform>().sizeDelta.y;
		return objectY == operatorY;
	}
}
