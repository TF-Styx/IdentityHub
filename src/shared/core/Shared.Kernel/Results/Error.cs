namespace Shared.Kernel.Results
{
    public sealed record Error(ErrorCode ErrorCode, string Message)
    {
        public static Error New(ErrorCode errorCode, string message) => new(errorCode, message);
        public static Error New(ErrorCode errorCode) => new(errorCode, "");
        public static Error Validation(string message) => new(ErrorCode.Validation, message);
        public static Error ValidationPhone(string message = "Номер телефона не корректный!") => new(ErrorCode.Validation, message);
        public static Error InternalServer() => new(ErrorCode.Server, "Произошла ошибка на стороне сервера!");

        /// <summary>
        /// $"Не удалось найти элемент {message}!"
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Error NotFound(string message = "") => new(ErrorCode.NotFound, $"Не удалось найти элемент {message}!");
        public static Error Conflict(string message) => new(ErrorCode.Conflict, message);
    }
}
