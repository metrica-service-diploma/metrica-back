TODO:

- [ ] Добавить безопасный проброс секретов
- [ ] Добавить эффективный сидинг данными БД

namespace metrica_back.src.Domain.Interfaces;

public interface IQuery<TResponse> { }

public interface ICommand<TResponse> { }

public interface IResult<out T>
{
    bool IsSuccess { get; }
    T? Data { get; }
    string? Error { get; }
    int StatusCode { get; }
}

public interface IHandler<TCommand, TResponse>
{
    Task<IResult<TResponse>> Handle(TCommand request);
}
