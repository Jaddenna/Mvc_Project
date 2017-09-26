using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mvc_Project.Helpers;
using System.Web.Mvc;

namespace Mvc_Project.Tests.Helpers
{
	[TestClass]
	public class TabTest
	{
		[TestMethod]
		public void GroupCreation()
		{
			string tabID = "TestTab";
			Assert.AreEqual(Tabstrip.GROUP_PREFIX + "TestTab", Tabstrip.GetGroup(tabID));
		}
		[TestMethod]
		public void GroupPrefixNotChanged()
		{
			Assert.AreEqual("Tab_", Tabstrip.GROUP_PREFIX);
		}
		[TestMethod]
		public void CreateEndTabDiv()
		{
			Assert.AreEqual("</div>", Tabpanel.CreateEndTab());
		}
		[TestMethod]
		public void CreateBeginTab()
		{
		}
	}
}
