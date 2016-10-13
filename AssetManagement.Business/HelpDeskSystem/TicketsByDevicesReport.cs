using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Business.HelpDeskSystem
{
    public class TicketsByDevicesReport
    {
        public int LaptopTickets { get; set; }
        public int PrinterTickets { get; set; }
        public int PCTickets { get; set; }
        public int KeyboardTickets { get; set; }
        public int MouseTickets { get; set; }
        public int MonitorTickets { get; set; }


        AssetManagement.AssetManagementLogic aml = new AssetManagement.AssetManagementLogic();
        HelpDeskLogic hdl = new HelpDeskLogic();

        public TicketsByDevicesReport()
        {
            LaptopTickets = 0;
            PrinterTickets = 0;
            PCTickets = 0;
            KeyboardTickets = 0;
            MouseTickets = 0;
            MonitorTickets = 0;
        }
        
        public void AssignTickets()
        {
            foreach (var ticket in hdl.allTickets)
            {
                dothis(ticket.assetid);
            }
        }

        public void dothis(int _assetID)
        {
            foreach (var asset in aml.laptops)
            {
                if (asset.assetID == _assetID)
                {
                    LaptopTickets += 1;
                }
            }
            foreach (var asset in aml.printers)
            {
                if (asset.assetID == _assetID)
                {
                    PrinterTickets += 1;
                }
            }
            foreach (var asset in aml.monitors)
            {
                if (asset.assetID == _assetID)
                {
                    PCTickets += 1;
                }
            }
            foreach (var asset in aml.keyboards)
            {
                if (asset.assetID == _assetID)
                {
                    KeyboardTickets += 1;
                }
            }
            foreach (var asset in aml.mice)
            {
                if (asset.assetID == _assetID)
                {
                    MouseTickets += 1;
                }
            }
            foreach (var asset in aml.monitors)
            {
                if (asset.assetID == _assetID)
                {
                    MonitorTickets += 1;
                }
            }
        }

        
    }
}
