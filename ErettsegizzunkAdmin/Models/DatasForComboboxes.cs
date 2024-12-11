using ErettsegizzunkApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErettsegizzunkAdmin.Services;

namespace ErettsegizzunkAdmin.Models
{
    internal class DatasForComboboxes
    {
        public List<Szint> szintek { get; set; }
        public List<Tantargyak> tantargyak { get; set; }
        public List<Tema> temak { get; set; }
        public List<Tipu> tipusok { get; set; }

        public DatasForComboboxes(ApiService apiService)
        {
            AdatokFeltoltese(apiService);
        }

        public async Task AdatokFeltoltese(ApiService apiService)
        {
            tantargyak = await apiService.GetTantargyaksAsync();
        }
    }
}
