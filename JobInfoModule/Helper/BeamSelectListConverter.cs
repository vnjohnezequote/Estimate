using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppModels.ResponsiveData;
using AppModels.ResponsiveData.EngineerMember;
using AppModels.ResponsiveData.Openings;
using JobInfoModule.ViewModels;
using Syncfusion.UI.Xaml.Grid;

namespace JobInfoModule.Helper
{
    public class BeamSelectListConverter: IItemsSourceSelector
    {
        public IEnumerable GetItemsSource(object record, object dataContext)
        {
            if (record == null)
            {
                return null;
            }

            var beam = record as EngineerMemberInfo;
            var beamGrade = beam.TimberGrade;
            if (beamGrade == null)
            {
                return null;
            }
            var viewModel = dataContext as EngineerInfoViewModel;


            if (viewModel != null && viewModel.SelectedClient.Beams.ContainsKey(beamGrade.ToString()))
            {
                List<TimberBase> beams = null;
                viewModel.SelectedClient.Beams.TryGetValue(beamGrade, out beams);
                return (beams ?? throw new InvalidOperationException()).ToList();
            }
            return null;
        }
    }
}
