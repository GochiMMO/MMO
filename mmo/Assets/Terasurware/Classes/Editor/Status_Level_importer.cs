using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

public class Status_Level_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/Player/Status/Status_Level.xls";
	private static readonly string exportPath = "Assets/Resources/Player/Status/Status_Level.asset";
	private static readonly string[] sheetNames = { "str","vit","int","mnd", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_StatusPoint data = (Entity_StatusPoint)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_StatusPoint));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_StatusPoint> ();
				AssetDatabase.CreateAsset ((ScriptableObject)data, exportPath);
				data.hideFlags = HideFlags.NotEditable;
			}
			
			data.sheets.Clear ();
			using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read)) {
				IWorkbook book = new HSSFWorkbook (stream);
				
				foreach(string sheetName in sheetNames) {
					ISheet sheet = book.GetSheet(sheetName);
					if( sheet == null ) {
						Debug.LogError("[QuestData] sheet not found:" + sheetName);
						continue;
					}

					Entity_StatusPoint.Sheet s = new Entity_StatusPoint.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_StatusPoint.Param p = new Entity_StatusPoint.Param ();
						
					cell = row.GetCell(0); p.HP = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.SP = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.Attack = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.MagicAttack = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.Defense = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.MagicDefense = (float)(cell == null ? 0 : cell.NumericCellValue);
						s.list.Add (p);
					}
					data.sheets.Add(s);
				}
			}

			ScriptableObject obj = AssetDatabase.LoadAssetAtPath (exportPath, typeof(ScriptableObject)) as ScriptableObject;
			EditorUtility.SetDirty (obj);
		}
	}
}
