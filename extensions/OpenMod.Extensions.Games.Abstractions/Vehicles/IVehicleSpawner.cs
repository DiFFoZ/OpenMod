﻿using System.Numerics;
using System.Threading.Tasks;
using OpenMod.API.Ioc;

namespace OpenMod.Extensions.Games.Abstractions.Vehicles
{
    /// <summary>
    /// The service for spawning vehicles.
    /// </summary>
    [Service]
    public interface IVehicleSpawner
    {
        /// <summary>
        /// Spawns a vehicle at the given position.
        /// </summary>
        /// <param name="position">The position to spawn the vehicle at.</param>
        /// <param name="vehicleAssetId">The ID of the vehicle asset.</param>
        /// <param name="state">The optional state of the vehicle.</param>
        /// <returns><b>The spawned vehicle</b> if successful; otherwise, <b>false</b>.</returns>
        Task<IVehicle?> SpawnVehicleAsync(Vector3 position, string vehicleAssetId, IVehicleState? state = null);
    }
}