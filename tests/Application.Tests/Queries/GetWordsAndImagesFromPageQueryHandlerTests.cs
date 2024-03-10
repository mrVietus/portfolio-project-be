using AutoFixture;
using Crawler.Application.Common.Interfaces;
using Crawler.Application.Crawler.Queries.GetWordsAndImagesFromPage;
using Crawler.Application.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute.ExceptionExtensions;

namespace Crawler.Application.Tests.Queries;

[TestFixture]
public class GetWordsAndImagesFromPageQueryHandlerTests
{
    private GetWordsAndImagesFromPageQueryHandler? _sut;

    private ILogger<GetWordsAndImagesFromPageQueryHandler>? _logger;
    private ICrawlingService? _crawlingService;
    private IOptions<CrawlerSettings>? _options;

    private readonly int _countOfTopWords = 10;

    [SetUp]
    public void SetUp()
    {
        _logger = Substitute.For<ILogger<GetWordsAndImagesFromPageQueryHandler>>();
        _crawlingService = Substitute.For<ICrawlingService>();
        _options = Options.Create(new CrawlerSettings()
        {
            CountOfTopWordsThatWillBeReturned = _countOfTopWords
        });

        _sut = new GetWordsAndImagesFromPageQueryHandler(_crawlingService, _logger, _options);
    }

    [Test, AutoData]
    public async Task Handle_WhenCrawlingSucceeds_ReturnsResponseWithUrlTopWordsAndImages(string url, [Frozen] List<string> urls, [Frozen] List<string> words)
    {
        // Arrange
        var fixture = new Fixture
        {
            RepeatCount = 100
        };
        fixture.AddManyTo(words);

        var crawlingResult = new CrawlResult()
        {
            ImageUrls = urls,
            Words = words
        };

        var query = new GetWordsAndImagesFromPageQuery(url);
        var cancellationToken = new CancellationToken();

        _crawlingService?.CrawlAsync(url, cancellationToken)
            .Returns(crawlingResult);

        // Act
        var result = await _sut!.Handle(query, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Value.Url.Should().Be(url);
        result.Value.Images.Should().BeEquivalentTo(crawlingResult.ImageUrls);
        result.Value.TopWords.Should().HaveCount(_countOfTopWords);
        result.Value.PageWordsCount.Should().Be(crawlingResult.WordsCount);

        await _crawlingService.Received(1)!.CrawlAsync(url, cancellationToken);
    }

    [Test, AutoData]
    public async Task Handle_WhenCrawlingFails_ReturnsError(string url)
    {
        // Arrange
        var query = new GetWordsAndImagesFromPageQuery(url);
        var cancellationToken = new CancellationToken();

        _crawlingService!.CrawlAsync(url, cancellationToken)
            .Throws(new Exception("Crawling error"));


        // Assert
        Assert.ThrowsAsync<Exception>(() => _sut!.Handle(query, cancellationToken));
        await _crawlingService.Received(1).CrawlAsync(url, cancellationToken);
    }
}
