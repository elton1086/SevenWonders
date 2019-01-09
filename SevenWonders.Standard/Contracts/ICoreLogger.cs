using System;

namespace SevenWonders.Contracts
{
    public interface ICoreLogger
    {
        void Debug(string message, params object[] args);
        void Info(string message, params object[] args);
        void Error(Exception exception, string message, params object[] args);
    }
}
