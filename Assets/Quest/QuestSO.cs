using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public struct PurolandQuest
{
	[Header("受注可能")]
	[SerializeField] private bool availableForOrders;
	[Header("新品")]
	[SerializeField] private bool inaugural;
	[Header("クエストのクリア")]
	[SerializeField] private bool clear;

	public (bool possible,bool inaugural,bool clear)GetKeys()
	{
		return (availableForOrders, inaugural, clear);
	}
}

[System.Serializable]
public class Quest
{
	[Flags]
	public enum KEY
	{
		/// <summary>出現</summary>
		POSSIBLE = 1,
		/// <summary>非新品</summary>
		NO_NEW = 2,
		/// <summary>クリア</summary>
		CLEAR = 4,
	}

	//クエスト名
	[Header("クエスト名")]
	[SerializeField] private string m_name;

	//クエスト詳細
	[Header("クエスト詳細")]
	[Multiline(8)]
	[SerializeField] private string m_detail;

	[Header("クエストフラグ")]
	[SerializeField] private int m_key;

	//クエストのセット
	public void SetQuest(string name, string detail)
	{
		m_name = name;
		m_detail = detail;
	}

	//クエストの情報取得
	public (string name, string detail) GetQuest()
	{
		return (this.m_name, this.m_detail);
	}

	/// <summary>
	/// キーの比較
	/// </summary>
	public bool IsKey(in KEY key)
	{
		KEY work = (KEY)Enum.ToObject(typeof(KEY), m_key);
		return work.HasFlag(key);
	}

	//クエストの設定
	public void SetKey(in KEY key)
	{
		m_key |= (int)key;
	}

	public void AllClear()
	{
		m_key = 0x00;
	}
}

[CreateAssetMenu]
public class QuestSO : ScriptableObject
{
	/// <summary>クエスト一覧</summary>
	public List<Quest> quests = new List<Quest>();

	/// <summary>
	/// クエストの状況を初期化する
	/// </summary>
	public void ResetQuestData()
	{
		foreach (Quest quest in quests)
		{
			quest.AllClear();
		}

		quests[0].SetKey(Quest.KEY.POSSIBLE);
	}
}
