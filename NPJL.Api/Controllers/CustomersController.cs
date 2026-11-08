using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NPJL.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    // Endast inloggade användare med rollen Customer
    // får använda funktionerna i denna controller.
    [Authorize(Roles = "Customer")]
    public class CustomersController : ControllerBase
    {
        
    }
}