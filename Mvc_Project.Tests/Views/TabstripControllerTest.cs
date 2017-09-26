using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace Mvc_Project
{
	[TestClass]
	public class TabstripControllerTest
	{
		[TestMethod]
		public void Index()
		{
			TabstripController c = new TabstripController();
			ViewResult vr = (ViewResult)c.Index();
			Assert.AreEqual("~/Views/Tabstrip.aspx", vr.ViewName);
		}
	}
}