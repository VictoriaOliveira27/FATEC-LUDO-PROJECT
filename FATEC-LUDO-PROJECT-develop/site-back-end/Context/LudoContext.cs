using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class LudoContext : DbContext {
    public DbSet<User> Users { get; set; }
    public DbSet<UserCosmetics> UserCosmetics { get; set; }
    public DbSet<Cosmetic> Cosmetics { get; set; }

    public string DbPath { get; }

    public LudoContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "blogging.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer("Server=ACT061; Database=ludo; Integrated Security=True; Trusted_Connection=True; TrustServerCertificate=True ");
}