﻿using Shared.Commands;
using Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Shared.Bus
{
    public interface IEventBus
    {
        //Task SendCommand<T>(T command) where T : Command;

        Task Publish<T>(T @event, string queue) where T : Event;

        void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>;
    }
}
