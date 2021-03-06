﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenMod.API;
using OpenMod.API.Eventing;
using OpenMod.API.Jobs;
using OpenMod.API.Permissions;
using OpenMod.API.Plugins;
using OpenMod.Core.Helpers;
using OpenMod.Runtime.Events;
using SmartFormat;

namespace OpenMod.Runtime
{
    [OpenModInternal]
    public class OpenModHostedService : IHostedService
    {
        private readonly ILogger<OpenModHostedService> m_Logger;
        private readonly IPermissionChecker m_PermissionChecker;
        private readonly IHostInformation m_HostInformation;
        private readonly IOpenModHost m_Host;
        private readonly IPluginAssemblyStore m_PluginAssemblyStore;
        private readonly IPluginActivator m_PluginActivator;
        private readonly IEventBus m_EventBus;
        private readonly IJobScheduler m_JobScheduler;

        public OpenModHostedService(
            ILogger<OpenModHostedService> logger,
            IPermissionChecker permissionChecker,
            IHostInformation hostInformation,
            IOpenModHost host,
            IPluginAssemblyStore pluginAssemblyStore,
            IPluginActivator pluginActivator,
            IEventBus eventBus,
            IJobScheduler jobScheduler
        )
        {
            m_Logger = logger;
            m_PermissionChecker = permissionChecker;
            m_HostInformation = hostInformation;
            m_Host = host;
            m_PluginAssemblyStore = pluginAssemblyStore;
            m_PluginActivator = pluginActivator;
            m_EventBus = eventBus;
            m_JobScheduler = jobScheduler;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await m_PermissionChecker.InitAsync();
            Smart.Default.Parser.UseAlternativeEscapeChar();// '\\' is the default value

            m_Logger.LogInformation($"Initializing for host: {m_HostInformation.HostName} v{m_HostInformation.HostVersion}");
            await m_Host.InitAsync();

            m_Logger.LogInformation("Loading plugins...");

            var i = 0;
            foreach (var pluginAssembly in m_PluginAssemblyStore.LoadedPluginAssemblies)
            {
                if (await m_PluginActivator.TryActivatePluginAsync(pluginAssembly) != null)
                {
                    i++;
                }
            }

            m_Logger.LogInformation($"> {i} plugins loaded.");

            AsyncHelper.Schedule("OpenMod initialize event", () => m_EventBus.EmitAsync(m_Host, this, new OpenModInitializedEvent(m_Host)));
            await m_JobScheduler.StartAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return m_EventBus.EmitAsync(m_Host, this, new OpenModShutdownEvent());
        }
    }
}