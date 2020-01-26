using System;

namespace NetCoreLambda {
  public class TimeProcessor : ITimeProcessor
  {
    public DateTime CurrentTimeUTC()
    {
      return DateTime.UtcNow;
    }
  }
}