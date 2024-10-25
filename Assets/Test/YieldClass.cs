using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YieldClass : MonoBehaviour
{
	void Start()
    {
		foreach (int i in ProduceEvenNumbers(9))
		{
			Debug.Log(i);
		}
		// Output: 0 2 4 6 8

		IEnumerable<int> ProduceEvenNumbers(int upto)
		{
			for (int i = 0; i <= upto; i++)
			{
				yield return i;
			}
		}
	}

	void Update()
	{
		float hor = Input.GetAxis("Horizontal");

		if (hor > 0.0f) Debug.Log(hor);
	}
}
