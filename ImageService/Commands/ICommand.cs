

namespace ImageService.Commands
{
    public interface ICommand
    {
        string Execute(string[] args, out bool result);          // The Function That will Execute The 
    }
}
