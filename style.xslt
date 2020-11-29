<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
                       xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output
	method="html"></xsl:output>
	<xsl:template match="/">
		<html>
			<body>
				<table border = "1">
					<TR>
						<th>title</th>
						<th>author</th>
						<th>genre</th>
						<th>language</th>
						<th>date</th>
						<th>pages</th>
						
					</TR>
					<xsl:for-each select = "library/book">
						<tr>
							<td>
								<xsl:value-of select = "@title"/>
							</td>

							<td>
								<xsl:value-of select = "@author"/>
							</td>

							<td>
								<xsl:value-of select = "@genre"/>
							</td>

							<td>
								<xsl:value-of select = "@language"/>
							</td>

							<td>
								<xsl:value-of select = "@date"/>
							</td>
							
							<td>
								<xsl:value-of select = "@pages"/>
							</td>

						</tr>
					</xsl:for-each>
				</table>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
