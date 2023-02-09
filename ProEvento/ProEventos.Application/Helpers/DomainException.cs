using System;

namespace ProEventos.Application.Helpers
{
    public class DomainException : Exception
    {
        public DomainException(string erro) : base(erro) { }
       
        public static void When(bool hasError, string error)
        {
            if (hasError)
            {
                throw new DomainException(error);
            }
        }
    }
}
