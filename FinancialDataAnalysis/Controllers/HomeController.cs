using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using FinancialDataAnalysis.Models;
using System.Data;
using FinancialDataAnalysis.Dto;

namespace FinancialDataAnalysis.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IDbContext context, ILogger<HomeController> logger)
        {
            _context=context;
            _logger=logger;
        }

        // GET: Home
        public IActionResult Index()
        {
            ViewBag.AssetNames = _context.GetAssetNames();
            ViewBag.MinDate = _context.GetMinDate();
            ViewBag.MaxDate = _context.GetMaxDate();

            return View();
        }

        //POST /Volitility   Volitility
        [HttpPost]
        public IActionResult Volitility([FromBody] MainRequest req)
        {
            try
            {
                bool assetNameInvalid = int.TryParse(req.AssetName, out _);
                if (assetNameInvalid)
                {
                    throw new InvalidDataException("Invalid asset name");
                }


                MainResponse res = _context.GetAssetVolitility(req.AssetName,req.StartDate, req.EndDate);
                var response = new GenericResponse<MainResponse>(res);
                return Json(response);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return Json(new GenericResponse<MainResponse>(ex));
            }
        }

        //POST /Correlation   Correlation
        [HttpPost]
        public IActionResult Correlation([FromBody] CorrelationRequest req)
        {
            try
            {
                bool assetNameInvalid = int.TryParse(req.AssetName, out _);
                bool assetName2Invalid = int.TryParse(req.Asset2Name, out _);
                if (assetNameInvalid || assetName2Invalid)
                {
                    throw new InvalidDataException("Invalid asset name");
                }

                MainResponse res = _context.GetAssetCorrelation(req.AssetName, req.Asset2Name, req.StartDate, req.EndDate);
                var response = new GenericResponse<MainResponse>(res);
                return Json(response);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return Json(new GenericResponse<MainResponse>(ex));
            }
        }


        //POST /Returns   Returns
        [HttpPost]
        public IActionResult Returns([FromBody] ReturnRequest req)
        {
            try
            {
                bool assetNameInvalid = int.TryParse(req.AssetName, out _);
                if (assetNameInvalid)
                {
                    throw new InvalidDataException("Invalid asset name");
                }

                MainResponse res = _context.GetAssetReturns(req.AssetName, req.Amount, req.StartDate, req.EndDate);
                var response = new GenericResponse<MainResponse>(res);
                return Json(response);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return Json(new GenericResponse<MainResponse>(ex));
            }
        }


        


    }
}
