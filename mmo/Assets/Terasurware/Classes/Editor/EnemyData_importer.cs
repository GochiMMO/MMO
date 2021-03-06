using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

public class EnemyData_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/Enemy/EnemyData/EnemyData.xls";
	private static readonly string exportPath = "Assets/Resources/Enemy/EnemyData/EnemyData.asset";
	private static readonly string[] sheetNames = { "Sahagin","Bee","Cokatris","Rat", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_Sahagin data = (Entity_Sahagin)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_Sahagin));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_Sahagin> ();
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

					Entity_Sahagin.Sheet s = new Entity_Sahagin.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_Sahagin.Param p = new Entity_Sahagin.Param ();
						
					cell = row.GetCell(0); p.Name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(1); p.HP = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.Attack = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.Defense = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.MagicAttack = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.MagicDefense = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.MoveSpeed = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.TrackingSpeed = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.ActionInterval = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.FieldOfView = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.ViewDistance = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(11); p.ActionDistance = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(12); p.HpRate = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(13); p.AttackRate = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(14); p.DefenseRate = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(15); p.MAtkRate = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(16); p.MDefRate = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(17); p.BaseExp = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(18); p.ExpRate = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(19); p.DamageRate = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(20); p.Exp = (int)(cell == null ? 0 : cell.NumericCellValue);
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
