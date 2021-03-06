﻿using System.Threading.Tasks;
using OpenMod.Extensions.Games.Abstractions.Acl;
using OpenMod.Extensions.Games.Abstractions.Entities;

namespace OpenMod.Extensions.Games.Abstractions.Building
{
    /// <summary>
    /// Represents a buildable object players can place.
    /// </summary>
    public interface IBuildable : IHasOwnership, IGameObject
    {
        /// <value>
        /// The asset of the object.
        /// </value>
        IBuildableAsset Asset { get; }

        /// <value>
        /// The state of the object.
        /// </value>
        IBuildableState State { get; }

        /// <value>
        /// The unique instance ID of the object.
        /// </value>
        string BuildableInstanceId { get; }

        /// <summary>
        /// Destroys the object.
        /// </summary>
        Task DestroyAsync();
    }
}