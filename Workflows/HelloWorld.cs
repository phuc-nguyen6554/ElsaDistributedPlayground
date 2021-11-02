﻿using Elsa.Activities.Console;
using Elsa.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elsa.Activities.MassTransit;
using ElsaPlayground.Messages;
using Microsoft.AspNetCore.Http;

namespace ElsaPlayground.Workflows
{
    public class HelloWorld : IWorkflow
    {
        public void Build(IWorkflowBuilder builder)
        {
            var test = Environment.GetEnvironmentVariable("TESTING_VARIABLE") ?? "";
            builder
                .WithDescription("test")
                .ReceiveMassTransitMessage(x => x.Set(y => y.MessageType, z => typeof(TestMessage)))
                .WriteLine("Hello " + test);
        }
    }
}