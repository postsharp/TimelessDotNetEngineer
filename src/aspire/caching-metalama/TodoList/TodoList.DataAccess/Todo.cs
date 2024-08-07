﻿// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.ComponentModel.DataAnnotations;

// [<snippet CacheKey>]
public partial class Todo
{
    public int Id { get; set; }

    public bool IsCompleted { get; set; }

    [Required]
    public string Title { get; set; } = null!;
}

// [<endsnippet CacheKey>]