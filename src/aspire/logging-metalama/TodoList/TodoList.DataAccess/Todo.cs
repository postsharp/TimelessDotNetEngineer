// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using System.ComponentModel.DataAnnotations;

public partial class Todo
{
    public int Id { get; set; }

    public bool IsCompleted { get; set; }

    [Required]
    public string Title { get; set; } = null!;

    public override string ToString() => $"\"{this.Title}\"";
}