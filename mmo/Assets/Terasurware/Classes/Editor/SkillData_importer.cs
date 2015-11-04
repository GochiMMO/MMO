using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

public class SkillData_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/Skill/SkillData.xls";
	private static readonly string exportPath = "Assets/Resources/Skill/SkillData.asset";
	private static readonly string[] sheetNames = { "Archer","Warrior","Sorcerer","Monk", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_Job data = (Entity_Job)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_Job));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_Job> ();
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

					Entity_Job.Sheet s = new Entity_Job.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_Job.Param p = new Entity_Job.Param ();
						
					cell = row.GetCell(0); p.id = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.lv = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.difficult = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.attack = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.defens = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.sp = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.cooltime = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.casttime = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.effect = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.point = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(11); p.bonus = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(12); p.type = (int)(cell == null ? 0 : cell.NumericCellValue);
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
