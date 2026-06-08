using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROG7311_POE_.Models;
using PROG7311_POE_.Services;
using PROG7311_POE_.Validation;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PROG7311_POE_.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly ServiceRequestApiService _serviceRequestApi;
        private readonly ContractApiService _contractApi;
        private readonly CurrencyService _currencyService;

        public ServiceRequestsController(
            ServiceRequestApiService serviceRequestApi,
            ContractApiService contractApi,
            CurrencyService currencyService)
        {
            _serviceRequestApi = serviceRequestApi;
            _contractApi = contractApi;
            _currencyService = currencyService;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JWT");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            try
            {
                var requests = await _serviceRequestApi.GetServiceRequestsAsync(token);
                return View(requests);
            }
            catch (Exception ex)
            {
                return Content(ex.Message); // show real API errors
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            var token = HttpContext.Session.GetString("JWT");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            if (id == null)
                return NotFound();

            var request = await _serviceRequestApi.GetServiceRequestByIdAsync(id.Value, token);

            if (request == null)
                return NotFound();

            return View(request);
        }

        public async Task<IActionResult> Create()
        {
            var token = HttpContext.Session.GetString("JWT");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            var contracts = await _contractApi.GetContractsAsync(token);

            ViewData["ContractID"] = new SelectList(contracts, "ContractID", "ContractID");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest serviceRequest)
        {
            var token = HttpContext.Session.GetString("JWT");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            var contracts = await _contractApi.GetContractsAsync(token);

            var contract = contracts.FirstOrDefault(c => c.ContractID == serviceRequest.ContractID);

            var validator = new ContractValidator();
            string error;

            if (!validator.CanCreateRequest(contract, out error))
            {
                ModelState.AddModelError("", error);
                ViewData["ContractID"] = new SelectList(contracts, "ContractID", "ContractID");
                return View(serviceRequest);
            }

            // Convert currency
            serviceRequest.Cost =
                await _currencyService.ConvertUsdToZar(serviceRequest.Cost);

            // 🔥 CRITICAL FIX (prevents API validation error)
            serviceRequest.Contract = null;

            try
            {
                await _serviceRequestApi.CreateServiceRequestAsync(serviceRequest, token);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var token = HttpContext.Session.GetString("JWT");

            if (id == null)
                return NotFound();

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            var request = await _serviceRequestApi.GetServiceRequestByIdAsync(id.Value, token);

            if (request == null)
                return NotFound();

            var contracts = await _contractApi.GetContractsAsync(token);

            ViewData["ContractID"] =
                new SelectList(contracts, "ContractID", "ContractID", request.ContractID);

            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceRequest serviceRequest)
        {
            var token = HttpContext.Session.GetString("JWT");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            if (id != serviceRequest.ServiceRequestID)
                return NotFound();

            if (!ModelState.IsValid)
            {
                var contracts = await _contractApi.GetContractsAsync(token);

                ViewData["ContractID"] =
                    new SelectList(contracts, "ContractID", "ContractID", serviceRequest.ContractID);

                return View(serviceRequest);
            }

            serviceRequest.Contract = null;

            try
            {
                await _serviceRequestApi.UpdateServiceRequestAsync(serviceRequest, token);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        // DELETE (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            var token = HttpContext.Session.GetString("JWT");

            if (id == null)
                return NotFound();

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            var request = await _serviceRequestApi.GetServiceRequestByIdAsync(id.Value, token);

            if (request == null)
                return NotFound();

            return View(request);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var token = HttpContext.Session.GetString("JWT");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            try
            {
                await _serviceRequestApi.DeleteServiceRequestAsync(id, token);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}