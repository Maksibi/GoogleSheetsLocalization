# GoogleSheetsLocalization

## Installation
### 1. Install NuGetForUnity via UPM:
    https://github.com/GlitchEnzo/NuGetForUnity.git?path=/src/NuGetForUnity
  - For older versions see [NuGetForUnity](https://github.com/GlitchEnzo/NuGetForUnity) Repo
### 2. Install Google API packages via NuGet:
  - Open `NuGet` > `Manage NuGet Package`
  - Install `Google.Apis` and `Google.Apis.Sheets.v4`
  <br><br>
  ![GoogleApi](Images~/GoogleApi.png)
### 3. Install [GoogleSheetsLocalization] via UPM:
    https://github.com/Maksibi/GoogleSheetsLocalization.git
## Usage
### 1. Create LocalizationHolder ScriptableObject
  - `Assets` > `Create` > `LocalizationHolder`
### 2. Fill out your information in LocalizationHolder
  - `Api Key` - your google sheets api key, you can find it [here](https://console.cloud.google.com/)
  - `Sheet Name` - name of sheet in GoogleSheets, can be found at bottom of GoogleSheets
  <br><br>
  ![SheetName](Images~/SheetName.png)
  - `Sheets ID` - id of sheets in GoogleSheets, can be fount in URL of GoogleSheets
  <br><br>
  ![SheetId](Images~/SheetId.png)
### 3. Setup your table
  - Your table should look like this:
  <br><br>
  ![SheetLayout](Images~/SheetLayout.png)
  - Fill your Keys and LocalizationValues according to layout of the table
### 4. Download localization
  - Open context menu of LocalizationHolder and press DownloadLocalization
### 5. Use localization
  - Example usage:
    ```
      localizationHolder.Values.First(x => x.i18n == "en").First(x => x.Key == "CoinsTextKey");
    ```
