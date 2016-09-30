using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssetManagement.Domain.Entities;

namespace AssetManagement.Business.AssetManagement
{
    public class AssetManagementLogic
    {
        private List<Laptop> laptops;
        private List<Printer> printers;
        private List<Mouse> mice;
        private List<Keyboard> keyboards;
        private List<PCBox> towers;

        private Domain.Context.AssetManagementEntities _context = new Domain.Context.AssetManagementEntities();
        public AssetManagementLogic()
        {
            laptops = _context.Laptops.ToList();
            printers = _context.Printers.ToList();
            mice = _context.Mice.ToList();
            keyboards = _context.Keyboards.ToList();
            towers = _context.PCBoxes.ToList();
        }

        public Laptop getLaptop(string id)
        {
            return laptops.Find(l=>l.assetNumber==id);
        }
        public Printer getPrinter(string id)
        {
            return printers.Find(l => l.assetNumber == id);
        }
        public Mouse getMouse(string id)
        {
            return mice.Find(l => l.assetNumber== id);
        }
        public Keyboard getKeyboard(string id)
        {
            return keyboards.Find(l => l.assetNumber == id);
        }

        public PCBox getPC(string id)
        {
            return towers.Find(l => l.assetNumber== id);
        }

        public object getAssetByType(string id,string category)
        {
          object asset = null;
            switch (category)
            {
                case "Laptop":
                    asset = getLaptop(id);
                    break;
                case "Printer":
                    asset = getPrinter(id);
                    break;
                case "Mouse":
                    asset = getMouse(id);
                    break;
                case "Keyboard":
                    asset = getKeyboard(id);
                    break;
                case "PCBox":
                    asset = getPC(id);
                    break;
            }
            return asset;
        }
        public Asset GetAsset(string id)
        {
            return _context.Assets.Find(int.Parse(id));
        }
    }

}
