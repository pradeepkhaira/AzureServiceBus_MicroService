#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BurgerKingOrderProcess.MicroService.Models;

    public class OrderProcessContext : DbContext
    {
    private string connectionString;

    public OrderProcessContext (DbContextOptions<OrderProcessContext> options)
            : base(options)
        {
        }

    public OrderProcessContext(string connectionString)
    {
        this.connectionString = connectionString;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(connectionString);
    }
    public DbSet<BurgerKingOrder> BurgerKingOrder { get; set; }
    }
