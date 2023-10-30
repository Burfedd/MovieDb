using Blazored.LocalStorage;
using Moq;
using MovieDb.WebApp.Services.Storage;
using Xunit;

namespace MovieDb.Tests.MovieDb.WebApp.Tests.Services.Storage
{
    public class SavedQueriesServiceTests
    {
        private SavedQueriesService? _service;
        private Mock<ILocalStorageService> _localStorageServiceMock;

        public SavedQueriesServiceTests() {
            _localStorageServiceMock = new Mock<ILocalStorageService>();
        }

        [Fact]
        public void GivenEmptyStorage_WhenGetQueries_ThenEmptyList()
        {
            // Arrange
            _localStorageServiceMock.Setup(s => s.GetItemAsync<IList<string>>(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(null);
            _service = new SavedQueriesService(_localStorageServiceMock.Object);

            // Act
            IList<string> result = _service.GetLastSearchQueriesAsync().Result;

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 0);
        }

        [Fact]
        public void GivenOneStorageEntry_WhenGetQueries_ThenSameList()
        {
            // Arrange
            Mock<IList<string>> listMock = new Mock<IList<string>>();
            listMock.Setup(l => l.Count).Returns(1);
            _localStorageServiceMock.Setup(s => s.GetItemAsync<IList<string>>(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(ValueTask.FromResult(listMock.Object));
            _service = new SavedQueriesService(_localStorageServiceMock.Object);

            // Act
            IList<string> result = _service.GetLastSearchQueriesAsync().Result;

            // Assert
            Assert.Same(listMock.Object, result);
        }

        [Fact]
        public void GivenSixStorageEntries_WhenGetQueries_ThenOnlyFive()
        {
            // Arrange
            IList<string> list = new List<string> {
                "one", "two", "three", "four", "five", "six"
            };
            _localStorageServiceMock.Setup(s => s.GetItemAsync<IList<string>>(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(ValueTask.FromResult(list));
            _service = new SavedQueriesService(_localStorageServiceMock.Object);
            int expectedResult = 5;

            // Act
            int result = _service.GetLastSearchQueriesAsync().Result.Count;

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task GivenEmptyStorage_WhenPushQuery_ThenNewListCreated()
        {
            // Arrange
            _localStorageServiceMock.Setup(s => s.GetItemAsync<IList<string>>(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(null);
            IList<string> result = null;
            _localStorageServiceMock.Setup(s => s.SetItemAsync(It.IsAny<string>(), It.IsAny<IList<string>>(), It.IsAny<CancellationToken>()))
                .Callback<string, IList<string>, CancellationToken>((s, l, c) =>
                {
                    result = l;
                });
            _service = new SavedQueriesService(_localStorageServiceMock.Object);

            // Act
            await _service.PushNewSearchQueryAsync("test value");

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 1);
            Assert.True(result.Contains("test value"));
        }

        [Fact]
        public async Task GivenTwoEntries_WhenPushQuery_ThenThreeQueries()
        {
            // Arrange
            IList<string> list = new List<string> {
                "one", "two", "three", "four", "five", "six"
            };
            _localStorageServiceMock.Setup(s => s.GetItemAsync<IList<string>>(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(ValueTask.FromResult(list));
            IList<string> result = null;
            _localStorageServiceMock.Setup(s => s.SetItemAsync(It.IsAny<string>(), It.IsAny<IList<string>>(), It.IsAny<CancellationToken>()))
                .Callback<string, IList<string>, CancellationToken>((s, l, c) =>
                {
                    result = l;
                });
            _service = new SavedQueriesService(_localStorageServiceMock.Object);

            // Act
            await _service.PushNewSearchQueryAsync("test value");

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 5);
            Assert.True(result.Contains("test value"));
            Assert.False(result.Contains("six"));
            Assert.False(result.Contains("five"));
        }

        [Fact]
        public async Task GivenSixEntries_WhenPushQuery_ThenFiveQueries()
        {
            // Arrange
            IList<string> list = new List<string> {
                "one", "two"
            };
            _localStorageServiceMock.Setup(s => s.GetItemAsync<IList<string>>(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(ValueTask.FromResult(list));
            IList<string> result = null;
            _localStorageServiceMock.Setup(s => s.SetItemAsync(It.IsAny<string>(), It.IsAny<IList<string>>(), It.IsAny<CancellationToken>()))
                .Callback<string, IList<string>, CancellationToken>((s, l, c) =>
                {
                    result = l;
                });
            _service = new SavedQueriesService(_localStorageServiceMock.Object);

            // Act
            await _service.PushNewSearchQueryAsync("test value");

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 3);
            Assert.True(result.Contains("test value"));
        }
    }
}
