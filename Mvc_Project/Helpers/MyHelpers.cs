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

		public static void Tabstrip(HtmlHelper html, string id, IEnumerable<string> tabs)
		{
			Tabstrip tabstrip = new Helpers.Tabstrip(html, id, tabs);
			tabstrip.CreateTabstrip();
		}
		public static void BeginTabstripPanel(HtmlHelper html, string tabstripID, int index)
		{
			Tabpanel panel = new Tabpanel(html, tabstripID, index);
			panel.CreateBeginTab();
		}
		public static void EndTabstripPanel(HtmlHelper html)
		{
			Tabpanel.CreateEndTab(html);
		}
	}
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
	public class Grid
	{
		public HtmlHelper Html { get; set; }
		public int[] ResultNumbers { get; set; } = new int[] { 2, 10, 25 };
		public int PageCount { get; set; }
		public int PageIndex
		{
			get
			{
				string requestPage = Html?.ViewContext.HttpContext?.Request?.Form["gridpageindex"] ?? "-1";
				return int.Parse(requestPage);
			}
		}
		public int ResultNumber
		{
			get
			{
				string requestPage = Html?.ViewContext.HttpContext?.Request?.Form["gridpager"] ?? "-1";
				return int.Parse(requestPage);
			}
		}
		public Grid(HtmlHelper html)
		{
			Html = html;
		}
		public void CreateGrid<T>(string id, IEnumerable<T> collection, params GridColum<T>[] renderFuncs)
		{
			double a = collection.Count() / (double)ResultNumber;
			PageCount = (int)Math.Ceiling(a);
			StringWriter sw = new StringWriter();
			using (HtmlTextWriter writer = new HtmlTextWriter(Html.ViewContext.Writer))
			{
				(new GridPager()).CreatePager(writer, ResultNumber, PageCount, PageIndex);
				writer.Write("<table><thead><tr>");
				foreach (GridColum<T> col in renderFuncs)
				{
					col.CreatHeader(writer);
				}
				writer.Write("</tr></thead>");

				int start = ResultNumber > 0 && PageIndex > 0 ? ResultNumber * (PageIndex - 1) : 0;
				int end = ResultNumber > 0 ? ResultNumber : collection.Count();
				foreach (T item in collection.Skip(start).Take(end))
				{
					writer.Write("<tr>");
					foreach (GridColum<T> col in renderFuncs)
					{
						col.CreateColumn(writer, item);
					}
					writer.Write("</tr>");
				}
				writer.Write("</tbody></table>");
			}
		}
	}
	public class GridPager
	{
		public int[] Pages { get; set; } = new int[] { 2, 5, 10 };
		public GridPager() { }
		public GridPager(int[] pages)
		{
			Pages = pages;
		}
		public void CreatePager(HtmlTextWriter writer, int selectedValue, int pageCount, int pageIndex)
		{
			writer.Write("<select name='gridpager' onchange='setPages(this)'>");
			foreach (int page in Pages)
			{
				string selected = page == selectedValue ? "selected='selected'" : "";
				writer.Write($"<option value='{page}' {selected}>{page}</option>");
			}
			writer.Write("</select>");

			writer.Write("<select name='gridpageindex' onchange='setPages(this)'>");
			for (int i = 1; i <= pageCount; i++)
			{
				string selected = i == pageIndex ? "selected='selected'" : "";
				writer.Write($"<option value='{i}' {selected}>{i}</option>");
			}
			writer.Write("</select>");
		}
	}
	public class GridColum<T>
	{
		public object Header { get; set; }
		public Action<T> Render { get; set; }

		public GridColum() { }

		public GridColum(object header, Action<T> render)
		{
			Header = header;
			Render = render;
		}

		public void CreatHeader(HtmlTextWriter writer)
		{
			writer.Write("<th>");
			if (Header != null)
			{
				if (Header is Action)
				{
					((Action)Header)();
				}
				else
				{
					writer.Write(Header.ToString());
				}
			}
			writer.Write("</th>");
		}

		public void CreateColumn(HtmlTextWriter writer, T item)
		{
			writer.Write("<td>");
			Render(item);
			writer.Write("</td>");
		}
	}
}