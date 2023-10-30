using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ma.Terminal.SelfCard.KDXF.Controls
{
    public class ClickEffectGrid : Grid
    {
        public delegate void ClickEffectGridOnClickHandler(ClickEffectGrid sender);
        public delegate void ClickEffectGridOnPressStatusChangedHandler(ClickEffectGrid sender, bool isPressing);

        static Thickness DEFAULT_MARGIN_THICKNESS = new Thickness(0);
        static Thickness HOVER_MARGIN_THICKNESS = new Thickness(2);

        public event ClickEffectGridOnPressStatusChangedHandler OnPressing;
        public event ClickEffectGridOnClickHandler OnClick;

        public bool IsPressing
        {
            get { return (bool)GetValue(IsPressingProperty); }
            set { SetValue(IsPressingProperty, value); }
        }

        public static readonly DependencyProperty IsPressingProperty =
            DependencyProperty.Register("IsPressing", typeof(bool), typeof(ClickEffectGrid), new PropertyMetadata(false));


        public bool IsClickable
        {
            get { return (bool)GetValue(IsClickableProperty); }
            set { SetValue(IsClickableProperty, value); }
        }

        public static readonly DependencyProperty IsClickableProperty =
            DependencyProperty.Register("IsClickable", typeof(bool), typeof(ClickEffectGrid), new PropertyMetadata(true));


        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (IsClickable)
            {
                this.Margin = HOVER_MARGIN_THICKNESS;

                IsPressing = true;
                OnPressing?.Invoke(this, true);
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (IsClickable)
            {
                this.Margin = DEFAULT_MARGIN_THICKNESS;

                OnClick?.Invoke(this);

                IsPressing = false;
                OnPressing?.Invoke(this, false);
            }
        }

        protected override void OnTouchDown(TouchEventArgs e)
        {
            base.OnTouchDown(e);

            if (IsClickable)
            {
                this.Margin = HOVER_MARGIN_THICKNESS;

                IsPressing = true;
                OnPressing?.Invoke(this, true);
            }
        }

        protected override void OnTouchUp(TouchEventArgs e)
        {
            base.OnTouchUp(e);

            if (IsClickable)
            {
                this.Margin = DEFAULT_MARGIN_THICKNESS;

                IsPressing = false;
                OnPressing?.Invoke(this, false);
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            if (IsClickable)
            {
                this.Margin = DEFAULT_MARGIN_THICKNESS;

                IsPressing = false;
                OnPressing?.Invoke(this, false);
            }
        }

        protected override void OnTouchLeave(TouchEventArgs e)
        {
            base.OnTouchLeave(e);

            if (IsClickable)
            {
                this.Margin = DEFAULT_MARGIN_THICKNESS;

                IsPressing = false;
                OnPressing?.Invoke(this, false);
            }
        }
    }
}
