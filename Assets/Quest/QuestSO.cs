using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public struct PurolandQuest
{
	[Header("�󒍉\")]
	[SerializeField] private bool availableForOrders;
	[Header("�V�i")]
	[SerializeField] private bool inaugural;
	[Header("�N�G�X�g�̃N���A")]
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
		/// <summary>�o��</summary>
		POSSIBLE = 1,
		/// <summary>��V�i</summary>
		NO_NEW = 2,
		/// <summary>�N���A</summary>
		CLEAR = 4,
	}

	//�N�G�X�g��
	[Header("�N�G�X�g��")]
	[SerializeField] private string m_name;

	//�N�G�X�g�ڍ�
	[Header("�N�G�X�g�ڍ�")]
	[Multiline(8)]
	[SerializeField] private string m_detail;

	[Header("�N�G�X�g�t���O")]
	[SerializeField] private int m_key;

	//�N�G�X�g�̃Z�b�g
	public void SetQuest(string name, string detail)
	{
		m_name = name;
		m_detail = detail;
	}

	//�N�G�X�g�̏��擾
	public (string name, string detail) GetQuest()
	{
		return (this.m_name, this.m_detail);
	}

	/// <summary>
	/// �L�[�̔�r
	/// </summary>
	public bool IsKey(in KEY key)
	{
		KEY work = (KEY)Enum.ToObject(typeof(KEY), m_key);
		return work.HasFlag(key);
	}

	//�N�G�X�g�̐ݒ�
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
	/// <summary>�N�G�X�g�ꗗ</summary>
	public List<Quest> quests = new List<Quest>();

	/// <summary>
	/// �N�G�X�g�̏󋵂�����������
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
