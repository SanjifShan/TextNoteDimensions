using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Media.Imaging;


namespace TextNoteDimensions
{
    class App : IExternalApplication
    {
        // define a method that will create our tab and button

        public Result OnShutdown(UIControlledApplication application)
        {
            // do nothing
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            // call our method that will load up our toolbar
            AddRibbonPanel(application);
            return Result.Succeeded;

        }

        static void AddRibbonPanel(UIControlledApplication application)
        {
            // Create a custom ribbon tab
            String tabName = "TextNote Dimensions";
            application.CreateRibbonTab(tabName);

            // Add a new ribbon panel
            RibbonPanel ribbonPanel = application.CreateRibbonPanel(tabName, "Tools");

            // Get dll assembly path
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            // create push button for CurveTotalLength
            PushButtonData b1Data = new PushButtonData(
                "TextNote Dimensions",
                " TextNote dimensions ",
                thisAssemblyPath,
                "TextNoteDimensions.Command");

            PushButton pb1 = ribbonPanel.AddItem(b1Data) as PushButton;
            pb1.ToolTip = "Extract TextNote dimensions";
            BitmapImage pb1Image = new BitmapImage(new Uri("pack://application:,,,/TextNoteDimensions;component/Resources/proBIMLogo.png"));
            pb1.LargeImage = pb1Image;
        }

    }
    
}
