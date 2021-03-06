﻿using System.Collections.Generic;
using System.Threading.Tasks;
using OpenMod.API.Ioc;

namespace OpenMod.API.Permissions
{
    /// <summary>
    /// The service used to checking permission authorizations.
    /// </summary>
    [Service]
    public interface IPermissionChecker
    {
        /// <value>
        /// The permission check providers.
        /// </value>
        IReadOnlyCollection<IPermissionCheckProvider> PermissionCheckProviders { get; }

        /// <value>
        /// The permission sources.
        /// </value>
        IReadOnlyCollection<IPermissionStore> PermissionStores { get; }

        /// <summary>
        /// Checks if an actor has authorization to execute an action.
        /// </summary>
        /// <param name="actor">The actor to check.</param>
        /// <param name="permission">The permission to check.</param>
        /// <returns>See <see cref="PermissionGrantResult"/>.</returns>
        Task<PermissionGrantResult> CheckPermissionAsync(IPermissionActor actor, string permission);

        /// <summary>
        /// Initializes the permission checker.
        /// </summary>
        /// <remarks>
        /// <b>This method is for internal usage only and should not be called by plugins.</b>
        /// </remarks>
        [OpenModInternal]
        Task InitAsync();
    }
}