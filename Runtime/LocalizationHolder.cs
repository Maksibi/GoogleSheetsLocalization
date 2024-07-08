using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using UnityEngine;

namespace QT.Localization
{
    [CreateAssetMenu(menuName = "", fileName = "LocaleHolder")]
    public class LocalizationHolder : ScriptableObject
    {
        [SerializeField] public List<LocalizationValues> Values = new();

        private const string SheetsID = "1pxpdbDExLmEoA733KU7VHn-sWKHo35NksFdFq0lHgP4";
        private const string SheetName = "Translate";

        [ContextMenu("Download Localization")]
        public async Task DownloadAndParseSheet()
        {
            Values.Clear();

            var _service = new SheetsService(new BaseClientService.Initializer
            {
                ApiKey = "AIzaSyCHsDX-Eu4ufTsbfyqDfHD_B3moS_srZTw"
            });

            Debug.Log($"Starting download sheet (${SheetName})...");

            var range = $"{SheetName}!A1:Z";
            var request = _service.Spreadsheets.Values.Get(SheetsID, range);

            ValueRange response;
            try
            {
                response = await request.ExecuteAsync();
            }
            catch (Exception e)
            {
                Debug.LogError($"Error retrieving Google Sheets data: {e.Message}");
                return;
            }

            if (response != null && response.Values != null)
            {
                var tableArray = response.Values;
                Debug.Log($"Sheet downloaded successfully: {SheetName}.");
                var firstRow = tableArray[0];

                for (int i = 1; i < firstRow.Count; i++)
                {
                    Values.Add(new LocalizationValues()
                    {
                        i18n = firstRow[i].ToString(),
                        KeyValuePairs = new()
                    });
                }

                var rowsCount = tableArray.Count;
                for (int i = 1; i < rowsCount; i++)
                {
                    var row = tableArray[i];
                    var rowLength = row.Count;

                    string key = row[0].ToString();

                    for (int j = 1; j < rowLength; j++)
                    {
                        var cell = row[j];

                        Values[j - 1].KeyValuePairs.Add(new()
                        {
                            Key = key,
                            Value = cell.ToString()
                        });
                    }
                }

                Debug.Log(string.Join("\n", Values));
            }
            else
            {
                Debug.LogWarning("No data found in Google Sheets.");
            }
        }
    }

    [Serializable]
    public struct LocalizationValues
    {
        public string i18n;
        public List<KeyValuePair> KeyValuePairs;

        public override string ToString()
        {
            string strToReturn = string.Empty;

            foreach (var pair in KeyValuePairs)
            {
                strToReturn += pair.Key + " " + pair.Value + "\n";
            }

            return i18n + "\n" + strToReturn;
        }
    }

    [Serializable]
    public struct KeyValuePair
    {
        public string Key;
        public string Value;
    }
}