using AutoMapper;
using backend.DTO;
using Xunit;

namespace backend.Tests.DtoUnitTests
{
    public class MappingProfileUnitTests
    {
        [Fact]
        public void MapperConfiguration_IsValid()
        {
            var config = new MapperConfiguration(c => c.AddProfile<MappingProfile>());
            config.AssertConfigurationIsValid();
        }
    }
}