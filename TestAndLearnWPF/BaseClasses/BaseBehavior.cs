using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;

namespace BasicClassLibrary.AttachedProperties
{
    /// <summary>
    /// A base behavior
    /// </summary>
    /// <typeparam name="Parent">The parent dependency object</typeparam>
    /// <typeparam name="Property">The type of binding property</typeparam>
    public abstract class BaseBehavior<Parent, PropertyType> : Behavior<Parent> where Parent : DependencyObject, new()
    {
        #region Public Events

        /// <summary>
        /// Fired when the value changes
        /// </summary>
        public event Action<DependencyObject, DependencyPropertyChangedEventArgs> ValueChanged = (sender, e) => { };

        /// <summary>
        /// Fired when the value changes, even when the value is the same
        /// </summary>
        public event Action<DependencyObject, object> ValueUpdated = (sender, value) => { };

        #endregion

        #region Attached Property Definitions
        /// <summary>
        /// The attached property for this class
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(PropertyType),
            typeof(BaseBehavior<Parent, PropertyType>),
            new UIPropertyMetadata(
                default(PropertyType),
                new PropertyChangedCallback(OnValuePropertyChanged),
                new CoerceValueCallback(OnValuePropertyUpdated)
                ));

        /// <summary>
        /// The callback event when the <see cref="ValueProperty"/> is changed
        /// </summary>
        /// <param name="d">The UI element that had it's property changed</param>
        /// <param name="e">The arguments for the event</param>
        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Call the parent function
            (d as BaseBehavior<Parent, PropertyType>)?.OnValueChanged(d, e);

            // Call event listeners
            (d as BaseBehavior<Parent, PropertyType>)?.ValueChanged?.Invoke(d, e);
        }

        /// <summary>
        /// The callback event when the <see cref="ValueProperty"/> is changed, even if it is the same value
        /// </summary>
        /// <param name="d">The UI element that had it's property changed</param>
        /// <param name="e">The arguments for the event</param>
        private static object OnValuePropertyUpdated(DependencyObject d, object value)
        {
            // Call the parent function
            (d as BaseBehavior<Parent, PropertyType>)?.OnValueUpdated(d, value);

            // Call event listeners
            (d as BaseBehavior<Parent, PropertyType>)?.ValueUpdated?.Invoke(d, value);

            // Return the value
            return value;
        }

        public PropertyType Value
        {
            get => (PropertyType)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        /// <summary>
        /// Gets the attached property
        /// </summary>
        /// <param name="d">The element to get the property from</param>
        /// <returns></returns>
        public static PropertyType GetValue(DependencyObject d)
        {
            return (PropertyType)d.GetValue(ValueProperty);
        }

        /// <summary>
        /// Sets the attached property
        /// </summary>
        /// <param name="d">The element to get the property from</param>
        /// <param name="value">The value to set the property to</param>
        public static void SetValue(DependencyObject d, PropertyType value)
        {
            d.SetValue(ValueProperty, value);
        }
        #endregion

        #region Event Methods
        /// <summary>
        /// The method that is called when any attached property of this type is changed
        /// </summary>
        /// <param name="sender">The UI element that this property was changed for</param>
        /// <param name="e">The arguments for this event</param>
        protected virtual void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) { }

        /// <summary>
        /// The method that is called when any attached property of this type is changed, even if the value is the same
        /// </summary>
        /// <param name="sender">The UI element that this property was changed for</param>
        /// <param name="e">The arguments for this event</param>
        protected virtual void OnValueUpdated(DependencyObject sender, object value) { }
        #endregion
    }
}
