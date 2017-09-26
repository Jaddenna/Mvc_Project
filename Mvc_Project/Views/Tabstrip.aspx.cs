using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mvc_Project
{
	public class TabstripController : Controller
	{
		public ActionResult Index()
		{
			return View("~/Views/Tabstrip.aspx");
		}
	}
}