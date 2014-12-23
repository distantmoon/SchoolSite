<!DOCTYPE html PUBLIC"-//W3C//DTD XHTML 1.0 Transitional//EN""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<meta http-equiv="content-type" content="text/html;charset=utf-8" />
<title></title>
<script type="text/javascript">
            function SetRequestTitle() {
                document.title = document.domain
            }
            function InitiateRequest() {
                var url = "/?ref="+encodeUrl(document.referrer)+"&error_page=1";
                window.location.href = url;
            }
			function encodeUrl(a) {
				var result = "";
				try {
					result = encodeURIComponent(a);
				} catch(e) {
					result = escape(a);
				}
				return result;
			}
            SetRequestTitle();
</script>
</head>
<script type="text/javascript">
        InitiateRequest();
</script>
<body bgcolor="FFFFFF">
    <table width="410" cellpadding="3" cellspacing="5">
    <tr>   
    <td align="left" valign="middle" width="360">
    <h1 style="COLOR:000000; FONT: 13pt/15pt verdana">You are not authorized to view this page</h1>
    </td>
    </tr>
    </table>
</body>
</html>