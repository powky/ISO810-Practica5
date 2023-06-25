<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="/">
    <html>
      <head>
        <style>
          /* Define your desired styles here */
          /* For example: */
          body {
            font-family: Arial, sans-serif;
            font-size: 14px;
            margin: 20px;
          }
        </style>
      </head>
      <body>
        <h1>Asientos Activos</h1>
        <xsl:apply-templates select="AsientoActivos"/>
      </body>
    </html>
  </xsl:template>

  <xsl:template match="AsientoActivos">
    <table>
      <xsl:apply-templates select="Encabezado"/>
      <xsl:apply-templates select="Cuentas"/>
    </table>
  </xsl:template>

  <xsl:template match="Encabezado">
    <tr>
      <th>NumeroAsiento</th>
      <th>DescripcionAsiento</th>
      <th>FechaAsiento</th>
    </tr>
    <tr>
      <td><xsl:value-of select="NumeroAsiento"/></td>
      <td><xsl:value-of select="DescripcionAsiento"/></td>
      <td><xsl:value-of select="FechaAsiento"/></td>
    </tr>
  </xsl:template>

  <xsl:template match="Cuentas">
    <tr>
      <th>Código</th>
      <th>Nombre</th>
      <th>Cuenta</th>
      <th>TipoMovimiento</th>
      <th>Monto</th>
    </tr>
    <xsl:apply-templates select="Cuenta"/>
  </xsl:template>

  <xsl:template match="Cuenta">
    <tr>
      <td><xsl:value-of select="Código"/></td>
      <td><xsl:value-of select="Nombre"/></td>
      <td><xsl:value-of select="Cuenta"/></td>
      <td><xsl:value-of select="TipoMovimiento"/></td>
      <td><xsl:value-of select="Monto"/></td>
    </tr>
  </xsl:template>

</xsl:stylesheet>
