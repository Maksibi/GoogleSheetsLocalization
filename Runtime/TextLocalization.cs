using System;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace QT.Localization
{

	public class TextLocalization : MonoBehaviour
	{
		[SerializeField] private TMP_Text tmpText;
		[SerializeField] private Text legacyText;
		[SerializeField] private string key;
    
		private string Locale => LocalizationSingleton.Instance.CurrentLocalization[key];

		private bool textChanged;

		#if UNITY_EDITOR
		private void OnValidate()
		{
			if (tmpText == null && legacyText == null)
			{
				tmpText = GetComponent<TMP_Text>();

				legacyText = GetComponent<Text>();
			}

			if (tmpText == null && legacyText == null)
				return;

			if (!string.IsNullOrWhiteSpace(key))
				return;

			key = tmpText != null ? tmpText.text : legacyText.text;

			EditorUtility.SetDirty(this);
		}
		#endif

		private void Start()
		{
			if(!textChanged)
				ChangeText(Locale);
		}

		private void ChangeText(string text)
		{
			if (tmpText != null)
				tmpText.text = text;
			else
				legacyText.text = text;
		}

		public void SetValue(object value)
		{
			ChangeText(string.Format(Locale, value));
			textChanged = true;
		}

		public void SetValue(object[] values)
		{
			ChangeText(string.Format(Locale, values));
			textChanged = true;
		}
	}
}
