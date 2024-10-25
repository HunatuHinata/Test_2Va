using System.Threading;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using MyTimer;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
	[SerializeField] private QuestSO m_questSO;
	[SerializeField] private GameObject m_requestsObject;
	[SerializeField] private GameObject m_requestPrefab;
	[SerializeField] private GameObject m_button;

	//表示用
	[Header("表示テキスト用")]
	[SerializeField] private GameObject m_title;
	[SerializeField] private GameObject m_texts;
	[SerializeField] private GameObject m_text;
	[SerializeField] private GameObject m_score;

	[Header("クエストの設定")]
	[Header("通常時")]
	[SerializeField] Vector2 m_nomalSize;
	[Header("選択時")]
	[SerializeField] Vector2 m_pickupSize;
	[SerializeField] Color m_pickupColor;

	/// <summary>外部アクセス用</summary>
	static public QuestManager Instans;
	/// <summary>選択された依頼</summary>
	public Quest SelectQuest { get; private set; }
	private List<Quest> m_appearances = new List<Quest>();
	private GameObject m_beforeObject;

	private void Awake()
	{
		if (!Instans) Instans = this;
	}

	void Start()
    {
		SelectQuest = null;
		m_button.SetActive(false);
		Appearance();

		//クエスト数分生成する
		for (int i = 0; i < m_appearances.Count; i++)
		{
			GameObject requestObject;
			requestObject = Instantiate(m_requestPrefab, m_requestsObject.transform);
			requestObject.GetComponent<Request>().Settings(i, m_appearances[i].GetQuest().name);
		}
	}

    void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			PointerEventData ped = new PointerEventData(EventSystem.current);
			ped.position = Input.mousePosition;
			List<RaycastResult> result = new List<RaycastResult>();
			EventSystem.current.RaycastAll(ped, result);

			//Linqでの名前の比較がわからないからこれで代用
			foreach (RaycastResult r in result)
			{
				if (r.gameObject.name == "Button") return;
			}

			// Rayで何もヒットしなかったら画面タッチイベント関数を呼ぶ
			if (result.Count > 0 && result.Any(o => o.gameObject.CompareTag("Quest")))
			{
				var target = result.Where(o => o.gameObject.CompareTag("Quest"));

				Select(target.ToArray()[0].gameObject);
				m_button.SetActive(true);
			}
			else
			{
				Select(null);
				m_button.SetActive(false);
			}
		}
	}

	//表示可能なクエストを収集する
	private void Appearance()
	{
		foreach (Quest work in m_questSO.quests)
		{
			if (work.IsKey(Quest.KEY.POSSIBLE)) continue;

			m_appearances.Add(work);
		}
	}

	//クエストの選択
	private void Select(GameObject target)
	{
		if (target == null)
		{
			SelectQuest = null;
			SetObjectSize(null);
			m_title.GetComponent<QuestTextCreater>().Resets();
			m_texts.GetComponent<QuestTextCreater>().Resets();
			m_text.GetComponent<QuestTextCreater>().Resets();
			m_score.GetComponent<QuestTextCreater>().Resets();
			return;
		}

		if (target == m_beforeObject) return;

		int num = target!.GetComponent<Request>().GetNumber();
		if (num < 0) return;

		//選択したクエスト大きさ変更
		SetObjectSize(target);

		//クエスト情報の可視化
		SelectQuest = m_appearances[num];
		m_title.GetComponent<QuestTextCreater>().SetLetters(SelectQuest.GetQuest().name);
		m_texts.GetComponent<QuestTextCreater>().SetLetters(SelectQuest.GetQuest().detail);
		m_text.GetComponent<QuestTextCreater>().SetLetters("Score :");

		// ========================スコアの表示========================
		string work = "";

		//foreach (float score in SelectQuest.GetQuest().scores)
		//{
		//	work += score.ToString() + "\n";
		//}
		m_score.GetComponent<QuestTextCreater>().SetLetters(work);
		// ============================================================
	}

	private void SetObjectSize(GameObject target = null)
	{
		if (m_beforeObject)
		{
			m_beforeObject.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 255.0f);
			m_beforeObject.GetComponent<RectTransform>().sizeDelta = m_nomalSize;
			foreach (Transform child in m_beforeObject.transform)
			{
				child.GetComponent<RectTransform>().sizeDelta = m_nomalSize;
			}
		}

		if (target)
		{
			target.GetComponent<Image>().color = m_pickupColor;
			target.GetComponent<RectTransform>().sizeDelta = m_pickupSize;

			foreach (Transform child in target.transform)
			{
				child.GetComponent<RectTransform>().sizeDelta = m_pickupSize;
			}
		}

		m_beforeObject = target;
	}
}
