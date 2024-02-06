using Core.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace Core.Basic
{
    public class Result
    {
        public static Result Success() => new Result();
        public static Result Failure(string message) => new Result(message);

        protected readonly List<Message> _messages = new();

        public bool IsSuccess => !_messages.ContainsErrors();

        public Message[] Messages => _messages.ToArray();
        public string Message => _messages.Select(m => m.Content).AggregateOrEmpty((x, y) => $"{x}. {y}", string.Empty);

        protected Result(string message = null)
        {
            if (message is not null)
            {
                Assert.IsFalse(message.IsNullOrEmpty());
                Fail(message);
            }

        }

        public Result Fail(string errorMessage)
        {
            _messages.Add(new Message(MessageLevel.Error, errorMessage));
            return this;
        }
    }

    public class Result<T> : Result
    {
        public static new Result<T> Success() => new Result<T>();
        public static Result<T> Success(T value) => new Result<T>(value);
        public static new Result<T> Failure(string message) => new Result<T>(message);
        
        public Result(T value = default) => Value = value;
        public Result(string message) : base(message) {}

        public T Value { get; private set; }

        public new Result<T> Fail(string message)
        {
            _messages.Add(new Message(MessageLevel.Error, message));
            return this;
        }

        public new Result<T> Fail(IEnumerable<Message> messages)
        {
            _messages.AddRange(messages);
            return this;
        }

        public new Result<T> With(T value)
        {
            Value = value;
            return this;
        }

        public static implicit operator Result<T>(string errorMessage) => new(errorMessage);
    }

    public record Message(MessageLevel Level, string Content);
    public record MessageLevel(string name)
    {
        public static readonly MessageLevel Info = new("info");
        public static readonly MessageLevel Warning = new("warning");
        public static readonly MessageLevel Error = new("error");
    }

    public static class MessageExtensions
    {
        public static bool ContainsErrors(this IEnumerable<Message> messages) =>
            messages.Any(m => m.Level == MessageLevel.Error);
    }
}
