# BlueprintData Extracted from DSP

This project contains a manually extracted version of the `BlueprintData` type and its dependencies from **Dyson Sphere Program's** `Assembly-CSharp.dll`.

## 🔍 Extraction Process

To isolate the functionality I needed, I used **JetBrains dotPeek** to decompile and extract types from the original game assembly.

### Steps taken:

1. Opened `Assembly-CSharp.dll` (from DSP's `Managed` folder) in **dotPeek**.
2. Located the `BlueprintData` type and exported its decompiled source.
3. Identified all referenced types, one by one, and extracted only the necessary ones to get a compiling setup.
4. Stripped out unrelated members and logic where possible (e.g. game engine calls, Unity types) to keep the code minimal and dependency-free.
5. Created stub types or simplified implementations as needed (e.g. structs, enums, helper methods).

The goal was to **carve out just enough to work with `BlueprintData` independently**, without referencing the full DSP game assembly.

## 💡 Notes

- The extracted code is not guaranteed to match the original exactly — it's tailored to compile and support the limited functionality I need.
- Some original functionality may be omitted or stubbed.
- This setup is intended for internal tooling or analysis, not for runtime use in the game.

## 📁 Original DLL Location (for reference)

If needed again, the original `Assembly-CSharp.dll` can be found at:

`..\Steam\steamapps\common\Dyson Sphere Program\DSPGAME_Data\Managed\Assembly-CSharp.dll`


> ⚠️ **Do not distribute the original DLL or any large portions of extracted game code.** This project only includes what’s necessary to work with blueprints in a standalone context.
