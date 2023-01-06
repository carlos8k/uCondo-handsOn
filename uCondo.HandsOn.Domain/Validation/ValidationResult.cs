namespace uCondo.HandsOn.Domain.Validation
{
    public class ValidationResult
    {
        public bool Invalid => !Valid;
        public bool Valid { get; protected set; }
        public string Message { get; protected set; }

        public static ValidationResult Success()
        {
            var result = new ValidationResult
            {
                Valid = true
            };

            return result;
        }

        public static ValidationResult Fail()
        {
            return Fail(string.Empty);
        }

        public static ValidationResult Fail(string message)
        {
            var result = new ValidationResult
            {
                Valid = false,
                Message = message
            };

            return result;
        }
    }

    public class ValidationResult<T> : ValidationResult
    {
        public T Data { get; private set; }

        public static ValidationResult<T> Success(T data)
        {
            var result = new ValidationResult<T>
            {
                Valid = true,
                Data = data
            };

            return result;
        }

        public new static ValidationResult<T> Fail()
        {
            return Fail(string.Empty);
        }

        public new static ValidationResult<T> Fail(string message)
        {
            var result = new ValidationResult<T>
            {
                Valid = false,
                Message = message
            };

            return result;
        }
    }
}