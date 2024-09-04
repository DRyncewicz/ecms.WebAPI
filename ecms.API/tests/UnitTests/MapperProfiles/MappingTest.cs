using AutoMapper;
using UnitTests.Mapping;

namespace UnitTests.MapperProfiles;

public class MappingTest : IClassFixture<MappingTestFixture>
{
    private readonly IConfigurationProvider _configuration;

    public MappingTest(MappingTestFixture fixture)
    {
        _configuration = fixture.ConfigurationProvider;
    }

    [Fact]
    public void ShouldHaveConfigurationIsValid()
    {
        _configuration.AssertConfigurationIsValid();
    }
}