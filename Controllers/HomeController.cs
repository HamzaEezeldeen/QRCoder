using IronBarCode;
using Microsoft.AspNetCore.Mvc;
using QRCoder.Models;
using System.Diagnostics;

namespace QRCoder.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWebHostEnvironment _environment;


    public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    public IActionResult CreateQRCode()
    {
        return View();
    }
    [HttpPost]
    public IActionResult CreateQRCode(GenerateQRCodeModel generateQRCode)
    {
        try
        {
            GeneratedBarcode barcode = QRCodeWriter.CreateQrCode(generateQRCode.QRCodeText, 200);
            barcode.AddBarcodeValueTextBelowBarcode();
            // Styling a QR code and adding annotation text
            barcode.SetMargins(10);
            barcode.ChangeBarCodeColor(Color.BlueViolet);
            string path = Path.Combine(_environment.WebRootPath, "GeneratedQRCode");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filePath = Path.Combine(_environment.WebRootPath, "GeneratedQRCode/qrcode.png");
            barcode.SaveAsPng(filePath);
            string fileName = Path.GetFileName(filePath);
            string imageUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/GeneratedQRCode/" + fileName;
            ViewBag.QrCodeUri = imageUrl;
        }
        catch (Exception)
        {
            throw;
        }
        return View();
    }
}
