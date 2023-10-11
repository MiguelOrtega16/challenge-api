﻿using challenge_api_base.Models;
using Microsoft.EntityFrameworkCore;

namespace challenge_api_base.Data;

public class AppDbContext : DbContext
{
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Sucursal> Sucursales { get; set; }
    public DbSet<InformacionContacto> InformacionesContacto { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
}