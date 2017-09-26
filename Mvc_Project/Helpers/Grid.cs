using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Mvc_Project.Helpers
{

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