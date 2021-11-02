using Elsa.Activities.Console;
using Elsa.Activities.Temporal;
using Elsa.Builders;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElsaPlayground.Workflows
{
    public class TestTimer : IWorkflow
    {
        public void Build(IWorkflowBuilder builder)
        {
            builder
                .Timer(Duration.FromSeconds(10))
                .WriteLine("Hello");
        }
    }
}
