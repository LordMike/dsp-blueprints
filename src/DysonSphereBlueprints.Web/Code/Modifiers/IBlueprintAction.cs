using DysonSphereBlueprints.Web.Code.Model;

namespace DysonSphereBlueprints.Web.Code.Modifiers;

public interface IBlueprintAction
{
    public string Title { get; }
    public string Description { get; }

    bool IsSupported(object entry);
    bool CanApply(object entry);
    void Apply(BlueprintEditModel bpModel, object entry);
    void ApplyAll(BlueprintEditModel bpModel);
}