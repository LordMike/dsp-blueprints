namespace DysonSphereBlueprints.Gamelibs.Code;

public enum BlueprintDataIOError
{
    OK,
    FileIOError,
    HeaderDataError,
    HeaderTooLong,
    MD5CannotMatch,
    DataCorruption,
}