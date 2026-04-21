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
        private readonly PROG7311_POE_Context _context;
        private readonly CurrencyService _currencyService;

        public ServiceRequestsController(PROG7311_POE_Context context, CurrencyService currencyService)
        {
            _context = context;
            _currencyService = currencyService;
        }

        // GET: ServiceRequests
        public async Task<IActionResult> Index()
        {
            var pROG7311_POE_Context = _context.ServiceRequest.Include(s => s.Contract);
            return View(await pROG7311_POE_Context.ToListAsync());
        }

        // GET: ServiceRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequest
                .Include(s => s.Contract)
                .FirstOrDefaultAsync(m => m.ServiceRequestID == id);
            if (serviceRequest == null)
            {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // GET: ServiceRequests/Create
        public IActionResult Create()
        {
            ViewData["ContractID"] = new SelectList(_context.Contract, "ContractID", "ContractID");
            return View();
        }

        // POST: ServiceRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest serviceRequest)
        {
            var contract = await _context.Contract
                .FirstOrDefaultAsync(c => c.ContractID == serviceRequest.ContractID);

            var validator = new ContractValidator();
            string error;

            if (!validator.CanCreateRequest(contract, out error))
            {
                ModelState.AddModelError("", error);
                return View(serviceRequest);
            }

            // FIXED: use injected service (NOT Singleton)
            serviceRequest.Cost =
                await _currencyService.ConvertUsdToZar(serviceRequest.Cost);

            _context.Add(serviceRequest);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: ServiceRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequest.FindAsync(id);
            if (serviceRequest == null)
            {
                return NotFound();
            }
            ViewData["ContractID"] = new SelectList(_context.Contract, "ContractID", "ContractID", serviceRequest.ContractID);
            return View(serviceRequest);
        }

        // POST: ServiceRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceRequestID,ContractID,Description,Cost,Status")] ServiceRequest serviceRequest)
        {
            if (id != serviceRequest.ServiceRequestID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceRequestExists(serviceRequest.ServiceRequestID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContractID"] = new SelectList(_context.Contract, "ContractID", "ContractID", serviceRequest.ContractID);
            return View(serviceRequest);
        }

        // GET: ServiceRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequest
                .Include(s => s.Contract)
                .FirstOrDefaultAsync(m => m.ServiceRequestID == id);
            if (serviceRequest == null)
            {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // POST: ServiceRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceRequest = await _context.ServiceRequest.FindAsync(id);
            if (serviceRequest != null)
            {
                _context.ServiceRequest.Remove(serviceRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceRequestExists(int id)
        {
            return _context.ServiceRequest.Any(e => e.ServiceRequestID == id);
        }
    }
}
