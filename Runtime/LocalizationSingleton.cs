using System;
using System.Collections;
using System.Collections.Generic;
using QT.Localization;
using UnityEngine;

namespace QT.Localization
{
	public class LocalizationSingleton : MonoBehaviour
	{
		[SerializeField] private LocalizationHolder localizationHolder;

		public static LocalizationSingleton Instance;

		public Dictionary<string, string> CurrentLocalization { get; private set; }

		private void Awake()
		{
			if (Instance != null)
			{
				Destroy(gameObject);
				return;
			}

			Instance = this;
			DontDestroyOnLoad(gameObject);
		}

		public void Init(string lang)
		{
			CurrentLocalization = localizationHolder.LocalizationTable[lang];
		}
	}
}