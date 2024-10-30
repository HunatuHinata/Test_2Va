using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class ItemCreat
{
	[SerializeField] GameObject settingObject;

	QuestSO m_questSO;

	//QuestSOをセット
	public void SetQuestListRef(ref QuestSO questSO)
	{
		m_questSO = questSO;
	}

	//追加
	public void AddQuest()
	{
		Quest quest;
		ExportQuest(out quest);
		m_questSO.quests.Add(quest);
	}

	//編集
	public void ReplacementQuest(ref Quest refQuest)
	{
		Quest quest;
		ExportQuest(out quest);
		refQuest = quest;
	}

	//入力内容をを出力
	void ExportQuest(out Quest outQuest)
	{
		outQuest = new Quest();
		Transform questTransform;
		string name, explanation;

		//名前＆説明
		FindChilled("QuestName/Input", out questTransform);
		name = questTransform.GetComponent<TMP_InputField>().text;
		FindChilled("QuestExplanation/Input", out questTransform);
		explanation = questTransform.GetComponent<TMP_InputField>().text;
		outQuest.SetQuest(name, explanation);
		//状態
		FindChilled("Conditions/ToggleAppearance", out questTransform);
		if (IsToggleOn(questTransform)) outQuest.SetKey(Quest.KEY.POSSIBLE);
		FindChilled("Conditions/ToggleNew", out questTransform);
		if (!IsToggleOn(questTransform)) outQuest.SetKey(Quest.KEY.NO_NEW);
		FindChilled("Conditions/ToggleClear", out questTransform);
		if (IsToggleOn(questTransform)) outQuest.SetKey(Quest.KEY.CLEAR);
	}

	//子オブジェクトの取得(検索)
	void FindChilled(in string name, out Transform findTransform)
	{
		findTransform = settingObject.transform.Find(name);
	}

	//Toggleコンポーネントのチェックの取得
	bool IsToggleOn(in Transform questTransform)
	{
		return questTransform.GetComponent<Toggle>().isOn;
	}
}
