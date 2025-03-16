using Radzen;

namespace DysonSphereBlueprints.Web.Code.Modifiers;

public interface IBlueprintAction
{
    public string Title { get; }
    public string Description { get; }

    /// <summary>
    /// Checks if this action can be applied to the given entry.
    /// </summary>
    bool CanApply(object entry);

    /// <summary>
    /// Applies the action to all applicable entries or a single specific entry.
    /// </summary>
    void Apply(object? single = null);
}

public abstract class BlueprintAction<T>(NotificationService notificationService) : IBlueprintAction where T : class
{
    public abstract string Title { get; }
    public abstract string Description { get; }

    bool IBlueprintAction.CanApply(object entry)
    {
        if (entry is T asT)
            return CanApply(asT);

        return false;
    }

    void IBlueprintAction.Apply(object? single)
    {
        if (single is T asT)
            Apply(asT);
        else if (single == null)
            Apply();
    }

    /// <summary>
    /// Checks if this action can be applied to the given entry.
    /// </summary>
    public abstract bool CanApply(T entry);

    /// <summary>
    /// Performs the action on a single entry.
    /// </summary>
    protected abstract BlueprintActionResult PerformSingle(T entry);

    /// <summary>
    /// Applies the action to all applicable entries or a single specific entry.
    /// </summary>
    public void Apply(T? single = null)
    {
        IEnumerable<T> targets = DiscoverEntries()
            .Where(s => single == null || EqualityComparer<T>.Default.Equals(s, single));

        BlueprintActionResult[] results = targets
            .Select(arg => CanApply(arg) ? PerformSingle(arg) : BlueprintActionResult.Skipped).ToArray();

        NotifyResults(results);
    }

    protected abstract IEnumerable<T> DiscoverEntries();

    /// <summary>
    /// Displays a notification summarizing the results.
    /// </summary>
    private void NotifyResults(BlueprintActionResult[] results)
    {
        int Count(BlueprintActionResult result) => results.Count(x => x == result);

        string str =
            $"Altered {Count(BlueprintActionResult.Success)} of {results.Length}, skipped: {Count(BlueprintActionResult.Skipped)}";

        notificationService.Notify(new NotificationMessage
        {
            Duration = 10000,
            Severity = results.All(x => x == BlueprintActionResult.Success)
                ? NotificationSeverity.Success
                : results.Any(x => x == BlueprintActionResult.Failed)
                    ? NotificationSeverity.Warning
                    : NotificationSeverity.Info,
            Summary = "Action Applied",
            Detail = str
        });
    }
}