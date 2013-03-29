<?xml version="1.0" ?>
<xsl:stylesheet version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:xlink="http://www.w3.org/1999/xlink">
  <xsl:output method="html" />

  <!-- totale html pagina met daarin ook afbeeldingen-->
  <xsl:template match="/">
    <html>
      <head>
        <link rel="stylesheet" type="text/css"/>
        <style>
          body, html {
          margin: 0;
          padding: 0;
          font-family: Verdana;
          font-size: 12px;
          }
          #header {
          height: 100px;
          border-bottom: 3px solid #0000FF;
          }
          .resultaat {
          margin-left: 12px;
          margin-top: 12px;
          }
          .clear {
          clear: both;
          }
          #footer {
          margin-top: 24px;
          border-top: 1px solid #0000FF;
          text-align: right;
          }

        </style>
      </head>
      <body>
        <div id="header">
          <!-- <img src='cid:ApplicationLogo'/>-->
        </div>
        <div class="resultaat">
          <table>
            <xsl:apply-templates select="resultaat"/>
          </table>
        </div>
        <div class="clear"></div>
        <div id="footer">
          <!--  <img src='cid:EsriLogo'/>- -->
        </div>
      </body>
    </html>
  </xsl:template>

  <!-- informatie over een resultaat-->
  <xsl:template match="resultaat">
    <tr>
      <td>datum</td>
      <td >
        <xsl:value-of select="datum"/>
      </td>
    </tr>
    <tr>
      <td>totale tijd</td>
      <td>
        <xsl:value-of select="totaletijd"/>
      </td>
    </tr>
    <tr>
      <td>status</td>
      <td>
        <xsl:value-of select="status"/>
      </td>
    </tr>
    <tr>
      <td>computer</td>
      <td>
        <xsl:value-of select="hostname"/>
      </td>
    </tr>
    <tr>
      <td>grootte in gigabytes</td>
      <td>
        <xsl:value-of select="sizeingb"/>
      </td>
    </tr>
    <tr>
      <td>grootte</td>
      <td>
        <xsl:value-of select="size"/>
      </td>
    </tr>
    <tr>
      <td>password</td>
      <td>
        <xsl:value-of select="password"/>
      </td>
    </tr>
    <tr>
      <td>naam</td>
      <td>
        <xsl:value-of select="naam"/>
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>