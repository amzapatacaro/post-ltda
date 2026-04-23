using Business.Dtos;
using DataAccess.Data;

namespace Business.Mapping
{
    internal static class MappingExtensions
    {
        public static CustomerDto ToDto(this Customer entity)
        {
            if (entity == null)
                return null;

            return new CustomerDto { CustomerId = entity.CustomerId, Name = entity.Name };
        }

        public static Customer ToCustomer(this CustomerDto dto)
        {
            if (dto == null)
                return null;

            return new Customer { CustomerId = dto.CustomerId, Name = dto.Name };
        }

        public static PostDto ToDto(this Post entity)
        {
            if (entity == null)
                return null;

            return new PostDto
            {
                PostId = entity.PostId,
                Title = entity.Title,
                Body = entity.Body,
                Type = entity.Type,
                Category = entity.Category,
                CustomerId = entity.CustomerId
            };
        }

        public static Post ToPost(this PostDto dto)
        {
            if (dto == null)
                return null;

            return new Post
            {
                PostId = dto.PostId,
                Title = dto.Title,
                Body = dto.Body,
                Type = dto.Type,
                Category = dto.Category,
                CustomerId = dto.CustomerId
            };
        }
    }
}
