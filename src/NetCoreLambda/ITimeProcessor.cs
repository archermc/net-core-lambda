using System;

namespace NetCoreLambda {
  public interface ITimeProcessor {
    DateTime CurrentTimeUTC();
  }
}