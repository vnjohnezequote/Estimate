using System.ComponentModel;
using System.Drawing;
using System.Windows.Input;
using ApplicationInterfaceCore.Enums;
using devDept.Eyeshot;
using devDept.Geometry;
using devDept.Graphics;

namespace ApplicationInterfaceCore
{
    public interface ICadDrawAble : INotifyPropertyChanged
    {
        string ActiveLevel { get; }
        int DimTextHeight { get; }
        ViewportList Viewports { get; }
        int ActiveViewportIndex { get; }
        Size Size{ get; }
        Point3D LastClickPoint { get; }
        Plane DrawingPlane { get; }
        //IDrawInteractive SelectTool { get; }
        int PickBoxSize { get; }
        EntityList Entities { get; }
        RenderContextBase renderContext { get; }
        double CurrentLengthDimension { get; }
        double CurrentAngleDimension { get; }
        double CurrentHeightDimension { get; }
        double CurrentWidthDimension { get; }
        string CurrentText { get; }
        double CurrentTextHeight { get; }
        double CurrentTextAngle { get; }
        double ScaleFactor { get; }
        BlockKeyedCollection Blocks { get; }
        IDynamicInputView DynamicInput { get; }
        LayerKeyedCollection Layers { get; }
        void RefreshEntities();
        void Invalidate();
        void HideCursor(bool isHide);
        bool Focus();
        void UpdateCurrentPointByLengthAndAngle(double length, double angle,double scaleFactor);
        bool ScreenToPlane(System.Drawing.Point mousePosition,Plane sketchPlant, out Point3D point);
        System.Windows.Point GetMousePosition(MouseEventArgs e);
        int[] GetAllEntitiesUnderMouseCursor(System.Drawing.Point mousePosition, bool selectTableOnly = true);
        Point3D WorldToScreen(Point3D point);
        Point3D WorldToScreen(double x, double y, double z);
        void UpdateCurrentPointByWidthAndHeight(double width, double height,SetDimensionType setType);
        int GetEntityUnderMouseCursor(System.Drawing.Point mousePs, bool selectableOnlu = true);
        //System.Drawing.Size DrawText(int x, int y, string text, Font textFont, System.Drawing.Color textColor, System.Drawing.Color fillColor, ContentAlignment textAlign, RotateFlipType rotateFlip);
        //System.Drawing.Size DrawText(int x, int y, string text, Font textFont, System.Drawing.Color textColor, System.Drawing.Color fillColor, ContentAlignment textAlign);
        System.Drawing.Size DrawTextString(int x, int y, string text, Font textFont, System.Drawing.Color textColor, ContentAlignment textAlign);


    }
}