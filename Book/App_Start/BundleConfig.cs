﻿using System.Web;
using System.Web.Optimization;

namespace Book
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/base.css",
                       "~/Content/book1.css",
                        "~/Content/cart.css"
                      ));
        }
    }
}
