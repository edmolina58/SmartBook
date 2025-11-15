using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBook.Persistence.Repositories.Interface;

namespace SmartBook.WebApi.Controllers;
[Route("api/[controller]")]
[AllowAnonymous]
[ApiController]
public class AccesosController : ControllerBase
{

    private readonly IUsuarioRepository _usuarioRepository;
    
    
    public AccesosController()
    {
        
    }


}
