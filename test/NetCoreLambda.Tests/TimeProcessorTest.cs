using System;
using Xunit;

namespace NetCoreLambda.Tests {
  public class TimeProcessorTest {

    [Fact]
    public void TestCurrentTimeUTC()
    {
      //Given
      var processor = new TimeProcessor();
      var prevTime = DateTime.UtcNow;
      
      //When
      var currTime = processor.CurrentTimeUTC();

      //Then
      var nextTime = DateTime.UtcNow;
      Assert.True(currTime >= prevTime);
      Assert.True(currTime <= nextTime);
    }
  }
}