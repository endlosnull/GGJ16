using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pooling;

public class UIPlaceholder : MonoBehaviour
{
	public string realname;
	public string realtext;

	public void Awake()
	{
		RectTransform localTransform = this.transform as RectTransform;
		GameObject realObject = GameObjectFactory.Instance.SpawnUI(realname, localTransform.parent);
		RectTransform realTransform = realObject.transform as RectTransform;
		realTransform.sizeDelta = localTransform.sizeDelta;
		realTransform.anchoredPosition = localTransform.anchoredPosition;
		realTransform.offsetMin = localTransform.offsetMin;
		realTransform.offsetMax = localTransform.offsetMax;
		realTransform.localPosition = localTransform.localPosition;
		realTransform.localScale = localTransform.localScale;
		Text textComponent = realObject.GetComponentInChildren<Text>();
		if( textComponent != null )
		{
			textComponent.text = realtext;
		}
	}

	void Start()
	{
		this.gameObject.SetActive(false);
	}
}
