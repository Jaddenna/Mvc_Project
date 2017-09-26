using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mvc_Connector;

namespace Mvc_Project
{
	public class GridController : Controller
	{
		public ActionResult Index()
		{
			ViewData["Title"] = "Böse Muschie!";
			return View("~/Views/Grid.aspx", Category.ExCats);
		}
	}
}