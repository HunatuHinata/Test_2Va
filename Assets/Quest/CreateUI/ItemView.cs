using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
	const float OVER_ALPHA = 50.0f;

	[SerializeField] float m_closeSize;
	[SerializeField] float m_openSize;

	[SerializeField] Vector2[] m_openFoldouts = new Vector2[2];

	[SerializeField] List<GameObject> m_lstHideObjects = new List<GameObject>();

	Quest m_quest;
	bool m_bOpen;

	//作成
	public void Creat(in Quest quest)
	{
		m_bOpen = false;
		m_quest = quest;

		Initialize();
	}

	//初期化
	public void Initialize()
	{
		SetItemDatas();
		BackScaleChange();
		ActiveObjectChanges();
		SetHighlightAnimation(false);
	}

	//Itemの表示or非表示
	public void ItemDisplaySwitching()
	{
		m_bOpen = !m_bOpen;

		FoldoutChanges();
		BackScaleChange();
		ActiveObjectChanges();
	}

	//テキスト・チェックマークのセット
	void SetItemDatas()
	{
		//クエスト名
		string strWork = m_quest.GetQuest().name;
		gameObject.name = strWork;
		Transform traWork = FindChildTra("Name");
		traWork.GetComponent<TextMeshProUGUI>().text = strWork;

		strWork = m_quest.GetQuest().detail;
		traWork = FindChildTra("Explanation");
		traWork.GetComponent<TextMeshProUGUI>().text = strWork;

		foreach (Transform child in FindChildTra("Conditions"))
			child.Find("Checkmark").GetComponent<Image>().color = Color.clear;

		traWork = FindChildTra("Conditions/Appearance/Checkmark");
		if (m_quest.IsKey(Quest.KEY.POSSIBLE)) traWork.GetComponent<Image>().color = Color.white;
		traWork = FindChildTra("Conditions/New/Checkmark");
		if (!m_quest.IsKey(Quest.KEY.NO_NEW)) traWork.GetComponent<Image>().color = Color.white;
		traWork = FindChildTra("Conditions/Clear/Checkmark");
		if (m_quest.IsKey(Quest.KEY.CLEAR)) traWork.GetComponent<Image>().color = Color.white;
	}

	void Start()
	{
		//RectTransformの座標を変更（なぜかここでは、使える??）
		for (int i = 0; i < transform.childCount; i++)
		{
			GameObject item = transform.GetChild(i).gameObject;
			Vector2 pos = item.transform.GetComponent<RectTransform>().anchoredPosition;
			pos.x = 0.0f;
			item.transform.GetComponent<RectTransform>().anchoredPosition = pos;
		}
		FoldoutChanges();
	}

	//Foldoutの位置変更
	void FoldoutChanges()
	{
		Transform parentFoldout = FindChildTra("Foldouts");

		for (int i = 0; i < parentFoldout.childCount; i++)
		{
			RectTransform rectTransform = parentFoldout.GetChild(i).GetComponent<RectTransform>();
			rectTransform.anchoredPosition = m_bOpen ? m_openFoldouts[i] : Vector2.zero;
		}
	}

	//Highlightのアニメーションをセット
	public void SetHighlightAnimation(in bool bAnimation)
	{
		//Highlightを消す
		Animator anime = FindChildTra("Highlight").GetComponent<Animator>();
		anime.SetBool("bHighlight", bAnimation);
	}

	//背景の表示サイズを変更
	void BackScaleChange()
	{
		//背景の縦サイズ変更
		RectTransform rectTra = GetComponent<RectTransform>();
		RectTransform effectRectTra = FindChildTra("Highlight").GetComponent<RectTransform>();
		float y = (m_bOpen ? m_openSize : m_closeSize);

		Vector2 scale = rectTra.sizeDelta;
		scale.y = y;
		rectTra.sizeDelta = scale;

		scale = effectRectTra.sizeDelta;
		scale.y = y;
		rectTra.sizeDelta = scale;
	}

	//HideObjectsの表示・非表示
	void ActiveObjectChanges()
	{
		foreach (GameObject item in m_lstHideObjects) item.SetActive(m_bOpen);
	}

	//子オブジェクトの取得(検索)
	Transform FindChildTra(in string name)
	{
		return transform.Find(name);
	}
}
