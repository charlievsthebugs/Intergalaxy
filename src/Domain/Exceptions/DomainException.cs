namespace Intergalaxy.Domain.Exceptions;

public class DomainException : Exception
{
    //TODO: CONSIDER ADDING CONSTRUCTORS FOR DIFFERENT USE CASES, E.G. SINGLE ERROR MESSAGE, INNER EXCEPTION, ETC.
    public DomainException()
       : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public DomainException(string[] errors)
        : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>
        {
            { "Error", errors }
        };
    }

    public IDictionary<string, string[]> Errors { get; }

}
