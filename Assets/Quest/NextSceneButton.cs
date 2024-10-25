using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneButton : MonoBehaviour
{
	[Header("���ɍs���V�[���̖��O")]
	[SerializeField] string m_sceneName;

    public void NextScene()
	{
		SceneManager.LoadScene(m_sceneName.ToString());
	}
}
