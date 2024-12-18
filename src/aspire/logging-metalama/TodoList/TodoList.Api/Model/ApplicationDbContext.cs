// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Microsoft.EntityFrameworkCore;

namespace TodoList.ApiService.Model;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext( DbContextOptions<ApplicationDbContext> options )
        : base( options ) { }

    public DbSet<Todo> Todos { get; set; } = null!;
}