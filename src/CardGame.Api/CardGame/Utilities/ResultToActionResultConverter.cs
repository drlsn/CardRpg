using CardGame.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CardGame.Utilities;

public static class ResultToActionResultConverter
{
    public static IActionResult ToActionResult(this Result result) =>
        result.IsSuccess ? new OkObjectResult(result) : new ObjectResult(result) { StatusCode = 400 };
}
