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

		//�N�G�X�g��
		string questName = m_quest.GetQuest().name;
		gameObject.name = questName;
		Transform name = transform.GetChild(0);
		name.transform.GetComponent<TextMeshProUGUI>().text = questName;

		//�q�I�u�W�F�N�g���擾
		BackScaleChange();
		ActiveChange();
	}

	void Start()
	{
		//RectTransform�̍��W��ύX�i�Ȃ��������ł́A�g����??�j
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
		//foldout�̐e
		Transform parentFoldout = transform.GetChild(1);

		for (int i = 0; i < parentFoldout.childCount; i++)
		{
			RectTransform rectTransform = parentFoldout.GetChild(i).GetComponent<RectTransform>();
			rectTransform.anchoredPosition = m_bOpen ? m_openFoldouts[i] : Vector2.zero;
		}
	}

	void BackScaleChange()
	{
		//�w�i�̏c�T�C�Y�ύX
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
