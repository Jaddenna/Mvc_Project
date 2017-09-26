using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Mvc_Project.Helpers
{

	public class Tabstrip
	{
		public const string GROUP_PREFIX = "Tab_";
		public HtmlHelper Html { get; set; }
		public string ID { get; set; }
		public IEnumerable<string> Tabs { get; set; }
		public string Group
		{
			get
			{
				return GetGroup(ID);
			}
		}
		public string SelectedTab
		{
			get
			{
				return GetSelectedTab(Html, ID);
			}
		}

		public Tabstrip(HtmlHelper html, string id, IEnumerable<string> tabs)
		{
			Html = html;
			ID = id;
			Tabs = tabs;
		}

		public void CreateTabstrip()
		{
			StringWriter sw = new StringWriter();
			using (HtmlTextWriter writer = new HtmlTextWriter(Html.ViewContext.Writer))
			{
				writer.Write("<div class='tabstrip'>");
				int index = 0;
				foreach (string tab in Tabs)
				{
					bool isSelected = SelectedTab == index.ToString();
					string checkedAttr = isSelected ? "checked='checked'" : "";
					string checkedClass = isSelected ? "tabRadioSelected" : "";
					writer.Write($"<label for='{Group}_{index}' class='tabRadio {checkedClass}'>");
					writer.Write($"<input type='radio' id='{Group}_{index}' name='{Group}' value='{index}' {checkedAttr} onchange='changeTab(this)' />");
					writer.Write($"{tab}");
					writer.Write("</label>");
					index++;
				}
				writer.Write("</div>");
			}
		}

		public static string GetSelectedTab(HtmlHelper html, string tabstripID)
		{
			return html?.ViewContext.HttpContext?.Request?.Form[GetGroup(tabstripID)] ?? "0";
		}

		public static string GetGroup(string tabstripID)
		{
			return Tabstrip.GROUP_PREFIX + tabstripID;
		}
	}
	public class Tabpanel
	{
		public string TabstripID { get; private set; }
		public HtmlHelper Html { get; set; }
		public int Index { get; private set; }
		public bool IsSelected
		{
			get
			{
				return Tabstrip.GetSelectedTab(Html, TabstripID) == Index.ToString();
			}
		}


		public Tabpanel(HtmlHelper html, string tabstripID, int index)
		{
			TabstripID = tabstripID;
			Html = html;
			Index = index;
		}

		public void CreateBeginTab()
		{
			StringWriter sw = new StringWriter();
			using (HtmlTextWriter writer = new HtmlTextWriter(Html.ViewContext.Writer))
			{
				string selectedClass = IsSelected ? "selectedTab" : "";
				writer.Write($"<div class='tab {selectedClass}' data-index='{Index}'>");
			}
		}

		public static void CreateEndTab(HtmlHelper html)
		{
			StringWriter sw = new StringWriter();
			using (HtmlTextWriter writer = new HtmlTextWriter(html.ViewContext.Writer))
			{
				writer.Write("</div>");
			}
		}

	}
}