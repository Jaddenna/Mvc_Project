using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace Mvc_Project
{
	[TestClass]
	public class GridControllerTest
	{
		[TestMethod]
		public void Index()
		{
			GridController c = new GridController();
			ViewResult vr = (ViewResult)c.Index();
			Assert.AreEqual("~/Views/Grid.aspx", vr.ViewName);
		}
	}
}