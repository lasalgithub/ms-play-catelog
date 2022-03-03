using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Play.Catalog.Contracts;
using Play.Catalog.Dtos;
using Play.Catalog.Entities;
using Play.Common;

namespace Play.Catalog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IRepository<Item> _itemRepository;
        private readonly ILogger<CatalogController> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public CatalogController(ILogger<CatalogController> logger, IRepository<Item> itemRepository,
         IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _itemRepository = itemRepository;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAll()
        {
            return Ok((await _itemRepository.GetAllAsync()).Select(x => x.AsDto()));
        }

        [HttpGet("{id}")]
        public async Task<ItemDto> GetById(Guid id)
        {
            return (await _itemRepository.GetAsync(id)).AsDto();
        }

        [HttpPost]
        public async Task Create(CreateItemDto createRequest)
        {
            var entity = new Item
            {
                Id = Guid.NewGuid(),
                Name = createRequest.Name,
                Description = createRequest.Description,
                Price = createRequest.Price,
                CreatedDate = DateTimeOffset.Now
            };
            await _itemRepository.CreateAsync(entity);

            await _publishEndpoint.Publish(new CatalogItemCreated(entity.Id, entity.Name, entity.Description));
        }

        [HttpPut]
        public async Task Update(UpdateItemDto updateRequest)
        {
            var entity = new Item
            {
                Id = updateRequest.Id,
                Name = updateRequest.Name,
                Description = updateRequest.Description,
                Price = updateRequest.Price
            };

            await _itemRepository.UpdateAsync(entity);

            await _publishEndpoint.Publish(new CatalogItemUpdated(entity.Id, entity.Name, entity.Description));
        }

        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            await _itemRepository.RemoveAsync(id);

            await _publishEndpoint.Publish(new CatalogItemDeleted(id));
        }
    }
}
