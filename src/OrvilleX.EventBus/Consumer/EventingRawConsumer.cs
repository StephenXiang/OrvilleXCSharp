﻿using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using Microsoft.Extensions.Logging;

namespace OrvilleX.EventBus.Consumer
{
    /// <summary>
    /// 事件原始消费者
    /// </summary>
    public class EventingRawConsumer : EventingBasicConsumer, IRawConsumer
    {
        private readonly ILogger _logger;
        public List<ulong> AcknowledgedTags { get; }

        public EventingRawConsumer(IModel channel, ILogger<EventingRawConsumer> logger) : base(channel)
        {
            _logger = logger;
            AcknowledgedTags = new List<ulong>();
            SetupLogging(this);
        }

        private void SetupLogging(EventingBasicConsumer rawConsumer)
        {
            rawConsumer.Shutdown += (sender, args) =>
            {
                _logger.LogInformation($"Consumer {string.Join(",", rawConsumer.ConsumerTags)} has been shut down.\n  Reason: {args.Cause}\n  Initiator: {args.Initiator}\n  Reply Text: {args.ReplyText}");
            };
            rawConsumer.ConsumerCancelled +=
                (sender, args) => _logger.LogDebug($"The consumer with tag '{string.Join(",", args.ConsumerTags)}' has been cancelled.");
            rawConsumer.Unregistered +=
                (sender, args) => _logger.LogDebug($"The consumer with tag '{string.Join(",", args.ConsumerTags)}' has been unregistered.");
        }

        public Func<object, BasicDeliverEventArgs, Task> OnMessageAsync { get; set; }

        public void Disconnect()
        {
            if (ConsumerTags == null || ConsumerTags.Length <= 0)
            {
                return;
            }
            try
            {
                Model.BasicCancel(ConsumerTags[0]);
            }
            catch (AlreadyClosedException)
            {

            }
        }
    }
}
