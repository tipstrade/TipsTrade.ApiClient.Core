using Microsoft.Extensions.Logging;
using Moq;
using TipsTrade.ApiClient.Core.Logging;

namespace Tests.Logging {
  public class Tests {
    private static Mock<ILogger> CreateLogger(LogLevel level) {
      var mock = new Mock<ILogger>();

      mock.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(level <= LogLevel.Trace);
      mock.Setup(x => x.IsEnabled(LogLevel.Debug)).Returns(level <= LogLevel.Debug);
      mock.Setup(x => x.IsEnabled(LogLevel.Information)).Returns(level <= LogLevel.Information);
      mock.Setup(x => x.IsEnabled(LogLevel.Warning)).Returns(level <= LogLevel.Warning);
      mock.Setup(x => x.IsEnabled(LogLevel.Error)).Returns(level <= LogLevel.Error);
      mock.Setup(x => x.IsEnabled(LogLevel.Critical)).Returns(level <= LogLevel.Critical);
      mock.Setup(x => x.IsEnabled(LogLevel.None)).Returns(level <= LogLevel.None);

      return mock;
    }

    [Test]
    public void GetLogger_Returns_Logger() {
      var mock = CreateLogger(LogLevel.Debug);
      var withLogger = new WithLogger(mock.Object);

      Assert.That(withLogger.GetLogger(), Is.Not.Null);
    }

    [Test]
    public void GetLogger_Returns_Null() {
      var withLogger = new WithLogger();

      Assert.That(withLogger.GetLogger(), Is.Null);
    }

    [Test]
    public void LogIf_Calls_Message() {
      var loggerMock = CreateLogger(LogLevel.Debug);
      var getMessageMock  = new Mock<Func<string>>();

      loggerMock.Object.LogIf(LogLevel.Information, getMessageMock.Object);

      getMessageMock.Verify(x=> x(), Times.Once);
    }

    [Test]
    public void LogIf_Does_Not_Call_Message() {
      var loggerMock = CreateLogger(LogLevel.Debug);
      var getMessageMock = new Mock<Func<string>>();

      loggerMock.Object.LogIf(LogLevel.Trace, getMessageMock.Object);

      getMessageMock.Verify(x => x(), Times.Never);
    }
  }

  internal class WithLogger : IWithLogger {
    public ILogger? Logger { get; }

    public WithLogger(ILogger? logger = null) {
      Logger = logger;
    }
  }
}
