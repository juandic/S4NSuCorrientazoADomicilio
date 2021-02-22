using S4N.SuCorrientazoADomicilio.Business.Services.Interfaces;
using S4N.SuCorrientazoADomicilio.Dto;
using S4N.SuCorrientazoADomicilio.Dto.Validation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace S4N.SuCorrientazoADomicilio.Business.Services
{
    public class Delivery : IDelivery
    {
        /// <summary>
        /// Starts the drone delivery calculation route
        /// </summary>
        /// <param name="deliveries">List of deliveries indications</param>
        /// <param name="coordinates">Drone initial position</param>
        /// <param name="maxCoverage">The maximum coverage blocks from initial point value</param>
        /// <param name="maxDeliveries">The maximum deliveries allowed by drone value</param>
        /// <returns></returns>
        public StringBuilder StartDelivery(List<string> deliveries, CoordinatesDto coordinates, int maxCoverage, int maxDeliveries)
        {
            var routeResult = new StringBuilder();
            ValidationResultDto validation;
            routeResult.AppendLine("== Reporte de entregas ==");
            validation = ValidateMaxDeliveryIndications(deliveries, maxDeliveries);
            if (validation.Conditions != null && !string.IsNullOrEmpty(validation.Conditions.Message))
            {
                routeResult.AppendLine($"{validation.Conditions.Severity}: {validation.Conditions.Message}");
                return routeResult;
            }

            validation = ValidateDeliveryIndications(deliveries);
            if (validation.Conditions != null && !string.IsNullOrEmpty(validation.Conditions.Message))
            {
                //If there is any error in the provided routes, the drone could not start the deliveries.
                routeResult.AppendLine($"{validation.Conditions.Severity}: {validation.Conditions.Message}");
                return routeResult;
            }

            foreach (var delivery in deliveries)
            {
                var route = new RouteDto
                {
                    RouteString = delivery,
                    Coordinates = coordinates
                };

                var result = CalculateRoute(route, maxCoverage);
                if (result.validationResult.Conditions != null && !string.IsNullOrEmpty(result.validationResult.Conditions.Message))
                {
                    routeResult.AppendLine($"{result.validationResult.Conditions.Severity}: {result.validationResult.Conditions.Message}");
                    break;
                }

                routeResult.AppendLine($"({result.coordinates.X},{result.coordinates.Y}) dirección {result.coordinates.CardinalDirection}");

                route.Coordinates = result.coordinates;
            }

            return routeResult;
        }

        

        #region Private methods
        /// <summary>
        /// Calculates the route according to provided parameters
        /// </summary>
        /// <param name="dronePosition">Initial drone position</param>
        /// <param name="maxCoverage">The max coverage blocks value</param>
        /// <returns></returns>
        private (CoordinatesDto coordinates, ValidationResultDto validationResult) CalculateRoute(RouteDto dronePosition, int maxCoverage)
        {
            var validation = new ValidationResultDto();

            for (var i = 0; i < dronePosition.RouteString.Length; i++)
            {
                var direction = dronePosition.RouteString.Substring(i, 1);
                
                switch (dronePosition.Coordinates.CardinalDirection)
                {
                    case Orientation.Norte:
                        dronePosition.Coordinates = CalculateNorthTrack(dronePosition.Coordinates, direction);
                        validation = ValidateCoverage(dronePosition, maxCoverage);
                        continue;
                    case Orientation.Oriente:
                        dronePosition.Coordinates = CalculateEastTrack(dronePosition.Coordinates, direction);
                        validation = ValidateCoverage(dronePosition, maxCoverage);
                        continue;
                    case Orientation.Occidente:
                        dronePosition.Coordinates = CalculateWestTrack(dronePosition.Coordinates, direction);
                        validation = ValidateCoverage(dronePosition, maxCoverage);
                        continue;
                    case Orientation.Sur:
                        dronePosition.Coordinates = CalculateSouthTrack(dronePosition.Coordinates, direction);
                        validation = ValidateCoverage(dronePosition, maxCoverage);
                        continue;
                }
            }
            return (dronePosition.Coordinates, validation);
        }

        /// <summary>
        /// Calculates north track
        /// </summary>
        /// <param name="dronePosition">Contains position and cardinal direction</param>
        /// <param name="direction">Direction instruction</param>
        /// <returns>Calculated coordinates</returns>
        private static CoordinatesDto CalculateNorthTrack(CoordinatesDto dronePosition, string direction)
        {
            if (direction.Equals("A"))
            {
                dronePosition.Y += 1;
                dronePosition.CardinalDirection = Orientation.Norte;
            }
            if (direction.Equals("I"))
            {
                dronePosition.CardinalDirection = Orientation.Occidente;
            }
            if (direction.Equals("D"))
            {
                dronePosition.CardinalDirection = Orientation.Oriente;
            }
            return dronePosition;
        }

        /// <summary>
        /// Calculates east track
        /// </summary>
        /// <param name="dronePosition">Contains position and cardinal direction</param>
        /// <param name="direction">Direction instruction</param>
        /// <returns>Calculated coordinates</returns>
        private static CoordinatesDto CalculateEastTrack(CoordinatesDto dronePosition, string direction)
        {
            if (direction.Equals("A"))
            {
                dronePosition.X += 1;
                dronePosition.CardinalDirection = Orientation.Oriente;
            }
            if (direction.Equals("I"))
            {
                dronePosition.CardinalDirection = Orientation.Norte;
            }
            if (direction.Equals("D"))
            {
                dronePosition.CardinalDirection = Orientation.Sur;
            }

            return dronePosition;
        }

        /// <summary>
        /// Calculates west track
        /// </summary>
        /// <param name="dronePosition">Contains position and cardinal direction</param>
        /// <param name="direction">Direction instruction</param>
        /// <returns>Calculated coordinates</returns>
        private static CoordinatesDto CalculateWestTrack(CoordinatesDto dronePosition, string direction)
        {
            if (direction.Equals("A"))
            {
                dronePosition.X -= 1;
                dronePosition.CardinalDirection = Orientation.Occidente;
            }
            if (direction.Equals("I"))
            {
                dronePosition.CardinalDirection = Orientation.Sur;
            }
            if (direction.Equals("D"))
            {
                dronePosition.CardinalDirection = Orientation.Norte;
            }
            return dronePosition;
        }

        /// <summary>
        /// Calculates south track
        /// </summary>
        /// <param name="dronePosition">Contains position and cardinal direction</param>
        /// <param name="direction">Direction instruction</param>
        /// <returns>Calculated coordinates</returns>
        private static CoordinatesDto CalculateSouthTrack(CoordinatesDto dronePosition, string direction)
        {
            if (direction.Equals("A"))
            {
                dronePosition.Y -= 1;
                dronePosition.CardinalDirection = Orientation.Sur;
            }
            if (direction.Equals("I"))
            {
                dronePosition.CardinalDirection = Orientation.Oriente;
            }
            if (direction.Equals("D"))
            {
                dronePosition.CardinalDirection = Orientation.Occidente;
            }
            return dronePosition;
        }
        #endregion

        #region Validations
        /// <summary>
        /// Validates that all delivery indications are valid
        /// </summary>
        /// <param name="deliveries"></param>
        /// <returns></returns>
        private static ValidationResultDto ValidateDeliveryIndications(List<string> deliveries)
        {
            var validation = new ValidationResultDto();
            var pattern = new Regex("[^ADI]");
            
            foreach (var delivery in deliveries.Where(delivery => pattern.IsMatch(delivery)))
            {
                validation.Conditions.Message = ($"Delivery indication ({delivery}) contains not valid characters");
                validation.Conditions.Severity = SeverityLevel.ERROR;
                return validation;
            }

            return validation;
        }

        /// <summary>
        /// Validate maximum delivery indications
        /// </summary>
        /// <param name="deliveries">List of deliveries</param>
        /// <param name="maxDeliveries">Maximum deliveries allowed</param>
        /// <returns></returns>
        private ValidationResultDto ValidateMaxDeliveryIndications(List<string> deliveries, int maxDeliveries)
        {
            var validation = new ValidationResultDto();
            if (deliveries.Count <= maxDeliveries) return validation;

            validation.Conditions.Message = $"File has specified more than the {maxDeliveries} allowed routes by drone.";
            validation.Conditions.Severity = SeverityLevel.ERROR;

            return validation;
        }

        /// <summary>
        /// Validates that calculated route has coverage
        /// </summary>
        /// <param name="dronePosition">Calculated drone coordinates</param>
        /// <param name="maxCoverage">The max coverage blocks value</param>
        /// <returns></returns>
        private ValidationResultDto ValidateCoverage(RouteDto dronePosition, int maxCoverage)
        {
            var validation = new ValidationResultDto();
            if (dronePosition.Coordinates.X > maxCoverage || dronePosition.Coordinates.Y > maxCoverage)
            {
                validation.Conditions.Message = $"The delivery with indications ({dronePosition.RouteString}) exceeds the delivery coverage";
                validation.Conditions.Severity = SeverityLevel.ERROR;

                return validation;
            }

            return validation;
        }
        #endregion

    }
}
