using ErettsegizzunkAdmin.Services;
using ErettsegizzunkApi.Models;

namespace ErettsegizzunkAdmin.Models
{
    internal class DatasForComboboxes
    {
        public List<Level> szintek { get; set; }
        public List<Subject> tantargyak { get; set; }
        public List<Theme> temak { get; set; }
        public List<ErettsegizzunkApi.Models.Type> tipusok { get; set; }

        public DatasForComboboxes(ApiService apiService)
        {
            AdatokFeltoltese(apiService);
        }

        public async System.Threading.Tasks.Task AdatokFeltoltese(ApiService apiService)
        {
            tantargyak = await apiService.GetTantargyaksAsync();
        }
    }
}
