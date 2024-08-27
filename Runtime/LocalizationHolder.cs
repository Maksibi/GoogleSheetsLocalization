using System;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private string apiKey = "";
        [SerializeField] private string SheetID = "";
        [SerializeField] private string SheetName = "";

        [SerializeField] private List<LocalizationValues> Values = new();
        
        private Dictionary<string, Dictionary<string, string>> cachedLocalizationTable;
        public Dictionary<string, Dictionary<string, string>> LocalizationTable => cachedLocalizationTable ??= CreateLocalizationTable();

        private Dictionary<string, Dictionary<string, string>> CreateLocalizationTable()
        {
            var dictionary = new Dictionary<string, Dictionary<string, string>>();

            foreach (var value in Values)
            {
                dictionary.Add(value.i18n, 
                    new Dictionary<string, string>(value.KeyValuePairs.Select(x => KeyValuePair.Create(x.Key, x.Value))));
            }
            
            return dictionary;
        }

        [ContextMenu("Download Localization")]
        public async Task DownloadAndParseSheet()
        {
            Values.Clear();

            var _service = new SheetsService(new BaseClientService.Initializer
            {
                ApiKey = apiKey
            });

            Debug.Log($"Starting download sheet (${SheetName})...");

            var range = $"{SheetName}!A1:Z";
            var request = _service.Spreadsheets.Values.Get(SheetID, range);

            ValueRange response;
            try
            {
                response = await request.ExecuteAsync();
                
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
                        
                        if(row.Count == 0 || row[0] == null || string.IsNullOrWhiteSpace(row[0].ToString()))
                            continue;
                        
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

                    CreateLocalizationTable();
                }
                else
                {
                    Debug.LogWarning("No data found in Google Sheets.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error retrieving Google Sheets data: {e.Message}");
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
        
        [Serializable]
        public struct KeyValuePair
        {
            public string Key;
            public string Value;
        }
    }
}
