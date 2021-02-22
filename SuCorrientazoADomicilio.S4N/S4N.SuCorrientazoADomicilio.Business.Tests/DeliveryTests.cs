using System.Collections.Generic;
using NUnit.Framework;
using S4N.SuCorrientazoADomicilio.Business.Services;
using S4N.SuCorrientazoADomicilio.Dto;

namespace S4N.SuCorrientazoADomicilio.Business.Tests
{
    public class DeliveryTests
    {
        private Delivery _delivery;
    
        [SetUp]
        public void Setup()
        {
            _delivery = new Delivery();
        }

        [Test]
        public void StartDelivery_NotValidIndications_ErrorMessage()
        {
            //Arrange
            var deliveries = new List<string> {"R"};
            var maxCoverage = 3;
            var maxDeliveries = 2;
            var coordinates = new CoordinatesDto
            {
                X = 0,
                Y = 0,
                CardinalDirection = Orientation.Norte
            };

            //Act
            var result = _delivery.StartDelivery(deliveries, coordinates, maxCoverage, maxDeliveries);

            //Assert
            Assert.IsTrue(result.ToString().Contains("ERROR"));
            Assert.IsTrue(result.ToString().Contains("Delivery indication (R) contains not valid characters"));
        }
        
        [Test]
        public void StartDelivery_IndicationsExceedsMaxRoutes_ErrorMessage()
        {
            //Arrange
            var deliveries = new List<string> {"DDDDD", "IIIII", "AAAAA", "AIDAID"};
            var maxCoverage = 3;
            var maxDeliveries = 2;
            var coordinates = new CoordinatesDto
            {
                X = 0,
                Y = 0,
                CardinalDirection = Orientation.Norte
            };

            //Act
            var result = _delivery.StartDelivery(deliveries, coordinates, maxCoverage, maxDeliveries);

            //Assert
            Assert.IsTrue(result.ToString().Contains("ERROR"));
            Assert.IsTrue(result.ToString().Contains("File has specified more than the 2 allowed routes by drone"));
        }

        [Test]
        public void StartDelivery_IndicationsExceedMaxCoverage_ErrorMessage()
        {
            //Arrange
            var deliveries = new List<string> { "AAAAA", "IIIII" };
            var maxCoverage = 3;
            var maxDeliveries = 2;
            var coordinates = new CoordinatesDto
            {
                X = 0,
                Y = 0,
                CardinalDirection = Orientation.Norte
            };

            //Act
            var result = _delivery.StartDelivery(deliveries, coordinates, maxCoverage, maxDeliveries);

            //Assert
            Assert.IsTrue(result.ToString().Contains("ERROR"));
            Assert.IsTrue(result.ToString().Contains("The delivery with indications (AAAAA) exceeds the delivery coverage"));
            
        }

        [Test]
        public void StartDelivery_ValidIndications_CalculatedCoordinates()
        {
            //Arrange
            var deliveries = new List<string> { "AAADAI", "IIAII" };
            var maxCoverage = 5;
            var maxDeliveries = 2;
            var coordinates = new CoordinatesDto
            {
                X = 0,
                Y = 0,
                CardinalDirection = Orientation.Norte
            };

            //Act
            var result = _delivery.StartDelivery(deliveries, coordinates, maxCoverage, maxDeliveries);

            //Assert
            var stringValues = result.ToString();

            StringAssert.DoesNotContain(stringValues, "ERROR");
            StringAssert.Contains("1,2", stringValues);
            StringAssert.Contains("1,3", stringValues);
        }
    }
}