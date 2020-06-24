using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AppModels.ApplicationData;

namespace DrawingModule.ViewModels
{
    public class TestMethodClass
    {
        [CommandMethod("TestMethod")]
        private void TestMethod()
        {
            Editor editor = Application.DocumentManager.MdiActiveDocument.Editor;
            //PromptResult @string = editor.GetString("TestMethod");
            string @string = "TestMethod";
            //Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(@string);
        }

        [CommandMethod("TestKiemTra")]
        private void TestMethod2()
        {
            MessageBox.Show("TestSuccess2");
        }

        [CommandMethod("Check")]
        private void TestAblitity()
        {
            MessageBox.Show("Thanh Cong roi");
        }

       
        [CommandMethod("Trim")]
        private void Trime()
        { }

        [CommandMethod("TryAl")]
        private void TryAl()
        {

        }
        [CommandMethod("Tick")]
        private void Tick()
        {

        }
        [CommandMethod("Tok")]
        private void Tok()
        {

        }
        [CommandMethod("Tell")]
        private void Tell()
        {

        }
        [CommandMethod("Thoundsand")]
        private void Thoundsand()
        {

        }
    }
}
