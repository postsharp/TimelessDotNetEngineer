﻿using Microsoft.EntityFrameworkCore;

namespace OutdoorTodoList.ApiService.Model;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Todo> Todos { get; set; } = null!;
}
