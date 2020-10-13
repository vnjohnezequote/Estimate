using devDept.Eyeshot;

namespace AppModels.NewReposiveData.Interface
{
    public interface IJob
    {
        string JobNumber { get; set; }
        string JobAddress { get; set; }
        string SubAddress { get; set; }
        string Builder { get; set; }
        string Client { get; set; }
    }
}