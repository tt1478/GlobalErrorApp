﻿namespace GlobalErrorApp.Exceptions
{
    public class UnauthorizedAccessException: Exception
    {
        public UnauthorizedAccessException(string message): base(message)
        {
        }
    }
}