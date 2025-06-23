using Microsoft.AspNetCore.Mvc;
using PharmaGo.Domain.Entities;
using PharmaGo.IBusinessLogic;
using PharmaGo.WebApi.Enums;
using PharmaGo.WebApi.Filters;
using PharmaGo.WebApi.Models.In;
using PharmaGo.WebApi.Models.Out;

namespace PharmaGo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ExceptionFilter))]
    public class CosmeticController : Controller
    {
        private readonly ICosmeticManager _cosmeticManager;
        
        public CosmeticController(ICosmeticManager manager)
        {
            _cosmeticManager = manager;
        }
        
        [HttpPost]
        [AuthorizationFilter(new string[] { nameof(RoleType.Employee) })]
        public IActionResult Create([FromBody] CosmeticRequestModel cosmeticRequestModel)
        {
            string token = HttpContext.Request.Headers["Authorization"];
            Cosmetic cosmetic = _cosmeticManager.Create(cosmeticRequestModel.ToEntity(), token);
            return Ok(new CosmeticResponseModel(cosmetic));
        }
        
        [HttpGet]
        public IActionResult GetAll([FromQuery] int pharmacyId)
        {
                var cosmetics = _cosmeticManager.GetAll(pharmacyId);
                CosmeticResponseModel[] cosmeticsToReturn = cosmetics.Select(c => new CosmeticResponseModel(c)).ToArray();
                return Ok(cosmeticsToReturn);
        }
    }
}