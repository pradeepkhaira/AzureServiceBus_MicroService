#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BurgerKingOrderProcess.MicroService.Models;

    public class OrderProcessContext : DbContext
    {
        public OrderProcessContext (DbContextOptions<OrderProcessContext> options)
            : base(options)
        {
        }

        public DbSet<BurgerKingOrder> BurgerKingOrder { get; set; }
    }
