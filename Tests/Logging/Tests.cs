using Microsoft.Extensions.Logging;
using Moq;

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
  }
}
