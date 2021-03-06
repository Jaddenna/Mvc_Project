﻿<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title></title>
	<style>
		*
		{
			font-family: Arial;
		}

		.tabstrip
		{
			width: 100%;
			display: inline-block;
			border-bottom: 2px solid;
		}

			.tabstrip .tabRadio
			{
				float: left;
				color: rgba(0, 0, 0, 0.3);
				padding: 5px 20px;
				border: 2px solid currentColor;
				border-bottom: 2px solid;
				border-top-left-radius: 5px;
				border-top-right-radius: 20px;
				margin-right: 20px;
				margin-bottom: -2px;
			}
			.tabstrip .tabRadio.tabRadioSelected
			{
				color: rgb(0, 0, 0);
				border-color: currentColor;
				border-bottom-color: rgb(255, 255, 255);
			}

				.tabstrip .tabRadio:first-child
				{
					margin-left: 20px;
				}

				.tabstrip .tabRadio input
				{
					display: none;
				}

			.tabstrip::after
			{
				content: '';
				clear: left;
			}

		.tab
		{
			display: none;
		}

			.tab.selectedTab
			{
				display: initial;
			}
	</style>
</head>
<body>
	<% Html.BeginForm("Index", "Tabstrip"); %>
	<% MyHelpers.MyTabstrip(Html, "Tabstrip", new string[] { "Tab1", "Tab2", "Tab3" }); %>
	<% MyHelpers.MyBeginTabstripPanel(Html, "Tabstrip", 0); %>
	<h1>Tab 1</h1>
	<% MyHelpers.MyEndTabstripPanel(Html); %>
	<% MyHelpers.MyBeginTabstripPanel(Html, "Tabstrip", 1); %>
	<h1>Tab 2</h1>
	<% MyHelpers.MyEndTabstripPanel(Html); %>
	<% MyHelpers.MyBeginTabstripPanel(Html, "Tabstrip", 2); %>
	<h1>Tab 3</h1>
	<% MyHelpers.MyEndTabstripPanel(Html); %>
	<% Html.EndForm(); %>
	<script>
		function changeTab()
		{
			document.forms[0].submit();
		}
	</script>
</body>
</html>
