using DysonSphereBlueprints.Web.Code.Model;
using Radzen;

namespace DysonSphereBlueprints.Web.Code.Modifiers;

public abstract class BlueprintAction<T>(NotificationService notificationService) : IBlueprintAction where T : class
{
    public abstract string Title { get; }
    public abstract string Description { get; }

    bool IBlueprintAction.IsSupported(object entry) => entry is T;

    bool IBlueprintAction.CanApply(object entry)
    {
        if (entry is T asT)
            return CanApply(asT);

        return false;
    }

    void IBlueprintAction.Apply(BlueprintEditModel bpModel, object entry)
    {
        if (entry is T asT)
            Apply(asT);
        else
            throw new InvalidOperationException("Unexpected");
    }

    /// <summary>
    /// Checks if this action can be applied to the given entry.
    /// </summary>
    public abstract bool CanApply(T entry);

    /// <summary>
    /// Performs the action on a single entry.
    /// </summary>
    protected abstract void PerformSingle(T entry);

    public void Apply(T entry)
    {
        if (!CanApply(entry))
            throw new InvalidOperationException("Unexpected");

        PerformSingle(entry);

        NotifyResults([entry]);
    }

    public void ApplyAll(BlueprintEditModel bpModel)
    {
        var entries = DiscoverEntries(bpModel)
            .Where(CanApply)
            .ToArray();

        foreach (var entry in entries)
            PerformSingle(entry);

        NotifyResults(entries);
    }

    protected abstract IEnumerable<T> DiscoverEntries(BlueprintEditModel bpModel);

    /// <summary>
    /// Displays a notification summarizing the results.
    /// </summary>
    private void NotifyResults(T[] entries)
    {
        string str = $"Altered {entries.Length} entries";

        notificationService.Notify(new NotificationMessage
        {
            Duration = 10000,
            Severity = entries.Length == 0 ? NotificationSeverity.Warning : NotificationSeverity.Info,
            Summary = "Action Applied",
            Detail = str
        });
    }
}