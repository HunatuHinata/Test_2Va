using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemView : MonoBehaviour
{
	const float OVER_ALPHA = 50.0f;

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
		ActiveObjectChanges();
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
		if (!m_bOpen) return;

	}

	//Foldout�̕\���ύX
	void FoldoutChanges()
	{
		//�q�I�u�W�F�N�g�̎擾(�C���f�b�N�X[�ォ��])
		Transform parentFoldout = transform.GetChild(1);

		for (int i = 0; i < parentFoldout.childCount; i++)
		{
			RectTransform rectTransform = parentFoldout.GetChild(i).GetComponent<RectTransform>();
			rectTransform.anchoredPosition = m_bOpen ? m_openFoldouts[i] : Vector2.zero;
		}
	}

	//Item�̕\���T�C�Y��ύX
	void BackScaleChange()
	{
		//�w�i�̏c�T�C�Y�ύX
		RectTransform rectTra = GetComponent<RectTransform>();
		RectTransform effectRectTra = transform.Find("Effect").GetComponent<RectTransform>();

		float y = (m_bOpen ? m_openSize : m_closeSize);

		Vector2 scale = rectTra.sizeDelta;
		scale.y = y;
		rectTra.sizeDelta = scale;

		scale = effectRectTra.sizeDelta;
		scale.y = y;
		rectTra.sizeDelta = scale;
	}

	//HideObjects�̕\���E��\��
	void ActiveObjectChanges()
	{
		foreach (GameObject item in m_lstHideObjects)
			item.SetActive(m_bOpen);
	}

	//Foldout���J���Ă��邩�ǂ���
	bool IsFoldoutOpen()
	{
		float operatorY = (m_bOpen) ? m_openSize : m_closeSize;
		float objectY = transform.GetComponent<RectTransform>().sizeDelta.y;
		return objectY == operatorY;
	}
}
