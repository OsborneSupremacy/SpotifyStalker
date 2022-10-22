using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;

namespace SpotifyStalker.Service.Tests;

[ExcludeFromCodeCoverage]
public abstract class TestBase
{
    public IFixture Fixture { get; set; }

    protected TestBase()
    {
        Fixture = new Fixture()
            .Customize(
                new AutoMoqCustomization()
                {
                    ConfigureMembers = true,
                    GenerateDelegates = true
                }
            );
    }
}
