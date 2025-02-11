using FluentAssertions;
using ProceduralExample;
using Xunit;

namespace UIConsoleExample.Tests
{
    // Тесты для проверки работы фабрик
    public class CarFactoryTests
    {
        [Theory]
        [InlineData(null, "Sedan")]
        [InlineData("MySedan", "MySedan")]
        public void SedanFactory_CreateCar_ShouldReturnCorrectModel(string input, string expected)
        {
            // Arrange
            var factory = new SedanFactory();

            // Act
            var car = factory.FactoryCar(input);

            // Assert
            car.Should().BeOfType<SedanCar>();
            car.Model.Should().Be(expected);
        }

        [Theory]
        [InlineData(null, "SUV")]
        [InlineData("MySuv", "MySuv")]
        public void SuvFactory_CreateCar_ShouldReturnCorrectModel(string input, string expected)
        {
            // Arrange
            var factory = new SuvFactory();

            // Act
            var car = factory.FactoryCar(input);

            // Assert
            car.Should().BeOfType<SuvCar>();
            car.Model.Should().Be(expected);
        }

        [Theory]
        [InlineData(null, "Coupe")]
        [InlineData("MyCoupe", "MyCoupe")]
        public void CoupeFactory_CreateCar_ShouldReturnCorrectModel(string input, string expected)
        {
            // Arrange
            var factory = new CoupeFactory();

            // Act
            var car = factory.FactoryCar(input);

            // Assert
            car.Should().BeOfType<CoupeCar>();
            car.Model.Should().Be(expected);
        }
    }

    // Тесты для проверки работы CarStore, включая вывод в консоль
    public class CarStoreTests : IDisposable
    {
        // Для восстановления стандартного вывода после тестов
        private readonly TextWriter _originalOutput;

        public CarStoreTests()
        {
            _originalOutput = Console.Out;
        }

        public void Dispose()
        {
            Console.SetOut(_originalOutput);
        }

        //[Fact]
        //public void SellCar_WithProvidedName_ShouldPrintCorrectMessages()
        //{
        //    // Arrange
        //    var store = new CarStore();
        //    var factory = new CarFactory();
        //    using var sw = new StringWriter();
        //    Console.SetOut(sw);

        //    string providedName = "Suv2025";

        //    // Act
        //    store.SellCar(factory, providedName);

        //    // Assert
        //    // Фабрика выводит сообщение о выпуске автомобиля, затем магазин – сообщение о продаже
        //    string expectedOutput = $"Выпущен автомобиль: {providedName}{Environment.NewLine}" +
        //                            $"Автомобиль {providedName} продан!{Environment.NewLine}";
        //    sw.ToString().Should().Be(expectedOutput);
        //}

        [Fact]
        public void SellCar_WithoutProvidedName_ShouldPrintCorrectMessagesWithDefaultModel()
        {
            // Arrange
            var store = new CarStore();
            var factory = new SedanFactory(); // по умолчанию модель "Sedan"
            using var sw = new StringWriter();
            Console.SetOut(sw);

            // Act
            store.SellCar(factory);  // имя не передано, используется значение по умолчанию

            // Assert
            string expectedOutput = $"Выпущен автомобиль: Sedan{Environment.NewLine}" +
                                    $"Автомобиль Sedan продан!{Environment.NewLine}";
            sw.ToString().Should().Be(expectedOutput);
        }
    }
}

