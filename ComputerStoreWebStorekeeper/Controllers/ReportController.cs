using ComputerStoreContracts.Services;
using ComputerStoreModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComputerStoreWebStorekeeper.Controllers
{
    public class ReportController : Controller
    {
        private readonly IStorekeeperService _storekeeperService;

        public ReportController(IStorekeeperService storekeeperService)
        {
            _storekeeperService = storekeeperService;
        }

        // Вьюха для Word/Excel отчета
        [HttpGet]
        public async Task<IActionResult> WordExcelReport()
        {
            var user = GetCurrentUser();
            ViewBag.Products = await _storekeeperService.GetAllProductsByUserIdAsync(user.Id);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> WordExcelReport(List<Guid> productIds, string format)
        {
            if (productIds == null || !productIds.Any() || string.IsNullOrEmpty(format))
            {
                ModelState.AddModelError("", "Выберите продукты и формат отчета");
                return await WordExcelReport();
            }

            var reportBytes = await _storekeeperService.GenerateAssembliesByProductsReportAsync(productIds, format);

            string contentType = format.ToLower() switch
            {
                "docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                "xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                _ => "application/octet-stream"
            };

            return File(reportBytes, contentType, $"Report.{format}");
        }

        // Вьюха для PDF отчета
        [HttpGet]
        public async Task<IActionResult> PdfReport()
        {
            var user = GetCurrentUser();
            ViewBag.Components = await _storekeeperService.GetAllComponentsByUserIdAsync(user.Id);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PdfReport(List<Guid> componentIds, DateTime startDate, DateTime endDate, string email)
        {
            if (componentIds == null || !componentIds.Any() || startDate == default || endDate == default)
            {
                ModelState.AddModelError("", "Заполните все поля");
                return await PdfReport();
            }

            if (!string.IsNullOrEmpty(email))
            {
                // Отправка по email
                await _storekeeperService.SendComponentDetailReportByEmailAsync(email, componentIds, startDate, endDate);
                ViewBag.Message = "Отчет отправлен на почту";
                return await PdfReport();
            }
            else
            {
                // Отрисовка данных в таблице
                var data = await _storekeeperService.GetComponentDetailReportData(componentIds, startDate, endDate);
                return View("PdfReportResult", data);
            }
        }

        private User GetCurrentUser()
        {
            var userData = HttpContext.Session.GetString("User");
            return userData != null ?
                System.Text.Json.JsonSerializer.Deserialize<User>(userData)! :
                new User();
        }
    }
}
