using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Play.Catalog.Contracts;
using Play.Catalog.Dtos;
using Play.Catalog.Entities;
using Play.Catalog.Services;
using Play.Common;

namespace Play.Catalog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = AdminRole)]
    public class CatalogController : ControllerBase
    {
        private const string AdminRole = "Admin";
        private readonly ILogger<CatalogController> _logger;
        private readonly ICatalogService _catalogService;

        public CatalogController(ILogger<CatalogController> logger, ICatalogService catalogService)
        {
            _logger = logger;
            _catalogService = catalogService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAll()
        {
            return Ok(await _catalogService.GetAllItems());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetById(Guid id)
        {
            var item = await _catalogService.GetItemById(id);
            if (item == null)
                return NotFound($"Item not found with id: '{id}'");
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> Create(CreateItemDto createRequest)
        {
            var response = await _catalogService.CreateItem(createRequest);
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult> Update(UpdateItemDto updateRequest)
        {

            var response = await _catalogService.UpdateItem(updateRequest);
            if (response == null)
                return NotFound($"Item not found with id: '{updateRequest.Id}'");
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var response = await _catalogService.DeleteItem(id);
            if (response == null)
                return NotFound($"Item not found with id: '{id}'");
            return Ok(response);
        }
    }
}
