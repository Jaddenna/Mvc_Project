<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="Mvc_Project.Helpers" %>
<%@ Import Namespace="Mvc_Connector" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
	<% Html.BeginForm("Index", "Default"); %>
        <div>
			<h1><%= ViewData["Title"] %></h1>
			<% Html.RenderPartial("~/Views/Partials/Partial.ascx", ViewData); %>
        </div>
		<% MyHelpers.MyGrid(Html, "hui", (List<Mvc_Connector.Category>)Model, 
		 new GridColum<Category>("Name", (c) =>{ %>
			<div><%= c.Name %></div>
		<% }), 
		new GridColum<Category>("Beschreibung", (c) => { %>
			<div><%= c.Name %></div>
		<% })); %>
    <% Html.EndForm(); %>
	<script type="text/javascript">
		function setPages(select)
		{
			document.forms[0].submit();
		}
	</script>
</body>
</html>
