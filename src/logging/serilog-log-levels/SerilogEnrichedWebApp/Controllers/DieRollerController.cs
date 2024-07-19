// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Microsoft.AspNetCore.Mvc;

namespace SerilogEnrichedWebApp.Controllers;

public class DieRollerController( DieRoller dieRoller ) : Controller
{
    public ActionResult<string> Index( int sides = 6 )
    {
        var result = dieRoller.Roll( sides );

        var dieShape = GetShapeName( sides );

        return $"Rolled {result} on a {sides}-sided die ({dieShape}).";
    }

    private static string GetShapeName( int sides )
        => sides switch
        {
            4 => "tetrahedron",
            6 => "cube",
            8 => "octahedron",
            10 => "pentagonal trapezohedron",
            12 => "dodecahedron",
            20 => "icosahedron",
            _ => throw new NotImplementedException()
        };
}