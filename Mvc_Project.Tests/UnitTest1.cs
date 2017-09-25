using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace Mvc_Project.Tests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			DefaultController c = new DefaultController();
			ViewResult vr = (ViewResult)c.Index();
			Assert.AreEqual("~/Views/Default.aspx", vr.ViewName);
		}
	}
}
