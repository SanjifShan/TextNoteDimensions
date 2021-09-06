using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;


/* 
The add-in works as follows:

1. Pick a point on the Document
2. Pick a textnote in the Document 
3. The addin creates a text note at the selected point printing the dimensions of the original textnote. A TaskDialog is also created.

This way we can check the dimensions of a textnote, rotate the plan view then check the dimensions again to see if they match.
 */

namespace TextNoteDimensions
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Get application and documnet objects 
            UIApplication uiapp = commandData.Application;
            Document doc = uiapp.ActiveUIDocument.Document;

            // Pick a point 
            Selection sel = uiapp.ActiveUIDocument.Selection;
            XYZ point = sel.PickPoint("Pick Text Note Location");

            // Pick the TextNote we want to check dimensions of
            Reference pickedref = null;
            pickedref = sel.PickObject(ObjectType.Element, "Please select a TextNote");
            Element elem = doc.GetElement(pickedref);

            // Cast TextNote as an Element object to use get width and get height methods (not available to TextNote object)
            TextElement textnote = elem as TextElement;

            // Get dimensions
            string width = textnote.Width.ToString();
            string height = textnote.Height.ToString();

            // Get the elemendid and viewid of TextNote
            ElementId elementid = doc.GetDefaultElementTypeId(ElementTypeGroup.TextNoteType);
            ElementId viewid = doc.ActiveView.Id;

            //Place the group
            Transaction trans = new Transaction(doc);
            trans.Start("TextNote Dimensions");
            TextNote.Create(doc, viewid, point, "width = " + width + "    height = " + height, elementid);
            TaskDialog.Show("TextNote Dimensions", "width = " + width + "    height = " + height);
            trans.Commit();
            trans.Dispose();

            return Result.Succeeded;

        }
    }
}
