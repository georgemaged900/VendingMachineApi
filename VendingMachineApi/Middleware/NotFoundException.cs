﻿namespace FlapKap.Middleware
{
    public class NotFoundException :ApplicationException
    {
        public NotFoundException(string message) : base(message)
        {

        }
    }
}
