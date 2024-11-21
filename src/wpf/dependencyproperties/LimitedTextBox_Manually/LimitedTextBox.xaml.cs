// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LimitedTextBox_Manually
{
    /// <summary>
    /// Interaction logic for LimitedTextBox.xaml
    /// </summary>
    public partial class LimitedTextBox : UserControl
    {
        public LimitedTextBox()
        {
            this.InitializeComponent();
        }

        // [<snippet MaxLength_Property>]
        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register(
            nameof(MaxLength),
            typeof(int),
            typeof(LimitedTextBox),
            new PropertyMetadata( 100, OnMaxLengthChanged ),
            ValidateMaxLength );

        public int MaxLength
        {
            get { return (int) this.GetValue( MaxLengthProperty ); }
            set
            {
                this.SetValue( MaxLengthProperty, value );
            }
        }

        // [<endsnippet MaxLength_Property>]

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(LimitedTextBox),
            new PropertyMetadata( string.Empty, OnTextChanged ),
            ValidateText );

        public string Text
        {
            get { return (string) this.GetValue( TextProperty ); }
            set
            {
                this.SetValue( TextProperty, value );
            }
        }

        // [<snippet OnMaxLengthChanged>]
        private static void OnMaxLengthChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e )
        {
            var control = (LimitedTextBox) d;
            control.UpdateRemainingCharsText( control._textBox.Text );
        }

        // [<endsnippet OnMaxLengthChanged>]

        private static void OnTextChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e )
        {
            var control = (LimitedTextBox) d;
            control.UpdateRemainingCharsText( (string) e.NewValue );
        }

        private void UpdateRemainingCharsText( string updateTextValue )
        {
            var remainingChars = this.MaxLength - updateTextValue.Length;
            this._remainingCharsTextBlock.Text = $"{remainingChars} characters remaining";
        }

        // [<snippet ValidateMaxLength>]
        private static bool ValidateMaxLength( object value )
        {
            if ( value is int maxLength && maxLength > 0 )
            {
                return true;
            }

            return false;
        }

        // [<endsnippet ValidateMaxLength>]

        private static bool ValidateText( object value )
        {
            var text = value as string;

            if ( !string.IsNullOrWhiteSpace( text )
                 && !text.All( c => char.IsLetter( c ) || char.IsWhiteSpace( c ) ) )
            {
                return false;
            }

            return true;
        }
    }
}