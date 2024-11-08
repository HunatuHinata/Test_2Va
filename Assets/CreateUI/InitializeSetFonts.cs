using UnityEngine;
using UnityEditor;
using TMPro;

[InitializeOnLoad]
public class Startup
{
	[SerializeField] TextMeshProUGUI[] m_meshProUGUI;
	[SerializeField] TMP_InputField[] m_tnputFields;
	[SerializeField] TMP_FontAsset m_FontAsset;

	static Startup()
	{
		
	}
}