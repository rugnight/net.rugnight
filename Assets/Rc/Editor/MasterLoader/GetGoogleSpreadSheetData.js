function doGet(e) {
  if (e) {
    var sheetName = e.parameter.sheetName;
    // URL 部分に自分で作ったスプレッドシートの URL を入れます
    var sheetUrl = e.parameter.sheetUrl;
    var process = e.parameter.process;
    var process = "data";
  } else {
    var sheetName = "UnitStatus";
    // URL 部分に自分で作ったスプレッドシートの URL を入れます
    var sheetUrl =
      "https://docs.google.com/spreadsheets/d/1CLMTBvGk5ZlQ_zX-tG_tr3AEjfT9mVfNCvmST0_yYl4/edit#gid=0";
    var process = "data";
  }

  // URL 部分に自分で作ったスプレッドシートの URL を入れます
  var ss = SpreadsheetApp.openByUrl(sheetUrl);
  var sheet = ss.getSheetByName(sheetName);
  if (sheet != null && process == "data") {
    var json = ConvertSheetToJson(sheet);
    Logger.log(json);
    return ContentService.createTextOutput(JSON.stringify(json)).setMimeType(
      ContentService.MimeType.JSON
    );
  } else {
    Logger.log("NoSheets here");
    Browser.msgBox(Logger.getLog());
  }
}

function ConvertSheetToJson(sheet) {
  // 変数名の行
  var dataNameRow = 1;
  // プロパティ名の行
  var propertyNameRow = 2;
  // 型の行
  var typeRow = 3;
  // データ開始行
  var dataStartRow = 4;
  // 抽出したい列の数。最初の行は定義なのでスキップ。
  var recordSize = sheet.getLastRow() - (dataStartRow - 1);
  // 抽出したい列の数
  var columnSize = sheet.getLastColumn();

  // データ名
  var dataNameColumns = sheet
    .getRange(dataNameRow, 1, 1, sheet.getLastColumn())
    .getValues()[0];

  // シートの中身を 2 列目 1 行目から文字列で抽出。
  var bodyData = sheet
    .getRange(dataStartRow, 1, recordSize, columnSize)
    .getValues();

  // 型情報を JSON 化
  var typeData = sheet.getRange(typeRow, 1, 1, columnSize).getValues();
  var typeJson = CreateRowDataJson(columnSize, dataNameColumns, typeData[0]);

  // プロパティ名情報を JSON 化
  var propertyNameColumns = sheet
    .getRange(propertyNameRow, 1, 1, sheet.getLastColumn())
    .getValues()[0];
  var propertyNameJson = CreateRowDataJson(
    columnSize,
    dataNameColumns,
    propertyNameColumns
  );

  // データ本体を JSON 化
  var bodyJson = [];
  for (var row = 0; row < recordSize; row++) {
    var typeRowData = typeData[0];
    var line = bodyData[row];
    var obj = new Object();
    // 変数の定義部分を取得
    for (var column = 0; column < columnSize; column++) {
      // コメント行を無視
      if (typeRowData[column] == "comment") {
        continue;
      }

      // 配列は組み立ててから保存
      if (typeRowData[column].toLowerCase().indexOf("array") != -1) {
        if (typeRowData[column].toLowerCase().indexOf("string") != -1) {
          obj[dataNameColumns[column]] = String(line[column])
            .split(",")
            .map(function (element) {
              return String(element);
            });
        } else {
          obj[dataNameColumns[column]] = String(line[column])
            .split(",")
            .map(function (element) {
              return Number(element);
            });
        }
      } else {
        // 通常の値はそのまま保存
        obj[dataNameColumns[column]] = line[column];
      }
    }
    bodyJson.push(obj);
  }

  var dataDict = { list: bodyJson };
  var jsonDict = { type: typeJson, data: dataDict, property: propertyNameJson };
  return jsonDict;
}

// 指定行のデータをデータ名をキーとしたJsonとして出力します
function CreateRowDataJson(columnSize, dataNameLine, targetRowLine) {
  // 型情報を JSON 化
  var typeJson = [];

  var obj = new Object();
  // 変数の定義部分を取得
  for (var column = 0; column < columnSize; column++) {
    // コメント行を無視
    if (targetRowLine[column] == "comment") {
      continue;
    }
    // 定義を Json の要素に注入
    obj[dataNameLine[column]] = targetRowLine[column];
  }
  typeJson.push(obj);
  return typeJson;
}
