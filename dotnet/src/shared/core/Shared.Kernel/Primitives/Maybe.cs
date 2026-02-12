namespace Shared.Kernel.Primitives
{
    public readonly record struct Maybe<T>
    {
        private readonly T _value;
        private readonly bool _hasValue;

        private Maybe(T value)
        {
            _value = value;
            _hasValue = true;
        }

        public bool HasValue => _hasValue;

        public bool IsNone => !_hasValue;

        public T Value => HasValue
            ? _value
            : throw new InvalidOperationException("Не удается получить доступ к значениею 'None'.");


        public static Maybe<T> None => new();

        /// <summary>
        /// Создает Maybe со значением (Some). Если переданное значение равно null, будет возвращен Maybe.None.
        /// </summary>
        /// <param name="value">Значение для инкапсуляции.</param>
        public static Maybe<T> Some(T? value) => value is null ? None : new Maybe<T>(value);

        /// <summary>
        /// Возвращает инкапсулированное значение, если оно существует, иначе возвращает значение по умолчанию.
        /// </summary>
        /// <param name="defaultValue">Значение, которое будет возвращено, если Maybe является None.</param>
        public T GetValueOrDefault(T defaultValue = default!) => HasValue ? _value : defaultValue;

        /// <summary>
        /// Выполняет одно из двух действий в зависимости от того, существует ли значение.
        /// </summary>
        /// <param name="onSome">Действие, выполняемое со значением, если оно существует.</param>
        /// <param name="onNone">Действие, выполняемое, если значение отсутствует.</param>
        public void Match(Action<T> onSome, Action onNone)
        {
            if (HasValue)
                onSome(_value);
            else
                onNone();
        }

        /// <summary>
        /// Проецирует значение в новую форму, если оно существует.
        /// </summary>
        /// <param name="onSome">Функция, выполняемая со значением, если оно существует.</param>
        /// <param name="onNone">Функция, возвращающая результат, если значение отсутствует.</param>
        public TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone) => HasValue ? onSome(_value) : onNone();

        /// <summary>
        /// Преобразует значение внутри Maybe в новый тип, если значение существует.
        /// Если Maybe является None, возвращает Maybe.None для нового типа.
        /// </summary>
        /// <typeparam name="TResult">Тип результата преобразования.</typeparam>
        /// <param name="map">Функция преобразования.</param>
        public Maybe<TResult> Map<TResult>(Func<T, TResult> map) => HasValue ? Maybe<TResult>.Some(map(_value)) : Maybe<TResult>.None;

        /// <summary>
        /// Связывает Maybe с результатом функции, которая также возвращает Maybe.
        /// Используется для цепочек вызовов, возвращающих Maybe.
        /// </summary>
        /// <typeparam name="TResult">Тип значения в результирующем Maybe.</typeparam>
        /// <param name="bind">Функция связывания.</param>
        public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> bind) => HasValue ? bind(_value) : Maybe<TResult>.None;

        public override string ToString() => HasValue ? $"Some({_value})" : "None";

        public static implicit operator Maybe<T>(T? value) => Some(value);
    }
}
