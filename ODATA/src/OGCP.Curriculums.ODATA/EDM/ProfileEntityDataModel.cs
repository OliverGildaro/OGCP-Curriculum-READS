using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using OGCP.Curriculums.DAL.Model;
using System;

namespace OGCP.Curriculums.ODATA.EDM
{
    public class ProfileEntityDataModel
    {
        public IEdmModel GetEntityDataModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.Namespace = "ProfilesCVs";
            builder.ContainerName = "ProfilesContainer";

            builder.EntitySet<Profile>("Profiles");
            builder.EntitySet<Language>("Languages");
            builder.EntitySet<Education>("Educations");

            //var isHighRatedFunction = builder.EntityType<RecordStore>()
            //    .Function("IsHighRated");
            //isHighRatedFunction.Returns<bool>();
            //isHighRatedFunction.Parameter<int>("minimumRating");
            //isHighRatedFunction.Namespace = "AirVinyl.Functions";

            //var areRatedByFunction = builder.EntityType<RecordStore>().Collection
            //    .Function("AreRatedBy");
            //areRatedByFunction.ReturnsCollectionFromEntitySet<RecordStore>("RecordStores");
            //areRatedByFunction.CollectionParameter<int>("personIds");
            //areRatedByFunction.Namespace = "AirVinyl.Functions";

            //var getHighRatedRecordStoresFunction = builder.Function("GetHighRatedRecordStores");
            //getHighRatedRecordStoresFunction.Parameter<int>("minimumRating");
            //getHighRatedRecordStoresFunction.ReturnsCollectionFromEntitySet<RecordStore>("RecordStores");
            //getHighRatedRecordStoresFunction.Namespace = "AirVinyl.Functions";

            //var rateAction = builder.EntityType<RecordStore>()
            //    .Action("Rate");
            //rateAction.Returns<bool>();
            //rateAction.Parameter<int>("rating");
            //rateAction.Parameter<int>("personId");
            //rateAction.Namespace = "AirVinyl.Actions";

            //var removeRatingsAction = builder.EntityType<RecordStore>().Collection
            //   .Action("RemoveRatings");
            //removeRatingsAction.Returns<bool>();
            //removeRatingsAction.Parameter<int>("personId");
            //removeRatingsAction.Namespace = "AirVinyl.Actions";

            //var removeRecordStoreRatingsAction = builder.Action("RemoveRecordStoreRatings");
            //removeRecordStoreRatingsAction.Parameter<int>("personId");
            //removeRecordStoreRatingsAction.Namespace = "AirVinyl.Actions";

            //// "Tim" singleton
            //builder.Singleton<Person>("Tim");

            return builder.GetEdmModel();
        }
    }
}
