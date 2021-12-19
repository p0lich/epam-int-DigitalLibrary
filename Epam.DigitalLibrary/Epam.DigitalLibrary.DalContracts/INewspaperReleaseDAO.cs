using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.Entities.Models.NewspaperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.DalContracts
{
    public interface INewspaperReleaseDAO
    {
        public List<NewspaperDetailsViewModel> GetAllNewspapers();
        public List<Newspaper> GetAllNewspaperReleases(Guid id);
        public NewspaperDetailsViewModel GetNewspaperRelease(Guid id);
        public int AddNewspaperRelease(NewspaperInputViewModel newspaperModel, out Guid newspaperReleaseId);
        public int UpdateNewspaperRelease(Guid id, NewspaperInputViewModel newspaperModel);
        public bool MarkForDeleteNewspaperRelease(Guid id);
        public bool SetRelease(Guid newspaperId, Guid releaseId);

        public List<Newspaper> GetReleaseNewspapers(Guid newspaperReleaseId);
    }
}
