using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MoreTokensApp {

    public class ShowMoreBehavior : Behavior<ComboBoxEdit> {
        protected ComboBoxEdit ComboBoxEdit => AssociatedObject;

        #region Dependency Properties

        public double CollapsedHeight {
            get => (double)GetValue(CollapsedHeightProperty);
            set => SetValue(CollapsedHeightProperty, value);
        }
        public static readonly DependencyProperty CollapsedHeightProperty =
         DependencyProperty.Register(nameof(CollapsedHeight), typeof(double), typeof(ShowMoreBehavior), new PropertyMetadata(22.0));

        public bool IsExpanded {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }
        public static readonly DependencyProperty IsExpandedProperty =
         DependencyProperty.Register(nameof(IsExpanded), typeof(bool), typeof(ShowMoreBehavior), new PropertyMetadata(false, OnIsExpandedChanged));

        protected static void OnIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ShowMoreBehavior behavior = d as ShowMoreBehavior;
            if(behavior?.ComboBoxEdit != null)
                behavior.ComboBoxEdit.MaxHeight = (bool)e.NewValue ? double.PositiveInfinity : behavior.CollapsedHeight;
        }

        public string ShowMoreButtonText {
            get => (string)GetValue(ShowMoreButtonTextProperty);
            set => SetValue(ShowMoreButtonTextProperty, value);
        }
        public static readonly DependencyProperty ShowMoreButtonTextProperty =
         DependencyProperty.Register(nameof(ShowMoreButtonText), typeof(string), typeof(ShowMoreBehavior), new PropertyMetadata(string.Empty));

        public bool ShowMoreButtonVisible {
            get => (bool)GetValue(ShowMoreButtonVisibleProperty);
            set => SetValue(ShowMoreButtonVisibleProperty, value);
        }
        public static readonly DependencyProperty ShowMoreButtonVisibleProperty =
         DependencyProperty.Register(nameof(ShowMoreButtonVisible), typeof(bool), typeof(ShowMoreBehavior), new PropertyMetadata(false));

        public double ScrollableHeight {
            get => (double)GetValue(ScrollableHeightProperty);
            set => SetValue(ScrollableHeightProperty, value);
        }
        public static readonly DependencyProperty ScrollableHeightProperty =
         DependencyProperty.Register(nameof(ScrollableHeight), typeof(double), typeof(ShowMoreBehavior), new PropertyMetadata(0d, OnScrollableHeightChanged));

        protected static void OnScrollableHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            (d as ShowMoreBehavior)?.UpdateShowMoreButtonVisibility();
        }

        #endregion

        public DelegateCommand ToggleExpandCollapseCommand { get; protected set; }

        protected override void OnAttached() {
            base.OnAttached();
            InitializeBehavior();
            ToggleExpandCollapseCommand = new DelegateCommand(ToggleExpandCollapse);
        }

        protected void InitializeBehavior() {
            ApplyStyleSettings();
            ConfigureShowMoreButton();
            ComboBoxEdit.ShowCustomItems = true;
            ComboBoxEdit.SizeChanged += ComboBoxEdit_SizeChanged;
            ComboBoxEdit.EditValueChanged += ComboBoxEdit_EditValueChanged;
            ComboBoxEdit.Loaded += ComboBoxEdit_Loaded;
            ComboBoxEdit.MaxHeight = CollapsedHeight;
        }

        protected void ComboBoxEdit_Loaded(object sender, RoutedEventArgs e) {
            ComboBoxEdit.Loaded -= ComboBoxEdit_Loaded;
            TokenEditor tokenEditor = LayoutTreeHelper.GetVisualChildren(ComboBoxEdit).OfType<TokenEditor>().FirstOrDefault();
            if(tokenEditor != null)
                ScrollViewer.SetVerticalScrollBarVisibility(tokenEditor, ScrollBarVisibility.Hidden);
            ScrollViewer scrollViewer = LayoutTreeHelper.GetVisualChildren(ComboBoxEdit).OfType<ScrollViewer>().FirstOrDefault();
            if(scrollViewer != null)
                BindingOperations.SetBinding(this, ScrollableHeightProperty, new Binding("ScrollableHeight") { Source = scrollViewer });
        }

        protected void ComboBoxEdit_EditValueChanged(object sender, EditValueChangedEventArgs e) {
            Dispatcher.BeginInvoke(new Action(UpdateShowMoreButtonVisibility), System.Windows.Threading.DispatcherPriority.Loaded);
        }

        protected void ComboBoxEdit_SizeChanged(object sender, SizeChangedEventArgs e) {
            UpdateShowMoreButtonVisibility();
        }

        protected void ApplyStyleSettings() {
            ComboBoxEdit.StyleSettings = new CheckedTokenComboBoxStyleSettings {
                EnableTokenWrapping = true,
                FilterOutSelectedTokens = false,
                NewTokenPosition = NewTokenPosition.None
            };
        }

        protected void ConfigureShowMoreButton() {
            ComboBoxEdit.AllowDefaultButton = false;
            var showMoreButton = new ButtonInfo {
                GlyphKind = GlyphKind.User,
                ButtonKind = ButtonKind.Toggle,
                Command = ToggleExpandCollapseCommand
            };
            showMoreButton.SetBinding(ButtonInfo.ContentProperty, new Binding(nameof(ShowMoreButtonText)) { Source = this });
            showMoreButton.SetBinding(ButtonInfo.VisibilityProperty, new Binding(nameof(ShowMoreButtonVisible)) { Source = this, Converter = new DevExpress.Mvvm.UI.BooleanToVisibilityConverter() });
            showMoreButton.SetBinding(ButtonInfo.IsCheckedProperty, new Binding(nameof(IsExpanded)) { Source = this });
            ComboBoxEdit.Buttons.Add(showMoreButton);
            ComboBoxEdit.Buttons.Add(new ButtonInfo { GlyphKind = GlyphKind.DropDown, IsDefaultButton = true });
        }

        protected override void OnDetaching() {
            ComboBoxEdit.SizeChanged -= ComboBoxEdit_SizeChanged;
            ComboBoxEdit.EditValueChanged -= ComboBoxEdit_EditValueChanged;
            base.OnDetaching();
        }

        protected void UpdateShowMoreButtonText() {
            ShowMoreButtonText = IsExpanded ? "Show Less" : "Show More";
        }

        protected void UpdateShowMoreButtonVisibility() {
            if(IsExpanded && ComboBoxEdit.ActualHeight / CollapsedHeight <= 1) {
                IsExpanded = false;
            }
            ShowMoreButtonVisible = ScrollableHeight > 0 || IsExpanded;
            Dispatcher.BeginInvoke(new Action(UpdateShowMoreButtonText), System.Windows.Threading.DispatcherPriority.Loaded);
        }

        protected void ToggleExpandCollapse() {
            IsExpanded = !IsExpanded;
        }
    }

}
