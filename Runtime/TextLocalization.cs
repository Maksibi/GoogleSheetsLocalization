using System;
using System.Collections.Generic;
using Eiko.YaSDK;
using QT.Localization;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

public class TextLocalization : MonoBehaviour
{
	[SerializeField] private TMP_Text tmpText;
	[SerializeField] private Text legacyText;
	[SerializeField] private string key;

	private static LocalizationHolder localizationHolder;

	private const string ValuePlaceHolder = "\\{n\\}";
	private string locale;

	#if UNITY_EDITOR
	private void OnValidate()
	{
		if (tmpText == null && legacyText == null)
		{
			tmpText = GetComponent<TMP_Text>();
			
			legacyText = GetComponent<Text>();
		}
		
		if(tmpText == null && legacyText == null)
			return;

		if (!string.IsNullOrWhiteSpace(key))
			return;
		
		key = tmpText != null ? tmpText.text : legacyText.text;

		EditorUtility.SetDirty(this);
	}
	#endif

	private void Awake()
	{
		if (localizationHolder == null)
			localizationHolder = Resources.Load<LocalizationHolder>("LocaleHolder");

		locale = LocalizationSingleton.Instance.CurrentLocalization[key];

		ChangeText(locale);
	}

	private void ChangeText(string text)
	{
		if (tmpText != null)
			tmpText.text = text;
		else
			legacyText.text = text;
	}

	public void SetValue(string value)
	{
		if(!locale.Contains(ValuePlaceHolder))
			throw new Exception($"No placeholder for values in locale: {locale}");

		ChangeText(locale.Replace(ValuePlaceHolder, value));
	}
}
