using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Extensions;

namespace RaspberryPi.Unit.Tests.Domain;

public class AllMessagesTests
{
    [Fact]
    public void AllMessages_WhenExceptionIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        Exception ex = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => ex.AllMessages());
    }

    [Fact]
    public void AllMessages_WhenExceptionHasNoInnerException_ReturnsSingleMessage()
    {
        // Arrange
        var ex = new InvalidOperationException("Operation is invalid");
        var expected = "Operation is invalid";

        // Act
        var result = ex.AllMessages();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void AllMessages_WithSimpleInnerException_ReturnsConcatenatedMessages()
    {
        // Arrange
        var innerEx = new ArgumentNullException("param", "Parameter cannot be null");
        var ex = new InvalidOperationException("Operation failed", innerEx);
        var expected = "Operation failed --> Parameter cannot be null (Parameter 'param')";

        // Act
        var result = ex.AllMessages();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void AllMessages_WithMultipleLevelInnerExceptions_ReturnsAllMessages()
    {
        // Arrange
        var innermostEx = new DivideByZeroException("Division by zero");
        var middleEx = new ArithmeticException("Arithmetic error", innermostEx);
        var ex = new ApplicationException("Application crashed", middleEx);

        var expected = "Application crashed --> Arithmetic error --> Division by zero";

        // Act
        var result = ex.AllMessages();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void AllMessages_WithAggregateException_HandlesMultipleInnerExceptions()
    {
        // Arrange
        var ex1 = new InvalidOperationException("Operation 1 failed");
        var ex2 = new ArgumentException("Invalid argument", "param");
        var aggregateEx = new AggregateException("Multiple errors occurred", ex1, ex2);

        var expected = "Multiple errors occurred (Operation 1 failed) (Invalid argument (Parameter 'param'))";

        // Act
        var result = aggregateEx.AllMessages();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void AllMessages_WithAggregateExceptionContainingNestedAggregateExceptions_RecursivelyProcessesAll()
    {
        // Arrange
        var innerEx1 = new InvalidOperationException("Inner operation failed");
        var innerEx2 = new ArgumentException("Inner argument invalid");
        var innerAggregate = new AggregateException("Inner aggregate", innerEx1, innerEx2);

        var ex1 = new DivideByZeroException("Division failed");
        var mainAggregate = new AggregateException("Main aggregate", ex1, innerAggregate);

        var expected = "Main aggregate (Division failed) (Inner aggregate (Inner operation failed) (Inner argument invalid))";

        // Act
        var result = mainAggregate.AllMessages();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void AllMessages_WithExceptionHavingEmptyMessage_ReturnsEmptyMessageString()
    {
        // Arrange
        var ex = new Exception(string.Empty);
        var expected = string.Empty;

        // Act
        var result = ex.AllMessages();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void AllMessages_WithExceptionHavingNullMessage_ReturnsDefaultString()
    {
        // Arrange
        var ex = new Exception(null);
        var expected = "Exception of type 'System.Exception' was thrown.";

        // Act
        var result = ex.AllMessages();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void AllMessages_SeparatorIsAlwaysArrowWithSpaces()
    {
        // Arrange
        var innerEx = new Exception("Inner message");
        var ex = new Exception("Outer message", innerEx);

        var result = ex.AllMessages();

        // Assert
        Assert.Contains(" --> ", result);
        var parts = result.Split(new[] { " --> " }, StringSplitOptions.None);
        Assert.Equal(2, parts.Length);
        Assert.Equal("Outer message", parts[0]);
        Assert.Equal("Inner message", parts[1]);
    }

    [Fact]
    public void AllMessages_WithCustomExceptionType_WorksCorrectly()
    {
        // Arrange
        var customEx = new AppException("Custom error occurred");
        var ex = new ApplicationException("App error", customEx);

        var expected = "App error --> Custom error occurred";

        // Act
        var result = ex.AllMessages();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void AllMessages_PreservesMessageOrder_InnerExceptionsFirstToLast()
    {
        // Arrange
        var ex3 = new Exception("Third");
        var ex2 = new Exception("Second", ex3);
        var ex1 = new Exception("First", ex2);

        var expected = "First --> Second --> Third";

        // Act
        var result = ex1.AllMessages();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void AllMessages_PreservesMessageOrder_InAggregateExceptions()
    {
        // Arrange
        var ex1 = new Exception("First inner");
        var ex2 = new Exception("Second inner");
        var ex3 = new Exception("Third inner");
        var aggregate = new AggregateException("Aggregate", ex1, ex2, ex3);

        var expected = "Aggregate (First inner) (Second inner) (Third inner)";

        // Act
        var result = aggregate.AllMessages();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void AllMessages_WithDeeplyNestedExceptions_DoesNotCauseStackOverflow()
    {
        // Arrange - Create a chain of 100 nested exceptions
        Exception current = null;
        for (int i = 100; i >= 1; i--)
        {
            current = new Exception($"Exception {i}", current);
        }

        // Act & Assert - Should not throw StackOverflowException
        var result = current.AllMessages();

        // Assert
        Assert.NotNull(result);
        Assert.Contains("Exception 1", result);
        Assert.Contains("Exception 100", result);
    }
}