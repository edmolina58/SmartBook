using Microsoft.Extensions.Options;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SmartBook.Application.Services.PDF.Interfaces;
using SmartBook.Domain.Entities.DatabaseEntities;
using SmartBook.Domain.Entities.DomainEntities;

namespace SmartBook.Application.Services.PDF.Implementations
{
    public class VentaPdfFormatter : IVentaPdfFormatter
    {
        private const string FORMATO_FECHA = "dd/MM/yyyy";
        private const string FORMATO_FECHA_HORA = "dd/MM/yyyy HH:mm";
        private readonly PdfSettings _pdfSettings;

        public VentaPdfFormatter(IOptions<PdfSettings> pdfSettings)
        {
            _pdfSettings = pdfSettings.Value;
        }

        public byte[] FormatearVenta(Venta venta)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    ConfigurarPagina(page);
                    GenerarEncabezado(page, venta);
                    GenerarContenido(page, venta);
                    GenerarPieDePagina(page);
                });
            }).GeneratePdf();
        }

        private void ConfigurarPagina(PageDescriptor page)
        {
            page.Size(PageSizes.A4);
            page.Margin(1.5f, Unit.Centimetre);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(x => x.FontSize(9));
        }

        private void GenerarEncabezado(PageDescriptor page, Venta venta)
        {
            page.Header().Column(column =>
            {
                column.Item().Row(row =>
                {

                    row.ConstantItem(100).Column(logoColumn =>
                    {
                        try
                        {
                            var rutaLogo = _pdfSettings.RutaLogo;

                            if (File.Exists(rutaLogo))
                            {
                                logoColumn.Item().Image(rutaLogo).FitWidth();
                            }
                            else
                            {

                                logoColumn.Item()
                                    .Border(1)
                                    .Padding(5)
                                    .AlignCenter()
                                    .Text($"LOGO\n(No encontrado:\n{rutaLogo})")
                                    .FontSize(6);
                            }
                        }
                        catch (Exception ex)
                        {

                            logoColumn.Item()
                                .Border(1)
                                .Padding(5)
                                .AlignCenter()
                                .Text($"ERROR LOGO:\n{ex.Message}")
                                .FontSize(6);
                        }
                    });

                    row.ConstantItem(10);


                    row.RelativeItem().Column(centerColumn =>
                    {
                        centerColumn.Item().AlignCenter().Text($"NIT: {_pdfSettings.Nit}").FontSize(9).SemiBold();
                        centerColumn.Item().AlignCenter().Text($"Teléfono: {_pdfSettings.Telefono}").FontSize(9);
                        centerColumn.Item().PaddingTop(5).AlignCenter().Text("RECIBO DE CAJA").FontSize(11).SemiBold();
                        centerColumn.Item().AlignCenter().Text("SISTEMA DE INFORMACIÓN FINANCIERO - TESORERÍA").FontSize(8);
                    });

                    row.ConstantItem(10);


                    row.ConstantItem(120).Column(rightColumn =>
                    {
                        rightColumn.Item().AlignRight().Text($"{DateTime.Now.ToString(FORMATO_FECHA_HORA)}").FontSize(8);
                        rightColumn.Item().PaddingTop(20).AlignCenter().Text($"{venta.NumeroReciboPago}").FontSize(16).SemiBold();
                    });
                });

                column.Item().PaddingTop(10).LineHorizontal(1).LineColor(Colors.Black);
            });
        }

        private void GenerarContenido(PageDescriptor page, Venta venta)
        {
            page.Content().PaddingTop(10).Column(column =>
            {
                column.Spacing(8);

                GenerarSeccionInformacionGeneral(column, venta);
                GenerarSeccionRecibimos(column, venta);
                GenerarSeccionConcepto(column, venta);
                GenerarTablaDocumentos(column, venta);
                GenerarDetallePago(column, venta);
                GenerarEspacioFirma(column);
            });
        }

        private void GenerarSeccionInformacionGeneral(ColumnDescriptor column, Venta venta)
        {
            column.Item().Border(1).BorderColor(Colors.Black).Padding(5).Row(row =>
            {
                row.RelativeItem().Text(text =>
                {
                    text.Span("Caja: 1 - CAJA GENERAL TESORERÍA  Estado: ").FontSize(8);
                    text.Span("Vigente").FontSize(8).SemiBold();
                });

                row.ConstantItem(150).AlignRight().Text(text =>
                {
                    text.Span("Fecha: ").FontSize(8);
                    text.Span(venta.Fecha.ToString(FORMATO_FECHA)).FontSize(8).SemiBold();
                });

                row.ConstantItem(100).AlignRight().Text(text =>
                {
                    text.Span("Número: ").FontSize(8);
                    text.Span(venta.NumeroReciboPago.ToString()).FontSize(8).SemiBold();
                });
            });
        }

        private void GenerarSeccionRecibimos(ColumnDescriptor column, Venta venta)
        {
            column.Item().Border(1).BorderColor(Colors.Black).Padding(5).Column(innerColumn =>
            {
                innerColumn.Item().Text(text =>
                {
                    text.Span("Recibimos de: ").FontSize(8).SemiBold();
                    text.Span($"CLIENTE ID: {venta.ClienteId}").FontSize(8);
                });

                innerColumn.Item().Row(row =>
                {
                    row.RelativeItem().Text(text =>
                    {
                        text.Span("Teléfono: ").FontSize(8).SemiBold();
                        text.Span("-").FontSize(8);
                    });

                    row.ConstantItem(200).Text(text =>
                    {
                        text.Span("Dirección: ").FontSize(8).SemiBold();
                        text.Span("-").FontSize(8);
                    });

                    row.ConstantItem(120).AlignRight().Text(text =>
                    {
                        text.Span("Consecutivo: ").FontSize(8).SemiBold();
                        text.Span(venta.Id.Substring(0, 8)).FontSize(8);
                    });
                });
            });
        }

        private void GenerarSeccionConcepto(ColumnDescriptor column, Venta venta)
        {
            column.Item().Border(1).BorderColor(Colors.Black).Padding(5).Text(text =>
            {
                text.Span("Por Concepto de: ").FontSize(8).SemiBold();
                text.Span($"VENTA DE LIBRO - ID LIBRO: {venta.LibroId}").FontSize(8);
            });
        }

        private void GenerarTablaDocumentos(ColumnDescriptor column, Venta venta)
        {
            column.Item().Text("Documentos Cancelados").FontSize(9).SemiBold();

            column.Item().Border(1).BorderColor(Colors.Black).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(50);
                    columns.RelativeColumn(3);
                    columns.ConstantColumn(70);
                    columns.ConstantColumn(70);
                    columns.ConstantColumn(80);
                    columns.ConstantColumn(80);
                });

                table.Header(header =>
                {
                    header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(3).Text("# Crédito").FontSize(8).SemiBold();
                    header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(3).Text("Documento").FontSize(8).SemiBold();
                    header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(3).Text("Fecha Dcmto").FontSize(8).SemiBold();
                    header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(3).Text("Vencimiento").FontSize(8).SemiBold();
                    header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(3).Text("Valor Documento").FontSize(8).SemiBold();
                    header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(3).Text("Valor Neto").FontSize(8).SemiBold();
                });

                var total = venta.Unidades * venta.Precio_unidad;

                table.Cell().Border(1).Padding(3).Text(venta.Id.Substring(0, 6)).FontSize(8);
                table.Cell().Border(1).Padding(3).Text("VENTA - LIBRO").FontSize(8);
                table.Cell().Border(1).Padding(3).Text(venta.Fecha.ToString(FORMATO_FECHA)).FontSize(8);
                table.Cell().Border(1).Padding(3).Text(venta.Fecha.AddDays(30).ToString(FORMATO_FECHA)).FontSize(8);
                table.Cell().Border(1).Padding(3).AlignRight().Text($"${total:N2}").FontSize(8);
                table.Cell().Border(1).Padding(3).AlignRight().Text($"${total:N2}").FontSize(8).SemiBold();
            });
        }

        private void GenerarDetallePago(ColumnDescriptor column, Venta venta)
        {
            var total = venta.Unidades * venta.Precio_unidad;

            column.Item().Text("Detalle del Pago Recibido").FontSize(9).SemiBold();

            column.Item().Border(1).BorderColor(Colors.Black).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(3);
                    columns.ConstantColumn(40);
                    columns.ConstantColumn(45);
                    columns.ConstantColumn(60);
                    columns.ConstantColumn(80);
                    columns.ConstantColumn(50);
                    columns.ConstantColumn(45);
                    columns.ConstantColumn(75);
                });

                table.Header(header =>
                {
                    header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(3).Text("Modalidad").FontSize(8).SemiBold();
                    header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(3).Text("Tipo").FontSize(8).SemiBold();
                    header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(3).Text("Ent.").FontSize(8).SemiBold();
                    header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(3).Text("Cuenta").FontSize(8).SemiBold();
                    header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(3).Text("Aprobación").FontSize(8).SemiBold();
                    header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(3).Text("Venc.").FontSize(8).SemiBold();
                    header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(3).Text("Mon.").FontSize(8).SemiBold();
                    header.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(3).Text("Valor").FontSize(8).SemiBold();
                });

                table.Cell().Border(1).Padding(3).Text("EFECTIVO").FontSize(7);
                table.Cell().Border(1).Padding(3).Text("FIN").FontSize(7);
                table.Cell().Border(1).Padding(3).Text("001").FontSize(7);
                table.Cell().Border(1).Padding(3).Text("-").FontSize(7);
                table.Cell().Border(1).Padding(3).Text(venta.Id.Substring(0, 8)).FontSize(7);
                table.Cell().Border(1).Padding(3).Text("-").FontSize(7);
                table.Cell().Border(1).Padding(3).Text("COP").FontSize(7);
                table.Cell().Border(1).Padding(3).AlignRight().Text($"${total:N2}").FontSize(8).SemiBold();

                table.Cell().ColumnSpan(7).Border(1).Background(Colors.Grey.Lighten4).Padding(3).AlignRight().Text("Total:").FontSize(8).SemiBold();
                table.Cell().Border(1).Background(Colors.Grey.Lighten4).Padding(3).AlignRight().Text($"${total:N2}").FontSize(9).SemiBold();
            });
        }

        private void GenerarEspacioFirma(ColumnDescriptor column)
        {
            column.Item().PaddingTop(20).Border(1).BorderColor(Colors.Black).Padding(20)
                .AlignCenter().Text("- CLIENTE -").FontSize(9).SemiBold();
        }

        private void GenerarPieDePagina(PageDescriptor page)
        {
            page.Footer()
                .AlignCenter()
                .PaddingTop(10)
                .Text($"Documento generado por SmartBook el {DateTime.Now.ToString(FORMATO_FECHA_HORA)}")
                .FontSize(7)
                .FontColor(Colors.Grey.Medium);
        }
    }
}