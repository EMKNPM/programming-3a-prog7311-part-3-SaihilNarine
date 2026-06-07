using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROG7311_POE_.Data;
using PROG7311_POE_.Models;
using PROG7311_POE_.Services;
using PROG7311_POE_.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROG7311_POE_.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly ServiceRequestApiService _serviceRequestApi;
        private readonly ContractApiService _contractApi;
        private readonly CurrencyService _currencyService;

        public ServiceRequestsController(ServiceRequestApiService serviceRequestApi, ContractApiService contractApi, CurrencyService currencyService)
        {
            _serviceRequestApi = serviceRequestApi;
            _contractApi = contractApi;
            _currencyService = currencyService;
        }

        // GET: ServiceRequests
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JWT");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var requests = await _serviceRequestApi.GetServiceRequestsAsync(token);
            return View(requests);
        }

        // GET: ServiceRequests/Details/5
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

        // GET: ServiceRequests/Create
        public async Task<IActionResult> Create()
        {
            var token = HttpContext.Session.GetString("JWT");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }
                

            var contracts = await _contractApi.GetContractsAsync(token);

            ViewData["ContractID"] = new SelectList(contracts, "ContractID", "ContractID");

            return View();
        }

        // POST: ServiceRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

            serviceRequest.Cost =
                await _currencyService.ConvertUsdToZar(serviceRequest.Cost);

            await _serviceRequestApi.CreateServiceRequestAsync(serviceRequest, token);

            return RedirectToAction(nameof(Index));
        }

        // GET: ServiceRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var token = HttpContext.Session.GetString("JWT");

            if (id == null) return NotFound();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Account");

            var request = await _serviceRequestApi.GetServiceRequestByIdAsync(id.Value, token);

            if (request == null) return NotFound();

            var contracts = await _contractApi.GetContractsAsync(token);

            ViewData["ContractID"] = new SelectList(contracts, "ContractID", "ContractID", request.ContractID);

            return View(request);
        }

        // POST: ServiceRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

                ViewData["ContractID"] = new SelectList(contracts, "ContractID", "ContractID", serviceRequest.ContractID);

                return View(serviceRequest);
            }

            await _serviceRequestApi.UpdateServiceRequestAsync(serviceRequest, token);

            return RedirectToAction(nameof(Index));
        }

        // GET: ServiceRequests/Delete/5
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

        // POST: ServiceRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var token = HttpContext.Session.GetString("JWT");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            await _serviceRequestApi.DeleteServiceRequestAsync(id, token);

            return RedirectToAction(nameof(Index));
        }
    }
}
