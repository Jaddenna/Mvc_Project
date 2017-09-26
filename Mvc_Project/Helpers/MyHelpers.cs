using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.WebPages;

namespace Mvc_Project.Helpers
{
	public class MyHelpers
	{
		public static string MyCheckBox(string id)
		{
			return $"<input type='checkbox' id='{id}'>";
		}

		public static string MyCheckBox(string id, string text)
		{
			return $"<label for={id}><input type='checkbox' id='{id}'>{text}</label>";
		}

		public static string MyCheckBox(string id, string text, Dictionary<string, string> attributes)
		{
			string attributeStr = "";
			foreach (KeyValuePair<string, string> attr in attributes)
			{
				attributeStr += $"{attr.Key}='{attr.Value}' ";
			}
			return $"<label for={id}><input type='checkbox' id='{id}' {attributeStr}>{text}</label>";
		}

		public static void MyGrid<T>(HtmlHelper html, string id, IEnumerable<T> collection, params GridColum<T>[] renderFuncs)
		{
			Grid grid = new Grid(html);
			grid.CreateGrid<T>(id, collection, renderFuncs);
		}

		public static void MyTabstrip(HtmlHelper html, string id, IEnumerable<string> tabs)
		{
			Tabstrip tabstrip = new Helpers.Tabstrip(html, id, tabs);
			tabstrip.CreateTabstrip();
		}
		public static void MyBeginTabstripPanel(HtmlHelper html, string tabstripID, int index)
		{
			Tabpanel panel = new Tabpanel(html, tabstripID, index);
			panel.CreateBeginTab();
		}
		public static void MyEndTabstripPanel(HtmlHelper html)
		{
			Tabpanel.CreateEndTab(html);
		}
	}
}