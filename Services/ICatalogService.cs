
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Play.Catalog.Dtos;

namespace Play.Catalog.Services
{
    public interface ICatalogService
    {
        Task<ItemDto> CreateItem(CreateItemDto createRequest);
        Task<string> DeleteItem(Guid id);
        Task<IEnumerable<ItemDto>> GetAllItems();
        Task<ItemDto> GetItemById(Guid id);
        Task<string> UpdateItem(UpdateItemDto updateRequest);
    }
}