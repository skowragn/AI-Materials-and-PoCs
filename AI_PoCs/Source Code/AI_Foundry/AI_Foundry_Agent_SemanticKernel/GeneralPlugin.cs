using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace AI_Foundry_Agent_SemanticKernel;

public class GeneralPlugin
{
    [KernelFunction]
    [Description("Gets the current time.")]
    public TimeSpan GetTime() => TimeProvider.System.GetLocalNow().TimeOfDay;

    [KernelFunction]
    [Description("Gets the current date.")]
    public string GetDate() => TimeProvider.System.GetLocalNow().Date.ToLongDateString();
}