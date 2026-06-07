using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PROG7311_POE_.Models;

namespace PROG7311_POE_.Data
{
    public class PROG7311_POE_Context : DbContext
    {
        public PROG7311_POE_Context (DbContextOptions<PROG7311_POE_Context> options)
            : base(options)
        {
        }

        public DbSet<PROG7311_POE_.Models.Client> Client { get; set; } = default!;
        public DbSet<PROG7311_POE_.Models.Contract> Contract { get; set; } = default!;
        public DbSet<PROG7311_POE_.Models.ServiceRequest> ServiceRequest { get; set; } = default!;
    }
}
