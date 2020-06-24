using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace DrawingModule.Helper
{
    public class AttachableForStyleBehavior<TComponent, TBehavior> : Behavior<TComponent>
        where TComponent : System.Windows.DependencyObject
        where TBehavior : AttachableForStyleBehavior<TComponent, TBehavior>, new()
    {
        public static readonly DependencyProperty IsEnabledForStyleProperty =
            DependencyProperty.RegisterAttached(name: "IsEnabledForStyle",
                propertyType: typeof(bool),
                ownerType: typeof(AttachableForStyleBehavior<TComponent, TBehavior>),
                defaultMetadata: new FrameworkPropertyMetadata(false, OnIsEnabledForStyleChanged));

        public bool IsEnabledForStyle
        {
            get => (bool)GetValue(IsEnabledForStyleProperty);
            set => SetValue(IsEnabledForStyleProperty, value);
        }

        private static void OnIsEnabledForStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement uiElement)
            {
                var behaviors = Interaction.GetBehaviors(uiElement);
                var existingBehavior = behaviors.FirstOrDefault(b => b.GetType() == typeof(TBehavior)) as TBehavior;

                if ((bool)e.NewValue == false && existingBehavior != null)
                {
                    behaviors.Remove(existingBehavior);
                }
                else if ((bool)e.NewValue == true && existingBehavior == null)
                {
                    behaviors.Add(new TBehavior());
                }
            }
        }
    }
}
