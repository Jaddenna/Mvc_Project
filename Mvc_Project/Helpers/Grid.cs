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
		public int[] ResultNumbers { get; set; } = new int[] { 1, 2, 10, 25 };
		public int PageCount { get; set; }
		public int PageIndex
		{
			get
			{
				string requestPage = Html?.ViewContext.HttpContext?.Request?.Form["gridpageindex"] ?? "1";
				return int.Parse(requestPage);
			}
		}
		public int ResultNumber
		{
			get
			{
				string requestPage = Html?.ViewContext.HttpContext?.Request?.Form["gridpager"] ?? ResultNumbers[0].ToString();
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
				GridPager pager = new GridPager(writer, ResultNumber, PageCount, PageIndex, renderFuncs.Length);
				pager.CreatePager();
				writer.Write("<table class='gridTable'><thead><tr>");
				pager.CreatePagerRow();
				writer.Write("<tr>");
				foreach (GridColum<T> col in renderFuncs)
				{
					col.CreatHeader(writer);
				}
				writer.Write("</tr></thead>");

				int start = ResultNumber > 0 && PageIndex > 0 && PageIndex <= PageCount ? ResultNumber * (PageIndex - 1) : 0;
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
				writer.Write("</tbody><tfoot>");
				pager.CreatePagerRow();
				writer.Write("</tfoot></table>");
			}
		}
	}
	public class GridPager
	{
		private HtmlTextWriter _writer;
		private int _resultNumber;
		private int _pageCount;
		private int _pageIndex;
		private int _colspan;

		public int[] Pages { get; set; } = new int[] { 1, 2, 5, 10 };
		public GridPager() { }
		public GridPager(int[] pages)
		{
			Pages = pages;
		}

		public GridPager(HtmlTextWriter writer, int resultNumber, int pageCount, int pageIndex, int colspan)
		{
			this._writer = writer;
			this._resultNumber = resultNumber;
			this._pageCount = pageCount;
			this._pageIndex = pageIndex;
			this._colspan = colspan;
		}

		public void CreatePager()
		{
			_writer.Write("<select name='gridpager' onchange='setPages(this)'>");
			foreach (int page in Pages)
			{
				string selected = page == _resultNumber ? "selected='selected'" : "";
				_writer.Write($"<option value='{page}' {selected}>{page}</option>");
			}
			_writer.Write("</select>");
		}
		public void CreatePagerRow()
		{
			_writer.Write($"<tr class='pagerRow'><td colspan={_colspan}>");

			string disable = _pageIndex == 1 ? "disabled='disabled'" : "";
			WritePagerNumber("<<", 1, "pagerPrevFirst", "class='pagerPrevFirst'", disable);
			WritePagerNumber("<", _pageIndex - 1, "pagerPrev", "class='pagerPrev'", disable);

			int startNumber = (_pageIndex - 2) > 0 ? _pageIndex - 2 : 1;
			int endNumber = (_pageIndex + 2) < _pageCount ? _pageIndex + 2 : _pageCount;

			for (int i = startNumber; i <= endNumber; i++)
			{
				bool selected = i == _pageIndex;
				string checkedAttr = selected ? "checked='checked'" : "";
				string cssClass = "class='pagerNumber " + (selected ? "pagerNumberSelected" : "") + "'";
				string id = "page" + i;
				WritePagerNumber(i.ToString(), i, id, checkedAttr, cssClass);
			}

			disable = _pageIndex >= _pageCount ? "disabled='disabled'" : "" ;
			WritePagerNumber(">", _pageIndex + 1, "pagerNext", "class='pagerNext'", disable);
			WritePagerNumber(">>", _pageCount, "pagerNextLast", "class='pagerNextLast'", disable);

			_writer.Write("</td></tr>");
		}

		private void WritePagerNumber(string text, int value, string id, params string[] inputAttr)
		{
			string attr = string.Join(" ", inputAttr);
			_writer.Write($"<input type='radio' id='{id}' name='gridpageindex' value='{value}' {attr} onchange='setPages(this)' />");
			_writer.Write($"<label for='{id}'>{text}</label>");
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