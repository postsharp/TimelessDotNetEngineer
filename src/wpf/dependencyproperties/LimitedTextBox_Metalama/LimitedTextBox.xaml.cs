// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Metalama.Patterns.Contracts;
using Metalama.Patterns.Wpf;

namespace LimitedTextBox_Manually;

/// <summary>
/// Interaction logic for LimitedTextBox.xaml
/// </summary>
public partial class LimitedTextBox
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
    private void ValidateText( string value )
    {
        if ( !string.IsNullOrWhiteSpace( value )
             && !value.All( c => char.IsLetter( c ) || char.IsWhiteSpace( c ) ) )
        {
            throw new ArgumentException(
                "Invalid Text value. Only letters and whitespace are allowed." );
        }
    }

    // [<endsnippet ValidateText>]

    private void UpdateRemainingCharsText( string updateTextValue )
    {
        var remainingChars = this.MaxLength - updateTextValue.Length;
        this._remainingCharsTextBlock.Text = $"{remainingChars} characters remaining";
    }
}