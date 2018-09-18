using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SpreadsheetLight;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.Drawing;

/// <summary>
/// Summary description for ExcelAdministrador
/// </summary>
public class ExcelAdministrador
{
    public ExcelAdministrador()
    {

    }

    /// <summary>
    /// Pasa Data table a SLDocument ExCEL
    /// </summary>
    /// <param name="objDocumento"></param>
    /// <param name="dttDatos"></param>
    /// <param name="xNombrePestana"></param>
    /// <returns></returns>
    public static SLDocument MapeoDataSetTOExcel(SLDocument objDocumento, DataTable dttDatos, string xNombrePestana)
    {

        Graphics DrawGraphics = null;
        Bitmap TextBitmap = null;
        TextBitmap = new Bitmap(1, 1);
        DrawGraphics = Graphics.FromImage(TextBitmap);
        System.Drawing.Font objfuente = new System.Drawing.Font("Arial", 12);

        float[] matrizWidth = new float[dttDatos.Columns.Count];

        SLStyle stilSubCabeza = new SLStyle();
        stilSubCabeza.Alignment.Horizontal = HorizontalAlignmentValues.Center;
        stilSubCabeza.Border.BottomBorder.BorderStyle = BorderStyleValues.Hair;
        stilSubCabeza.Fill.SetPatternBackgroundColor(System.Drawing.Color.Gray);
        stilSubCabeza.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.GreenYellow, System.Drawing.Color.Gray);
        stilSubCabeza.Font.FontColor = System.Drawing.Color.Black;

        /*CARGA ENCABEZADO DE COLUMNAS*/
        for (int i = 1; i <= dttDatos.Columns.Count; i++)
        {
            objDocumento.SetCellValue(1, i, dttDatos.Columns[i - 1].Caption);
            objDocumento.SetCellStyle(1, i, stilSubCabeza);
            matrizWidth[i - 1] = DrawGraphics.MeasureString(dttDatos.Columns[i - 1].Caption, objfuente).Width / 8;
        }

        /*CARGA CONTENIDO*/
        for (int i = 1; i <= dttDatos.Columns.Count; i++)
        {
            for (int j = 2; j <= dttDatos.Rows.Count + 1; j++)
            {
                if (dttDatos.Columns[i - 1].DataType.FullName == "System.String")
                {
                    objDocumento.SetCellValue(j, i, dttDatos.Rows[j - 2][i - 1].ToString());
                }
                if (dttDatos.Columns[i - 1].DataType.FullName == "System.Int32")
                {
                    objDocumento.SetCellValue(j, i, Convert.ToInt32(dttDatos.Rows[j - 2][i - 1].ToString()));
                }
                if (dttDatos.Columns[i - 1].DataType.FullName == "System.Decimal")
                {
                    SLStyle styloDecimal = new SLStyle();
                    styloDecimal.FormatCode = "0.0000";
                    objDocumento.SetCellStyle(j, i, styloDecimal);
                    objDocumento.SetCellValue(j, i, Convert.ToDecimal(dttDatos.Rows[j - 2][i - 1].ToString()));
                }
                if (dttDatos.Columns[i - 1].DataType.FullName == "System.Double")
                {
                    SLStyle styloDecimal = new SLStyle();
                    styloDecimal.FormatCode = "0.0000";
                    objDocumento.SetCellStyle(j, i, styloDecimal);
                    objDocumento.SetCellValue(j, i, Convert.ToDecimal(dttDatos.Rows[j - 2][i - 1].ToString()));
                }
                if (dttDatos.Columns[i - 1].DataType.FullName == "System.Int64")
                {
                    objDocumento.SetCellValue(j, i, Convert.ToInt64(dttDatos.Rows[j - 2][i - 1].ToString()));
                }
                if (dttDatos.Columns[i - 1].DataType.FullName == "System.DateTime")
                {
                    DateTime dtFechaTemporal = DateTime.MinValue;
                    if (DateTime.TryParse(dttDatos.Rows[j - 2][i - 1].ToString(), out dtFechaTemporal))
                    {
                        SLStyle styloFecha = new SLStyle();
                        styloFecha.FormatCode = "MM/dd/yyyy hh:mm:ss";
                        objDocumento.SetCellStyle(j, i, styloFecha);
                        objDocumento.SetCellValue(j, i, dtFechaTemporal);
                    }
                }
                float largo = DrawGraphics.MeasureString(dttDatos.Rows[j - 2][i - 1].ToString(), objfuente).Width / 8;
                if (largo > matrizWidth[i - 1])
                {
                    matrizWidth[i - 1] = largo;
                }
            }
        }

        /*Carga width de columas*/
        for (int i = 1; i <= dttDatos.Columns.Count; i++)
        {
            objDocumento.SetColumnWidth(i, matrizWidth[i - 1]);
        }

        objDocumento.RenameWorksheet(SLDocument.DefaultFirstSheetName, xNombrePestana);
        return objDocumento;
    }
}
