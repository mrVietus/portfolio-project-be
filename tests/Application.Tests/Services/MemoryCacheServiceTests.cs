using Application.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Application.Tests.Services;

[TestFixture]
public class MemoryCacheServiceTests
{
    private MemoryCacheService _sut;
    private IMemoryCache _memoryCache;
    private IOptions<CrawlerSettings> _options;

    [SetUp]
    public void SetUp()
    {
        _memoryCache = Substitute.For<IMemoryCache>();
        _options = Options.Create(new CrawlerSettings()
        {
            CacheItemsTimeSpanInDays = 1
        });

        _sut = new MemoryCacheService(_memoryCache, _options);
    }

    [Test]
    public void GetFromCache_WhenKeyExists_ReturnsValue()
    {
        // Arrange
        var key = "some-key";
        var value = "some-value";
        _memoryCache.TryGetValue(key, out Arg.Any<object>()).Returns(x =>
        {
            x[1] = value;
            return true;
        });

        // Act
        var result = _sut.GetFromCache<string>(key);

        // Assert
        result.Should().Be(value);
    }

    [Test]
    public void GetFromCache_WhenKeyDoesNotExist_ReturnsNull()
    {
        // Arrange
        var key = "some-key";
        _memoryCache.TryGetValue(key, out Arg.Any<object>())
            .Returns(false);

        // Act
        var result = _sut.GetFromCache<string>(key);

        // Assert
        result.Should().BeNull();
    }

    [Test, AutoData]
    public void SetCache_WhenCalled_SetsValueWithExpiration(int expirationTimeInDays)
    {
        // Arrange
        var key = "some-key";
        var value = "some-value";
        var options = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromDays(expirationTimeInDays));

        _options = Options.Create(new CrawlerSettings()
        {
            CacheItemsTimeSpanInDays = expirationTimeInDays
        });

        // Act
        _sut.SetCache<string>(key, value);

        // Assert
        _memoryCache.Received(1).Set(key, value, options);
    }
}
