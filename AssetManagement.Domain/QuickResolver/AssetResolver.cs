using AssetManagement.Domain.Context;
using AssetManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AssetManagement.Domain.QuickResolver
{
    public class AssetResolver
    {
        private readonly AssetManagementEntities Context = new AssetManagementEntities();

        public List<Asset> Assets()
        {
            return Context.Assets.ToList();
        }
        public List<Employee> Employees()
        {
            return Context.Employees.ToList();
        }
        public List<Department> Departments()
        {
            return Context.Departments.ToList();
        }

        //public List<SelectListItem> getCategories()
        //{
        //    List<SelectListItem> category = new List<SelectListItem>();

        //    category.Add(new SelectListItem { Text = "Category", Value = "0", Selected = true});

        //    var assetList = Context.Assets.Select(x => x.catergory)
        //        .OrderBy(x => x != null)
        //        .Distinct()
        //        .ToList();

        //    for (int i = 0; i < assetList.Count; i++)
        //    {
        //        category.Add(new SelectListItem { Text = assetList[i], Value= (i + 1).ToString() });
        //    }
        //    return category;
        //}

        //private List<SelectListItem> getManufacturers()
        //{
        //    var products = db.Products.Include(p => p.Category);
        //    var manufacturers = products.OrderBy(x => x.Manufacturer).Distinct().ToList();

        //    var manufacturerList = new List<string>();
        //    foreach (var item in manufacturers)
        //    {
        //        manufacturerList.Add(item.Manufacturer);
        //    }
        //    manufacturerList = manufacturerList.Distinct().ToList();
        //    List<SelectListItem> manufacturer = new List<SelectListItem>();
        //    manufacturer.Add(new SelectListItem { Text = "Manufacturer", Value = "0", Selected = true });

        //    for (int i = 0; i < manufacturerList.Count; i++)
        //    {
        //        manufacturer.Add(new SelectListItem { Text = manufacturerList[i], Value = (i + 1).ToString() });
        //    }
        //    return manufacturer;
        //}

        public List<SelectListItem> getCategories()
        {
            var assets = Context.Assets;
            var categories = assets.OrderBy(c => c.catergory).Distinct().ToList();

            var categoryList = new List<string>();
            foreach (var item in categories)
            {
                categoryList.Add(item.catergory);
            }
            categoryList = categoryList.Distinct().ToList();
            List<SelectListItem> catergory = new List<SelectListItem>();
            catergory.Add(new SelectListItem { Text = "Category", Value = "0", Selected = true });

            for (int i = 0; i < categoryList.Count; i++)
            {
                catergory.Add(new SelectListItem { Text = categoryList[i], Value = (i + 1).ToString() });
            }
            return catergory;
        }



    }
}
