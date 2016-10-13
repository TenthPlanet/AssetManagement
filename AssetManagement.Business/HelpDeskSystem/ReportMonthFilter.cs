using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Business.HelpDeskSystem
{
    public class ReportMonthFilter
    {
        public FinencialReport ThisMonth;
        public FinencialReport PrevousMonth;
        public FinencialReport TwoMonthsBefore;
        public FinencialReport ThreeMonthsBefore;
        public FinencialReport FourMonthsBefore;
        public FinencialReport FiveMonthsBefore;
        public DateTime today = DateTime.Today;
        private HelpDeskLogic hdl = new HelpDeskLogic();
        public ReportMonthFilter()
        {
            ThisMonth = hdl.FinencialReportPerMonth(today.Month);
            PrevousMonth = hdl.FinencialReportPerMonth(today.Month-1);
            TwoMonthsBefore = hdl.FinencialReportPerMonth(today.Month-2);
            ThreeMonthsBefore = hdl.FinencialReportPerMonth(today.Month-3);
            FourMonthsBefore = hdl.FinencialReportPerMonth(today.Month-4);
            FiveMonthsBefore = hdl.FinencialReportPerMonth(today.Month-5);
        }
    }
}
