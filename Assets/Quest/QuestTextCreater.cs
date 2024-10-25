using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTimer;
using UnityEngine.UI;
using System.Linq;

public class QuestTextCreater : MonoBehaviour
{
	[Header("ˆê•¶Žš‚Ã‚Â•\Ž¦‘¬“x")]
	[SerializeField] float m_maxTimer = 1.0f;
	[SerializeField] float m_minTimer = 0.2f;

	private Timer m_charTimer = new Timer();
	private Text m_text;
	private string m_detail;
	private int m_count;

	void Start()
	{
		m_text = GetComponent<Text>();
		Resets();
	}

	public void Resets()
	{
		m_detail = "";
		m_text.text = "";
		m_count = 0;
		m_charTimer.ResetInterval();
	}

	public void SetLetters(string letter)
	{
		m_detail = letter;
		m_count = 0;
		m_text.text = "";

		SetTimer();
	}

    void Update()
    {
		if (m_detail == "") return;
		if (m_detail.Count() == m_count) return;
		if (!m_charTimer.GetTiming()) return;

		m_text.text += m_detail[m_count++];

		if (m_detail.Count() == m_count) return;
		if (Random.Range(0, 6) < 4)
		{
			SetTimer();
		}
		else
		{
			PlayTimer();
		}
	}

	void SetTimer()
	{
		m_charTimer.SetInterval(Random.Range(m_minTimer, m_maxTimer), false);
	}

	void PlayTimer()
	{
		m_charTimer.PlayOrStop(true);
	}
}
