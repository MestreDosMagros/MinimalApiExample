using Application;
using Domain;

namespace MinimalApi
{
    public static class MyEntityControllerDelegates
    {
        public static Func<IMyEntityService, Task<IEnumerable<MyEntityDto>>> GetAll = async (myEntityService) =>
        {
            return (await myEntityService.GetAll())
                .Select(e =>
                    new MyEntityDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Value = e.Value
                    });
        };


        public static Func<IMyEntityService, MyEntityDto, Task<IEnumerable<MyEntityDto>>> Add = async (myEntityService, entityDto) =>
        {
            var entity = new MyEntity
            {
                Name = entityDto.Name,
                Value = entityDto.Value
            };

            return (await myEntityService.Add(entity))
                .Select(e =>
                    new MyEntityDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Value = e.Value
                    });
        };

        public static Func<IMyEntityService, MyEntityDto, Task<IEnumerable<MyEntityDto>>> Update = async (myEntityService, entityDto) =>
        {
            var entity = new MyEntity
            {
                Name = entityDto.Name,
                Value = entityDto.Value
            };

            return (await myEntityService.Update(entity))
                .Select(e =>
                    new MyEntityDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Value = e.Value
                    });
        };

        public static Func<IMyEntityService, Guid, Task<IEnumerable<MyEntityDto>>> Delete = async (myEntityService, id) =>
        {
            return (await myEntityService.Delete(id))
                .Select(e =>
                    new MyEntityDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Value = e.Value
                    });
        };
    }
}
