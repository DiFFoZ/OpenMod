﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using OpenMod.API.Ioc;

namespace OpenMod.API.Eventing
{
    /// <summary>
    /// The type safe callback for event notifications.
    /// </summary>
    /// <typeparam name="TEvent">The event type.</typeparam>
    /// <param name="sender">The event sender.</param>
    /// <param name="event">The event instance.</param>
    public delegate Task EventCallback<in TEvent>(IServiceProvider serviceProvider, object? sender, TEvent @event) where TEvent : IEvent;

    /// <summary>
    /// The callback for event notifications.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="event">The event instance.</param>
    public delegate Task EventCallback(IServiceProvider serviceProvider, object? sender, IEvent @event);

    /// <summary>
    /// The callback called after an event has been emitted and all listeners have been notified.
    /// </summary>
    /// <param name="event"></param>
    public delegate Task EventExecutedCallback(IEvent @event);

    /// <summary>
    /// The service for managing events and event subscriptions.
    /// </summary>
    [Service]
    public interface IEventBus
    {
        /// <summary>
        /// Subscribes a component to an event.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="eventName">The event to subscribe to.</param>
        /// <param name="callback">The action to execute. See <see cref="EventCallback" /></param>
        void Subscribe(IOpenModComponent component, string eventName, EventCallback callback);

        /// <summary>
        /// <inheritdoc cref="Subscribe(IOpenModComponent,string,EventCallback)" />
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="callback">The action to execute after all listeners were notified.</param>
        /// <typeparam name="TEvent">The event to subscribe to.</typeparam>
        void Subscribe<TEvent>(IOpenModComponent component, EventCallback<TEvent> callback)
            where TEvent : IEvent;

        /// <summary>
        /// <inheritdoc cref="Subscribe(IOpenModComponent,string,EventCallback)" />
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="callback">The action to execute after all listeners were notified.</param>
        /// <param name="eventType">The event to subscribe to.</param>
        void Subscribe(IOpenModComponent component, Type eventType, EventCallback callback);

        /// <summary>
        /// Finds and registers all <see cref="IEventListener"/>s.
        /// </summary>
        /// <param name="component">The component registering the event listeners.</param>
        /// <param name="assembly">The assembly to search for event listeners in.</param>
        void Subscribe(IOpenModComponent component, Assembly assembly);

        /// <summary>
        /// Unsubscribes a component from all events.
        /// </summary>
        /// <param name="component">The component owning the event listeners.</param>
        void Unsubscribe(IOpenModComponent component);

        /// <summary>
        /// Unsubscribes a component from an event.
        /// </summary>
        /// <param name="component">The component unsubscribing.</param>
        /// <param name="eventName">The event to unsubscribe from.</param>
        void Unsubscribe(IOpenModComponent component, string eventName);

        /// <summary>
        /// Unsubscribes a component from an event.
        /// </summary>
        /// <param name="component">The component unsubscribing.</param>
        /// <typeparam name="TEvent">The event to unsubscribe from.</typeparam>
        void Unsubscribe<TEvent>(IOpenModComponent component) where TEvent : IEvent;

        /// <summary>
        /// Unsubscribe a component from an event.
        /// </summary>
        /// <param name="component">The component unsubscribing.</param>
        /// <param name="eventType">The event to unsubscribe from.</param>
        void Unsubscribe(IOpenModComponent component, Type eventType);

        /// <summary>
        /// Emits an event.
        /// </summary>
        /// <param name="component">The component emitting the event.</param>
        /// <param name="sender">The object emitting the event.</param>
        /// <param name="event">The event object.</param>
        /// <param name="callback">The optional event callback. See <see cref="EventExecutedCallback" />.</param>
        Task EmitAsync(IOpenModComponent component, object? sender, IEvent @event, EventExecutedCallback? callback = null);
    }
}