namespace GMG.Domain.Common.Result
{
    public class Validation
    {
        private List<string> _errors = new();

        public Validation Required(bool condition, string errorMessage)
        {
            if (!condition)
            {
                _errors.Add(errorMessage);
            }

            return this;
        }

        public Result<T> Build<T>(Func<T> factory)
            => _errors.Any()
                ? Result<T>.Failure(string.Join(". ", _errors))
                : Result<T>.Success(factory());
    }
}
