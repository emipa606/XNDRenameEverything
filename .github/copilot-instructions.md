# GitHub Copilot Instructions for [XND] Rename Everything! (Continued)

## Mod Overview and Purpose
"[XND] Rename Everything! (Continued)" is a comprehensive RimWorld mod designed to enhance the player's experience by allowing the renaming of most in-game objects. Whether you want to recall a notable event associated with a weapon or treat a specific tree as a landmark, this mod empowers players to assign names to objects, creating a more personalized and immersive gameplay experience. The mod focuses on enabling renaming for anything that is a `ThingWithComps`, excluding pawns and corpses. 

## Key Features and Systems
- **Object Renaming**: Allows players to select most in-game objects and rename them for easier identification or storytelling purposes.
- **Gear Integration**: Supports renaming of equipped items via the pawn's gear tab.
- **Bill and Stockpile Configuration**: Configure workbench bills and stockpiles to accept or reject named/unnamed objects, providing greater control over resource management.
- **Dual Wield Compatibility**: Fully integrated with Roolo's Dual Wield mod, allowing renaming of both weapons when dual-wielding.
- **Infusion Compatibility**: Seamless integration with NotFood's Infused mod, ensuring compatibility with infused items.

## Coding Patterns and Conventions
- **Class and Method Structure**: The mod organizes functionality into static classes and methods, ensuring ease of access and reduced overhead. Abstract classes like `Command_Renamable` are utilized to enforce a consistent pattern for all renaming commands.
- **Modularity**: Each feature is handled by a distinct class, such as `RenameEverything`, `CompRenamable`, and multiple specialized `Command` classes, following the SOLID design principles.
- **Naming Conventions**: Follow C# standard practices; use PascalCasing for class and method names, camelCase for method parameters, and meaningful names to enhance code readability.

## XML Integration
Utilize RimWorld’s XML modding capabilities to define `ThingDefs` that will grant rename functionality. Ensure proper XML tags are in place, and consider adding custom XML attributes to support mod-specific behaviors.

## Harmony Patching
The mod harnesses the power of the Harmony Patch Library to inject features into the game without altering the base game code. It's crucial to identify the right methods to patch for the desired behaviors:
- Use `HarmonyPatch` annotations to specify target methods within RimWorld.
- Ensure patches are concise and effective, influencing game behavior with minimal impact on performance.

## Suggestions for Copilot
- **Enhanced Autocompletion**: Suggest names for objects based on their properties or context (e.g., a weapon’s material or type).
- **Context-Aware Suggestions**: Identify common renaming themes (e.g., event-based, descriptive terms) and suggest based on existing patterns.
- **XML Definitions**: Assist in creating well-structured XML files that mirror new content definitions or compatibility improvements.
- **Harmony Annotations**: Provide inline assistance with Harmony annotations to simplify complex patching logic.
- **Consistent Styling**: Automatically maintain the mod's coding conventions, especially when generating new classes or methods.

By following these guidelines, you can effectively contribute to the "[XND] Rename Everything! (Continued)" mod, maintaining its quality and ensuring compatibility across various mod setups. These instructions should assist new contributors and experienced modders alike in understanding the mod's structure and enhancing it further.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.
