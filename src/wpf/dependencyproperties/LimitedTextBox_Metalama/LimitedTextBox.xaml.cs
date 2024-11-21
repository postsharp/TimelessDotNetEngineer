// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Metalama.Patterns.Contracts;
using Metalama.Patterns.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        // [<snippet MaxLength_Property_WithContract>]
        [StrictlyGreaterThan( 0 )]

        // [<snippet MaxLength_Property>]
        [DependencyProperty]
        public int MaxLength { get; set; } = 100;

        // [<endsnippet MaxLength_Property>]
        // [<endsnippet MaxLength_Property_WithContract>]

        [DependencyProperty]
        public string Text { get; set; } = string.Empty;

        // [<snippet OnPropertyChanged>]
        private void OnMaxLengthChanged( int oldValue, int newValue )
        {
            this.UpdateRemainingCharsText( this._textBox.Text );
        }

        private void OnTextChanged( string oldValue, string newValue )
        {
            this.UpdateRemainingCharsText( newValue );
        }

        // [<endsnippet OnPropertyChanged>]

        // [<snippet ValidateText>]
        private void ValidateText( object value )
        {
            var text = value as string;

            if ( !string.IsNullOrWhiteSpace( text )
                 && !text.All( c => char.IsLetter( c ) || char.IsWhiteSpace( c ) ) )
            {
                throw new ArgumentException(
                    "Invalid Text value. Only English characters are allowed." );
            }
        }

        // [<endsnippet ValidateText>]

        private void UpdateRemainingCharsText( string updateTextValue )
        {
            var remainingChars = this.MaxLength - updateTextValue.Length;
            this._remainingCharsTextBlock.Text = $"{remainingChars} characters remaining";
        }
    }
}