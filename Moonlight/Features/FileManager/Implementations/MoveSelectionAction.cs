using MoonCoreUI.Services;
using Moonlight.Features.FileManager.Interfaces;
using Moonlight.Features.FileManager.Models.Abstractions.FileAccess;

namespace Moonlight.Features.FileManager.Implementations;

public class MoveSelectionAction : IFileManagerSelectionAction
{
    public string Name => "Move";
    public string Color => "primary";
    
    public async Task Execute(BaseFileAccess access, UI.NewFileManager.FileManager fileManager, FileEntry[] entries, IServiceProvider provider)
    {
        await fileManager.OpenFolderSelect("Select the location to move the items to", async path =>
        {
            var toastService = provider.GetRequiredService<ToastService>();

            await toastService.CreateProgress("fileManagerSelectionMove", "Moving items");

            foreach (var entry in entries)
            {
                await toastService.ModifyProgress("fileManagerSelectionMove", $"Moving '{entry.Name}'");

                await access.Move(entry, path + entry.Name);
            }

            await toastService.RemoveProgress("fileManagerSelectionMove");

            await toastService.Success("Successfully moved selection");
            await fileManager.View.Refresh();
        });
    }
}