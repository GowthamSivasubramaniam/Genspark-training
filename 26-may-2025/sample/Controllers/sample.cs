using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class SampleController: ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult GetGreet()
    {
        return Created("", "Hello, World!");
    }
}