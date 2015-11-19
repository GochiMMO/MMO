using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

public class FirstStatus_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/Player/Status/FirstStatus.xls";
	private static readonly string exportPath = "Assets/Resources/Player/Status/FirstStatus.asset";
	private static readonly string[] sheetNames = { "Archer","Warrior","Sorcerer","Monk", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_FirstStatus data = (Entity_FirstStatus)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_FirstStatus));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_FirstStatus> ();
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

					Entity_FirstStatus.Sheet s = new Entity_FirstStatus.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_FirstStatus.Param p = new Entity_FirstStatus.Param ();
						
					cell = row.GetCell(0); p.HP = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.SP = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.Attack = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.Defense = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.MagicAttack = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.MagicDefense = (int)(cell == null ? 0 : cell.NumericCellValue);
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
