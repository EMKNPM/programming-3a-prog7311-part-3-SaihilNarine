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
            var requests = await _serviceRequestApi.GetServiceRequestsAsync();
            return View(requests);
        }

        // GET: ServiceRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _serviceRequestApi.GetServiceRequestByIdAsync(id.Value);

            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // GET: ServiceRequests/Create
        public async Task<IActionResult> Create()
        {
            var contracts = await _contractApi.GetContractsAsync();

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
            var contracts = await _contractApi.GetContractsAsync();

            var contract = contracts.FirstOrDefault(c => c.ContractID == serviceRequest.ContractID);

            var validator = new ContractValidator();
            string error;

            if (!validator.CanCreateRequest(contract, out error))
            {
                ModelState.AddModelError("", error);

                ViewData["ContractID"] = new SelectList(contracts, "ContractID", "ContractID");

                return View(serviceRequest);
            }

            serviceRequest.Cost = await _currencyService.ConvertUsdToZar(serviceRequest.Cost);

            await _serviceRequestApi.CreateServiceRequestAsync(serviceRequest);

            return RedirectToAction(nameof(Index));
        }

        // GET: ServiceRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var request = await _serviceRequestApi.GetServiceRequestByIdAsync(id.Value);

            if (request == null) return NotFound();

            var contracts = await _contractApi.GetContractsAsync();

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
            if (id != serviceRequest.ServiceRequestID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var contracts = await _contractApi.GetContractsAsync();

                ViewData["ContractID"] = new SelectList(contracts, "ContractID", "ContractID", serviceRequest.ContractID);

                return View(serviceRequest);
            }

            await _serviceRequestApi.UpdateServiceRequestAsync(serviceRequest);

            return RedirectToAction(nameof(Index));
        }

        // GET: ServiceRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
                
            var request = await _serviceRequestApi.GetServiceRequestByIdAsync(id.Value);

            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // POST: ServiceRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _serviceRequestApi.DeleteServiceRequestAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
