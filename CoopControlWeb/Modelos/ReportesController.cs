using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoopControlWeb.Modelos;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;

namespace CoopControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly IDbContextFactory<AppDBContext> _dbFactory;

        public ReportesController(IDbContextFactory<AppDBContext> dbFactory)
        {
            _dbFactory = dbFactory;
            // Configuración de licencia de QuestPDF (Gratis para uso comunitario/pequeñas empresas)
            QuestPDF.Settings.License = LicenseType.Community;
        }

        [HttpGet("certificado-pdf/{id}")]
        public async Task<IActionResult> GenerarCertificadoPdf(int id)
        {
            using var context = _dbFactory.CreateDbContext();
            var prestamo = await context.Set<Prestamo>()
                .Include(p => p.Socio)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prestamo == null) return NotFound("Préstamo no encontrado");

            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A5.Landscape());
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily(Fonts.Verdana));

                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("COOPCONTROL").FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);
                            col.Item().Text("Certificado de Préstamo Oficial");
                        });
                        row.RelativeItem().AlignRight().Text(DateTime.Now.ToString("dd/MM/yyyy"));
                    });

                    page.Content().PaddingVertical(1, Unit.Centimetre).Column(x =>
                    {
                        x.Spacing(20);
                        x.Item().Text($"Por la presente se certifica que el socio:").Bold();
                        x.Item().Text($"{prestamo.Socio?.Nombre} {prestamo.Socio?.Apellido}").FontSize(16).SemiBold();
                        
                        x.Item().LineHorizontal(1f);

                        x.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(150);
                                columns.RelativeColumn();
                            });
                            
                            table.Cell().PaddingVertical(5).Text("Monto del Préstamo:");
                            table.Cell().PaddingVertical(5).Text($"RD$ {prestamo.Monto:N2}").Bold();
                            
                            table.Cell().PaddingVertical(5).Text("Fecha de Emisión:");
                            table.Cell().PaddingVertical(5).Text(prestamo.FechaPrestamo.ToString("dd/MM/yyyy"));
                            
                            table.Cell().PaddingVertical(5).Text("Tipo de Préstamo:");
                            table.Cell().PaddingVertical(5).Text(prestamo.TipoPrestamo ?? "N/A");
                        });
                    });

                    page.Footer().AlignCenter().Text(x => { x.Span("Página "); x.CurrentPageNumber(); });
                });
            });

            byte[] pdfBytes = documento.GeneratePdf();
            return File(pdfBytes, "application/pdf", $"Certificado_{id}.pdf");
        }

        [HttpGet("comprobante-aporte-pdf/{id}")]
        public async Task<IActionResult> GenerarComprobanteAportePdf(int id)
        {
            using var context = _dbFactory.CreateDbContext();
            var aporte = await context.Aportes
                .Include(a => a.Socio)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (aporte == null) return NotFound("Aporte no encontrado");

            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    // Formato de recibo (A5 Horizontal)
                    page.Size(PageSizes.A5.Landscape());
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily(Fonts.Verdana));

                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("COOPCONTROL").FontSize(18).SemiBold().FontColor(Colors.Green.Medium);
                            col.Item().Text("Comprobante de Aporte").FontSize(14);
                        });
                        row.RelativeItem().AlignRight().Column(col => {
                            col.Item().Text($"Recibo No: #{aporte.Id}").SemiBold();
                            col.Item().Text(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                        });
                    });

                    page.Content().PaddingVertical(0.5f, Unit.Centimetre).Column(x =>
                    {
                        x.Spacing(10);
                        x.Item().LineHorizontal(0.5f);

                        x.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(120);
                                columns.RelativeColumn();
                            });
                            
                            table.Cell().Text("Socio:").Bold();
                            table.Cell().Text($"{aporte.Socio?.Nombre} {aporte.Socio?.Apellido}");
                            
                            table.Cell().Text("Cédula:");
                            table.Cell().Text(aporte.Socio?.Cedula ?? "N/A");
                            
                            table.Cell().Text("Tipo de Aporte:");
                            table.Cell().Text(aporte.Tipo ?? "Aporte Ordinario");

                            table.Cell().PaddingTop(10).Text("MONTO:").Bold().FontSize(14);
                            table.Cell().PaddingTop(10).Text($"RD$ {aporte.Monto:N2}").Bold().FontSize(14).FontColor(Colors.Green.Darken2);
                        });
                    });

                    page.Footer().AlignCenter().Text(x => { x.Span("Documento generado por el Sistema CoopControl Web").FontSize(8).Italic(); });
                });
            });

            byte[] pdfBytes = documento.GeneratePdf();
            return File(pdfBytes, "application/pdf", $"Comprobante_{id}.pdf");
        }

        [HttpGet("informe-global-pdf")]
        public async Task<IActionResult> GenerarInformeGlobalPdf()
        {
            using var context = _dbFactory.CreateDbContext();

            // Obtener métricas globales
            var totalAportes = await context.Aportes.SumAsync(a => a.Monto);
            var totalAhorros = await context.Ahorros.SumAsync(a => a.Saldo);
            var totalPrestamos = await context.Prestamos.Where(p => p.Estado == "Activo").SumAsync(p => p.Monto);
            var totalSocios = await context.Socios.CountAsync();

            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1.5f, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily(Fonts.Verdana));

                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("COOPCONTROL").FontSize(22).Bold().FontColor(Colors.Blue.Medium);
                            col.Item().Text("Informe Consolidado de Operaciones").FontSize(14).SemiBold();
                        });
                        row.RelativeItem().AlignRight().Column(col =>
                        {
                            col.Item().Text(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                            col.Item().Text("Reporte Administrativo").FontSize(9);
                        });
                    });

                    page.Content().PaddingVertical(1, Unit.Centimetre).Column(x =>
                    {
                        x.Spacing(20);
                        x.Item().Text("Resumen Ejecutivo Financiero").FontSize(16).SemiBold().Underline();

                        x.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Cell().PaddingVertical(5).Text("Total de Socios Registrados:");
                            table.Cell().PaddingVertical(5).Text($"{totalSocios}");

                            table.Cell().PaddingVertical(5).Text("Cartera Total de Aportes:");
                            table.Cell().PaddingVertical(5).Text($"RD$ {totalAportes:N2}").Bold();

                            table.Cell().PaddingVertical(5).Text("Cartera Total de Ahorros:");
                            table.Cell().PaddingVertical(5).Text($"RD$ {totalAhorros:N2}").Bold();

                            table.Cell().PaddingVertical(5).Text("Cartera de Préstamos Activos:");
                            table.Cell().PaddingVertical(5).Text($"RD$ {totalPrestamos:N2}").Bold();
                        });

                        x.Item().PaddingTop(30).LineHorizontal(1f);
                        x.Item().AlignCenter().Text("Fin del Informe Global").FontSize(10).Italic();
                    });

                    page.Footer().AlignCenter().Text(x => { x.Span("Página "); x.CurrentPageNumber(); });
                });
            });

            byte[] pdfBytes = documento.GeneratePdf();
            return File(pdfBytes, "application/pdf", $"InformeGlobal_{DateTime.Now:yyyyMMdd}.pdf");
        }
    }
}