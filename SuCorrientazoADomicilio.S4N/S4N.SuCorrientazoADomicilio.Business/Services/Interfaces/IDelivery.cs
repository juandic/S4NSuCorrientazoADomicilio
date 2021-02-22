using S4N.SuCorrientazoADomicilio.Dto;
using System.Collections.Generic;
using System.Text;

namespace S4N.SuCorrientazoADomicilio.Business.Services.Interfaces
{
    public interface IDelivery
    {
        /// <summary>
        /// Calculates the delivery indications
        /// </summary>
        /// <param name="deliveries">List of deliveries indications</param>
        /// <param name="coordinates">Drone initial position</param>
        /// <param name="maxCoverage">The maximum coverage blocks from initial point value</param>
        /// <param name="maxDeliveries">The maximum deliveries allowed by drone value</param>
        /// <returns>The drone route result</returns>
        StringBuilder StartDelivery(List<string> deliveries, CoordinatesDto coordinates, int maxCoverage, int maxDeliveries);
    }
}
