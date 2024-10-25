using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.UI;

public class Request : MonoBehaviour
{
	[SerializeField] Text text;
	string m_title;
	int m_number;

    void Start()
    {
		text.text = m_title;
	}

	public void Settings(int num, string title)
	{
		m_number = num;
		m_title = title;
	}

	public int GetNumber() => m_number;

	//https://note.com/yamasho55/n/nbfc128e13082
	//https://youtube.com/watch?v=6faMCg6oohc


	//ÉJÉâÅ[
	//https://paletton.com/#uid=12M1k0k94ND1k+i4UVCdwCFi1tQ
}
