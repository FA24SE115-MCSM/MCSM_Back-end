using MCSM_Utility.Helpers.ModelBinder;
using Microsoft.AspNetCore.Mvc;

namespace MCSM_Data.Models.Requests.Post
{
    [ModelBinder(BinderType = typeof(MetadataValueModelBinder))]
    public class CreateRetreatLearningOutcomeModel
    {
        public string Title { get; set; } = null!;

        public string? SubTitle { get; set; }

        public string? Description { get; set; }
    }
}
