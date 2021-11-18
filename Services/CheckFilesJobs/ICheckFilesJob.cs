using System.Threading.Tasks;

namespace Services.CheckFilesJobs
{
    public interface ICheckFilesJob
    {
        Task DeleteImageFromDbIfFileNotExists();
    }
}