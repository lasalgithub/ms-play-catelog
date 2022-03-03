using System;

namespace Play.Catalog.Dtos
{
    public record ItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreateDateTime);

    public record CreateItemDto(string Name, string Description, decimal Price);

    public record UpdateItemDto(Guid Id, string Name, string Description, decimal Price);
}

