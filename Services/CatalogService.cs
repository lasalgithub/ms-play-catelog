
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Play.Catalog;
using Play.Catalog.Contracts;
using Play.Catalog.Dtos;
using Play.Catalog.Entities;
using Play.Common;

namespace Play.Catalog.Services
{
    public class CatalogService : ICatalogService
    {

        private readonly IRepository<Item> _itemRepository;

        private readonly IPublishEndpoint _publishEndpoint;

        public CatalogService(IRepository<Item> itemRepository, IPublishEndpoint publishEndpoint)
        {
            _itemRepository = itemRepository;
            _publishEndpoint = publishEndpoint;
        }


        public async Task<IEnumerable<ItemDto>> GetAllItems()
        {
            return (await _itemRepository.GetAllAsync()).Select(x => x.AsDto()).ToList();
        }

        public async Task<ItemDto> GetItemById(Guid id)
        {
            var item = await _itemRepository.GetAsync(id);
            if (item == null)
                return null;

            return item.AsDto();
        }

        public async Task<ItemDto> CreateItem(CreateItemDto createRequest)
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

            await _publishEndpoint.Publish(new CatalogItemCreated(entity.Id, entity.Name, entity.Description, entity.Price));

            return entity.AsDto();
        }

        public async Task<string> UpdateItem(UpdateItemDto updateRequest)
        {
            //For better user experience
            var item = await _itemRepository.GetAsync(updateRequest.Id);
            if (item == null)
                return null;

            var entity = new Item
            {
                Id = updateRequest.Id,
                Name = updateRequest.Name,
                Description = updateRequest.Description,
                Price = updateRequest.Price
            };

            await _itemRepository.UpdateAsync(entity);

            await _publishEndpoint.Publish(new CatalogItemUpdated(entity.Id, entity.Name, entity.Description, entity.Price));

            return "Updated successfully";
        }

        public async Task<string> DeleteItem(Guid id)
        {
            //For better user experience
            var item = await _itemRepository.GetAsync(id);
            if (item == null)
                return null;
            await _itemRepository.RemoveAsync(id);

            await _publishEndpoint.Publish(new CatalogItemDeleted(id));

            return "Deleted successfully";
        }
    }
}